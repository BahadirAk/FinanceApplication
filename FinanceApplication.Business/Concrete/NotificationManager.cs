using System.Linq.Expressions;
using FinanceApplication.Business.Abstract;
using FinanceApplication.Business.Constants;
using FinanceApplication.Core.Result;
using FinanceApplication.Core.Security;
using FinanceApplication.Dal.Abstract;
using FinanceApplication.Entities.Concrete;
using FinanceApplication.Entities.Dto.Notification;

namespace FinanceApplication.Business.Concrete;

public class NotificationManager : INotificationService
{
    private readonly INotificationDal _notificationDal;

    public NotificationManager(INotificationDal notificationDal)
    {
        _notificationDal = notificationDal;
    }

    public IDataResult<bool> Add(AddNotificationDto addNotificationDto)
    {
        try
        {
            _notificationDal.Add(new Notification
            {
                UserId = addNotificationDto.UserId,
                Message = addNotificationDto.Message
            });
            return new SuccessDataResult<bool>(true, Messages.Success);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<bool>(false, ex.Message);
        }
    }

    // public IDataResult<List<NotificationDto>> GetList(Expression<Func<Notification, bool>> expression = null)
    // {
    //     try
    //     {
    //         var userId = UserIdentityHelper.GetUserId();
    //
    //         var notificationList = _notificationDal.GetList(n => n.UserId == userId);
    //     }
    //     catch (Exception ex)
    //     {
    //         return new ErrorDataResult<List<NotificationDto>>(new(), ex.Message);
    //     }
    // }
}