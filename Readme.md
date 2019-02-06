This little utility is supposed to make it a little easier to manage Windows Defender file exclusions.

![Preview](https://i.imgur.com/0cZvCLl.png)

# Features
* Select Files via. FileOpenDialog
* Drag & Drop file into the window
* Add/Remove Files to exclude from Windows Defender

# How it works
This tool utilizes three Powershell CmdLets.
* Add-MpPreference -ExclusionPath
* Get-MpPreference -ExclusionPath
* Remove-MpPreference -ExclusionPath

That's it already.

# Notes
This utility uses Costura.Fody in order to being able to ship a single .exe-file, without having to ship any seperate DLL's.
