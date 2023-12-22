using ExpenseApi.Domain.Interfaces.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Service.Commands.Manager
{
    public class TransactionManager
    {
        private readonly Stack<ICommand> _commands = new Stack<ICommand>();

        public async Task ExecuteCommand(ICommand command)
        {
            await command.ExecuteAsync();
            _commands.Push(command);
        }

        public async Task UndoLastCommand()
        {
            if (_commands.Count > 0)
            {
                var command = _commands.Pop();
                await command.UndoAsync();
            }
        }
    }
}
