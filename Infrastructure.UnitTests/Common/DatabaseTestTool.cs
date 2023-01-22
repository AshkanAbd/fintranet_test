using Bogus;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitTests.Common;

public class DatabaseTestTool
{
    private static int DbNameCounter = 0;

    public static async Task<AppDbContext> GetTestDbContext()
    {
        var connectionString = $"Server=127.0.0.1,1433;Database=TestDb{DbNameCounter++};UID=sa;PWD=Ashkan007;";
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        var dbContext = new AppDbContext(options);
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();

        return dbContext;
    }

    public static async Task Cleanup(AppDbContext dbContext)
    {
        await dbContext.Database.EnsureDeletedAsync();
    }

    public static Faker<TodoItem> TodoItemFaker = new Faker<TodoItem>()
        .RuleFor(x => x.Title, f => f.Lorem.Sentences())
        .RuleFor(x => x.Note, f => f.Lorem.Paragraphs())
        .RuleFor(x => x.Priority, f => f.Random.Int(1));

    public static Faker<TodoList> TodoListFaker = new Faker<TodoList>()
        .RuleFor(x => x.Title, f => f.Lorem.Sentences())
        .RuleFor(x => x.Color, f => f.Commerce.Color());
}