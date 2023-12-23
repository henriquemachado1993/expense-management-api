using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ExpenseApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
