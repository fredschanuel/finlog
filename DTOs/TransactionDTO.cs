using FinLog.Models;
namespace FinLog.DTOs;

public class TransactionDTO
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public string? Category { get; set; }
}