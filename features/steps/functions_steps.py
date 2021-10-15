import os


@given(u'the faas configuration \'{faas_configuration_file}\'')
def step_impl(context, faas_configuration_file):
    context.current_path_to_faas_configuration = os.path.join(
        context.path_to_serverless_functions, faas_configuration_file)


@given(u'the function \'{function_name}\'')
def step_impl(context, function_name):
    context.current_function = function_name


@given(u'it is pushed')
def step_impl(context):
    exit_code = context.faas_client.push(
        context.current_path_to_faas_configuration, context.current_function)
    assert exit_code == 0


@given(u'it is built')
def step_impl(context):
    exit_code = context.faas_client.build(
        context.current_path_to_faas_configuration, context.current_function)
    assert exit_code == 0


@when(u'I build it')
def step_impl(context):
    context.exit_code = context.faas_client.build(
        context.current_path_to_faas_configuration, context.current_function)


@when(u'I deploy it')
def step_impl(context):
    context.exit_code = context.faas_client.deploy(
        context.current_function)


@then(u'I get no error')
def step_impl(context):
    assert 0 == context.exit_code
