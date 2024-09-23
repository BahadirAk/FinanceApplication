namespace FinanceApplication.Core.Result;

public interface IDataResult<out T> : IResult
{
    T Data { get; }
}