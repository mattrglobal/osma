using System.Net.Http;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;


namespace Osma.Mobile.App.Services
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var services = new ServiceCollection();
            services.AddAriesFramework();
            builder.Populate(services);

            builder.RegisterType<AgentContextProvider>()
                .AsImplementedInterfaces()
                .SingleInstance();


        }
    }
}
