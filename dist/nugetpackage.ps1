rm ./*.nupkg
$apiKey = Get-Content "~\.nuget_api_key.txt"
$src = "https://www.nuget.org/api/v2/package"
nuget pack ../RandomCodeOrg.ENetFramework/RandomCodeOrg.ENetFramework.csproj
nuget pack ../RandomCodeOrg.Pluto/RandomCodeOrg.Pluto.csproj

Get-ChildItem *.nupkg* -recurse | forEach{ nuget push $_.Name $apiKey -Source $src }

