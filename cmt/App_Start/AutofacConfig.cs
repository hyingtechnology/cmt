using Autofac;
using Autofac.Integration.Mvc;
using cmt.Models;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace cmt.App_Start
{
    public class AutofacConfig
    {
		public static void Register()
		{
			// 容器建立者
			ContainerBuilder builder = new ContainerBuilder();

			// 註冊Controllers
			builder.RegisterControllers(Assembly.GetExecutingAssembly());

			// 註冊Service
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
				   .Where(t => t.Name.EndsWith("Service"))
				   .AsImplementedInterfaces();

			// 註冊Repository
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
				   .Where(t => t.Name.StartsWith("EF") && t.Name.EndsWith("Repo"))
				   .AsImplementedInterfaces();

			// 註冊UnitOfWork
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
				   .Where(t => t.Name.StartsWith("EF") && t.Name.EndsWith("Uow"))
				   .AsImplementedInterfaces();

			// 註冊DbContext
			builder.RegisterType<cmtEntities>().InstancePerRequest();


			// 建立容器
			IContainer container = builder.Build();

			// 解析容器內的型別
			AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);




			// 建立相依解析器
			DependencyResolver.SetResolver(resolver);
		}
	}
}