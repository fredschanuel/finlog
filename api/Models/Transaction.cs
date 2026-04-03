namespace FinLog.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public String Category { get; set; } = string.Empty;
}

public enum TransactionType
{
    Expense = 0,
    Income = 1,
}