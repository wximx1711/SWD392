# Ho Tuan Minh | HE191253 | SE1990NJ
# Package source, documentation and SQL only. No dependencies, builds or databases.
$ErrorActionPreference='Stop'
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem
$root=Split-Path -Parent $PSScriptRoot
$target=Join-Path $root 'HE191253_CourtChain_UC51-UC55.zip'
if(Test-Path -LiteralPath $target){Remove-Item -LiteralPath $target}
$files=@()
foreach($folder in @('backend','frontend','tests','docs','database','scripts')){
    $files+=Get-ChildItem -LiteralPath (Join-Path $root $folder) -Recurse -File | Where-Object {
        $_.FullName -notmatch '[\\/](node_modules|bin|obj|dist|test-results|playwright-report|\.git)[\\/]' -and
        $_.Name -notmatch '\.db($|-)|\.zip$|\.tsbuildinfo$' -and
        ($_.Name -ne '.env')
    }
}
foreach($name in @('README.md','CourtChain.slnx','.gitignore')){$files+=Get-Item -LiteralPath (Join-Path $root $name)}
$archive=[IO.Compression.ZipFile]::Open($target,[IO.Compression.ZipArchiveMode]::Create)
try{
    foreach($file in $files){
        $entry='CourtChain/'+$file.FullName.Substring($root.Length+1).Replace('\','/')
        [IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive,$file.FullName,$entry,[IO.Compression.CompressionLevel]::Optimal)|Out-Null
    }
}finally{$archive.Dispose()}
Get-Item -LiteralPath $target | Select-Object Name,Length
