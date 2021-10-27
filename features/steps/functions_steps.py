import json

from behave import *


@given(u'I am logged on the faas engine')
def step_impl(context):
    context.faas_client.login()


@given(u'the function \'{function_name}\'')
def step_impl(context, function_name):
    context.current_function = function_name


@given(u'it is pushed')
def step_impl(context):
    exit_code = context.developer.push_function(context.current_function)
    assert exit_code == 0


@given(u'it is built')
def step_impl(context):
    exit_code = context.developer.build_function(context.current_function)
    assert exit_code == 0


@given(u'it is up')
def step_impl(context):
    exit_code = context.developer.up_function(context.current_function)
    assert exit_code == 0


@when(u'I build it')
def step_impl(context):
    context.exit_code = context.developer.build_function(context.current_function)


@when(u'I deploy it')
def step_impl(context):
    context.exit_code = context.developer.deploy_function(context.current_function)


@when(u'I invoke it with payload')
def step_impl(context):
    payload = json.loads(context.text)
    context.response = context.developer.invoke_function(
        context.current_function, payload)


@then(u'I get no error')
def step_impl(context):
    assert 0 == context.exit_code


@then(u'I get a success response')
def step_impl(context):
    assert context.response.status_code == 200, f'expected 200, got {context.response.status_code}'


@then("I get a bad request")
def step_impl(context):
    assert context.response.status_code == 400, f'expected 400, got {context.response.status_code}'


@then(u'the response payload')
def step_impl(context):
    expected_payload = json.loads(context.text)
    actual_payload = json.loads(context.response.text)
    assert expected_payload == actual_payload, f'expected {expected_payload}, got {actual_payload}'
