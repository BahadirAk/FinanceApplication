using System.Linq.Expressions;
using FinanceApplication.Core.Result;
using FinanceApplication.Entities.Concrete;
using FinanceApplication.Entities.Dto.Invoice;

namespace FinanceApplication.Business.Abstract;

public interface IInvoiceService
{
    IDataResult<bool> Add(List<AddInvoiceDto> addInvoiceDtos);
    IDataResult<List<InvoiceDto>> GetList(Expression<Func<Invoice, bool>> expression = null);
    IDataResult<Invoice> Get(Expression<Func<Invoice, bool>> expression);
    IDataResult<bool> Update(Invoice invoice);
}