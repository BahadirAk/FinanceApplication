using Autofac;
using FinanceApplication.Business.Abstract;
using FinanceApplication.Business.Concrete;
using FinanceApplication.Core.Security;
using FinanceApplication.Dal.Abstract;
using FinanceApplication.Dal.Concrete.EntityFramework;

namespace FinanceApplication.Business.DependencyResolvers.Autofac;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserManager>().As<IUserService>();
        builder.RegisterType<EfUserDal>().As<IUserDal>();
            
        builder.RegisterType<AuthManager>().As<IAuthService>();
        builder.RegisterType<JwtHelper>().As<ITokenHelper>();

        builder.RegisterType<InvoiceManager>().As<IInvoiceService>();
        builder.RegisterType<EfInvoiceDal>().As<IInvoiceDal>();

        builder.RegisterType<NotificationManager>().As<INotificationService>();
        builder.RegisterType<EfNotificationDal>().As<INotificationDal>();
    }
}