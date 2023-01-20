using Application.Common;
using Application.Common.Pagination;
using Application.Common.Response;
using Infrastructure.Repositories.TodoItem;

namespace Application.TodoItem.Queries.GetTodoItemList;

public class GetTodoItemListQueryHandler :
    AbstractRequestHandler<GetTodoItemListQuery, StdResponse<PaginationModel<GetTodoItemListDto>>>
{
    private ITodoItemRepository TodoItemRepository { get; }

    public GetTodoItemListQueryHandler(ITodoItemRepository todoItemRepository)
    {
        TodoItemRepository = todoItemRepository;
    }

    public override async Task<StdResponse<PaginationModel<GetTodoItemListDto>>> Handle(GetTodoItemListQuery request,
        CancellationToken _)
    {
        var todoItemList = await TodoItemRepository.GetWithPagination(
            request.TodoListId,
            request.Page,
            request.PageSize,
            _
        );

        return Ok(new PaginationModel<GetTodoItemListDto> {
            PageCount = todoItemList.PageCount,
            CurrentPage = todoItemList.CurrentPage,
            CurrentPageSize = todoItemList.CurrentPageSize,
            Total = todoItemList.Total,
            List = todoItemList.List.Select(x => new GetTodoItemListDto {
                Id = x.Id,
                Title = x.Title,
                Note = x.Note,
                Priority = x.Priority,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            })
        });
    }
}