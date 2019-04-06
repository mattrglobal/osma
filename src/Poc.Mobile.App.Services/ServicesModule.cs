using System.Net.Http;
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
                .RegisterType<HttpClientHandler>()
                .As<HttpMessageHandler>();

            builder
                .RegisterType<EventAggregator>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<AgentContextProvider>()
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
                .RegisterType<DefaultMessageService>()
                .AsImplementedInterfaces()
                .SingleInstance();
            
            builder
                .RegisterType<DefaultLedgerService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DefaultSchemaService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DefaultTailsService>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
