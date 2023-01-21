using System.Net;
using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoItem.Queries.GetTodoItemList;
using Application.TodoList.Queries.GetTodoList;
using Application.UnitTests.Common;
using Infrastructure.Repositories.TodoList;
using MediatR;

namespace Application.UnitTests.TodoList;

public class GetTodoListQueryUnitTests : AbstractTest
{
    [Fact]
    public async Task GetTodoListQuery_ShouldReturnTodoList()
    {
        var todoList = TodoListFaker.Generate();
        var todoItems = TodoItemFaker.Generate(10);

        var todoListRepoMock = new Mock<ITodoListRepository>();
        todoListRepoMock.Setup(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);
        todoListRepoMock.Setup(
            x => x.Get(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(todoList);

        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(
            x => x.Send(It.IsAny<GetTodoItemListQuery>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync((GetTodoItemListQuery query, CancellationToken _) => {
            return StdResponseFactory.Ok<PaginationModel<GetTodoItemListDto>>(
                new PaginationModel<GetTodoItemListDto> {
                    PageCount = (int) Math.Ceiling(todoItems.Count / (double) query.PageSize!),
                    CurrentPage = query.Page!.Value,
                    CurrentPageSize = query.PageSize!.Value,
                    Total = todoItems.Count,
                    List = todoItems
                        .Skip((query.Page!.Value - 1) * query.PageSize!.Value)
                        .Take(query.PageSize!.Value).Select(x => new GetTodoItemListDto {
                            Id = x.Id,
                            Title = x.Title,
                            Note = x.Note,
                            Priority = x.Priority,
                            CreatedAt = x.CreatedAt,
                            UpdatedAt = x.UpdatedAt,
                        }),
                }
            );
        });

        var handler = new GetTodoListQueryHandler(todoListRepoMock.Object, mediatorMock.Object);

        var query = new GetTodoListQuery {
            Id = todoList.Id,
            Page = 2,
            PageSize = 3,
        };

        var res = await handler.Handle(query, CancellationToken.None);

        Assert.Equal("Success", res.Message);
        Assert.Equal(HttpStatusCode.OK, res.Status);

        var resTodoList = res.DataAsDataStruct();

        Assert.NotNull(resTodoList);
        Assert.Equal(todoList.Id, resTodoList!.Id);
        Assert.Equal(todoList.Title, resTodoList.Title);
        Assert.Equal(todoList.Color, resTodoList.Color);

        Assert.Equal(query.Page, resTodoList.TodoItemList.CurrentPage);
        Assert.Equal(query.PageSize, resTodoList.TodoItemList.CurrentPageSize);
        Assert.Equal(query.PageSize, resTodoList.TodoItemList.List.Count());
        Assert.Equal(todoItems.Count, resTodoList.TodoItemList.Total);
        Assert.Equal((int) Math.Ceiling(todoItems.Count / (double) query.PageSize), resTodoList.TodoItemList.PageCount);

        var acutalList = resTodoList.TodoItemList.List.ToList();
        var expectedList = todoItems.Skip((query.Page!.Value - 1) * query.PageSize!.Value).ToList();
        for (var i = 0; i < query.PageSize; i++) {
            Assert.Equal(expectedList[i].Id, acutalList[i].Id);
            Assert.Equal(expectedList[i].Title, acutalList[i].Title);
            Assert.Equal(expectedList[i].Note, acutalList[i].Note);
            Assert.Equal(expectedList[i].Priority, acutalList[i].Priority);
            Assert.Equal(expectedList[i].CreatedAt, acutalList[i].CreatedAt);
            Assert.Equal(expectedList[i].UpdatedAt, acutalList[i].UpdatedAt);
        }

        mediatorMock.Verify(
            x => x.Send(It.IsAny<GetTodoItemListQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        todoListRepoMock.Verify(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        todoListRepoMock.Verify(
            x => x.Get(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );

        mediatorMock.VerifyNoOtherCalls();
        todoListRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetDotoListQuery_ShouldReturnNotFoundWhenTodoListNotExists()
    {
        var todoListRepoMock = new Mock<ITodoListRepository>();
        todoListRepoMock.Setup(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        var mediatorMock = new Mock<IMediator>();

        var handler = new GetTodoListQueryHandler(todoListRepoMock.Object, mediatorMock.Object);

        var query = new GetTodoListQuery {
            Id = 1,
            PageSize = 5,
            Page = 1,
        };

        var res = await handler.Handle(query, CancellationToken.None);

        Assert.Equal("Todo list notfound.", res.Message);
        Assert.Equal(HttpStatusCode.NotFound, res.Status);
        Assert.Null(res.Data);

        todoListRepoMock.Verify(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );

        mediatorMock.VerifyNoOtherCalls();
        todoListRepoMock.VerifyNoOtherCalls();
    }
}