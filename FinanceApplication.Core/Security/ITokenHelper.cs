using FinanceApplication.Core.Result;
using FinanceApplication.Entities.Dto.User;

namespace FinanceApplication.Core.Security;

public interface ITokenHelper
{
    AccessToken CreateToken(UserDto user);
    IDataResult<TokenInfo> GetTokenInfo(string token);
}