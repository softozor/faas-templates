import os
import random

from behave import fixture
from faas_client import FaasClientFactory
from jelastic_client import JelasticClientFactory
from test_utils import get_new_random_env_name


@fixture
def random_seed(context):
    random.seed('hasura-serverless-tests')


@fixture
def worker_id(context):
    context.worker_id = 'master'
    return context.worker_id


@fixture
def commit_sha(context):
    userdata = context.config.userdata
    context.commit_sha = userdata['commit-sha']
    return context.commit_sha


@fixture
def project_root_folder(context):
    userdata = context.config.userdata
    context.project_root_folder = userdata['project-root-folder'] if 'project-root-folder' in userdata else '.'
    return context.project_root_folder


@fixture
def api_clients(context):
    userdata = context.config.userdata
    api_url = userdata['api-url']
    api_token = userdata['api-token']
    api_client_factory = JelasticClientFactory(api_url, api_token)
    context.jps_client = api_client_factory.create_jps_client()
    context.control_client = api_client_factory.create_control_client()
    context.file_client = api_client_factory.create_file_client()


@fixture
def faas_port(context):
    context.faas_port = 8080
    return faas_port


@fixture
def path_to_serverless_functions(context):
    context.path_to_serverless_functions = os.path.join(
        context.project_root_folder, 'features', 'examples', 'functions')
    return context.path_to_serverless_functions


@fixture
def faas_dotnet_definition_yaml(context):
    context.faas_dotnet_definition_yaml = 'faas-dotnet.yml'
    return context.faas_dotnet_definition_yaml


@fixture
def faas_nodejs_definition_yaml(context):
    context.faas_nodejs_definition_yaml = 'faas-nodejs.yml'
    return context.faas_nodejs_definition_yaml


@fixture
def path_to_jelastic_environment_manifest(context):
    context.path_to_jelastic_environment_manifest = os.path.join(
        context.project_root_folder, 'features', 'jelastic', 'manifest.yml')
    return context.path_to_jelastic_environment_manifest


@fixture
def faas_client_factory(context):
    context.faas_client_factory = FaasClientFactory(
        context.path_to_serverless_functions,
        context.faas_port,
        context.faas_definition_yaml)
    return context.faas_client_factory


@fixture
def jelastic_environment(context):
    context.current_env_name = get_new_random_env_name(
        context.control_client, context.commit_sha, context.worker_id)
    path_to_manifest = context.path_to_jelastic_environment_manifest
    context.jps_client.install_from_file(
        path_to_manifest, context.current_env_name
    )
    yield context.current_env_name
    env_info = context.control_client.get_env_info(
        context.current_env_name)
    if env_info.exists():
        context.control_client.delete_env(context.current_env_name)


fixtures_registry = {

}