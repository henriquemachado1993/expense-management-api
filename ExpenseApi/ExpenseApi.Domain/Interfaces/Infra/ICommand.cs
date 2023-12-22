using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Interfaces.Infra
{
    public interface ICommand
    {
        Task ExecuteAsync();
        Task UndoAsync();
    }
}
