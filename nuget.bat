@echo off
cls
".nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"
".nuget\NuGet.exe" "Install" "EWSoftware.SHFB" "-OutputDirectory" "packages" "-ExcludeVersion"
pause