using ExpenseApi.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Entities
{
    public class FinancialGoal
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string AmountText
        {
            get
            {
                return Amount.ConvertToBrazilianReal();
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
