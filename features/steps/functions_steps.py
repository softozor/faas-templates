import json

import requests as requests
from behave import *
from softozor_test_utils import wait_until


@given(u'I am logged on the faas engine')
def step_impl(context):
    context.faas_client.login()


@given(u'the function \'{function_name}\'')
def step_impl(context, function_name):
    context.current_function = function_name


@given(u'it is pushed')
def step_impl(context):
    exit_code = context.faas_client.push(
        context.path_to_serverless_configuration, context.current_function)
    assert exit_code == 0


@given(u'it is built')
def step_impl(context):
    exit_code = context.faas_client.build(
        context.path_to_serverless_configuration, context.current_function)
    assert exit_code == 0


@given(u'it is up')
def step_impl(context):
    exit_code = context.faas_client.build(
        context.path_to_serverless_configuration, context.current_function)
    assert exit_code == 0
    exit_code = context.faas_client.push(
        context.path_to_serverless_configuration, context.current_function)
    assert exit_code == 0
    exit_code = context.faas_client.deploy(
        context.path_to_serverless_configuration, context.current_function)
    assert exit_code == 0


@when(u'I build it')
def step_impl(context):
    context.exit_code = context.faas_client.build(
        context.path_to_serverless_configuration, context.current_function)


@when(u'I deploy it')
def step_impl(context):
    context.exit_code = context.faas_client.deploy(
        context.path_to_serverless_configuration, context.current_function)


# testing if the function is ready is not enough, we really need to wait until
# we can really invoke it
def can_invoke_function(url, timeout_in_sec=120, period_in_sec=5):
    def invoke():
        response = requests.post(url)
        return response.status_code < 500

    try:
        wait_until(lambda: invoke(),
                   timeout_in_sec=timeout_in_sec, period_in_sec=period_in_sec)
        return True
    except TimeoutError:
        return False


@when(u'I invoke it with payload')
def step_impl(context):
    payload = json.loads(context.text)
    function_url = f'http://{context.faas_client.endpoint}/function/{context.current_function}'
    assert can_invoke_function(function_url)
    context.response = requests.post(function_url, json=payload)


@then(u'I get no error')
def step_impl(context):
    assert 0 == context.exit_code


@then(u'I get a success response')
def step_impl(context):
    assert context.response.status_code == 200, f'status code is {context.response.status_code}'


@then("I get a bad request")
def step_impl(context):
    assert context.response.status_code == 400, f'status code is {context.response.status_code}'


@then(u'the response payload')
def step_impl(context):
    expected_payload = json.loads(context.text)
    actual_payload = json.loads(context.response.text)
    assert expected_payload == actual_payload
