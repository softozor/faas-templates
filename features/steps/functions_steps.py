import os

import requests as requests
from behave import *

@given(u'I am logged on the faas engine')
def step_impl(context):
    context.faas_client.login()


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


@given("it is up")
def step_impl(context):
    exit_code = context.faas_client.up(
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


@when("I invoke it with payload")
def step_impl(context):
    payload = context.text
    context.response = requests.post(
        f'http://{context.faas_client.endpoint}/{context.current_function}', data=payload)
    print('status code = ', context.response.status_code)


@then(u'I get no error')
def step_impl(context):
    assert 0 == context.exit_code


@then("I get a success response")
def step_impl(context):
    assert context.response.status_code == 200


@step("the response payload")
def step_impl(context):
    expected_payload = context.text
    actual_payload = context.response.json()
    print('expected payload = ', expected_payload)
    print('actual payload   = ', actual_payload)
    assert expected_payload == actual_payload
