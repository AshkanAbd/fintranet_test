using Infrastructure.Repositories.TodoItem;
using Infrastructure.UnitTests.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitTests.Repositories;

public class TodoItemRepositoryUnitTest
{
    [Fact]
    public async Task Add_ShouldAddEntityToDatabase()
    {
        await using var dbContext = await DatabaseTestTool.GetTestDbContext();

        var todoList = DatabaseTestTool.TodoListFaker.Generate();

        await dbContext.TodoLists.AddAsync(todoList);
        await dbContext.SaveChangesAsync();

        var repo = new TodoItemRepository(dbContext);

        var todoItem = DatabaseTestTool.TodoItemFaker.Generate();
        todoItem.TodoListId = todoList.Id;

        var res = await repo.Add(todoItem);

        Assert.NotEqual(0, res.Id);
        Assert.Equal(todoItem.Title, res.Title);
        Assert.Equal(todoItem.Note, res.Note);
        Assert.Equal(todoItem.Priority, res.Priority);
        Assert.Equal(todoItem.TodoListId, res.TodoListId);
        Assert.NotEqual(new DateTime(), res.CreatedAt);
        Assert.NotEqual(new DateTime(), res.UpdatedAt);
        Assert.Null(res.DeletedAt);

        dbContext.ForceRemoveRange(todoItem, todoList);
        await dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task Add_ShouldThrowErrorWhenTodoListDoesNotExists()
    {
        await using var dbContext = await DatabaseTestTool.GetTestDbContext();

        var repo = new TodoItemRepository(dbContext);

        var todoItem = DatabaseTestTool.TodoItemFaker.Generate();

        try {
            var res = await repo.Add(todoItem);
            
            Assert.Null(res);
        }
        catch (DbUpdateException e) {
            Assert.NotNull(e);
        }
    }

    [Fact]
    public async Task Get_ShouldReturnTodoItem()
    {
        await using var dbContext = await DatabaseTestTool.GetTestDbContext();

        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        var todoItem = DatabaseTestTool.TodoItemFaker.Generate();
        todoItem.TodoList = todoList;
        await dbContext.AddAsync(todoItem);
        await dbContext.SaveChangesAsync();

        var repo = new TodoItemRepository(dbContext);

        var res = await repo.Get(todoItem.Id);

        Assert.NotEqual(0, res.Id);
        Assert.Equal(todoItem.Title, res.Title);
        Assert.Equal(todoItem.Note, res.Note);
        Assert.Equal(todoItem.Priority, res.Priority);
        Assert.Equal(todoItem.TodoListId, res.TodoListId);
        Assert.NotEqual(new DateTime(), res.CreatedAt);
        Assert.NotEqual(new DateTime(), res.UpdatedAt);
        Assert.Null(res.DeletedAt);

        dbContext.ForceRemoveRange(todoItem, todoList);
        await dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task Get_ShouldReturnNullWhenNotExsits()
    {
        await using var dbContext = await DatabaseTestTool.GetTestDbContext();

        var repo = new TodoItemRepository(dbContext);

        var res = await repo.Get(1);

        Assert.Null(res);
    }
}