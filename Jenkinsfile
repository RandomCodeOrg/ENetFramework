node {
	stage 'Checkout'
		checkout scm

	stage 'Build'
		bat 'nuget restore ENetFramework.sln'
		bat "\"${tool 'MSBuild'}\" ENetFramework.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"

	stage 'Archive'
		archive 'RandomCodeOrg.ENetFramework/bin/Release/**'
		archiveArtifacts artifacts: 'RandomCodeOrg.ENetFramework/bin/Release/**', excludes: 'output/*.md'
	
		archive 'RandomCodeOrg.Pluto/bin/Release/**'
		archiveArtifacts artifacts: 'RandomCodeOrg.Pluto/bin/Release/**', excludes: 'output/*.md'

}
