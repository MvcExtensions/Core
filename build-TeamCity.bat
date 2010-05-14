@echo off

if "%1%" == "" goto release
%windir%\Microsoft.NET\Framework\v3.5\MSBuild MvcExtensions-TeamCity.build /t:Full /p:Configuration=%1 /m:2 /tv:3.5
goto end

:release
%windir%\Microsoft.NET\Framework\v3.5\MSBuild MvcExtensions-TeamCity.build /t:Full /p:Configuration=release /m:2 /tv:3.5

:end
pause