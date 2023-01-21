using System.Net;
using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoItem.Commands.CreateTodoItem;
using Application.TodoItem.Queries.GetTodoItemList;
using Application.UnitTests.Common;
using Infrastructure.Repositories.TodoItem;
using Infrastructure.Repositories.TodoList;
using MediatR;

namespace Application.UnitTests.TodoItem;

public class CreateTodoItemCommandItemUnitTests : AbstractTest
{
    [Fact]
    public async Task CreateTodoItemCommand_ShouldCreateTodoItem()
    {
        var todoItems = TodoItemFaker.Generate(10);
        todoItems.ForEach(x => x.TodoListId = 1);

        var todoListRepoMock = new Mock<ITodoListRepository>();
        var todoItemRepoMock = new Mock<ITodoItemRepository>();

        todoListRepoMock.Setup(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

        todoItemRepoMock.Setup(
            x => x.Add(It.IsAny<Domain.Models.TodoItem>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync((Domain.Models.TodoItem x, CancellationToken _) => x);

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

        var handler = new CreateTodoItemCommandHandler(
            todoItemRepoMock.Object,
            todoListRepoMock.Object,
            mediatorMock.Object
        );

        var command = new CreateTodoItemCommand {
            TodoListId = 1,
            Title = Faker.Lorem.Sentence(3),
            Note = Faker.Lorem.Sentences(6),
            Priority = Faker.Random.Int(1, 5),
            Page = 1,
            PageSize = 3,
        };

        var res = await handler.Handle(command, CancellationToken.None);
        Assert.Equal("Todo item created.", res.Message);
        Assert.Equal(HttpStatusCode.OK, res.Status);

        var resValue = res.DataAsDataStruct();
        Assert.NotNull(resValue);
        Assert.Equal(command.Page, resValue!.CurrentPage);
        Assert.Equal(command.PageSize, resValue.CurrentPageSize);
        Assert.Equal((int) Math.Ceiling(todoItems.Count / (double) command.PageSize), resValue.PageCount);
        Assert.Equal(todoItems.Count, resValue.Total);
        Assert.Equal(command.PageSize, resValue.List.Count());

        var acutalList = resValue.List.ToList();
        for (var i = 0; i < command.PageSize; i++) {
            Assert.Equal(todoItems[i].Id, acutalList[i].Id);
            Assert.Equal(todoItems[i].Title, acutalList[i].Title);
            Assert.Equal(todoItems[i].Note, acutalList[i].Note);
            Assert.Equal(todoItems[i].Priority, acutalList[i].Priority);
            Assert.Equal(todoItems[i].CreatedAt, acutalList[i].CreatedAt);
            Assert.Equal(todoItems[i].UpdatedAt, acutalList[i].UpdatedAt);
        }


        mediatorMock.Verify(
            x => x.Send(It.IsAny<GetTodoItemListQuery>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        todoItemRepoMock.Verify(
            x => x.Add(It.IsAny<Domain.Models.TodoItem>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        todoListRepoMock.Verify(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateTodoItemCommand_ShouldReturnNotfoundWhenTodoListDoesnotExists()
    {
        var todoListRepoMock = new Mock<ITodoListRepository>();
        todoListRepoMock.Setup(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        var todoItemRepoMock = new Mock<ITodoItemRepository>();
        var mediatorMock = new Mock<IMediator>();

        var handler = new CreateTodoItemCommandHandler(
            todoItemRepoMock.Object,
            todoListRepoMock.Object,
            mediatorMock.Object
        );

        var command = new CreateTodoItemCommand {
            TodoListId = 1,
            Title = Faker.Lorem.Sentence(3),
            Note = Faker.Lorem.Sentences(6),
            Priority = Faker.Random.Int(1, 5),
            Page = 1,
            PageSize = 3,
        };

        var res = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Todo list notfound.", res.Message);
        Assert.Equal(HttpStatusCode.NotFound, res.Status);
        Assert.Null(res.Data);

        todoListRepoMock.Verify(
            x => x.Exists(It.IsAny<long>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}