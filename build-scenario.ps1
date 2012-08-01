properties {
	$projectDir = Resolve-Path "."
	$solution = "$projectDir\MvcExtensions.sln"
	$configuration = "Debug"
	$artifactPath = "$projectDir\Drops"
	$buildNumber = "3.0.0"
	$version = "$buildNumber.0"
	$semVer = $buildNumber
	$xunit = "$projectDir\packages\xunit.runners.1.9.1\tools\xunit.console.clr4.x86.exe"
	$corePath = "$projectDir\src\MvcExtensions"
	$coreFile = "MvcExtensions"
	$nuget = "$projectDir\.nuget\nuget"
	$referencePath = "$projectDir\References"
}

task default -depends Full

task Full -depends Init, Clean, Build

task Init {
	if(-not(Test-Path $artifactPath)) {
		md $artifactPath
	}
}

task FxCop {
	$fxCopOutput = "$artifactPath\FxCop.xml"
	$fxCopTotalErrors = 0

	Copy-Item $projectDir\Build\FxCop\Xml\FxCopReport.xsl $artifactPath

	exec { & "$projectDir\Build\FxCop\FxCopCmd.exe" `
		/f:"$corePath\bin\$configuration\$coreFile.dll" `
		/d:"$referencePath\AspNetMvc" `
		/d:"$referencePath\Autofac" `
		/d:"$referencePath\Castle" `
		/d:"$referencePath\Ninject" `
		/d:"$referencePath\PnP" `
		/d:"$referencePath\StructureMap" `
		/dic:"$projectDir\src\SharedFiles\CodeAnalysisDictionary.xml" `
		/o:"$fxCopOutput" `
		/oxsl:"FxCopReport.xsl" `
		/to:0 /fo /gac /igc /q
	}

	[xml]$xd = Get-Content $fxCopOutput
	$nodelist = $xd.selectnodes("//Issue")
	$fxCopTotalErrors = $nodelist.Count
	if($fxCopTotalErrors -gt 0){
		Write-Host "FxCop encountered $fxCopTotalErrors rule violations"
	}
}

task Simian {
	Copy-Item $projectDir\Build\Simian\simian.xsl $artifactPath
	& "$projectDir\Build\Simian\simian-2.2.24.exe" `
		-formatter=xml:"$artifactPath\Simian.xml" `
		-reportDuplicateText+ `
		"$corePath\**\*.cs"
}

task Build {
	Generate-Assembly-Info `
        -outputFile "$projectDir\src\SharedFiles\CommonAssemblyInfo.cs" `
        -company "MvcExtensions" `
        -copyright "Copyright © Kazi Manzur Rashid 2009-2012, hazzik 2011-2012" `
        -comVisible "false" `
        -version $version `
        -fileVersion $version `
        -informationalVersion $semVer

	exec { msbuild $solution /t:Build /p:Configuration=$configuration /m }
}

task Clean {
	exec { msbuild $solution /t:Clean /p:Configuration=$configuration /m }
}

task Tests -depends Init {
	exec { & $xunit "$corePath.Tests\bin\$configuration\$coreFile.Tests.dll" /noshadow /xml $artifactPath\$coreFile.Tests.xunit.xml }
}

task Deploy {
	Import-Module ".\build\PScx"
	$files = @("$projectDir\license.txt")
	Get-ChildItem -Path "$corePath\bin\$configuration" | Where-Object { $_.name -match "$coreFile.*" } | ForEach-Object { $files += $_.FullName }
	Write-Zip -Path $files -OutputPath "$artifactPath\$coreFile.$semVer.zip" -level 9
	exec { & "${nuget}.exe" pack $corePath\$coreFile.nuspec /basepath $corePath\bin\$configuration /outputdirectory $artifactPath /version $semVer }
}

task Publish {
	exec { & "${nuget}.cmd" push $artifactPath\$coreFile.$semVer.nupkg }
}

#https://github.com/ayende/rhino-mocks/blob/master/psake_ext.ps1
function Generate-Assembly-Info
{
param(
	[string]$company,
	[string]$copyright,
	[string]$comVisible,
	[string]$version,
	[string]$fileVersion,
	[string]$informationalVersion,
	[string]$outputFile = $(throw "file is a required parameter.")
)
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany(""$company"")]
[assembly: AssemblyCopyright(""$copyright"")]
[assembly: ComVisible($comVisible)]
[assembly: AssemblyVersion(""$version"")]
[assembly: AssemblyFileVersion(""$fileVersion"")]
[assembly: AssemblyInformationalVersion(""$informationalVersion"")]
"

	Write-Host $outputFile
	$dir = [System.IO.Path]::GetDirectoryName($outputFile)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating assembly info file: $outputFile"
	out-file -filePath $outputFile -encoding UTF8 -inputObject $asmInfo
}