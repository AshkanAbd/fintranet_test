using Application.Common;
using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoList.Queries.GetTodoListList;
using Infrastructure.Repositories.TodoList;
using Microsoft.AspNetCore.Http;

namespace Application.TodoList.Commands.DeleteTodoList;

public class DeleteTodoListCommandHandler :
    AbstractRequestHandler<DeleteTodoListCommand, StdResponse<PaginationModel<GetTodoListListDto>>>
{
    public ITodoListRepository TodoListRepository { get; }

    public DeleteTodoListCommandHandler(IHttpContextAccessor? httpContextAccessor,
        ITodoListRepository todoListRepository) : base(httpContextAccessor)
    {
        TodoListRepository = todoListRepository;
    }

    public override async Task<StdResponse<PaginationModel<GetTodoListListDto>>> Handle(DeleteTodoListCommand request,
        CancellationToken _)
    {
        if (!await TodoListRepository.Exists(request.Id, _)) {
            return NotFoundMsg<PaginationModel<GetTodoListListDto>>();
        }

        await TodoListRepository.Remove(request.Id, _);

        var todoListList = await Mediator!.Send(new GetTodoListListQuery {
            Page = request.Page,
            PageSize = request.PageSize,
        }, _);
        todoListList.Message = "Todo list removed";

        return todoListList;
    }
}