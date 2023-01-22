using System.Net;
using Application.Common.Pagination;
using Application.TodoList.Queries.GetTodoListList;
using Application.UnitTests.Common;
using Infrastructure.Repositories.TodoList;

namespace Application.UnitTests.TodoList;

public class GetTodoListListQueryUnitTests : AbstractTest
{
    [Fact]
    public async Task GetTodoListListQuery_ShouldReturnListOfTodoLists()
    {
        var todoLists = TodoListFaker.Generate(10);

        var todoListRepoMock = new Mock<ITodoListRepository>();
        todoListRepoMock.Setup(
            x => x.GetWithPagination(
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync((int? page, int? pageSize, CancellationToken _) => {
            return new PaginationModel<Domain.Models.TodoList> {
                PageCount = (int) Math.Ceiling(todoLists.Count / (double) pageSize!),
                CurrentPage = page!.Value,
                CurrentPageSize = pageSize.Value,
                Total = todoLists.Count,
                List = todoLists.Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value)
            };
        });

        var handler = new GetTodoListListHandler(todoListRepoMock.Object);

        var query = new GetTodoListListQuery {
            PageSize = 1,
            Page = 4
        };

        var res = await handler.Handle(query, CancellationToken.None);

        Assert.Equal("Success", res.Message);
        Assert.Equal(HttpStatusCode.OK, res.Status);

        var resValue = res.DataAsDataStruct();

        Assert.NotNull(resValue);
        Assert.Equal(todoLists.Count, resValue!.Total);
        Assert.Equal(query.Page, resValue.CurrentPage);
        Assert.Equal((int) Math.Ceiling(todoLists.Count / (double) query.PageSize), resValue.PageCount);
        Assert.Equal(query.PageSize, resValue.CurrentPageSize);

        var actualList = resValue.List.ToList();
        var expectedList = todoLists.Skip((query.Page!.Value - 1) * query.PageSize!.Value)
            .Take(query.PageSize!.Value)
            .ToList();

        for (var i = 0; i < query.PageSize!.Value; i++) {
            Assert.Equal(expectedList[i].Id, actualList[i].Id);
            Assert.Equal(expectedList[i].Title, actualList[i].Title);
            Assert.Equal(expectedList[i].Color, actualList[i].Color);
            Assert.Equal(expectedList[i].CreatedAt, actualList[i].CreatedAt);
            Assert.Equal(expectedList[i].UpdatedAt, actualList[i].UpdatedAt);
        }

        todoListRepoMock.Verify(
            x => x.GetWithPagination(
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()
            ), Times.Once
        );

        todoListRepoMock.VerifyNoOtherCalls();
    }
}