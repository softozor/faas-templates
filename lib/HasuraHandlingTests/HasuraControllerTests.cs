using FluentAssertions;
using HasuraHandling;
using HasuraHandlingTests.Fixtures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CommonLibTests
{
  public class HasuraControllerTests
  {
    TestController _sut;

    [SetUp]
    public void Setup()
    {
      var logger = Mock.Of<ILogger>();
      _sut = new TestController(logger);
    }

    [TestCase(typeof(UnableToHandleException), typeof(UnauthorizedObjectResult), 401)]
    [TestCase(typeof(GraphqlException), typeof(ObjectResult), 500)]
    [TestCase(typeof(FormatException), typeof(BadRequestObjectResult), 400)]
    [TestCase(typeof(Exception), typeof(ObjectResult), 500)]
    public async Task ShouldReturnResponseCorrespondingToException(
      Type handlerExceptionType, Type expectedResponseType, int expectedStatusCode)
    {
      // Given the callback throws some exception
      Func<Task<IActionResult>> callback = () => throw (Exception)Activator.CreateInstance(handlerExceptionType);

      // When the controller handles this callback
      var response = await _sut.TestPost(callback);

      // Then we get the corresponding response
      response.Should().BeOfType(expectedResponseType);
      var actualStatusCode = expectedResponseType.GetProperty(nameof(ObjectResult.StatusCode)).GetValue(response);
      actualStatusCode.Should().Be(expectedStatusCode);
    }
  }
}
