using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoList.Commands.CreateTodoList;
using Application.TodoList.Commands.DeleteTodoList;
using Application.TodoList.Commands.UpdateTodoList;
using Application.TodoList.Queries.GetTodoList;
using Application.TodoList.Queries.GetTodoListList;
using MediatR;

namespace Application.TodoList;

public class TodoListService : ITodoListService
{
    public TodoListService(IMediator mediator)
    {
        Mediator = mediator;
    }

    private IMediator Mediator { get; }

    public async Task<StdResponse<PaginationModel<GetTodoListListDto>>> GetTodoListList(GetTodoListListQuery query,
        CancellationToken _)
    {
        return await Mediator.Send(query, _);
    }

    public async Task<StdResponse<GetTodoListDto>> GetTodoList(GetTodoListQuery query,
        CancellationToken _)
    {
        return await Mediator.Send(query, _);
    }

    public async Task<StdResponse<PaginationModel<GetTodoListListDto>>> CreateTodoList(CreateTodoListCommand command,
        CancellationToken _)
    {
        return await Mediator.Send(command, _);
    }

    public async Task<StdResponse<PaginationModel<GetTodoListListDto>>> UpdateTodoList(UpdateTodoListCommand command,
        CancellationToken _)
    {
        return await Mediator.Send(command, _);
    }

    public async Task<StdResponse<PaginationModel<GetTodoListListDto>>> DeleteTodoList(DeleteTodoListCommand command,
        CancellationToken _)
    {
        return await Mediator.Send(command, _);
    }
}