import json

from behave import given, when, then


@given("the function '{function_name}'")
def step_impl(context, function_name):
    context.current_function = function_name


@given("the developer has put it up")
def step_impl(context):
    context.developer.up_function(context.current_function)


@when("she invokes it with payload")
def step_impl(context):
    payload = json.loads(context.text)
    context.response = context.developer.invoke_function(
        context.current_function, payload
    )


@then("she gets a success response")
def step_impl(context):
    assert (
        context.response.status_code == 200
    ), f"expected 200, got {context.response.status_code}"


@then("the response payload")
def step_impl(context):
    expected_payload = json.loads(context.text)
    actual_payload = json.loads(context.response.text)
    assert (
        expected_payload == actual_payload
    ), f"expected {expected_payload}, got {actual_payload}"


@then("she gets status code {status_code:d}")
def step_impl(context, status_code):
    assert (
        context.response.status_code == status_code
    ), f"expected {status_code}, got {context.response.status_code}"


@then("error message")
def step_impl(context):
    actual_error_message = json.loads(context.response.text)["message"]
    expected_error_message = context.text
    assert (
        expected_error_message == actual_error_message
    ), f"expected {expected_error_message}, got {actual_error_message}"
