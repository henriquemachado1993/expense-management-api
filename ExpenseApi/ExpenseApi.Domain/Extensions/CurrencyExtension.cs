using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Extensions
{
    public static class CurrencyExtension
    {
        public static string ConvertToBrazilianReal(this decimal amount)
        {
            return amount.ToString("c", CultureInfo.GetCultureInfo("pt-BR"));
        }
    }
}
