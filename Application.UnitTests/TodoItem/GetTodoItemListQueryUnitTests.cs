using System.Net;
using Application.Common.Pagination;
using Application.TodoItem.Queries.GetTodoItemList;
using Application.UnitTests.Common;
using Infrastructure.Repositories.TodoItem;

namespace Application.UnitTests.TodoItem;

public class GetTodoItemListQueryUnitTests : AbstractTest
{
    [Fact]
    public async Task GetTodoItemListQuery_ShouldReturnListOfTodoItems()
    {
        var todoItems = TodoItemFaker.Generate(10);
        todoItems.ForEach(x => x.TodoListId = 1);
        var todoItemRepoMock = new Mock<ITodoItemRepository>();

        todoItemRepoMock.Setup(
            x => x.GetWithPagination(
                It.IsAny<long>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync((long _, int page, int pageSize, CancellationToken _) =>
            new PaginationModel<Domain.Models.TodoItem> {
                List = todoItems.Take(pageSize),
                PageCount = (int) Math.Ceiling(todoItems.Count / (double) pageSize),
                CurrentPage = page,
                CurrentPageSize = pageSize,
                Total = todoItems.Count,
            }
        );

        var handler = new GetTodoItemListQueryHandler(todoItemRepoMock.Object);

        var query = new GetTodoItemListQuery {
            TodoListId = 1,
            Page = 1,
            PageSize = 5,
        };
        var res = await handler.Handle(query, CancellationToken.None);

        Assert.Equal("Success", res.Message);
        Assert.Equal(HttpStatusCode.OK, res.Status);
        var resValue = res.DataAsDataStruct();
        Assert.NotNull(resValue);
        Assert.Equal(query.Page, resValue!.CurrentPage);
        Assert.Equal(query.PageSize, resValue.CurrentPageSize);
        Assert.Equal((int) Math.Ceiling(todoItems.Count / (double) query.PageSize), resValue.PageCount);
        Assert.Equal(todoItems.Count, resValue.Total);
        Assert.Equal(query.PageSize, resValue.List.Count());

        var actualList = resValue.List.ToList();
        for (var i = 0; i < query.PageSize; i++) {
            Assert.Equal(todoItems[i].Id, actualList[i].Id);
            Assert.Equal(todoItems[i].Title, actualList[i].Title);
            Assert.Equal(todoItems[i].Note, actualList[i].Note);
            Assert.Equal(todoItems[i].Priority, actualList[i].Priority);
            Assert.Equal(todoItems[i].CreatedAt, actualList[i].CreatedAt);
            Assert.Equal(todoItems[i].UpdatedAt, actualList[i].UpdatedAt);
        }

        todoItemRepoMock.Verify(x => x.GetWithPagination(
            It.IsAny<long>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            CancellationToken.None
        ), Times.Once);
        todoItemRepoMock.VerifyNoOtherCalls();
    }
}