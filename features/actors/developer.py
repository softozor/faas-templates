from datetime import timedelta

import requests
from faas_client import FaasClientException
from faas_client.faas_client import FaasClient
from tenacity import (
    retry,
    wait_fixed,
    stop_after_delay,
    retry_if_result,
    retry_if_exception_type,
)


class Developer:
    def __init__(self, faas_client: FaasClient, path_to_serverless_configuration: str):
        self._faas_client = faas_client
        self._path_to_serverless_configuration = path_to_serverless_configuration

    def build_function(self, function_name):
        self._faas_client.build(self._path_to_serverless_configuration, function_name)

    def push_function(self, function_name):
        self._faas_client.push(self._path_to_serverless_configuration, function_name)

    @retry(
        retry=retry_if_exception_type(FaasClientException),
        wait=wait_fixed(timedelta(seconds=5).seconds),
        stop=stop_after_delay(timedelta(minutes=1).seconds),
    )
    def deploy_function(self, function_name):
        path_to_config = self._path_to_serverless_configuration
        self._faas_client.deploy(path_to_config, function_name)

    def up_function(self, function_name):
        self.build_function(function_name)
        self.push_function(function_name)
        self.deploy_function(function_name)
        self._check_function_is_ready(function_name)

    def invoke_function(self, function_name, payload=None):
        self._wait_for_invocable_function(function_name, payload)
        return self._do_invoke_function(function_name, payload)

    @retry(
        retry=retry_if_result(lambda is_ready: is_ready is False),
        wait=wait_fixed(timedelta(seconds=5).seconds),
        stop=stop_after_delay(timedelta(minutes=1).seconds),
    )
    def _check_function_is_ready(self, function_name):
        is_ready = self._faas_client.is_ready(function_name)
        return is_ready

    @retry(
        retry=retry_if_result(lambda status_code: status_code >= 500),
        wait=wait_fixed(timedelta(seconds=0.5).seconds),
        stop=stop_after_delay(timedelta(minutes=1).seconds),
    )
    def _wait_for_invocable_function(self, function_name, payload):
        response = self._do_invoke_function(function_name, payload)
        return response.status_code

    def _do_invoke_function(self, function_name, payload):
        function_url = f"http://{self._faas_client.endpoint}/function/{function_name}"
        return requests.post(function_url, json=payload)
