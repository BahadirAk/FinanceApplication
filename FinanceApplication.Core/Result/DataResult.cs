namespace FinanceApplication.Core.Result;

public class DataResult<T> : Result, IDataResult<T>
{
    public DataResult(bool success, string message,int httpStatusCode, T data) : base(success, message,httpStatusCode)
    {
        Data = data;

    }

    public DataResult(T data, bool success) : base(success)
    {
        Data = data;
    }

    public T Data { get; }

}