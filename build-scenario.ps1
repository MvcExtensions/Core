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
}

task default -depends Full

task Full -depends Init, Clean, Build

task Init {
	if(-not(Test-Path $artifactPath)) {
		md $artifactPath
	}
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