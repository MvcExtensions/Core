@echo off

if "%1%" == "" goto release
%windir%\Microsoft.NET\Framework\v3.5\MSBuild MvcExtensions.build /t:Full /p:Configuration=%1 /m:2
goto end

:release
%windir%\Microsoft.NET\Framework\v3.5\MSBuild MvcExtensions.build /t:Full /p:Configuration=release /m:2

:end
pause