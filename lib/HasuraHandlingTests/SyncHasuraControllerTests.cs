namespace Softozor.HasuraHandlingTests
{
  using FluentAssertions;
  using Softozor.HasuraHandling;
  using Softozor.HasuraHandlingTests.Fixtures;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;
  using Moq;
  using NUnit.Framework;
  using System;
  
  public class SyncHasuraControllerTests
  {
    SyncTestController _sut;

    [SetUp]
    public void Setup()
    {
      var logger = Mock.Of<ILogger>();
      _sut = new SyncTestController(logger);
    }

    [TestCase(typeof(UnableToHandleException), typeof(UnauthorizedObjectResult), 401)]
    [TestCase(typeof(GraphqlException), typeof(ObjectResult), 500)]
    [TestCase(typeof(FormatException), typeof(BadRequestObjectResult), 400)]
    [TestCase(typeof(Exception), typeof(ObjectResult), 500)]
    public void ShouldReturnResponseCorrespondingToException(
      Type handlerExceptionType, Type expectedResponseType, int expectedStatusCode)
    {
      // Given the callback throws some exception
      Func<IActionResult> callback = () => throw (Exception)Activator.CreateInstance(handlerExceptionType);

      // When the controller handles this callback
      var response = _sut.TestPost(callback);

      // Then we get the corresponding response
      response.Should().BeOfType(expectedResponseType);
      var actualStatusCode = expectedResponseType.GetProperty(nameof(ObjectResult.StatusCode)).GetValue(response);
      actualStatusCode.Should().Be(expectedStatusCode);
    }
  }
}
