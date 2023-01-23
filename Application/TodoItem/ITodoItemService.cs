using Application.Common.Response;
using Application.TodoItem.Commands.CreateTodoItem;
using Application.TodoItem.Commands.DeleteTodoItem;
using Application.TodoItem.Commands.UpdateTodoItem;
using Application.TodoItem.Queries.GetTodoItemList;
using Infrastructure.Common.Pagination;

namespace Application.TodoItem;

public interface ITodoItemService
{
    public Task<StdResponse<PaginationModel<GetTodoItemListDto>>> GetTodoItemList(
        GetTodoItemListQuery query, CancellationToken _
    );

    public Task<StdResponse<PaginationModel<GetTodoItemListDto>>> CreateTodoItem(
        CreateTodoItemCommand command, CancellationToken _
    );

    public Task<StdResponse<PaginationModel<GetTodoItemListDto>>> UpdateTodoItem(
        UpdateTodoItemCommand command, CancellationToken _
    );

    public Task<StdResponse<PaginationModel<GetTodoItemListDto>>> DeleteTodoItem(
        DeleteTodoItemCommand command, CancellationToken _
    );
}