using System.Linq;
using System.Reflection;
using Autofac;
using ESSPSERVICE.Generic;

namespace AppESSP.Modules
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            //builder.RegisterAssemblyTypes(Assembly.Load("SERPSERVICES"))

            //          .Where(t => t.Name.EndsWith("Service"))

            //          .AsImplementedInterfaces()

            //          .InstancePerRequest();
            builder.RegisterAssemblyTypes(Assembly.Load("ESSPSERVICE")).Where(t => t.IsClass && t.Name.EndsWith("Service"))
            .As(t => t.GetInterfaces().Single(i => i.Name.EndsWith(t.Name)));
            builder.RegisterGeneric(typeof(EntityService<>)).As(typeof(IEntityService<>)).InstancePerDependency();

        }
    }
}