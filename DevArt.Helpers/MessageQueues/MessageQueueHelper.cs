using Azure;
using DevArt.Helpers.MessageQueues.Configurations;
using DevArt.Helpers.MessageQueues.Constants;
using DevArt.Helpers.MessageQueues.Exceptions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevArt.Helpers.MessageQueues;

public static class MessageQueueHelper
{
    public static void AddMessageQueue(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        Type consumersContainingAssemblyType)
    {
        var queueOptions = configuration
            .GetSection(nameof(QueueOptions))
            .Get<QueueOptions>();

        if (queueOptions is null)
        {
            throw new MessageQueueException("Queue options is not configured.");
        }

        if (!Uri.TryCreate(queueOptions.Host, UriKind.Absolute, out var hostUri))
        {
            throw new MessageQueueException("Queue host is not in a correct format.");
        }

        switch (hostUri.Scheme)
        {
            case QueueProviderSchemes.RabbitMq or QueueProviderSchemes.RabbitMqTls:
            {
                AddRabbitMq(serviceCollection, queueOptions, hostUri, consumersContainingAssemblyType);
                break;
            }
            case QueueProviderSchemes.AzureServiceBus:
            {
                AddServiceBus(serviceCollection, queueOptions, hostUri, consumersContainingAssemblyType);
                break;
            }
            default:
            {
                throw new MessageQueueException($"Scheme {hostUri.Scheme} is not recognized. Consider adding it to {nameof(QueueProviderSchemes)}. Or use a different scheme.");
            }
        }
    }

    private static void AddRabbitMq(
        this IServiceCollection serviceCollection,
        QueueOptions queueOptions,
        Uri hostUri,
        Type consumersContainingAssemblyType)
    {
        serviceCollection.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(hostUri, hostConfigurator =>
                {
                    hostConfigurator.Username(queueOptions.Username);
                    hostConfigurator.Password(queueOptions.Password);
                });

                factoryConfigurator.ReceiveEndpoint(queueOptions.QueueName, endpointConfigurator =>
                {
                    endpointConfigurator.DeadLetterExchange = $"{queueOptions.QueueName}_dead_letter";

                    endpointConfigurator.ConfigureConsumers(context);
                });
            });

            configurator.AddConsumers(consumersContainingAssemblyType.Assembly);
        });
    }

    private static void AddServiceBus(
        this IServiceCollection serviceCollection,
        QueueOptions queueOptions,
        Uri hostUri,
        Type consumersContainingAssemblyType)
    {
        serviceCollection.AddMassTransit(configurator =>
        {
            configurator.UsingAzureServiceBus((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(hostUri, hostConfigurator =>
                {
                    hostConfigurator.NamedKeyCredential = new AzureNamedKeyCredential
                    (
                        queueOptions.Username,
                        queueOptions.Password
                    );
                });

                factoryConfigurator.ReceiveEndpoint(queueOptions.QueueName, endpointConfigurator =>
                {
                    endpointConfigurator.EnableDeadLetteringOnMessageExpiration = true;

                    endpointConfigurator.ConfigureConsumers(context);
                });
            });

            configurator.AddConsumers(consumersContainingAssemblyType.Assembly);
        });
    }
}
