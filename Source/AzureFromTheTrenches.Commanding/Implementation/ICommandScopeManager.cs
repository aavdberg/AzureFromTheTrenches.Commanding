﻿using AzureFromTheTrenches.Commanding.Abstractions;

namespace AzureFromTheTrenches.Commanding.Implementation
{
    // the intention is to eventually make this a public replaceable interface
    internal interface ICommandScopeManager
    {
        ICommandDispatchContext Enter();
        void Exit();
        ICommandDispatchContext GetCurrent();
    }
}
