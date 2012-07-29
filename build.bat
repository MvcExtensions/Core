@echo off
pushd .nuget
echo Restoring nuget packages
call nuget install packages.config -o ..\packages
popd

set msbuild=%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild
set configuration=%1
if "%configuration%" == "" set configuration=Release

%msbuild% MvcExtensions.build /t:Full /p:Configuration=%configuration% /m:2

pause