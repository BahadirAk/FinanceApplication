namespace FinanceApplication.Core.Result;

public class ErrorDataResult<T> : DataResult<T>
{
    public ErrorDataResult(T data) : base(data, false)
    {
    }

    public ErrorDataResult(T data, string message,int httpStatusCode = 400) : base(false, message,httpStatusCode, data)
    {
    }
}