Feature: .NET functions
  
  As a hasura API developer,
  I want to build .NET functions,
  so that I can benefit of the whole .NET ecosystem.
  
  Background: Faas configuration
    
    Given the faas configuration 'faas-dotnet.yml'
    And the function 'dotnet-fnc'
    
  Scenario: The function builds
    
    When I build it
    Then I get no error
    
  Scenario: The function gets deployed on the faas engine
    
    Given it is built
    And it is pushed
    When I deploy it
    Then I get no error

  @wip
  Scenario: The function returns the expected success response
    
  @wip
  Scenario: The function returns the expected failure response  