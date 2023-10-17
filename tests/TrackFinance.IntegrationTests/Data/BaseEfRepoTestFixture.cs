﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TrackFinance.Core.TransactionAgregate;
using TrackFinance.Infrastructure.Data;
using TrackFinance.SharedKernel.Interfaces;

namespace TrackFinance.IntegrationTests.Data;
public abstract class BaseEfRepoTestFixture
{
  protected AppDbContext _dbContext;

  protected BaseEfRepoTestFixture()
  {
    var options = CreateNewContextOptions();
    var mockEventDispatcher = new Mock<IDomainEventDispatcher>();

    _dbContext = new AppDbContext(options, mockEventDispatcher.Object);
  }

  protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
  {
    // Create a fresh service provider, and therefore a fresh
    // InMemory database instance.
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    // Create a new options instance telling the context to use an
    // InMemory database and the new service provider.
    var builder = new DbContextOptionsBuilder<AppDbContext>();
    builder.UseInMemoryDatabase("cleanarchitecture")
           .UseInternalServiceProvider(serviceProvider);

    return builder.Options;
  }

  protected EfRepository<Transaction> GetRepository()
  {
    return new EfRepository<Transaction>(_dbContext);
  }

  public EfRepository<T> GetRepositoryGeneric<T>() where T : class, IAggregateRoot
  {
    return new EfRepository<T>(_dbContext);
  }
}
