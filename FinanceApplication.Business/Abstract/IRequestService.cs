using System.Linq.Expressions;
using FinanceApplication.Core.Result;
using FinanceApplication.Entities.Concrete;
using FinanceApplication.Entities.Dto.Request;

namespace FinanceApplication.Business.Abstract;

public interface IRequestService
{
    IDataResult<bool> AddRequest(AddRequestDto addRequestDto);
    IDataResult<List<RequestDto>> GetList(Expression<Func<Request, bool>> expression = null);
    IDataResult<bool> ApproveRequest(int id);
}