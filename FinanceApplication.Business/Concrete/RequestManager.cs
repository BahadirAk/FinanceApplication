using System.Linq.Expressions;
using FinanceApplication.Business.Abstract;
using FinanceApplication.Business.Constants;
using FinanceApplication.Core.Result;
using FinanceApplication.Core.Security;
using FinanceApplication.Dal.Abstract;
using FinanceApplication.Entities.Concrete;
using FinanceApplication.Entities.Dto.Notification;
using FinanceApplication.Entities.Dto.Request;
using FinanceApplication.Entities.Enums;

namespace FinanceApplication.Business.Concrete;

public class RequestManager : IRequestService
{
    private readonly IRequestDal _requestDal;
    private readonly INotificationService _notificationService;
    private readonly IInvoiceService _invoiceService;
    private readonly IUserService _userService;

    public RequestManager(IRequestDal requestDal, INotificationService notificationService, IInvoiceService invoiceService, IUserService userService)
    {
        _requestDal = requestDal;
        _notificationService = notificationService;
        _invoiceService = invoiceService;
        _userService = userService;
    }

    public IDataResult<bool> AddRequest(AddRequestDto addRequestDto)
    {
        try
        {
            var taxId = UserIdentityHelper.GetUserTaxId();
            
            var request = _requestDal.Get(r =>
                r.InvoiceNumber == addRequestDto.InvoiceNumber && r.Status == (byte)StatusEnum.Active);
            if (request != null)
            {
                if (request.RequestStatus == (byte)RequestStatusEnum.IsWaiting)
                    return new ErrorDataResult<bool>(false, Messages.IsWaitingRequest.Replace("{0}", request.InvoiceNumber));
                else if (request.RequestStatus == (byte)RequestStatusEnum.Approved)
                    return new ErrorDataResult<bool>(false, Messages.ApprovedRequest.Replace("{0}", request.InvoiceNumber));
            }
            
            _requestDal.Add(new Request
            {
                InvoiceNumber = addRequestDto.InvoiceNumber,
                SupplierTaxId = taxId,
                RequestStatus = (byte)RequestStatusEnum.IsWaiting
            });

            var invoice = _invoiceService.Get(i =>
                i.InvoiceNumber == addRequestDto.InvoiceNumber && i.InvoiceStatus == (byte)InvoiceStatusEnum.New &&
                i.Status == (byte)StatusEnum.Active);
            if (invoice == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
            if (!invoice.Success || invoice.Data == null)
                return new ErrorDataResult<bool>(false, invoice.Message);

            invoice.Data.InvoiceStatus = (byte)InvoiceStatusEnum.Used;
            var updateResult = _invoiceService.Update(invoice.Data);
            if (updateResult == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
            if (!updateResult.Success || !updateResult.Data)
                return new ErrorDataResult<bool>(false, updateResult.Message);

            var buyer = _userService.Get(u =>
                u.TaxId == invoice.Data.BuyerTaxId && u.Status == (byte)StatusEnum.Active);
            if (buyer == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
            if (!buyer.Success || buyer.Data == null) return new ErrorDataResult<bool>(false, buyer.Message);
            
            var buyerNotification = _notificationService.Add(new AddNotificationDto
            {
                UserId = buyer.Data.Id,
                Message = Messages.BuyerRequest.Replace("{0}", invoice.Data.InvoiceNumber)
            });
            if (buyerNotification == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
            if (!buyerNotification.Success || !buyerNotification.Data)
                return new ErrorDataResult<bool>(false, buyerNotification.Message);

            return new SuccessDataResult<bool>(true, Messages.Success);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<bool>(false, ex.Message);
        }
    }

    public IDataResult<List<RequestDto>> GetList(Expression<Func<Request, bool>> expression = null)
    {
        try
        {
            var requests = _requestDal.GetList(r =>
                r.RequestStatus == (byte)RequestStatusEnum.IsWaiting && r.Status == (byte)StatusEnum.Active);

            var requestList = new List<RequestDto>();
            foreach (var request in requests)
            {
                requestList.Add(new RequestDto
                {
                    Id = request.Id,
                    InvoiceNumber = request.InvoiceNumber,
                    SupplierTaxId = request.SupplierTaxId,
                    RequestStatus = request.RequestStatus,
                    CreatedDate = request.CreatedDate,
                    UpdatedDate = request.UpdatedDate,
                    DeletedDate = request.DeletedDate,
                    Status = request.Status
                });
            }

            return new SuccessDataResult<List<RequestDto>>(requestList, Messages.Success);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<List<RequestDto>>(new(), ex.Message);
        }
    }

    public IDataResult<bool> ApproveRequest(int id)
    {
        try
        {
            var request = _requestDal.Get(r =>
                r.Id == id && r.RequestStatus == (byte)RequestStatusEnum.IsWaiting &&
                r.Status == (byte)StatusEnum.Active);
            if (request == null) return new ErrorDataResult<bool>(false, Messages.DataNotFound);

            request.RequestStatus = (byte)RequestStatusEnum.Approved;
            _requestDal.Update(request);

            var supplier =
                _userService.Get(u => u.TaxId == request.SupplierTaxId && u.Status == (byte)StatusEnum.Active);
            if (supplier == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
            if (!supplier.Success || supplier.Data == null) return new ErrorDataResult<bool>(false, supplier.Message);

            var supplierNotification = _notificationService.Add(new AddNotificationDto
            {
                UserId = supplier.Data.Id,
                Message = Messages.ApprovedRequestSupplier.Replace("{0}", request.InvoiceNumber)
            });
            if (supplierNotification == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
            if (!supplierNotification.Success || !supplierNotification.Data)
                return new ErrorDataResult<bool>(false, supplierNotification.Message);

            var invoice = _invoiceService.Get(i =>
                i.InvoiceNumber == request.InvoiceNumber && i.InvoiceStatus == (byte)InvoiceStatusEnum.Used &&
                i.Status == (byte)StatusEnum.Active);
            if (invoice == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
            if (!invoice.Success || invoice.Data == null) return new ErrorDataResult<bool>(false, invoice.Message);

            invoice.Data.InvoiceStatus = (byte)InvoiceStatusEnum.Paid;
            var updateResult = _invoiceService.Update(invoice.Data);
            if (updateResult == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
            if (!updateResult.Success || !updateResult.Data)
                return new ErrorDataResult<bool>(false, updateResult.Message);

            var buyer = _userService.Get(u =>
                u.TaxId == invoice.Data.BuyerTaxId && u.Status == (byte)StatusEnum.Active);
            if (buyer == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
            if (!buyer.Success || buyer.Data == null) return new ErrorDataResult<bool>(false, buyer.Message);

            var buyerNotification = _notificationService.Add(new AddNotificationDto
            {
                UserId = buyer.Data.Id,
                Message = Messages.ApprovedRequestBuyer.Replace("{0}", invoice.Data.InvoiceNumber)
            });
            if (buyerNotification == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
            if (!buyerNotification.Success || !buyerNotification.Data)
                return new ErrorDataResult<bool>(false, buyerNotification.Message);

            return new SuccessDataResult<bool>(true, Messages.Success);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<bool>(false, ex.Message);
        }
    }
}