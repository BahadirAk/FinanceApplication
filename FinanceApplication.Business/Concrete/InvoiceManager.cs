using System.Linq.Expressions;
using FinanceApplication.Business.Abstract;
using FinanceApplication.Business.Constants;
using FinanceApplication.Core.Result;
using FinanceApplication.Core.Security;
using FinanceApplication.Dal.Abstract;
using FinanceApplication.Entities.Concrete;
using FinanceApplication.Entities.Dto.Invoice;
using FinanceApplication.Entities.Dto.Notification;
using FinanceApplication.Entities.Enums;

namespace FinanceApplication.Business.Concrete;

public class InvoiceManager : IInvoiceService
{
    private readonly IInvoiceDal _invoiceDal;
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;

    public InvoiceManager(IInvoiceDal invoiceDal, IUserService userService, INotificationService notificationService)
    {
        _invoiceDal = invoiceDal;
        _userService = userService;
        _notificationService = notificationService;
    }

    public IDataResult<bool> Add(List<AddInvoiceDto> addInvoiceDtos)
    {
        try
        {
            var invoiceCheck =
                addInvoiceDtos.Where(x => _invoiceDal.Get(i => i.InvoiceNumber == x.InvoiceNumber) != null).ToList();

            var supplierCheck = addInvoiceDtos
                .Where(x => _userService.Get(u => u.TaxId == x.SupplierTaxId).Data == null).ToList();

            var taxId = UserIdentityHelper.GetUserTaxId();

            var notifications = new Dictionary<int, List<string>>();
            foreach (var addInvoiceDto in addInvoiceDtos.Except(invoiceCheck).Except(supplierCheck))
            {
                _invoiceDal.Add(new Invoice
                {
                    InvoiceNumber = addInvoiceDto.InvoiceNumber,
                    TermDate = addInvoiceDto.TermDate,
                    BuyerTaxId = taxId,
                    SupplierTaxId = addInvoiceDto.SupplierTaxId,
                    InvoiceCost = addInvoiceDto.InvoiceCost,
                    InvoiceStatus = (byte)InvoiceStatusEnum.New
                });
                
                var supplier = _userService.Get(u => u.TaxId == addInvoiceDto.SupplierTaxId);
                if (supplier == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
                if (!supplier.Success || supplier.Data == null)
                    return new ErrorDataResult<bool>(false, Messages.DataNotFound);

                if (notifications.ContainsKey(supplier.Data.Id))
                    notifications[supplier.Data.Id].Add(addInvoiceDto.InvoiceNumber);
                else notifications[supplier.Data.Id] = new List<string> { addInvoiceDto.InvoiceNumber };
            }

            foreach (var supplier in notifications.Keys)
            {
                var notificationResult = _notificationService.Add(new AddNotificationDto
                {
                    UserId = supplier,
                    Message = $"{taxId} alıcı tarafından {string.Join(",", notifications[supplier])} numaralı fatura/faturalar tarafınıza iletilmiştir."
                });
                if (notificationResult == null) return new ErrorDataResult<bool>(false, Messages.UnknownError);
                if (!notificationResult.Success || !notificationResult.Data)
                    return new ErrorDataResult<bool>(false, Messages.AddFailed);
            }
            
            var message = "";
            if (invoiceCheck.Any())
            {
                message += Messages.InvoiceSuccess.Replace("{0}", string.Join(",", invoiceCheck.Select(i => i.InvoiceNumber)));
            }
            if (supplierCheck.Any())
            {
                message += Messages.InvoiceSupplierSuccess.Replace("{0}", string.Join(",", supplierCheck.Select(i => i.SupplierTaxId)));
            }
            return new SuccessDataResult<bool>(true, !string.IsNullOrEmpty(message) ? message : Messages.Success);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<bool>(false, ex.Message);
        }
    }

    public IDataResult<List<InvoiceDto>> GetList(Expression<Func<Invoice, bool>> expression = null)
    {
        throw new NotImplementedException();
    }
}