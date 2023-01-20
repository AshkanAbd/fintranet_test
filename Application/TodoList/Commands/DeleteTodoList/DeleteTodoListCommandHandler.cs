using Application.Common;
using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoList.Queries.GetTodoListList;
using Infrastructure.Repositories.TodoList;
using MediatR;

namespace Application.TodoList.Commands.DeleteTodoList;

public class DeleteTodoListCommandHandler :
    AbstractRequestHandler<DeleteTodoListCommand, StdResponse<PaginationModel<GetTodoListListDto>>>
{
    private ITodoListRepository TodoListRepository { get; }
    private IMediator Mediator { get; }

    public DeleteTodoListCommandHandler(ITodoListRepository todoListRepository, IMediator mediator)
    {
        TodoListRepository = todoListRepository;
        Mediator = mediator;
    }

    public override async Task<StdResponse<PaginationModel<GetTodoListListDto>>> Handle(DeleteTodoListCommand request,
        CancellationToken _)
    {
        if (!await TodoListRepository.Exists(request.Id, _)) {
            return NotFoundMsg<PaginationModel<GetTodoListListDto>>();
        }

        await TodoListRepository.Remove(request.Id, _);

        var todoListList = await Mediator.Send(new GetTodoListListQuery {
            Page = request.Page,
            PageSize = request.PageSize,
        }, _);
        todoListList.Message = "Todo list removed";

        return todoListList;
    }
}