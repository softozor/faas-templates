Feature: Faas templates
  
  As a hasura API developer,
  I want to use faas templates specific for hasura,
  so that I can reduce the development overhead.
  
  Scenario Outline: The function builds

    Given the function '<function name>'
    When the developer builds it
    Then she gets no error

    Examples:
      | function name |
      | hasura-dotnet |
      | hasura-nodejs |
    
  Scenario Outline: The function gets deployed on the faas engine
    
    Given the function '<function name>'
    And the developer has built it
    And she has pushed it
    When she deploys it
    Then she gets no error
    
    Examples:
      | function name |
      | hasura-dotnet |
      | hasura-nodejs |
    
  Scenario Outline: The function returns the expected success response
    
    A function is up after it has been built, pushed, and deployed  
    to the faas engine.
    
    Given the function '<function name>'
    And the developer has put it up
    When she invokes it with payload
    """
    {
      "input": {
        "value": 10
      }
    }
    """
    Then she gets a success response
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
    
    A function is up after it has been built, pushed, and deployed  
    to the faas engine.
    
    The functions have currently discrepant behaviors in case of 
    failure. This will have to change at some point (cf. [this issue](https://gitlab.hidora.com/softozor/faas-templates/-/issues/4)).
    
    Given the function '<function name>'
    And the developer has put it up
    When she invokes it with payload
    """
    {
      "input": {
        "unsupportedProperty": 10
      }
    }
    """
    Then she gets status code <status code>
    
    Examples:
      | function name | status code |
      | hasura-dotnet | 500         |
      | hasura-nodejs | 400         |
