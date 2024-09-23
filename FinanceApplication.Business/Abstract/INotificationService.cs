using System.Linq.Expressions;
using FinanceApplication.Core.Result;
using FinanceApplication.Entities.Concrete;
using FinanceApplication.Entities.Dto.Notification;

namespace FinanceApplication.Business.Abstract;

public interface INotificationService
{
    IDataResult<bool> Add(AddNotificationDto addNotificationDto);
    // IDataResult<List<NotificationDto>> GetList(Expression<Func<Notification, bool>> expression = null);
}