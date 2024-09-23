using System.Net;

namespace FinanceApplication.Core.Result;

public class Result : IResult
{
    public Result(bool success, string message,int httpStatusCode) : this(success)
    {
        Message = message;
        HttpStatusCode = httpStatusCode;
    }

    public Result(bool success)
    {
        Success = success;
    }

    public bool Success { get; }

    public string Message { get; }
    public int HttpStatusCode { get; }
        
}