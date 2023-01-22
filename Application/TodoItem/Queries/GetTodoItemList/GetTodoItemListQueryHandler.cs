using Application.Common;
using Application.Common.Pagination;
using Application.Common.Response;
using Infrastructure.Repositories.TodoItem;
using Infrastructure.Repositories.TodoList;

namespace Application.TodoItem.Queries.GetTodoItemList;

public class GetTodoItemListQueryHandler :
    AbstractRequestHandler<GetTodoItemListQuery, StdResponse<PaginationModel<GetTodoItemListDto>>>
{
    private ITodoItemRepository TodoItemRepository { get; }
    private ITodoListRepository TodoListRepository { get; }

    public GetTodoItemListQueryHandler(ITodoItemRepository todoItemRepository, ITodoListRepository todoListRepository)
    {
        TodoItemRepository = todoItemRepository;
        TodoListRepository = todoListRepository;
    }

    public override async Task<StdResponse<PaginationModel<GetTodoItemListDto>>> Handle(GetTodoItemListQuery request,
        CancellationToken _)
    {
        if (!await TodoListRepository.Exists(request.TodoListId, _)) {
            return NotFoundMsg<PaginationModel<GetTodoItemListDto>>("Todo list notfound.");
        }

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