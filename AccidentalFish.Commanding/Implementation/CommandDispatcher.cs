﻿using System.Threading.Tasks;
using AccidentalFish.Commanding.Model;

namespace AccidentalFish.Commanding.Implementation
{
    internal class CommandDispatcher : ICommandDispatcher
    {
        private readonly ICommandRegistry _commandRegistry;

        public CommandDispatcher(ICommandRegistry commandRegistry, ICommandExecuter commandExecuter)
        {
            _commandRegistry = commandRegistry;
            AssociatedExecuter = commandExecuter;
        }

        public async Task<CommandResult<TResult>> DispatchAsync<TCommand, TResult>(TCommand command) where TCommand : class
        {
            CommandResult<TResult> dispatchResult = null;
            ICommandDispatcher dispatcher = _commandRegistry.GetCommandDispatcher<TCommand>();
            ICommandExecuter executer = null;
            if (dispatcher != null)
            {
                dispatchResult = await dispatcher.DispatchAsync<TCommand, TResult>(command);
                executer = dispatcher.AssociatedExecuter;
            }

            if (dispatchResult != null && dispatchResult.DeferExecution)
            {
                return new CommandResult<TResult>(default(TResult), true);
            }

            if (executer == null)
            {
                executer = AssociatedExecuter;
            }
            return new CommandResult<TResult>(await executer.ExecuteAsync<TCommand,TResult>(command), false);            
        }

        public ICommandExecuter AssociatedExecuter { get; }
    }
}
