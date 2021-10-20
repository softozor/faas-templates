Feature: .NET functions
  
  As a hasura API developer,
  I want to build .NET functions,
  so that I can benefit of the whole .NET ecosystem.
  
  # TODO: write one single feature file with scenario outlines
  # TODO: try to login upon faas client creation
  
  Background: Faas configuration
    
    Given the faas configuration 'faas-dotnet.yml'
    And the function 'hasura-dotnet'
    
  Scenario: The function builds
    
    When I build it
    Then I get no error
    
  Scenario: The function gets deployed on the faas engine
    
    Given I am logged on the faas engine
    And it is built
    And it is pushed
    When I deploy it
    Then I get no error
    
  Scenario: The function returns the expected success response
    
    Given I am logged on the faas engine
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
    
  @wip
  Scenario: The function returns the expected failure response  
