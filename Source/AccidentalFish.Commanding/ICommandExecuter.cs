﻿using System.Threading.Tasks;

namespace AccidentalFish.Commanding
{
    /// <summary>
    /// Implementations of this interface execute commands immediately using the registered set of actors
    /// </summary>
    public interface ICommandExecuter
    {
        /// <summary>
        /// Executes the given command
        /// </summary>
        /// <typeparam name="T">The type of the command</typeparam>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="command">The command to execute</param>
        /// <returns>Awaitable task</returns>
        Task<TResult> ExecuteAsync<T, TResult>(T command) where T : class;
    }
}