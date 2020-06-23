using ESSPREPO.Generic;
using ESSPCORE.EF;
using System.Data.Entity;
using Autofac;

namespace AppESSP.Modules
{
    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());

            builder.RegisterType(typeof(ABESSPEntities)).As(typeof(DbContext)).InstancePerRequest();
            //builder.RegisterType(typeof(BTRMSEntities)).As(typeof(DbContext)).InstancePerRequest();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();

        }

    }
}