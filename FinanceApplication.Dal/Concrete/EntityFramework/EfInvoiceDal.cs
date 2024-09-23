using FinanceApplication.Core.EntityFramework;
using FinanceApplication.Dal.Abstract;
using FinanceApplication.Dal.Concrete.Context;
using FinanceApplication.Entities.Concrete;

namespace FinanceApplication.Dal.Concrete.EntityFramework;

public class EfInvoiceDal : EfEntityRepositoryBase<Invoice, FinanceAppDbContext>, IInvoiceDal
{
    
}