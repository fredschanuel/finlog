using FinLog.Models;

namespace FinLog.DTOs
{
    public class TransactionFilter
    {
        public TransactionType? Type { get; set; }
        public string? Category { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }
        public bool Desc { get; set; } = false;
    }
}
