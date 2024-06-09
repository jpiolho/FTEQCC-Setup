# FTEQCC-Setup
**NOTE:** _This project/repo is not affiliated with FTE Team or FTEQCC in any way_

This repo contains a [WixSharp](https://github.com/oleg-shilo/wixsharp) project and a [Github action workflow](https://github.com/jpiolho/FTEQCC-Setup/blob/main/.github/workflows/build.yaml), which together will automatically get the latest [FTEQCC](https://www.fteqcc.org/) version, check if it's new and build a `.msi` Windows 64-bit setup file.

You can find the latest build in the releases.

The msi setup is intended to be compatible with [Winget](https://github.com/microsoft/winget-cli). Although that's still a work in progress.

## What does the setup do?
The msi setup installs FTEQCC as a normal app on your computer. It includes both console and gui version.

* Shortcuts for the gui version get added to both Start Menu and Desktop
* The install path gets added to `PATH` so you can call the console version from your terminal using `fteqcc`
* Install path is `C:\Program Files\FTEQCC` and cannot be changed

## How it works?

The [github workflow](https://github.com/jpiolho/FTEQCC-Setup/blob/main/.github/workflows/build.yaml) does the following:

1. Parses FTEQCC website to fetch the latest build date
2. Compares build date with the one in [this file](https://github.com/jpiolho/FTEQCC-Setup/blob/main/last_build_date.txt)
3. If date differs, then it proceeds with downloading both FTEQCC GUI and Console zip files. If the date is the same then nothing happens.
4. Extracts zips, builds the setup
5. Creates a Github release
