using Application.Common.Response;
using Application.TodoList.Commands.CreateTodoList;
using Application.TodoList.Commands.DeleteTodoList;
using Application.TodoList.Commands.UpdateTodoList;
using Application.TodoList.Queries.GetTodoList;
using Application.TodoList.Queries.GetTodoListList;
using Infrastructure.Common.Pagination;

namespace Application.TodoList;

public interface ITodoListService
{
    public Task<StdResponse<PaginationModel<GetTodoListListDto>>> GetTodoListList(
        GetTodoListListQuery query, CancellationToken _
    );

    public Task<StdResponse<GetTodoListDto>> GetTodoList(
        GetTodoListQuery query, CancellationToken _
    );

    public Task<StdResponse<PaginationModel<GetTodoListListDto>>> CreateTodoList(
        CreateTodoListCommand command, CancellationToken _
    );

    public Task<StdResponse<PaginationModel<GetTodoListListDto>>> UpdateTodoList(
        UpdateTodoListCommand command, CancellationToken _
    );

    public Task<StdResponse<PaginationModel<GetTodoListListDto>>> DeleteTodoList(
        DeleteTodoListCommand command, CancellationToken _
    );
}