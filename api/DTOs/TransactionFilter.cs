using FinLog.Models;

namespace FinLog.DTOs
{
    public class TransactionFilter
    {
        public TransactionType? Type { get; set; }
        public string? Category { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
