Feature: Nodejs functions
  
  As a hasura API developer,
  I want to build nodejs functions,
  so that I can benefit of the whole nodejs ecosystem.
  
  Background: Faas configuration
    
    Given the faas configuration 'faas-nodejs.yml'
    And the function 'nodejs-hasura-example-fnc'

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