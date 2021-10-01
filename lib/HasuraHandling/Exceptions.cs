namespace Softozor.HasuraHandling
{
  using GraphQL;
  using System;
  
  [Serializable]
  public class UnableToHandleException : Exception
  {
    public int StatusCode { get; set; }

    public UnableToHandleException() { }
    public UnableToHandleException(string message) : base(message) { }
    public UnableToHandleException(string message, Exception inner) : base(message, inner) { }
    protected UnableToHandleException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }

  [Serializable]
  public class GraphqlException : Exception
  {
    public GraphqlException() { }
    public GraphqlException(string message) : base(message) { }
    public GraphqlException(string message, GraphQLError[] errors) : base(message) { Errors = errors; }
    public GraphqlException(string message, Exception inner) : base(message, inner) { }
    public GraphqlException(string message, GraphQLError[] errors, Exception inner) : base(message, inner) { Errors = errors; }
    protected GraphqlException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    public GraphQLError[] Errors { get; private set; }
  }
}
