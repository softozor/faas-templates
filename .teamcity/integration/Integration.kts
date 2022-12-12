package integration

import common.lint.lint
import common.templates.NexusDockerLogin
import jetbrains.buildServer.configs.kotlin.BuildType
import jetbrains.buildServer.configs.kotlin.DslContext
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildSteps.DockerCommandStep
import jetbrains.buildServer.configs.kotlin.buildSteps.dockerCommand
import jetbrains.buildServer.configs.kotlin.triggers.vcs

object Integration : BuildType({
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

    steps {
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
                    --build-arg FAAS_CLI_VERSION=%faas-cli.version%
                    --build-arg INDEX_URL=%system.pypi-registry.group%
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
    }

    features {
        perfmon {
        }
    }

    params {
        param("faas-cli.version", "0.15.4")
        param("teamcity.vcsTrigger.runBuildInNewEmptyBranch", "true")
    }
})