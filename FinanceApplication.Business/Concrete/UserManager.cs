using System.Linq.Expressions;
using FinanceApplication.Business.Abstract;
using FinanceApplication.Business.Constants;
using FinanceApplication.Core.Result;
using FinanceApplication.Core.Security;
using FinanceApplication.Dal.Abstract;
using FinanceApplication.Entities.Concrete;
using FinanceApplication.Entities.Dto.Notification;
using FinanceApplication.Entities.Dto.User;
using FinanceApplication.Entities.Enums;

namespace FinanceApplication.Business.Concrete;

public class UserManager : IUserService
    {
        private IUserDal _userDal;
        public UserManager(IUserDal usersDal)
        {
            _userDal = usersDal;
        }

        public IDataResult<bool> Add(UserCreateDto userCreateDto)
        {
            try
            {
                var user = _userDal.Get(u => u.TaxId == userCreateDto.TaxId && u.Status != (byte)StatusEnum.Deleted);
                if(user != null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataExists);
                }
                
                byte[] passwordsalt, passwordhash;
                HashingHelper.CreatePasswordHash(userCreateDto.Password, out passwordsalt, out passwordhash);
                
                _userDal.Add(new User
                {
                    Name = userCreateDto.Name,
                    TaxId = userCreateDto.TaxId,
                    PasswordSalt = passwordsalt,
                    PasswordHash = passwordhash,
                    RoleId = userCreateDto.RoleId
                });
                return new SuccessDataResult<bool>(true, Messages.Success);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<bool>(false, ex.Message);
            }
        }

        public IDataResult<bool> Delete(int id)
        {
            try
            {
                var user = _userDal.Get(u => u.Id == id && u.Status != (byte)StatusEnum.Deleted);
                if (user == null)
                {
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);
                }
                _userDal.Delete(user);
                return new SuccessDataResult<bool>(true, Messages.Success);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<bool>(false, ex.Message);
            }
        }

        public IDataResult<UserDto> Get(Expression<Func<User, bool>> expression, bool isAuth = false)
        {
            try
            {
                var user = _userDal.Get(expression);
                if (user == null)
                {
                    return new ErrorDataResult<UserDto>(null, Messages.DataNotFound);
                }
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    TaxId = user.TaxId,
                    Status = user.Status,
                    RoleId = user.RoleId,
                    CreatedDate = user.CreatedDate,
                    UpdatedDate = user.UpdatedDate,
                    DeletedDate = user.DeletedDate
                };

                if (isAuth)
                {
                    userDto.PasswordSalt = user.PasswordSalt;
                    userDto.PasswordHash = user.PasswordHash;
                }
                
                return new SuccessDataResult<UserDto>(userDto, Messages.Success);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserDto>(null, ex.Message);
            }
        }

        public IDataResult<List<UserDto>> GetList(Expression<Func<User, bool>> expression = null)
        {
            try
            {
                var userList = _userDal.GetList(expression);

                List<UserDto> userListDto = new();
                foreach (var user in userList)
                {
                    userListDto.Add(new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        TaxId = user.TaxId,
                        Status = user.Status,
                        RoleId = user.RoleId,
                        CreatedDate = user.CreatedDate,
                        UpdatedDate = user.UpdatedDate,
                        DeletedDate = user.DeletedDate
                    });
                }
                return new SuccessDataResult<List<UserDto>>(userListDto, Messages.Success);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserDto>>(new List<UserDto>(), ex.Message);
            }
        }

        public IDataResult<List<NotificationDto>> GetUserNotifications()
        {
            try
            {
                var userId = UserIdentityHelper.GetUserId();

                var user = _userDal.Get(u => u.Id == userId, "Notifications");
                if (user == null) return new ErrorDataResult<List<NotificationDto>>(new(), Messages.DataNotFound);

                var notifications = new List<NotificationDto>();
                foreach (var notification in user.Notifications)
                {
                    notifications.Add(new NotificationDto
                    {
                        Id = notification.Id,
                        UserId = notification.UserId,
                        Message = notification.Message,
                        CreatedDate = notification.CreatedDate,
                        UpdatedDate = notification.UpdatedDate,
                        DeletedDate = notification.DeletedDate,
                        Status = notification.Status
                    });
                }
                return new SuccessDataResult<List<NotificationDto>>(notifications, Messages.Success);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<NotificationDto>>(new(), ex.Message);
            }
        }
    }