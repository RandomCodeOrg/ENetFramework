node {
	stage 'Checkout'
		checkout scm

	stage 'Build'
		bat 'nuget restore ENetFramework.sln'
		bat "\"${tool 'MSBuild'}\" ENetFramework.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"

	stage 'Archive'
		archiveArtifacts artifacts: 'RandomCodeOrg.ENetFramework/bin/Release/*.dll', excludes: 'output/*.md'
	
		archiveArtifacts artifacts: 'RandomCodeOrg.Pluto/bin/Release/*.exe', excludes: 'output/*.md'
		archiveArtifacts artifacts: 'RandomCodeOrg.ENetFramework/bin/Release/*.dll', excludes: 'output/*.md'

}
