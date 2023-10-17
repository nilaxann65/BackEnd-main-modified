using Microsoft.EntityFrameworkCore;
using TrackFinance.Core.TransactionAgregate;
using TrackFinance.Core.TransactionAgregate.Enum;
using TrackFinance.Infrastructure.Data;

namespace TrackFinance.Web;

public static class SeedDataTrackFinance
{
  public static readonly Transaction TestTransaction = new("compra de equipos", 20, TransactionDescriptionType.Services, Convert.ToDateTime("2023-08-01T20:31:50.647Z"), 1, TransactionType.Expense);
  public static readonly Transaction TestTransaction1 = new("compra computadoras", 10, TransactionDescriptionType.Services, Convert.ToDateTime("2023-08-31T20:31:50.647Z"), 1, TransactionType.Expense);
  public static readonly Transaction TestTransaction2 = new("compra accesorios de gimnasio", 10, TransactionDescriptionType.Services, DateTime.Now, 1, TransactionType.Expense);
  public static readonly Transaction TestTransaction3 = new("compra joyeria", 10, TransactionDescriptionType.Services, DateTime.Now, 1, TransactionType.Expense);
  public static readonly Transaction TestTransaction4 = new("venta comida", 30, TransactionDescriptionType.Food, DateTime.Now, 1, TransactionType.Income);
  public static readonly Transaction TestTransaction5 = new("venta desayuno", 25, TransactionDescriptionType.Food, DateTime.Now, 1, TransactionType.Income);

  public static void Initialize(IServiceProvider serviceProvider)
  {
    using var dbContext = new AppDbContext(
        serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), null);
    PopulateTestData(dbContext);
  }
  public static void PopulateTestData(AppDbContext dbContext)
  {
    foreach (var item in dbContext.Transactions)
    {
      dbContext.Remove(item);
    }

    dbContext.SaveChanges();

    dbContext.Transactions.Add(TestTransaction);
    dbContext.Transactions.Add(TestTransaction1);
    dbContext.Transactions.Add(TestTransaction2);
    dbContext.Transactions.Add(TestTransaction3);
    dbContext.Transactions.Add(TestTransaction4);
    dbContext.Transactions.Add(TestTransaction5);
    dbContext.SaveChanges();
  }
}
