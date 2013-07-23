#please pass task name. Sampel: .\build-scenario.ps1 Clean. Full task by default

pushd .nuget
echo "Restoring nuget packages"
& ".\nuget" install packages.config -o ..\packages
popd

import-module .\packages\Psake.4.2.0.1\tools\psake.psm1
invoke-psake .\default.ps1 $args
remove-module psake