#please pass task name. Sampel: .\build-scenario.ps1 Clean. Full task by default

pushd .nuget
echo "Restoring nuget packages"
& ".\nuget" install packages.config -o ..\packages
popd

import-module .\build\Psake\psake.psm1
invoke-psake .\build-scenario.ps1 $args[0]
remove-module psake