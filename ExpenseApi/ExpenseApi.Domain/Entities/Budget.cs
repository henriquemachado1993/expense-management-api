using ExpenseApi.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Entities
{
    public class Budget
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Amount { get; set; }
        public string AmountText
        {
            get
            {
                return Amount.ConvertToBrazilianReal();
            }
        }

        public string MonthYearText
        {
            get
            {
                return $"{Month}/{Year}";
            }
        }
    }
}
