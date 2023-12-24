namespace ExpenseApi.Models.Shared
{
    public class BasePage
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int? Count { get; set; }
    }
}
