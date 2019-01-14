using AgentFramework.Core.Runtime;
using Autofac;

namespace Poc.Mobile.App.Services
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder
                .RegisterType<AgentContextService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultWalletRecordService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultWalletService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultPoolService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultConnectionService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultCredentialService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultProofService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultProvisioningService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultRouterService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DefaultMessageSerializer>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
