from behave import use_fixture
from behave.fixture import use_fixture_by_tag

from fixtures import *


def before_tag(context, tag):
    if tag.startswith('fixture.'):
        return use_fixture_by_tag(tag[8:], context, fixtures_registry)


def before_all(context):
    use_fixture(api_clients, context)
    use_fixture(random_seed, context)
    use_fixture(worker_id, context)
    use_fixture(commit_sha, context)
    use_fixture(project_root_folder, context)
    use_fixture(path_to_jelastic_environment_manifest, context)
    use_fixture(faas_port, context)
    use_fixture(path_to_serverless_functions, context)
    use_fixture(faas_dotnet_definition_yaml, context)
    use_fixture(faas_nodejs_definition_yaml, context)
    use_fixture(faas_client_factory, context)
    use_fixture(jelastic_environment, context)
