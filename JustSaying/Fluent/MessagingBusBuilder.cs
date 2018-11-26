using System;
using JustSaying.AwsTools;
using JustSaying.AwsTools.QueueCreation;
using JustSaying.Messaging.MessageHandling;
using JustSaying.Messaging.MessageSerialization;
using JustSaying.Messaging.Monitoring;
using Microsoft.Extensions.Logging;

namespace JustSaying.Fluent
{
    /// <summary>
    /// A class representing a builder for an <see cref="IMessagingBus"/>.
    /// </summary>
    public class MessagingBusBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceResolver"/> to use.
        /// </summary>
        internal IServiceResolver ServiceResolver { get; private set; } = new DefaultServiceResolver();

        /// <summary>
        /// Gets or sets the builder to use for creating an AWS client factory.
        /// </summary>
        private AwsClientFactoryBuilder ClientFactoryBuilder { get; set; }

        /// <summary>
        /// Gets or sets the builder to use to configure messaging.
        /// </summary>
        private MessagingConfigurationBuilder MessagingConfig { get; set; }

        /// <summary>
        /// Gets or sets the builder to use for subscriptions.
        /// </summary>
        private SubscriptionsBuilder SubscriptionBuilder { get; set; }

        /// <summary>
        /// Gets or sets a delegate to a method to create the <see cref="ILoggerFactory"/> to use.
        /// </summary>
        private Func<ILoggerFactory> LoggerFactory { get; set; }

        /// <summary>
        /// Gets or sets a delegate to a method to create the <see cref="IMessageMonitor"/> to use.
        /// </summary>
        private Func<IMessageMonitor> MessageMonitoring { get; set; }

        /// <summary>
        /// Gets or sets a delegate to a method to create the <see cref="IMessageLockAsync"/> to use.
        /// </summary>
        private Func<IMessageLockAsync> MessageLock { get; set; }

        /// <summary>
        /// Gets or sets a delegate to a method to create the <see cref="IMessageSerializationFactory"/> to use.
        /// </summary>
        private Func<IMessageSerializationFactory> MessageSerializationFactory { get; set; }

        /// <summary>
        /// Gets or sets a delegate to a method to create the <see cref="INamingStrategy"/> to use.
        /// </summary>
        private Func<INamingStrategy> NamingStrategy { get; set; }

        /// <summary>
        /// Gets or sets a delegate to a method to create the <see cref="IMessageSerializationRegister"/> to use.
        /// </summary>
        private Func<IMessageSerializationRegister> SerializationRegister { get; set; }

        /// <summary>
        /// Configures the factory for AWS clients.
        /// </summary>
        /// <param name="configure">A delegate to a method to use to configure the AWS clients.</param>
        /// <returns>
        /// The current <see cref="MessagingBusBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public MessagingBusBuilder Client(Action<AwsClientFactoryBuilder> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            if (ClientFactoryBuilder == null)
            {
                ClientFactoryBuilder = new AwsClientFactoryBuilder(this);
            }

            configure(ClientFactoryBuilder);

