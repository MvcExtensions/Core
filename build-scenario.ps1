Framework 4.0

properties {
	if ($buildNumber -eq $null) {
		$buildNumber = "3.0.0"
	}
	$projectDir = Resolve-Path "."
	$solution = "$projectDir\MvcExtensions.sln"
	$configuration = "Debug"
	$artifactPath = "$projectDir\Drops"
	$version = "$buildNumber.0"
	$semVer = $buildNumber
	$xunit = "$projectDir\packages\xunit.runners.1.9.1\tools\xunit.console.clr4.exe"
	$projects = @("MvcExtensions", "MvcExtensions.FluentMetadata")
	$nuget = "$projectDir\.nuget\nuget"
	$referencePath = "$projectDir\References"
	$coverageRunner = "$projectDir\build\PartCover\partcover.exe"
	$styleCop = "$projectDir\build\StyleCop\StyleCopCLI.exe"
}

task default -depends Full

task Full -depends Init, Clean, StyleCop, Simian, Build, FxCop, Tests, Deploy

task Init {
	if(-not(Test-Path $artifactPath)) {
		md $artifactPath
	}
}

task Clean {
	msbuild $solution /t:Clean /p:Configuration=$configuration /m
}

#http://sourceforge.net/projects/stylecopcli/
task StyleCop {
	& $styleCop -sln "$solution" -out "$artifactPath\StyleCop.xml"
}

task Simian {
	Copy-Item $projectDir\Build\Simian\simian.xsl $artifactPath
	$projects | ForEach-Object {
		$out = "$artifactPath\Simian$_.xml"
		
		& "$projectDir\Build\Simian\simian-2.2.24.exe" `
			-formatter=xml:"$out" `
			-reportDuplicateText+ `
			"$projectDir\src\$_\**\*.cs"
	}
}

task Build {
	Generate-Assembly-Info `
        -outputFile "$projectDir\src\SharedFiles\CommonAssemblyInfo.cs" `
        -company "MvcExtensions" `
        -copyright "Copyright &copy; Kazi Manzur Rashid 2009-2012, hazzik 2011-2012" `
        -comVisible "false" `
        -version $version `
        -fileVersion $version `
        -informationalVersion $semVer

	msbuild $solution /t:Build /p:Configuration=$configuration /m
}

task FxCop {
	$fxCopTotalErrors = 0
	Copy-Item $projectDir\Build\FxCop\Xml\FxCopReport.xsl $artifactPath

	$projects | ForEach-Object { 
		$out = "$artifactPath\FxCop$_.xml"

		& "$projectDir\Build\FxCop\FxCopCmd.exe" `
			/f:"$projectDir\src\$_\bin\$configuration\$_.dll" `
			/d:"$referencePath\AspNetMvc3" `
			/dic:"$projectDir\src\SharedFiles\CodeAnalysisDictionary.xml" `
			/o:"$out" `
			/oxsl:"FxCopReport.xsl" `
			/to:0 /fo /gac /igc /q

		[xml]$xd = Get-Content $out
		$nodelist = $xd.selectnodes("//Issue")
		$fxCopTotalErrors = $nodelist.Count
		if ($fxCopTotalErrors -gt 0){
			Write-Host "FxCop encountered $fxCopTotalErrors rule violations"
		}	
	}
}

task CodeCoverage {	
	Copy-Item $projectDir\Build\PartCover\partcover.xml $artifactPath
	$projects | ForEach-Object { 
		& $coverageRunner --register --settings $artifactPath\partcover.xml --output $artifactPath\$_-coverage.xml 
	}
}

task Tests -depends Init {
	$projects | ForEach-Object {
		& $xunit "$projectDir\src\$_.Tests\bin\$configuration\$_.Tests.dll" /noshadow /xml $artifactPath\$_.Tests.xunit.xml
	}
}

task Deploy -depends Deploy-Zip, Deploy-Nuget  

task Deploy-Zip {
	Import-Module ".\build\PScx"

	$projects | ForEach-Object {
		$f = $_
		$files = @("$projectDir\license.txt")
		Get-ChildItem -Path "$projectDir\src\$f\bin\$configuration" | Where-Object { $_.name -match "$f.*" } | ForEach-Object { $files += $_.FullName }
		Write-Zip -Path $files -OutputPath "$artifactPath\$f.$semVer.zip" -level 9
	}
}

task Deploy-Nuget {
	$projects | ForEach-Object { 
		& "${nuget}.exe" pack $projectDir\src\$_\$_.nuspec /basepath $projectDir\src\$_\bin\$configuration /outputdirectory $artifactPath /version $semVer 
	}
}

task Publish {
	$projects | ForEach-Object { 
		& "${nuget}.cmd" push "$artifactPath\$_.$semVer.nupkg" 
	}
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