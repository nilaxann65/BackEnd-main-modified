using Ardalis.GuardClauses;
using System.Xml.Linq;
using TrackFinance.Core.TransactionAgregate.Enum;
using TrackFinance.SharedKernel;
using TrackFinance.SharedKernel.Interfaces;

namespace TrackFinance.Core.TransactionAgregate;
public class Transaction : EntityBase, IAggregateRoot
{
  public string Description { get; private set; }
  public decimal Amount { get; private set; }
  public TransactionDescriptionType TransactionDescriptionType { get; private set; }
  public DateTime ExpenseDate { get; private set; }
  public int UserId { get; private set; }
  public TransactionType TransactionType { get; private set; }



  public Transaction(string description, decimal amount, TransactionDescriptionType transactionDescriptionType, DateTime date, int userId, TransactionType transactionType)
  {
    Description = description;
    Amount = amount;
    TransactionDescriptionType = transactionDescriptionType;
    ExpenseDate = date;
    UserId = userId;
    TransactionType = transactionType;
  }

  public Transaction()
  {
  }

  public void UpdateValue(string description, decimal amount, TransactionDescriptionType transactionDescriptionType, DateTime expenseDate, int userId, TransactionType transactionType)
  {
    Description = description;
    Amount = amount;
    TransactionDescriptionType = transactionDescriptionType;
    ExpenseDate = expenseDate;
    UserId = userId;
    TransactionType = transactionType;
  }
  public void UpdateDescription(string description)
  {
    Description = description;
  }
}
