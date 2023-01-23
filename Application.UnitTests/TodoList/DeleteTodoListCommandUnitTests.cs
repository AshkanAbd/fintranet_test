using System.Net;
using Application.Common.Response;
using Application.TodoList.Commands.DeleteTodoList;
using Application.TodoList.Queries.GetTodoListList;
using Application.UnitTests.Common;
using Infrastructure.Common.Pagination;
using Infrastructure.Repositories.TodoList;
using MediatR;

namespace Application.UnitTests.TodoList;

public class DeleteTodoListCommandUnitTests : AbstractTest
{
    [Fact]
    public async Task DeleteTodoList_ShouldDeleteTodoList()
    {
        var todoLists = TodoListFaker.Generate(10);

        var todoListRepoMock = new Mock<ITodoListRepository>();
        todoListRepoMock.Setup(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);
        todoListRepoMock.Setup(
            x => x.Remove(
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            )
        ).Returns(Task.CompletedTask);

        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(
            x => x.Send(It.IsAny<GetTodoListListQuery>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync((GetTodoListListQuery command, CancellationToken _) =>
            StdResponseFactory.Ok<PaginationModel<GetTodoListListDto>>(new PaginationModel<GetTodoListListDto> {
                    PageCount = (int) Math.Ceiling(todoLists.Count / (double) command.PageSize!),
                    CurrentPage = command.Page!.Value,
                    CurrentPageSize = command.PageSize.Value,
                    Total = todoLists.Count,
                    List = todoLists.Skip((command.Page.Value - 1) * command.PageSize.Value)
                        .Take(command.PageSize.Value)
                        .Select(x => new GetTodoListListDto {
                            Id = x.Id,
                            Title = x.Title,
                            Color = x.Color,
                            CreatedAt = x.CreatedAt,
                            UpdatedAt = x.UpdatedAt,
                        })
                }
            )
        );

        var handler = new DeleteTodoListCommandHandler(todoListRepoMock.Object, mediatorMock.Object);

        var command = new DeleteTodoListCommand() {
            Id = 1,
            Page = 2,
            PageSize = 3,
        };

        var res = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Todo list removed.", res.Message);
        Assert.Equal(HttpStatusCode.OK, res.Status);

        var resValue = res.DataAsDataStruct();

        Assert.NotNull(resValue);
        Assert.Equal(todoLists.Count, resValue!.Total);
        Assert.Equal(command.Page, resValue.CurrentPage);
        Assert.Equal((int) Math.Ceiling(todoLists.Count / (double) command.PageSize), resValue.PageCount);
        Assert.Equal(command.PageSize, resValue.CurrentPageSize);

        var actualList = resValue.List.ToList();
        var expectedList = todoLists.Skip((command.Page!.Value - 1) * command.PageSize!.Value)
            .Take(command.PageSize!.Value)
            .ToList();

        for (var i = 0; i < command.PageSize!.Value; i++) {
            Assert.Equal(expectedList[i].Id, actualList[i].Id);
            Assert.Equal(expectedList[i].Title, actualList[i].Title);
            Assert.Equal(expectedList[i].Color, actualList[i].Color);
            Assert.Equal(expectedList[i].CreatedAt, actualList[i].CreatedAt);
            Assert.Equal(expectedList[i].UpdatedAt, actualList[i].UpdatedAt);
        }

        mediatorMock.Verify(
            x => x.Send(It.IsAny<GetTodoListListQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        mediatorMock.VerifyNoOtherCalls();

        todoListRepoMock.Verify(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        todoListRepoMock.Verify(
            x => x.Remove(
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
        todoListRepoMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task DeleteTodoList_ShouldReturnNotFoundWhenTodoListNotExists()
    {
        var todoListRepoMock = new Mock<ITodoListRepository>();
        todoListRepoMock.Setup(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        var mediatorMock = new Mock<IMediator>();

        var handler = new DeleteTodoListCommandHandler(todoListRepoMock.Object, mediatorMock.Object);

        var command = new DeleteTodoListCommand {
            Id = 1,
            Page = 3,
            PageSize = 2,
        };

        var res = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Todo list notfound.", res.Message);
        Assert.Equal(HttpStatusCode.NotFound, res.Status);
        Assert.Null(res.Data);

        todoListRepoMock.Verify(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        todoListRepoMock.VerifyNoOtherCalls();
        mediatorMock.VerifyNoOtherCalls();
    }
}