namespace FinanceApplication.Core.Result;

public class SuccessDataResult<T> : DataResult<T>
{
    public SuccessDataResult(T data) : base(data, true)
    {
    }

    public SuccessDataResult(T data, string message,int httpStatusCode = 200) : base(true, message,httpStatusCode, data)
    {
    }
}