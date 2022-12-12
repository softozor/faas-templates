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
                behave --junit --junit-directory ./features/test-reports --tags ~wip \
                    -D project-root-folder="%system.teamcity.build.checkoutDir%" \
                    -D api-url="%system.jelastic.api-url%" \
                    -D api-token="%system.jelastic.access-token%" \
                    -D commit-sha="%build.vcs.number%"
            """.trimIndent()
            dockerPull = true
            dockerImage = "%system.docker-registry.group%/softozor/faas-templates-test:%build.vcs.number%"
            dockerImagePlatform = ScriptBuildStep.ImagePlatform.Linux
        }
        generateLivingDocumentation(
            systemUnderTestName = "faas-templates",
            pathToFeaturesDir = "./features",
            picklesReportDir = picklesReportDir,
            dockerTag = dockerTag,
        )
    }

    addLivingDocArtifacts(this, picklesReportDir, livingDocZip)

    features {
        perfmon {
        }
    }

    params {
        param("faas-cli.version", "0.15.4")
        param("teamcity.vcsTrigger.runBuildInNewEmptyBranch", "true")
    }
})