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

    The sample functions are implemented in such a way that values
    smaller than 100 lead to a success response.

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

    The sample functions are implemented in such a way that a value
    larger than 100 results in an error.

    Given the function '<function name>'
    And the developer has put it up
    When she invokes it with payload
    """
    {
      "input": {
        "value": 110
      }
    }
    """
    Then she gets status code 400
    And error message
    """
    too high value 110
    """

    Examples:
      | function name | status code |
      | hasura-dotnet | 400         |
      | hasura-nodejs | 400         |
