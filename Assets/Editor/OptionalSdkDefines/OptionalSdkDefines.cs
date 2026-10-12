using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Compilation;
using UnityEngine;

/// <summary>
/// Maintains the compiler define for the optional proprietary bHaptics SDK, which is not
/// distributed with this repository (it must be imported separately):
///   XRT_BHAPTICS - bHaptics SDK2 (Assets/Bhaptics)
/// The define lives in Assets/csc.rsp (gitignored, machine-local) rather than PlayerSettings,
/// so a fresh clone never inherits it. Integration scripts are wrapped in #if on this
/// symbol so the project compiles with or without the SDK. Lives in its own editor-only
/// assembly so removing an SDK while the editor is open is handled. If an SDK is removed
/// while Unity is closed, delete Assets/csc.rsp before reopening the project.
/// </summary>
[InitializeOnLoad]
public static class OptionalSdkDefines
{
    const string BhapticsDefine = "XRT_BHAPTICS";
    const string RspPath = "Assets/csc.rsp";

    static readonly string[] ManagedDefines = { BhapticsDefine };
    static readonly NamedBuildTarget[] LegacyTargets = { NamedBuildTarget.Android, NamedBuildTarget.Standalone };

    static OptionalSdkDefines()
    {
        Refresh();
        // Changes made during Play Mode are skipped; re-check once back in Edit Mode.
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.EnteredEditMode)
                Refresh();
        };
    }

    [MenuItem("Tools/XRT/Refresh Optional SDK Defines")]
    static void RefreshFromMenu()
    {
        if (!Refresh())
            Debug.Log($"[OptionalSdkDefines] Defines up to date ({BhapticsDefine}={HasBhaptics()}).");
    }

    /// <summary>Syncs csc.rsp with the SDK on disk. Returns true if anything was changed.</summary>
    public static bool Refresh()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return false;

        var wanted = new Dictionary<string, bool>
        {
            { BhapticsDefine, HasBhaptics() },
        };

        bool changed = RemoveLegacyPlayerSettingsDefines();

        if (UpdateRspFile(wanted))
        {
            changed = true;
            Debug.Log($"[OptionalSdkDefines] Updated {RspPath}: {BhapticsDefine}={wanted[BhapticsDefine]}.");
            // Defer AssetDatabase/compilation calls out of InitializeOnLoad / asset postprocessing.
            EditorApplication.delayCall += () =>
            {
                if (File.Exists(RspPath))
                    AssetDatabase.ImportAsset(RspPath, ImportAssetOptions.ForceUpdate);
                // csc.rsp is not a tracked compile input for incremental builds; a clean build
                // cache is required for the new defines to take effect.
                CompilationPipeline.RequestScriptCompilation(RequestScriptCompilationOptions.CleanBuildCache);
            };
        }

        return changed;
    }

    /// <summary>
    /// Rewrites csc.rsp so it contains exactly the wanted -define lines for the managed symbols,
    /// preserving any other lines. Deletes the file (and .meta) if nothing would remain.
    /// Only touches disk when the content actually changes.
    /// </summary>
    static bool UpdateRspFile(Dictionary<string, bool> wanted)
    {
        string existing = File.Exists(RspPath) ? File.ReadAllText(RspPath) : null;

        var lines = (existing ?? "")
            .Replace("\r\n", "\n")
            .Split('\n')
            .Where(l => !ManagedDefines.Any(d => l.Trim() == "-define:" + d))
            .ToList();
        while (lines.Count > 0 && lines[lines.Count - 1].Trim().Length == 0)
            lines.RemoveAt(lines.Count - 1);

        foreach (var d in ManagedDefines)
        {
            if (wanted[d])
                lines.Add("-define:" + d);
        }

        if (lines.All(l => l.Trim().Length == 0))
        {
            if (existing == null)
                return false;
            File.Delete(RspPath);
            if (File.Exists(RspPath + ".meta"))
                File.Delete(RspPath + ".meta");
            return true;
        }

        string content = string.Join("\n", lines) + "\n";
        if (existing != null && existing.Replace("\r\n", "\n") == content)
            return false;

        File.WriteAllText(RspPath, content);
        return true;
    }

    /// <summary>Cleanup: these symbols used to be written to PlayerSettings; keep them out of it.</summary>
    static bool RemoveLegacyPlayerSettingsDefines()
    {
        bool changed = false;
        foreach (var target in LegacyTargets)
        {
            var current = PlayerSettings.GetScriptingDefineSymbols(target)
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => s.Length > 0)
                .ToList();

            var updated = current.Where(s => !ManagedDefines.Contains(s)).ToList();
            if (updated.Count == current.Count)
                continue;

            PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", updated));
            changed = true;
        }

        if (changed)
            Debug.Log($"[OptionalSdkDefines] Removed {BhapticsDefine} from PlayerSettings scripting defines (now managed via {RspPath}).");

        return changed;
    }

    static bool HasBhaptics()
    {
        return HasAsset("BhapticsLibrary.cs", "Assets/Bhaptics/SDK2/Scripts/Core/Plugins/BhapticsLibrary.cs")
            && HasAsset("BhapticsPhysicsGlove.cs", "Assets/Bhaptics/SDK2/Scripts/Core/BhapticsPhysicsGlove.cs");
    }


    /// <summary>
    /// True if an asset with exactly this file name exists anywhere under Assets/ (so a moved
    /// SDK folder is still found), or at its default path on disk (covers the window before
    /// the AssetDatabase has imported it on a fresh project load).
    /// </summary>
    static bool HasAsset(string fileName, string defaultPath)
    {
        if (File.Exists(defaultPath))
            return true;

        string query = Path.GetFileNameWithoutExtension(fileName);
        foreach (var guid in AssetDatabase.FindAssets(query, new[] { "Assets" }))
        {
            if (Path.GetFileName(AssetDatabase.GUIDToAssetPath(guid)) == fileName)
                return true;
        }
        return false;
    }

    /// <summary>Re-sync when SDK files are imported, deleted or moved without a domain reload.</summary>
    class SdkAssetWatcher : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
        {
            if (TouchesSdk(imported) || TouchesSdk(deleted) || TouchesSdk(moved) || TouchesSdk(movedFrom))
                Refresh();
        }

        static bool TouchesSdk(string[] paths)
        {
            foreach (var p in paths)
            {
                if (p.StartsWith("Assets/Bhaptics", StringComparison.OrdinalIgnoreCase) ||
                    p.EndsWith("BhapticsLibrary.cs", StringComparison.OrdinalIgnoreCase) ||
                    p.EndsWith("BhapticsPhysicsGlove.cs", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
