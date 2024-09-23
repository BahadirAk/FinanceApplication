using FinanceApplication.Core.Result;
using FinanceApplication.Core.Security;
using FinanceApplication.Entities.Dto.Auth;

namespace FinanceApplication.Business.Abstract;

public interface IAuthService
{
    IDataResult<AccessToken> Login(LoginDto loginDto);
}