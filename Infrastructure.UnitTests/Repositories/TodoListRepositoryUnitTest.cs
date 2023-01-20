using Infrastructure.Repositories.TodoList;
using Infrastructure.UnitTests.Common;

namespace Infrastructure.UnitTests.Repositories;

public class TodoListRepositoryUnitTest : AbstractDatabaseTest
{
    [Fact]
    public async Task Add_ShouldAddEntityToDatabase()
    {
        var repo = new TodoListRepository(DbContext);
        var todoList = DatabaseTestTool.TodoListFaker.Generate();

        var res = await repo.Add(todoList);

        Assert.NotEqual(0, res.Id);
        Assert.Equal(todoList.Title, res.Title);
        Assert.Equal(todoList.Color, res.Color);
    }

    [Fact]
    public async Task Get_ShouldReturnTodoList()
    {
        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        await DbContext.AddAsync(todoList);
        await DbContext.SaveChangesAsync();


        var repo = new TodoListRepository(DbContext);

        var res = await repo.Get(todoList.Id);

        Assert.NotNull(res);
        Assert.NotEqual(0, res.Id);
        Assert.Equal(todoList.Title, res.Title);
        Assert.Equal(todoList.Color, res.Color);
    }

    [Fact]
    public async Task Get_ShouldReturnNullWhenNotExists()
    {
        var repo = new TodoListRepository(DbContext);

        var res = await repo.Get(1);

        Assert.Null(res);
    }

    [Fact]
    public async Task GetWithPagination_ShouldReturnItemsWithPagination()
    {
        var todoLists = DatabaseTestTool.TodoListFaker.Generate(10);
        await DbContext.AddRangeAsync(todoLists);
        await DbContext.SaveChangesAsync();

        var repo = new TodoListRepository(DbContext);

        var res = await repo.GetWithPagination(1, 3);
        Assert.Equal(10, res.Total);
        Assert.Equal(4, res.PageCount);
        Assert.Equal(1, res.CurrentPage);
        Assert.Equal(3, res.CurrentPageSize);

        var expectedList = todoLists.OrderByDescending(x => x.CreatedAt).ToList();
        var actualList = res.List.ToList();
        for (var i = 0; i < 3; i++) {
            Assert.NotEqual(0, actualList[i].Id);
            Assert.Equal(expectedList[i].Title, actualList[i].Title);
            Assert.Equal(expectedList[i].Color, actualList[i].Color);
            Assert.NotEqual(new DateTime(), actualList[i].CreatedAt);
            Assert.NotEqual(new DateTime(), actualList[i].UpdatedAt);
            Assert.Null(actualList[i].DeletedAt);
        }
    }

    [Fact]
    public async Task Update_ShouldUpdateEntityInDatabaseWhenExistsInTracker()
    {
        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        await DbContext.AddAsync(todoList);
        await DbContext.SaveChangesAsync();

        var repo = new TodoListRepository(DbContext);
        var newList = DatabaseTestTool.TodoListFaker.Generate();

        var res = await repo.Update(todoList.Id, newList);
        Assert.Equal(todoList.Id, res.Id);
        Assert.Equal(newList.Title, res.Title);
        Assert.Equal(newList.Color, res.Color);
        Assert.Equal(todoList.CreatedAt, res.CreatedAt);
        Assert.Equal(todoList.UpdatedAt, res.UpdatedAt);
        Assert.Null(res.DeletedAt);
    }

    [Fact]
    public async Task Update_ShouldUpdateEntityInDatabaseWhenNotExistsInTracker()
    {
        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        await DbContext.AddAsync(todoList);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var repo = new TodoListRepository(DbContext);
        var newList = DatabaseTestTool.TodoListFaker.Generate();

        var res = await repo.Update(todoList.Id, newList);
        Assert.Equal(todoList.Id, res.Id);
        Assert.Equal(newList.Title, res.Title);
        Assert.Equal(newList.Color, res.Color);
        Assert.Equal(todoList.CreatedAt, res.CreatedAt);
        Assert.NotEqual(todoList.UpdatedAt, res.UpdatedAt);
        Assert.Null(res.DeletedAt);
    }

    [Fact]
    public async Task Remove_ShouldRemoveEntityFromDatabaseWhenExistsInTracker()
    {
        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        await DbContext.AddAsync(todoList);
        await DbContext.SaveChangesAsync();

        var repo = new TodoListRepository(DbContext);

        await repo.Remove(todoList.Id);

        var res = await repo.Get(todoList.Id);

        Assert.Null(res);
    }

    [Fact]
    public async Task Remove_ShouldRemoveEntityFromDatabaseWhenNotExistsInTracker()
    {
        var todoList = DatabaseTestTool.TodoListFaker.Generate();
        await DbContext.AddAsync(todoList);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var repo = new TodoListRepository(DbContext);

        await repo.Remove(todoList.Id);

        var res = await repo.Get(todoList.Id);

        Assert.Null(res);
    }
}