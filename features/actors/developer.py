import requests
from softozor_test_utils.timing import fail_after_timeout


class Developer:
    def __init__(self, context):
        self._faas_client = context.faas_client
        self._path_to_serverless_configuration = (
            context.path_to_serverless_configuration
        )

    def build_function(self, function_name):
        return self._faas_client.build(
            self._path_to_serverless_configuration, function_name
        )

    def push_function(self, function_name):
        return self._faas_client.push(
            self._path_to_serverless_configuration, function_name
        )

    def deploy_function(self, function_name):
        path_to_config = self._path_to_serverless_configuration

        def deploy():
            try:
                exit_code = self._faas_client.deploy(path_to_config, function_name)
                print("exit code = ", exit_code)
                return exit_code == 0
            except Exception as e:
                print("caught exception: ", e)
                return 1

        return not fail_after_timeout(
            lambda: deploy(), timeout_in_sec=60, period_in_sec=5
        )

    def up_function(self, function_name):
        exit_code = self.build_function(function_name)
        assert exit_code == 0
        exit_code = self.push_function(function_name)
        assert exit_code == 0
        exit_code = self.deploy_function(function_name)
        if exit_code == 0:
            return not self._is_function_ready(function_name)
        return exit_code

    def invoke_function(self, function_name, payload=None):
        assert self._can_invoke_function(function_name, payload)
        return self._do_invoke_function(function_name, payload)

    def _is_function_ready(self, function_name):
        def is_ready():
            return self._faas_client.is_ready(function_name)

        return fail_after_timeout(lambda: is_ready())

    def _can_invoke_function(self, function_name, payload):
        def can_invoke():
            response = self._do_invoke_function(function_name, payload)
            return response.status_code < 500

        return fail_after_timeout(
            lambda: can_invoke(), timeout_in_sec=30, period_in_sec=0.5
        )

    def _do_invoke_function(self, function_name, payload):
        function_url = f"http://{self._faas_client.endpoint}/function/{function_name}"
        return requests.post(function_url, json=payload)
