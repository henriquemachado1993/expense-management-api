using BeireMKit.Data.Interfaces.Entity;
using ExpenseApi.Domain.Extensions;

namespace ExpenseApi.Domain.Entities
{
    public class BankAccount : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// Valores possíveis "Corrente", "Conta salário"
        /// </summary>
        public string Type { get; set; }
        public decimal AccountValue { get; private set; }
        public string AccountValueText
        {
            get
            {
                return AccountValue.ConvertToBrazilianReal();
            }
        }

        public bool IsMain { get; set; }

        public void Deposit(decimal amount)
        {
            AccountValue += amount;
        }

        public void WithDraw(decimal amount)
        {
            AccountValue -= amount;
        }
    }
}
