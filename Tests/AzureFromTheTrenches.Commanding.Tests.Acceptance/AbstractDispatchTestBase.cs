﻿using System;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureFromTheTrenches.Commanding.Tests.Acceptance.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace AzureFromTheTrenches.Commanding.Tests.Acceptance
{
    public abstract class AbstractDispatchTestBase
    {
        protected AbstractDispatchTestBase(Action<ICommandRegistry> registrations)
        {
            var serviceCollection = new ServiceCollection();
            var resolver = new CommandingDependencyResolver(
                (type, instance) => serviceCollection.AddSingleton(type, instance),
                (type, impl) => serviceCollection.AddTransient(type, impl),
                type => ServiceProvider.GetService(type)
            );
            CommandingConfiguration = new CommandingRuntime();
            var registry = CommandingConfiguration.UseCommanding(resolver);
            CommandTracer = new CommandTracer();
            serviceCollection.AddSingleton(CommandTracer);

            registrations(registry);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            Dispatcher = ServiceProvider.GetRequiredService<ICommandDispatcher>();
        }

        protected CommandingRuntime CommandingConfiguration { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected ICommandDispatcher Dispatcher { get; }

        protected ICommandTracer CommandTracer { get; }
    }
}