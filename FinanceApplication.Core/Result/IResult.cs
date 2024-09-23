namespace FinanceApplication.Core.Result;

public interface IResult
{
    bool Success { get; }

    string Message { get; }
    int HttpStatusCode { get; }
}