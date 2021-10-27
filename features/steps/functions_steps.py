import json

from behave import *


@given(u'the function \'{function_name}\'')
def step_impl(context, function_name):
    context.current_function = function_name


@given(u'she has pushed it')
def step_impl(context):
    exit_code = context.developer.push_function(context.current_function)
    assert exit_code == 0


@given(u'the developer has built it')
def step_impl(context):
    exit_code = context.developer.build_function(context.current_function)
    assert exit_code == 0


@given(u'the developer has put it up')
def step_impl(context):
    exit_code = context.developer.up_function(context.current_function)
    assert exit_code == 0


@when(u'the developer builds it')
def step_impl(context):
    context.exit_code = context.developer.build_function(context.current_function)


@when(u'she deploys it')
def step_impl(context):
    context.exit_code = context.developer.deploy_function(context.current_function)


@when(u'she invokes it with payload')
def step_impl(context):
    payload = json.loads(context.text)
    context.response = context.developer.invoke_function(
        context.current_function, payload)


@then(u'she gets no error')
def step_impl(context):
    assert 0 == context.exit_code


@then(u'she gets a success response')
def step_impl(context):
    assert context.response.status_code == 200, f'expected 200, got {context.response.status_code}'


@then("she gets a bad request")
def step_impl(context):
    assert context.response.status_code == 400, f'expected 400, got {context.response.status_code}'


@then(u'the response payload')
def step_impl(context):
    expected_payload = json.loads(context.text)
    actual_payload = json.loads(context.response.text)
    assert expected_payload == actual_payload, f'expected {expected_payload}, got {actual_payload}'


@then("she gets status code {status_code}")
def step_impl(context, status_code):
    assert context.response.status_code == status_code
