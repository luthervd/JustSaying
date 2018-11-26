using System;
using System.Collections.Generic;
using JustSaying.Models;

namespace JustSaying.Fluent
{
    /// <summary>
    /// A class representing a builder for subscriptions. This class cannot be inherited.
    /// </summary>
    public sealed class SubscriptionsBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionsBuilder"/> class.
        /// </summary>
        /// <param name="parent">The <see cref="MessagingBusBuilder"/> that owns this instance.</param>
        internal SubscriptionsBuilder(MessagingBusBuilder parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Gets the parent of this builder.
        /// </summary>
        internal MessagingBusBuilder Parent { get; }

        /// <summary>
        /// Gets the configured subscription builders.
        /// </summary>
        private IList<ISubscriptionBuilder<Message>> Subscriptions { get; } = new List<ISubscriptionBuilder<Message>>();

        /// <summary>
        /// Configures a subscription.
        /// </summary>
        /// <param name="configure">A delegate to a method to use to configure a subscription.</param>
        /// <returns>
        /// The current <see cref="SubscriptionsBuilder"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        public SubscriptionsBuilder WithSubscription<T>(Action<SubscriptionBuilder<T>> configure)
            where T : Message
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var builder = new SubscriptionBuilder<T>();

            configure(builder);

            Subscriptions.Add(builder);

            return this;
        }

        /// <summary>
        /// Configures the subscriptions for the <see cref="JustSayingFluently"/>.
        /// </summary>
        /// <param name="bus">The <see cref="JustSayingFluently"/> to configure subscriptions for.</param>
        /// <exception cref="InvalidOperationException">
        /// No instance of <see cref="IHandlerResolver"/> could be resolved.
        /// </exception>
        internal void Configure(JustSayingFluently bus)
        {
            IHandlerResolver resolver = Parent.ServiceResolver.ResolveService<IHandlerResolver>();

            if (resolver == null)
            {
                throw new InvalidOperationException($"No {nameof(IHandlerResolver)} is registered.");
            }

            foreach (ISubscriptionBuilder<Message> builder in Subscriptions)
            {
                builder.Configure(bus, resolver);
            }
        }
    }
}
