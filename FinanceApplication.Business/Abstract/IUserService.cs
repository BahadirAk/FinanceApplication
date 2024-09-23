using System.Linq.Expressions;
using FinanceApplication.Core.Result;
using FinanceApplication.Entities.Concrete;
using FinanceApplication.Entities.Dto.Notification;
using FinanceApplication.Entities.Dto.User;

namespace FinanceApplication.Business.Abstract;

public interface IUserService
{
    IDataResult<UserDto> Get(Expression<Func<User, bool>> expression, bool isAuth = false);
    IDataResult<List<UserDto>> GetList(Expression<Func<User, bool>> expression = null);
    IDataResult<bool> Add(UserCreateDto userCreateDto);
    IDataResult<List<NotificationDto>> GetUserNotifications();
}