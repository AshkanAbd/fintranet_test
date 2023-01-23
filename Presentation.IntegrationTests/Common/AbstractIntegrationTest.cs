using Application;
using Bogus;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories.TodoItem;
using Infrastructure.Repositories.TodoList;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.IntegrationTests.Common;

public abstract class AbstractIntegrationTest : IDisposable
{
    private static int DbNameCounter = 0;

    public static Faker<Domain.Models.TodoItem> TodoItemFaker = new Faker<Domain.Models.TodoItem>()
        .RuleFor(x => x.Title, f => f.Lorem.Word())
        .RuleFor(x => x.Note, f => f.Lorem.Paragraphs())
        .RuleFor(x => x.Priority, f => f.Random.Int(0, 5));

    public static Faker<Domain.Models.TodoList> TodoListFaker = new Faker<Domain.Models.TodoList>()
        .RuleFor(x => x.Title, f => f.Lorem.Word())
        .RuleFor(x => x.Color, f => "#556644");

    protected AbstractIntegrationTest()
    {
        Faker = new Faker();

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddApplication();

        serviceCollection.AddDbContext<AppDbContext>(options => {
            options.UseSqlServer(
                $"Server=127.0.0.1,1433;Database=IntTestDb{DbNameCounter++};UID=sa;PWD=Ashkan007;"
            );
        });

        serviceCollection.AddScoped<ITodoItemRepository, TodoItemRepository>();
        serviceCollection.AddScoped<ITodoListRepository, TodoListRepository>();

        ServiceProvider = serviceCollection.BuildServiceProvider();

        PrepareDb();
    }

    protected IServiceProvider ServiceProvider { get; }
    protected Faker Faker { get; }

    public async void Dispose()
    {
        var dbContext = ServiceProvider.GetService<AppDbContext>();
        await dbContext!.Database.EnsureDeletedAsync();
    }

    private void PrepareDb()
    {
        var dbContext = ServiceProvider.GetService<AppDbContext>();
        dbContext!.Database.EnsureDeleted();
        dbContext.Database.Migrate();
    }
}