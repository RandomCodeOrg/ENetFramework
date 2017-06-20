$ErrorActionPreference = "Stop"

nuget restore ../
msbuild ../ENetFramework.sln

rm ./*.nupkg
rm ./*.zip

$apiKey = Get-Content "~\.nuget_api_key.txt"
$src = "https://www.nuget.org/api/v2/package"
nuget pack ../RandomCodeOrg.ENetFramework/RandomCodeOrg.ENetFramework.csproj
nuget pack ../RandomCodeOrg.Pluto/RandomCodeOrg.Pluto.csproj

# Get-ChildItem *.nupkg* -recurse | forEach{ nuget push $_.Name $apiKey -Source $src }

Compress-Archive -Path ../RandomCodeOrg.Pluto/bin/Debug/*.* -DestinationPath RandomCodeOrg.Pluto-SNAPSHOT.zip







