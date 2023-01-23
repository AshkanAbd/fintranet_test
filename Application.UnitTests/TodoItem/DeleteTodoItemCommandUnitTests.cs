using Application.Common.Response;
using Application.TodoItem.Commands.DeleteTodoItem;
using Application.TodoItem.Queries.GetTodoItemList;
using Application.UnitTests.Common;
using Infrastructure.Common.Pagination;
using Infrastructure.Repositories.TodoItem;
using MediatR;

namespace Application.UnitTests.TodoItem;

public class DeleteTodoItemCommandUnitTests : AbstractTest
{
    [Fact]
    public async Task DeleteTodoItemCommand_ShouldDeleteTodoItem()
    {
        var todoItems = TodoItemFaker.Generate(10);
        todoItems.ForEach(x => x.TodoListId = 1);

        var todoItemRepoMock = new Mock<ITodoItemRepository>();
        todoItemRepoMock.Setup(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

        todoItemRepoMock.Setup(
            x => x.Get(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync((long _, CancellationToken _) => todoItems[0]);

        todoItemRepoMock.Setup(
            x => x.Remove(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).Returns((long _, CancellationToken _) => Task.CompletedTask);
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
                    List = todoItems.Take(query.PageSize!.Value).Select(x => new GetTodoItemListDto {
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

        var handler = new DeleteTodoItemCommandHandler(todoItemRepoMock.Object, mediatorMock.Object);

        var command = new DeleteTodoItemCommand {
            Id = 1,
            Page = 1,
            PageSize = 3,
        };

        var res = await handler.Handle(command, CancellationToken.None);

        var resValue = res.DataAsDataStruct();
        Assert.NotNull(resValue);
        Assert.Equal(command.Page, resValue!.CurrentPage);
        Assert.Equal(command.PageSize, resValue.CurrentPageSize);
        Assert.Equal((int) Math.Ceiling(todoItems.Count / (double) command.PageSize), resValue.PageCount);
        Assert.Equal(todoItems.Count, resValue.Total);
        Assert.Equal(command.PageSize, resValue.List.Count());

        var actualList = resValue.List.ToList();
        for (var i = 0; i < command.PageSize; i++) {
            Assert.Equal(todoItems[i].Id, actualList[i].Id);
            Assert.Equal(todoItems[i].Title, actualList[i].Title);
            Assert.Equal(todoItems[i].Note, actualList[i].Note);
            Assert.Equal(todoItems[i].Priority, actualList[i].Priority);
            Assert.Equal(todoItems[i].CreatedAt, actualList[i].CreatedAt);
            Assert.Equal(todoItems[i].UpdatedAt, actualList[i].UpdatedAt);
        }


        mediatorMock.Verify(
            x => x.Send(It.IsAny<GetTodoItemListQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        todoItemRepoMock.Verify(
            x => x.Exists(
                It.IsAny<long>(),
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
        todoItemRepoMock.Verify(
            x => x.Get(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );

        todoItemRepoMock.Verify(
            x => x.Remove(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );

        mediatorMock.VerifyNoOtherCalls();
        todoItemRepoMock.VerifyNoOtherCalls();
    }
}