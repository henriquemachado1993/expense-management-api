using ExpenseApi.Domain.Enums;

namespace ExpenseApi.Domain.ValueObjects
{
    public class MessageResult
    {
        public string Key { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        /// <summary>
        /// Destinado para usar em alerts
        /// </summary>
        public string TypeCustom { get; set; }
    }
}
