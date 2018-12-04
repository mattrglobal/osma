using Autofac;
using Poc.Mobile.App.Services;

namespace Poc.Mobile.App.iOS
{
    public class PlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new ServicesModule());
        }
    }
}