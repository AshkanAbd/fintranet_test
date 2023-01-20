using Domain.Models;
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

        Assert.NotNull(res);
        Assert.NotEqual(0, res.Id);
        Assert.Equal(todoItem.Title, res.Title);
        Assert.Equal(todoItem.Note, res.Note);
        Assert.Equal(todoItem.Priority, res.Priority);
        Assert.Equal(todoItem.TodoListId, res.TodoListId);
        Assert.NotEqual(new DateTime(), res.CreatedAt);
        Assert.NotEqual(new DateTime(), res.UpdatedAt);
        Assert.Null(res.DeletedAt);
    }

    [Fact]
    public async Task Get_ShouldReturnNullWhenNotExsits()
    {
        await using var dbContext = await DatabaseTestTool.GetTestDbContext();

        var repo = new TodoItemRepository(dbContext);

        var res = await repo.Get(1);

        Assert.Null(res);
    }

    [Fact]
    public async Task GetWithPagination_ShouldReturnItemsWithPagination()
    {
        await using var dbContext = await DatabaseTestTool.GetTestDbContext();
        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        var todoItem = DatabaseTestTool.TodoItemFaker.Generate(10);
        todoList.TodoItems = todoItem;
        await dbContext.AddAsync(todoList);
        await dbContext.SaveChangesAsync();

        var repo = new TodoItemRepository(dbContext);

        var res = await repo.GetWithPagination(
            todoList.Id, 1, 3
        );

        Assert.Equal(10, res.Total);
        Assert.Equal(4, res.PageCount);
        Assert.Equal(1, res.CurrentPage);
        Assert.Equal(3, res.CurrentPageSize);

        var expectedList = todoList.TodoItems.OrderByDescending(x => x.Priority).ToList();
        var actualList = res.List.ToList();
        for (var i = 0; i < 3; i++) {
            Assert.NotEqual(0, actualList[i].Id);
            Assert.Equal(expectedList[i].Title, actualList[i].Title);
            Assert.Equal(expectedList[i].Note, actualList[i].Note);
            Assert.Equal(expectedList[i].Priority, actualList[i].Priority);
            Assert.Equal(expectedList[i].TodoListId, actualList[i].TodoListId);
            Assert.NotEqual(new DateTime(), actualList[i].CreatedAt);
            Assert.NotEqual(new DateTime(), actualList[i].UpdatedAt);
            Assert.Null(actualList[i].DeletedAt);
        }
    }

    [Fact]
    public async Task Update_ShouldUpdateEntityInDatabaseWhenExistsInTracker()
    {
        await using var dbContext = await DatabaseTestTool.GetTestDbContext();
        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        var todoItem = DatabaseTestTool.TodoItemFaker.Generate();
        todoItem.TodoList = todoList;
        await dbContext.AddAsync(todoItem);
        await dbContext.SaveChangesAsync();

        var repo = new TodoItemRepository(dbContext);
        var newItem = DatabaseTestTool.TodoItemFaker.Generate();
        newItem.TodoListId = todoList.Id;

        var res = await repo.Update(todoItem.Id, newItem);
        Assert.Equal(todoItem.Id, res.Id);
        Assert.Equal(newItem.Title, res.Title);
        Assert.Equal(newItem.Note, res.Note);
        Assert.Equal(newItem.Priority, res.Priority);
        Assert.Equal(newItem.TodoListId, res.TodoListId);
        Assert.Equal(todoItem.CreatedAt, res.CreatedAt);
        Assert.Equal(todoItem.UpdatedAt, res.UpdatedAt);
        Assert.Null(res.DeletedAt);
    }

    [Fact]
    public async Task Update_ShouldUpdateEntityInDatabaseWhenNotExistsInTracker()
    {
        await using var dbContext = await DatabaseTestTool.GetTestDbContext();
        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        var todoItem = DatabaseTestTool.TodoItemFaker.Generate();
        todoItem.TodoList = todoList;
        await dbContext.AddAsync(todoItem);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        var repo = new TodoItemRepository(dbContext);
        var newItem = DatabaseTestTool.TodoItemFaker.Generate();
        newItem.TodoListId = todoList.Id;

        var res = await repo.Update(todoItem.Id, newItem);
        Assert.Equal(todoItem.Id, res.Id);
        Assert.Equal(newItem.Title, res.Title);
        Assert.Equal(newItem.Note, res.Note);
        Assert.Equal(newItem.Priority, res.Priority);
        Assert.Equal(newItem.TodoListId, res.TodoListId);
        Assert.Equal(todoItem.CreatedAt, res.CreatedAt);
        Assert.NotEqual(todoItem.UpdatedAt, res.UpdatedAt);
        Assert.Null(res.DeletedAt);
    }

    [Fact]
    public async Task Remove_ShouldRemoveEntityFromDatabaseWhenExistsInTracker()
    {
        await using var dbContext = await DatabaseTestTool.GetTestDbContext();
        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        var todoItem = DatabaseTestTool.TodoItemFaker.Generate();
        todoItem.TodoList = todoList;
        await dbContext.AddAsync(todoItem);
        await dbContext.SaveChangesAsync();

        var repo = new TodoItemRepository(dbContext);

        await repo.Remove(todoItem.Id);

        var res = await repo.Get(todoItem.Id);

        Assert.Null(res);
    }

    [Fact]
    public async Task Remove_ShouldRemoveEntityFromDatabaseWhenNotExistsInTracker()
    {
        await using var dbContext = await DatabaseTestTool.GetTestDbContext();
        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        var todoItem = DatabaseTestTool.TodoItemFaker.Generate();
        todoItem.TodoList = todoList;
        await dbContext.AddAsync(todoItem);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        var repo = new TodoItemRepository(dbContext);

        await repo.Remove(todoItem.Id);

        var res = await repo.Get(todoItem.Id);

        Assert.Null(res);
    }
}