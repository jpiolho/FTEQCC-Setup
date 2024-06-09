# FTEQCC-Setup
This repo contains a [WixSharp](https://github.com/oleg-shilo/wixsharp) project and a [Github action workflow](https://github.com/jpiolho/FTEQCC-Setup/blob/main/.github/workflows/build.yaml), which together will automatically get the latest [FTEQCC](https://www.fteqcc.org/) version, check if it's new and build a `.msi` Windows 64-bit setup file.

You can find the latest build in the releases.

The msi setup is intended to be compatible with [Winget](https://github.com/microsoft/winget-cli). Although that's still a work in progress.