﻿using System;
using AzureFromTheTrenches.Commanding.Abstractions;

namespace AzureFromTheTrenches.Commanding.MicrosoftDependencyInjection
{
    [Obsolete("Please use IMicrosoftDependencyInjectionCommandingResolverAdapter instead")]
    public interface IMicrosoftDependencyInjectionCommandingResolver : ICommandingDependencyResolver
    {
        ICommandRegistry Registry { get; }

        IServiceProvider ServiceProvider { get; set; }
    }
}
