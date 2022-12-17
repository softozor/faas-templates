from datetime import timedelta

import requests
from faas_client.faas_client import FaasClient
from tenacity import (
    retry,
    retry_if_not_result,
    wait_fixed,
    stop_after_delay,
    retry_if_result,
)


# TODO: rework this class completely
class Developer:
    def __init__(self, faas_client: FaasClient, path_to_serverless_configuration: str):
        self._faas_client = faas_client
        self._path_to_serverless_configuration = path_to_serverless_configuration

    def build_function(self, function_name):
        return self._faas_client.build(
            self._path_to_serverless_configuration, function_name
        )

    def push_function(self, function_name):
        return self._faas_client.push(
            self._path_to_serverless_configuration, function_name
        )

    @retry(
        retry=retry_if_not_result(lambda exit_code: exit_code == 0),
        wait=wait_fixed(timedelta(seconds=5).seconds),
        stop=stop_after_delay(timedelta(minutes=1).seconds),
    )
    def deploy_function(self, function_name):
        path_to_config = self._path_to_serverless_configuration
        exit_code = self._faas_client.deploy(path_to_config, function_name)
        return exit_code

    # TODO: the assertions should not be there
    def up_function(self, function_name):
        exit_code = self.build_function(function_name)
        assert exit_code == 0
        exit_code = self.push_function(function_name)
        assert exit_code == 0
        exit_code = self.deploy_function(function_name)
        if exit_code == 0:
            return not self._is_function_ready(function_name)
        return exit_code

    # TODO: the assertions should not be there
    def invoke_function(self, function_name, payload=None):
        assert self._can_invoke_function(function_name, payload)
        return self._do_invoke_function(function_name, payload)

    @retry(
        retry=retry_if_result(lambda is_ready: is_ready is False),
        wait=wait_fixed(timedelta(seconds=5).seconds),
        stop=stop_after_delay(timedelta(minutes=1).seconds),
    )
    def _is_function_ready(self, function_name):
        is_ready = self._faas_client.is_ready(function_name)
        return is_ready

    @retry(
        retry=retry_if_result(lambda status_code: status_code >= 500),
        wait=wait_fixed(timedelta(seconds=0.5).seconds),
        stop=stop_after_delay(timedelta(minutes=1).seconds),
    )
    def _can_invoke_function(self, function_name, payload):
        response = self._do_invoke_function(function_name, payload)
        return response.status_code

    def _do_invoke_function(self, function_name, payload):
        function_url = f"http://{self._faas_client.endpoint}/function/{function_name}"
        return requests.post(function_url, json=payload)
