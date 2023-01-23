using Application.Common;
using Application.Common.Response;
using Infrastructure.Common.Pagination;
using Infrastructure.Repositories.TodoList;

namespace Application.TodoList.Queries.GetTodoListList;

public class GetTodoListListHandler : AbstractRequestHandler<GetTodoListListQuery,
    StdResponse<PaginationModel<GetTodoListListDto>>>
{
    public GetTodoListListHandler(ITodoListRepository todoListRepository)
    {
        TodoListRepository = todoListRepository;
    }

    private ITodoListRepository TodoListRepository { get; }

    public override async Task<StdResponse<PaginationModel<GetTodoListListDto>>> Handle(GetTodoListListQuery request,
        CancellationToken _)
    {
        var list = await TodoListRepository.GetWithPagination(
            request.Page,
            request.PageSize,
            _
        );

        return Ok(new PaginationModel<GetTodoListListDto> {
            PageCount = list.PageCount,
            CurrentPage = list.CurrentPage,
            CurrentPageSize = list.CurrentPageSize,
            Total = list.Total,
            List = list.List.Select(x => new GetTodoListListDto {
                Id = x.Id,
                Title = x.Title,
                Color = x.Color,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }),
        });
    }
}