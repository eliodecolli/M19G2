using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using M19G2.Common;
using M19G2.DAL;
using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace M19G2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            var builder = new ContainerBuilder();

            builder.RegisterType<UnitOfWork>().InstancePerRequest();

            builder.Register(c => new Interceptor()).InstancePerRequest();

            builder.RegisterInstance<M19G2.SessionStorage.ISessionStorage>(new SessionStorage.UserSessionStorage()).SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.Load("M19G2.BLL"))
                .Where(t => t.Name.EndsWith("Service", StringComparison.InvariantCultureIgnoreCase))
                .AsImplementedInterfaces().EnableInterfaceInterceptors().InterceptedBy(typeof(Interceptor)).InstancePerRequest();

            /*builder.RegisterAssemblyTypes(Assembly.Load("M19G2.BLL"))
                .Where(t => t == typeof(M19G2.IBLL.IOrderQueueService)).AsImplementedInterfaces().SingleInstance();*/

            var instance = new BLL.OrderQueueService();
            var delivery = new BLL.DeliveryAutomation();
            builder.Register(c => instance).As<IBLL.IOrderQueueService>().SingleInstance();
            builder.Register(c => delivery).As<IBLL.IDeliveryAutomation>().SingleInstance();

            builder.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerDependency();

            builder.RegisterModelBinderProvider();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
