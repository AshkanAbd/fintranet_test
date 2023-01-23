using System.Net;
using Application.Common.Response;
using Application.TodoList;
using Application.TodoList.Commands.CreateTodoList;
using Application.TodoList.Queries.GetTodoListList;
using Infrastructure.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Controllers;
using Presentation.IntegrationTests.Common;

namespace Presentation.IntegrationTests.TodoList;

public class CreatTodoListIntegrationTests : AbstractIntegrationTest
{
    [Fact]
    public async Task CreateTodoList_ShouldCreateTodoListInDatabase()
    {
        var controller = new TodoListController(
            ServiceProvider.GetService<ITodoListService>()!
        );

        var command = new CreateTodoListCommand {
            Title = "Test title for todo list",
            Color = "#001122",
            Page = 1,
            PageSize = 2,
        };
        var res = await controller.CreateTodoList(command, CancellationToken.None);

        Assert.NotNull(res);
        Assert.NotNull(res.Result as JsonResult);

        var jsonRes = (res.Result as JsonResult)!;

        Assert.NotNull(jsonRes.Value as StdResponse<PaginationModel<GetTodoListListDto>>);

        var value = (jsonRes.Value as StdResponse<PaginationModel<GetTodoListListDto>>)!;
        Assert.Equal("Todo list created.", value.Message);
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
        Assert.Equal(command.Color, actualList[0].Color);
    }
}