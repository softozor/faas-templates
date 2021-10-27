Feature: Faas templates
  
  As a hasura API developer,
  I want to use faas templates specific for hasura,
  so that I can reduce the development overhead.
  
  Scenario Outline: The function builds

    Given the function '<function name>'
    When I build it
    Then I get no error

    Examples:
      | function name |
      | hasura-dotnet |
      | hasura-nodejs |
    
  Scenario Outline: The function gets deployed on the faas engine
    
    Given the function '<function name>'
    And it is built
    And it is pushed
    When I deploy it
    Then I get no error
    
    Examples:
      | function name |
      | hasura-dotnet |
      | hasura-nodejs |
    
  @current  
  Scenario Outline: The function returns the expected success response
    
    Given the function '<function name>'
    And it is up
    When I invoke it with payload
    """
    {
      "input": {
        "value": 10
      }
    }
    """
    Then I get a success response
    And the response payload
    """
    {
      "value": 10
    }
    """
    
    Examples:
      | function name |
      | hasura-dotnet |
      | hasura-nodejs |
    
  Scenario Outline: The function returns the expected failure response
    
    Given the function '<function name>'
    And it is up
    When I invoke it with payload
    """
    {
      "input": {
        "unsupportedProperty": 10
      }
    }
    """
    Then I get a bad request
    And the response payload
    """
    {
      "message": "input has no 'value' property"
    }
    """
    
    Examples:
      | function name |
#      | hasura-dotnet |
      | hasura-nodejs |
