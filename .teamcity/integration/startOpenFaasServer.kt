package integration

import common.scripts.readScript
import jetbrains.buildServer.configs.kotlin.BuildSteps
import jetbrains.buildServer.configs.kotlin.buildSteps.PythonBuildStep
import jetbrains.buildServer.configs.kotlin.buildSteps.python

fun BuildSteps.startOpenFaasServer(): PythonBuildStep {
    return python {
        name = "Start OpenFaaS server"
        command = script {
            content = readScript("integration/start_openfaas_server.py")
            scriptArguments = """
                    --jelastic-api-url %system.jelastic.api-url%
                    --jelastic-access-token %system.jelastic.access-token%
                    --openfaas-env-name %system.openfaas.env-name%
                """.trimIndent()
        }
    }
}