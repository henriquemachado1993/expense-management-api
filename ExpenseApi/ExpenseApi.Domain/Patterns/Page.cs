
namespace ExpenseApi.Domain.Patterns
{
    public class Page
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}