            return this;
        }

        /// <summary>
        /// Configures messaging.
        /// </summary>
        /// <param name="configure">A delegate to a method to use to configure messaging.</param>
        /// <returns>
        /// The current <see cref="MessagingBusBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public MessagingBusBuilder Messaging(Action<MessagingConfigurationBuilder> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            if (MessagingConfig == null)
            {
                MessagingConfig = new MessagingConfigurationBuilder(this);
            }

            configure(MessagingConfig);

            return this;
        }

        /// <summary>
        /// Configures the subscriptions.
        /// </summary>
        /// <param name="configure">A delegate to a method to use to configure subscriptions.</param>
        /// <returns>
        /// The current <see cref="MessagingBusBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public MessagingBusBuilder Subscriptions(Action<SubscriptionsBuilder> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            if (SubscriptionBuilder == null)
            {
                SubscriptionBuilder = new SubscriptionsBuilder(this);
            }

            configure(SubscriptionBuilder);

            return this;
        }

        /// <summary>
        /// Specifies the <see cref="ILoggerFactory"/> to use.
        /// </summary>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <returns>
        /// The current <see cref="MessagingBusBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is <see langword="null"/>.
        /// </exception>
        public MessagingBusBuilder WithLoggerFactory(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            return WithLoggerFactory(() => loggerFactory);
        }

        /// <summary>
        /// Specifies the <see cref="ILoggerFactory"/> to use.
        /// </summary>
        /// <param name="loggerFactory">A delegate to a method to get the <see cref="ILoggerFactory"/> to use.</param>
        /// <returns>
        /// The current <see cref="MessagingBusBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is <see langword="null"/>.
        /// </exception>
        public MessagingBusBuilder WithLoggerFactory(Func<ILoggerFactory> loggerFactory)
        {
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            return this;
        }

        /// <summary>
        /// Specifies the <see cref="IMessageLockAsync"/> to use.
        /// </summary>
        /// <param name="messageLock">A delegate to a method to get the <see cref="IMessageLockAsync"/> to use.</param>
        /// <returns>
        /// The current <see cref="MessagingBusBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageLock"/> is <see langword="null"/>.
        /// </exception>
        public MessagingBusBuilder WithMessageLock(Func<IMessageLockAsync> messageLock)
        {
            MessageLock = messageLock ?? throw new ArgumentNullException(nameof(messageLock));
            return this;
        }

        /// <summary>
        /// Specifies the <see cref="IMessageMonitor"/> to use.
        /// </summary>
        /// <param name="monitoring">A delegate to a method to get the <see cref="IMessageMonitor"/> to use.</param>
        /// <returns>
        /// The current <see cref="MessagingBusBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="monitoring"/> is <see langword="null"/>.
        /// </exception>
        public MessagingBusBuilder WithMessageMonitoring(Func<IMessageMonitor> monitoring)
        {
            MessageMonitoring = monitoring ?? throw new ArgumentNullException(nameof(monitoring));
            return this;
        }

        /// <summary>
        /// Specifies the <see cref="INamingStrategy"/> to use.
        /// </summary>
        /// <param name="strategy">A delegate to a method to get the <see cref="INamingStrategy"/> to use.</param>
        /// <returns>
        /// The current <see cref="MessagingBusBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="strategy"/> is <see langword="null"/>.
        /// </exception>
        public MessagingBusBuilder WithNamingStrategy(Func<INamingStrategy> strategy)
        {
            NamingStrategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
            return this;
        }

        /// <summary>
        /// Specifies the <see cref="IMessageSerializationFactory"/> to use.
        /// </summary>
        /// <param name="factory">A delegate to a method to get the <see cref="IMessageSerializationFactory"/> to use.</param>
        /// <returns>
        /// The current <see cref="MessagingBusBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="factory"/> is <see langword="null"/>.
        /// </exception>
        public MessagingBusBuilder WithMessageSerializationFactory(Func<IMessageSerializationFactory> factory)
        {
            MessageSerializationFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            return this;
        }

        /// <summary>
        /// Specifies the <see cref="IServiceResolver"/> to use.
        /// </summary>
        /// <param name="serviceResolver">The <see cref="IServiceResolver"/> to use.</param>
        /// <returns>
        /// The current <see cref="MessagingBusBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="serviceResolver"/> is <see langword="null"/>.
        /// </exception>
        public MessagingBusBuilder WithServiceResolver(IServiceResolver serviceResolver)
        {
            ServiceResolver = serviceResolver ?? throw new ArgumentNullException(nameof(serviceResolver));
            return this;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IMessagingBus"/>.
        /// </summary>
        /// <returns>
        /// The created instance of <see cref="IMessagingBus"/>
        /// </returns>
        public IMessagingBus Build()
        {
            IMessagingConfig config = CreateConfig();

            config.Validate();

            ILoggerFactory loggerFactory =
                LoggerFactory?.Invoke() ?? ServiceResolver.ResolveService<ILoggerFactory>();

            JustSayingBus bus = CreateBus(config, loggerFactory);
            JustSayingFluently fluent = CreateFluent(bus, loggerFactory);

            if (NamingStrategy != null)
            {
                fluent.WithNamingStrategy(NamingStrategy);
            }

            // TODO Publishers
            // TODO Where do topic/queue names come in?
            if (SubscriptionBuilder != null)
            {
                SubscriptionBuilder.Configure(fluent);
            }

            return bus;
        }

        private JustSayingBus CreateBus(IMessagingConfig config, ILoggerFactory loggerFactory)
        {
            IMessageSerializationRegister register =
                SerializationRegister?.Invoke() ?? ServiceResolver.ResolveService<IMessageSerializationRegister>();

            return new JustSayingBus(config, register, loggerFactory);
        }

        private IMessagingConfig CreateConfig()
        {
            return MessagingConfig != null ?
                MessagingConfig.Build() :
                ServiceResolver.ResolveService<IMessagingConfig>();
        }

        private IAwsClientFactoryProxy CreateFactoryProxy()
        {
            return ClientFactoryBuilder != null ?
                new AwsClientFactoryProxy(new Lazy<IAwsClientFactory>(ClientFactoryBuilder.Build)) :
                ServiceResolver.ResolveService<IAwsClientFactoryProxy>();
        }

        private IMessageMonitor CreateMessageMonitor()
        {
            return MessageMonitoring?.Invoke() ?? ServiceResolver.ResolveService<IMessageMonitor>();
        }

        private IMessageSerializationFactory CreateMessageSerializationFactory()
        {
            return MessageSerializationFactory?.Invoke() ?? ServiceResolver.ResolveService<IMessageSerializationFactory>();
        }

        private JustSayingFluently CreateFluent(JustSayingBus bus, ILoggerFactory loggerFactory)
        {
            IAwsClientFactoryProxy proxy = CreateFactoryProxy();
            IVerifyAmazonQueues queueCreator = new AmazonQueueCreator(proxy, loggerFactory);

            var fluent = new JustSayingFluently(bus, queueCreator, proxy, loggerFactory);

            IMessageSerializationFactory serializationFactory = CreateMessageSerializationFactory();
            IMessageMonitor messageMonitor = CreateMessageMonitor();

            fluent.WithSerializationFactory(serializationFactory)
                  .WithMonitoring(messageMonitor);

            if (MessageLock != null)
            {
                fluent.WithMessageLockStoreOf(MessageLock());
            }

            return fluent;
        }
    }
}
