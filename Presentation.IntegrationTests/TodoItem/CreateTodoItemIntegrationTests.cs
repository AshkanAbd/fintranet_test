using System.Net;
using Application.Common.Response;
using Application.TodoItem;
using Application.TodoItem.Commands.CreateTodoItem;
using Application.TodoItem.Queries.GetTodoItemList;
using Application.TodoList.Queries.GetTodoListList;
using Infrastructure;
using Infrastructure.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Controllers;
using Presentation.IntegrationTests.Common;

namespace Presentation.IntegrationTests.TodoItem;

public class CreateTodoItemIntegrationTests : AbstractIntegrationTest
{
    [Fact]
    public async Task CreateTodoItem_ShouldCreateTodoItemInDatabase()
    {
        var todoList = TodoListFaker.Generate();
        var dbContext = ServiceProvider.GetService<AppDbContext>();

        await dbContext!.AddAsync(todoList);
        await dbContext.SaveChangesAsync();

        var controller = new TodoItemController(
            ServiceProvider.GetService<ITodoItemService>()!
        );

        var command = new CreateTodoItemCommand {
            TodoListId = todoList.Id,
            Title = Faker.Lorem.Word(),
            Note = Faker.Lorem.Paragraph(),
            Priority = Faker.Random.Int(1, 5),
            Page = 1,
            PageSize = 2,
        };

        var res = await controller.CreateTodoItem(command, CancellationToken.None);

        Assert.NotNull(res);
        Assert.NotNull(res.Result as JsonResult);

        var jsonRes = (res.Result as JsonResult)!;

        Assert.NotNull(jsonRes.Value as StdResponse<PaginationModel<GetTodoItemListDto>>);

        var value = (jsonRes.Value as StdResponse<PaginationModel<GetTodoItemListDto>>)!;
        Assert.Equal("Todo item created.", value.Message);
        Assert.Equal(HttpStatusCode.OK, value.Status);

        var paginationModel = value.DataAsDataStruct();
        Assert.NotNull(paginationModel);
        Assert.Equal(1, paginationModel!.Total);
        Assert.Equal(1, paginationModel.PageCount);
        Assert.Equal(command.Page, paginationModel.CurrentPage);
        Assert.Equal(command.PageSize, paginationModel.CurrentPageSize);
        Assert.Single(paginationModel.List);

        var actualList = paginationModel.List.ToList();

        Assert.Equal(1, actualList[0].Id);
        Assert.Equal(command.Title, actualList[0].Title);
        Assert.Equal(command.Note, actualList[0].Note);
        Assert.Equal(command.Priority, actualList[0].Priority);
    }
}