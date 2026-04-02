using Microsoft.EntityFrameworkCore;
using FinLog.Models;
namespace FinLog.Data;

public class TransactionDb : DbContext
{
    public TransactionDb(DbContextOptions<TransactionDb> options)
        : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();
}

