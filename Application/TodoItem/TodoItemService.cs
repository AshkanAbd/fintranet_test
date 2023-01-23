using Application.Common.Response;
using Application.TodoItem.Commands.CreateTodoItem;
using Application.TodoItem.Commands.DeleteTodoItem;
using Application.TodoItem.Commands.UpdateTodoItem;
using Application.TodoItem.Queries.GetTodoItemList;
using Infrastructure.Common.Pagination;
using MediatR;

namespace Application.TodoItem;

public class TodoItemService : ITodoItemService
{
    public TodoItemService(IMediator mediator)
    {
        Mediator = mediator;
    }

    private IMediator Mediator { get; }

    public async Task<StdResponse<PaginationModel<GetTodoItemListDto>>> GetTodoItemList(GetTodoItemListQuery query,
        CancellationToken _)
    {
        return await Mediator.Send(query, _);
    }

    public async Task<StdResponse<PaginationModel<GetTodoItemListDto>>> CreateTodoItem(CreateTodoItemCommand command,
        CancellationToken _)
    {
        return await Mediator.Send(command, _);
    }

    public async Task<StdResponse<PaginationModel<GetTodoItemListDto>>> UpdateTodoItem(UpdateTodoItemCommand command,
        CancellationToken _)
    {
        return await Mediator.Send(command, _);
    }

    public async Task<StdResponse<PaginationModel<GetTodoItemListDto>>> DeleteTodoItem(DeleteTodoItemCommand command,
        CancellationToken _)
    {
        return await Mediator.Send(command, _);
    }
}