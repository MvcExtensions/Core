#please pass task name. Sampel: .\build-scenario.ps1 Clean. Full task by default

pushd .nuget
echo "Restoring nuget packages"
& ".\nuget" install packages.config -o ..\packages
popd

$psm = Get-Item ".\packages\Psake.*\tools\psake.psm1"
Import-Module "$psm"
$psake.use_exit_on_error = $true
Invoke-psake .\default.ps1 $args[0]