using Microsoft.EntityFrameworkCore;
using FinLog.Data;
using FinLog.Models;
using FinLog.DTOs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TransactionDb>(opt => opt.UseSqlite("Data Source=finlog.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });
var app = builder.Build();
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();

var transactions = app.MapGroup("/transactions");

transactions.MapGet("/", async (TransactionDb db) =>
    await db.Transactions.ToListAsync());

transactions.MapGet("/expenses", async (TransactionDb db) =>
    await db.Transactions
        .Where(t => t.Type == TransactionType.Expense)
        .ToListAsync());

transactions.MapGet("/{id}", async (int id, TransactionDb db) =>
    await db.Transactions.FindAsync(id)
        is Transaction transaction
        ? Results.Ok(transaction)
        : Results.NotFound());

transactions.MapGet("/balance", async (TransactionDb db) =>
{
    var income = await db.Transactions
        .Where(t => t.Type == TransactionType.Income)
        .SumAsync(t => t.Amount);

    var expense = await db.Transactions
        .Where(t => t.Type == TransactionType.Expense)
        .SumAsync(t => t.Amount);

    var balance = income - expense;

    return Results.Ok(new
    {
        income,
        expense,
        balance
    });
});

transactions.MapPost("/", async (TransactionDTO dto, TransactionDb db) =>
{
    if (dto.Amount <= 0)
        return Results.BadRequest("Amount must be greater than zero");
    
    var transaction = new Transaction()
    {
        Amount = dto.Amount,
        Description = dto.Description,
        Date = dto.Date,
        Type = dto.Type,
        Category = dto.Category
    };
    
    db.Transactions.Add(transaction);
    await db.SaveChangesAsync();
    return Results.Created($"/transactions/{transaction.Id}", transaction);
});

transactions.MapPut("/{id}", async (int id, TransactionDTO dto, TransactionDb db) =>
{
    var transaction = await db.Transactions.FindAsync(id);

    if (transaction is null) return Results.NotFound();

    transaction.Amount = dto.Amount;
    transaction.Description = dto.Description;
    transaction.Date = dto.Date;
    transaction.Type = dto.Type;
    transaction.Category = dto.Category;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

transactions.MapDelete("/{id}", async (int id, TransactionDb db) =>
{
    if (await db.Transactions.FindAsync(id) is Transaction transaction)
    {
        db.Transactions.Remove(transaction);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransactionDb>();
    db.Database.EnsureCreated();
}

app.Run();