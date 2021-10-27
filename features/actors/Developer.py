import requests
from softozor_test_utils import wait_until


class Developer:

    def __init__(self, context):
        self.__faas_client = context.faas_client
        self.__path_to_serverless_configuration = context.path_to_serverless_configuration

    def build_function(self, function_name):
        return self.__faas_client.build(
            self.__path_to_serverless_configuration, function_name)

    def push_function(self, function_name):
        return self.__faas_client.push(
            self.__path_to_serverless_configuration, function_name)

    def deploy_function(self, function_name):
        return self.__faas_client.deploy(
            self.__path_to_serverless_configuration, function_name)

    def up_function(self, function_name):
        exit_code = self.build_function(function_name)
        assert exit_code == 0
        exit_code = self.push_function(function_name)
        assert exit_code == 0
        exit_code = self.deploy_function(function_name)
        if exit_code == 0:
            return not self.__is_function_ready(function_name)
        return exit_code

    def invoke_function(self, function_name, payload=None):
        function_url = f'http://{self.__faas_client.endpoint}/function/{function_name}'
        return requests.post(function_url, json=payload)

    def __is_function_ready(self, function_name):
        def is_ready():
            return self.__faas_client.is_ready(function_name)

        try:
            wait_until(lambda: is_ready())
            return True
        except TimeoutError:
            return False
