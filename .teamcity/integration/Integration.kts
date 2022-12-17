package integration

import common.git.publishCommitShortSha
import common.lint.lint
import common.livingDoc.addLivingDocArtifacts
import common.livingDoc.generateLivingDocumentation
import common.templates.NexusDockerLogin
import jetbrains.buildServer.configs.kotlin.BuildType
import jetbrains.buildServer.configs.kotlin.DslContext
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildSteps.DockerCommandStep
import jetbrains.buildServer.configs.kotlin.buildSteps.ScriptBuildStep
import jetbrains.buildServer.configs.kotlin.buildSteps.dockerCommand
import jetbrains.buildServer.configs.kotlin.buildSteps.script
import jetbrains.buildServer.configs.kotlin.triggers.vcs

class Integration(dockerTag: String, livingDocZip: String) : BuildType({
    templates(NexusDockerLogin)

    id("Integration")
    name = "Integration"

    vcs {
        root(DslContext.settingsRoot)
    }

    triggers {
        vcs {
            branchFilter = """
                +:*
            """.trimIndent()
        }
    }

    val picklesReportDir = "./pickles"
    val behaveResultsFile = "behave-report.json"

    steps {
        publishCommitShortSha()
        lint()
        dockerCommand {
            name = "Build Docker Image For Tests"
            commandType = build {
                source = file {
                    path = "ci/feature-tests/Dockerfile"
                }
                contextDir = "ci/feature-tests/"
                platform = DockerCommandStep.ImagePlatform.Linux
                namesAndTags = "%system.docker-registry.hosted%/softozor/faas-templates-test:%build.vcs.number%"
                commandArgs = """
                    --pull
                    --build-arg DOCKER_REGISTRY=%system.docker-registry.group%/
                    --build-arg FAAS_CLI_VERSION=%faas-cli.version%
                    --build-arg INDEX_URL=https://%system.package-manager.deployer.username%:%system.package-manager.deployer.password%@%system.pypi-registry.group%
                """.trimIndent()
            }
        }
        dockerCommand {
            name = "Push Docker Image For Tests"
            commandType = push {
                namesAndTags = """
                    %system.docker-registry.hosted%/softozor/faas-templates-test:%build.vcs.number%
                """.trimIndent()
            }
        }
        script {
            name = "Run Acceptance Tests"
            scriptContent = """
                docker login ${'$'}DOCKER_HOSTED_REGISTRY -u %system.package-manager.deployer.username% -p %system.package-manager.deployer.password%
                ./run_behave.sh $behaveResultsFile
            """.trimIndent()
            dockerPull = true
            dockerImage = "%system.docker-registry.group%/softozor/faas-templates-test:%build.vcs.number%"
            dockerImagePlatform = ScriptBuildStep.ImagePlatform.Linux
            dockerRunParameters = "-v /var/run/docker.sock:/var/run/docker.sock"
        }
        generateLivingDocumentation(
            systemUnderTestName = "faas-templates",
            pathToFeaturesDir = "./features",
            picklesReportDir = picklesReportDir,
            testResultsFile = behaveResultsFile,
            testResultsFormat = "cucumberjson",
            dockerTag = dockerTag,
        )
    }

    artifactRules += ", $behaveResultsFile"

    addLivingDocArtifacts(this, picklesReportDir, livingDocZip)

    features {
        perfmon {
        }
    }

    params {
        param("env.FAAS_HOSTNAME", "%system.openfaas.url%")
        param("env.FAAS_USERNAME", "%system.openfaas.admin-user%")
        param("env.FAAS_PASSWORD", "%system.openfaas.admin-password%")
        param("env.DOCKER_HOSTED_REGISTRY", "%system.docker-registry.hosted%")
        param("faas-cli.version", "0.15.4")
        param("teamcity.vcsTrigger.runBuildInNewEmptyBranch", "true")
    }
})