@echo off
set psake=packages\psake.4.4.1\tools\psake

pushd .nuget
echo Restoring nuget packages
call nuget install packages.config -o ..\packages
popd

call %psake% %%*

pause