using Bogus;

namespace Application.UnitTests.Common;

public abstract class AbstractTest: IDisposable
{
    public static Faker<Domain.Models.TodoItem> TodoItemFaker = new Faker<Domain.Models.TodoItem>()
        .RuleFor(x => x.Title, f => f.Lorem.Sentences())
        .RuleFor(x => x.Note, f => f.Lorem.Paragraphs())
        .RuleFor(x => x.Priority, f => f.Random.Int(1, 5))
        .RuleFor(x => x.Id, f => f.Random.Int(1, 100))
        .RuleFor(x => x.TodoListId, f => f.Random.Int(1, 100))
        .RuleFor(x => x.CreatedAt, f => f.Date.Recent())
        .RuleFor(x => x.UpdatedAt, f => f.Date.Recent());

    public static Faker<Domain.Models.TodoList> TodoListFaker = new Faker<Domain.Models.TodoList>()
        .RuleFor(x => x.Title, f => f.Lorem.Sentences())
        .RuleFor(x => x.Color, f => f.Commerce.Color())
        .RuleFor(x => x.Id, f => f.Random.Int(1, 100))
        .RuleFor(x => x.CreatedAt, f => f.Date.Recent())
        .RuleFor(x => x.UpdatedAt, f => f.Date.Recent());
    protected Faker Faker { get; }

    protected AbstractTest()
    {
        Faker = new Faker();
    }

    public void Dispose()
    {
    }
}