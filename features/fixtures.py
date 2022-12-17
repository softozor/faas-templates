import os

from behave import fixture
from faas_client import FaasClientFactory
from docker import DockerClient
from features.actors.developer import Developer

here = os.path.dirname(os.path.abspath(__file__))


@fixture
def docker_connection(context):
    docker_client = DockerClient()
    docker_client.login(
        registry=os.environ["DOCKER_HOSTED_REGISTRY"],
        username=os.environ["DOCKER_REGISTRY_USERNAME"],
        password=os.environ["DOCKER_REGISTRY_PASSWORD"],
    )
    yield
    docker_client.close()


@fixture
def faas_port(context):
    context.faas_port = 8080
    return faas_port


@fixture
def path_to_serverless_configuration(context):
    context.path_to_serverless_configuration = os.path.join(
        here, "examples", "faas.yml"
    )
    return context.path_to_serverless_configuration


@fixture
def faas_client(context):
    port = 8080
    hostname = os.environ["FAAS_HOSTNAME"]
    username = os.environ["FAAS_USERNAME"]
    password = os.environ["FAAS_PASSWORD"]
    faas_client_factory = FaasClientFactory(port, env=os.environ)
    context.faas_client = faas_client_factory.create(hostname, username, password)
    context.faas_client.login()


@fixture
def developer(context):
    context.developer = Developer(
        faas_client=context.faas_client,
        path_to_serverless_configuration=context.path_to_serverless_configuration,
    )
    return context.developer


fixtures_registry = {}
