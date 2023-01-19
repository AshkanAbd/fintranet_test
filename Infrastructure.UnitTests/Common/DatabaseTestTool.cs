using Bogus;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitTests.Common;

public class DatabaseTestTool
{
    const string ConnectionString =
        "Server=127.0.0.1,1433;Database=TestDb;UID=sa;PWD=Ashkan007;";

    public static async Task<AppDbContext> GetTestDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        var dbContext = new AppDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
        
        return dbContext;
    }

    public static Faker<TodoItem> TodoItemFaker = new Faker<TodoItem>()
        .RuleFor(x => x.Title, f => f.Lorem.Sentences())
        .RuleFor(x => x.Note, f => f.Lorem.Paragraphs())
        .RuleFor(x => x.Priority, f => f.Random.Int());

    public static Faker<TodoList> TodoListFaker = new Faker<TodoList>()
        .RuleFor(x => x.Title, f => f.Lorem.Sentences())
        .RuleFor(x => x.Color, f => f.Commerce.Color());
}