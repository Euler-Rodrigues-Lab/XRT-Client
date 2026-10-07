# Third-Party Notices

This project's own code is MIT-licensed (see [LICENSE](LICENSE)). The components listed below are
either **bundled** in this repository under their own licenses, or **required/optional dependencies
that are not redistributed here** and must be obtained separately.

## Bundled in this repository

| Component | Location | License |
|---|---|---|
| Meta XR Movement SDK samples (v81.0.0) | `Assets/Samples/Meta XR Movement SDK/` | [Oculus SDK License](https://developers.meta.com/horizon/licenses/oculussdk/) |
| AprilTag 3 native library (prebuilt) | `Assets/Plugins/Android/arm64-v8a/libapriltag.so`, `Assets/Plugins/Windows/x86_64/apriltag.dll` | BSD 2-Clause ([AprilRobotics/apriltag](https://github.com/AprilRobotics/apriltag)) |
| In-game Debug Console (yasirkula) | `Assets/Plugins/IngameDebugConsole/` | MIT ([yasirkula/UnityIngameDebugConsole](https://github.com/yasirkula/UnityIngameDebugConsole)) |
| TextMesh Pro essential resources | `Assets/TextMesh Pro/` | [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license); bundled Liberation Sans font under SIL OFL 1.1 (`Assets/TextMesh Pro/Fonts/LiberationSans - OFL.txt`) |

### Meta XR Movement SDK samples

The sample assets (character models, materials, prefabs) are distributed under the Oculus SDK
License Agreement, which permits redistribution of SDK sample material as part of an application
project provided the copyright notices are retained:

> Copyright (c) Meta Platforms, Inc. and affiliates. All rights reserved.
> Your use of this SDK or tool is subject to the Oculus SDK License Agreement, available at
> https://developers.meta.com/horizon/licenses/oculussdk/

They can also be re-imported from **Window > Package Manager > Meta XR Movement SDK > Samples**.

### AprilTag

```
BSD 2-Clause License

Copyright (C) 2013-2016, The Regents of The University of Michigan.
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
```

### In-game Debug Console

```
MIT License

Copyright (c) 2016 Süleyman Yasir KULA

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## Package dependencies (resolved by the Unity Package Manager, not stored here)

Listed in `Packages/manifest.json`; each is governed by its own license, including:
Meta XR Core / Interaction / MR Utility Kit SDKs (Oculus SDK License), Meta XR Movement SDK
(Oculus License, from `github.com/oculus-samples/Unity-Movement`), Unity WebRTC
(Unity Companion License), NativeWebSocket (Apache-2.0, `github.com/endel/NativeWebSocket`),
and Unity first-party packages (Unity Companion License).

## Optional SDKs (NOT redistributed — import them yourself)

| SDK | Why it is not included | How to get it |
|---|---|---|
| bHaptics SDK2 for Unity (TactGlove support) | The bHaptics SDK License Agreement does not permit redistribution of its binaries/source outside an application build. | Install "bHaptics Haptic Plugin" from the Unity Asset Store or the bHaptics developer portal into `Assets/Bhaptics/`. |
| SenseGlove Unity plugin (experimental) | Contains proprietary native binaries (`SGCoreCs.dll`) with unclear redistribution terms. | Download from [Adjuvo/SenseGlove-Unity](https://github.com/Adjuvo/SenseGlove-Unity) into `Assets/SenseGlove/`. |

The editor script `Assets/Editor/OptionalSdkDefines/OptionalSdkDefines.cs` detects these SDKs and
enables the `XRT_BHAPTICS` / `XRT_SENSEGLOVE` compile symbols automatically. Without them the
project still compiles and runs; only the corresponding glove integrations are disabled.

## Logos

The Robot Danger Lab (RDL) and SAIL logos in `Assets/Image Imports/` are the property of their
respective labs and are **not** covered by the MIT license. Forks should replace them.
