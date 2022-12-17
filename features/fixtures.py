import os

from behave import fixture
from faas_client import FaasClientFactory

from features.actors.developer import Developer

here = os.path.dirname(os.path.abspath(__file__))


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
    # TODO: pass this hostname as an argument
    hostname = os.environ["FAAS_HOSTNAME"]
    username = "admin"
    password = "OAPaL5mw3UtPcxBS660ppH9TsWhpkahSszHQGLZwZFebQPBLiuKQRC72P1ehro0"
    faas_client_factory = FaasClientFactory(port, env=os.environ)
    context.faas_client = faas_client_factory.create(hostname, username, password)
    context.faas_client.login()


@fixture
def developer(context):
    context.developer = Developer(context)
    return context.developer


fixtures_registry = {}
