using FinanceApplication.Business.Abstract;
using FinanceApplication.Business.Constants;
using FinanceApplication.Core.Result;
using FinanceApplication.Core.Security;
using FinanceApplication.Entities.Dto.Auth;
using FinanceApplication.Entities.Enums;

namespace FinanceApplication.Business.Concrete;

public class AuthManager : IAuthService
{
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserService _userService;

    public AuthManager(ITokenHelper tokenHelper, IUserService userService)
    {
        _tokenHelper = tokenHelper;
        _userService = userService;
    }

    public IDataResult<AccessToken> Login(LoginDto loginDto)
    {
        try
        {
            var userCheck = _userService.Get(u => u.TaxId == loginDto.TaxId && u.Status == (byte)StatusEnum.Active, true);
            if (userCheck == null) return new ErrorDataResult<AccessToken>(null, Messages.UnknownError);
            if (!userCheck.Success || userCheck.Data == null)
                return new ErrorDataResult<AccessToken>(null, Messages.DataNotFound);
            
            if (!HashingHelper.VerifyPasswordHash(loginDto.Password, userCheck.Data.PasswordSalt, userCheck.Data.PasswordHash))
            {
                return new ErrorDataResult<AccessToken>(null, Messages.UserLoginError);
            }
            
            var token = _tokenHelper.CreateToken(userCheck.Data);
            if (token == null) return new ErrorDataResult<AccessToken>(null, Messages.UnknownError);

            return new SuccessDataResult<AccessToken>(token);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<AccessToken>(null, ex.Message);
        }
    }
}