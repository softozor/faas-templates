from behave import use_fixture
from behave.fixture import use_fixture_by_tag

from fixtures import (
    docker_connection,
    path_to_serverless_configuration,
    faas_client,
    developer,
    fixtures_registry,
)


def before_tag(context, tag):
    if tag.startswith("fixture."):
        return use_fixture_by_tag(tag[8:], context, fixtures_registry)


def before_all(context):
    use_fixture(docker_connection, context)
    use_fixture(path_to_serverless_configuration, context)
    use_fixture(faas_client, context)
    use_fixture(developer, context)
