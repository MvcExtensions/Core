@echo off
pushd .nuget
echo Restoring nuget packages
call nuget install packages.config -o ..\packages
popd

powershell.exe -NoProfile -ExecutionPolicy unrestricted -Command "& {Import-Module '.\build\Psake'; Import-Module '.\build\Pscx'; invoke-psake .\build-scenario.ps1 %*; remove-module psake; if ($lastexitcode -ne 0) {write-host "ERROR: $lastexitcode" -fore RED; exit $lastexitcode} }" 


pause