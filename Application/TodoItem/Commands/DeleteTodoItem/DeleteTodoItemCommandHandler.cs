using Application.Common;
using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoItem.Queries.GetTodoItemList;
using Infrastructure.Repositories.TodoItem;
using Microsoft.AspNetCore.Http;

namespace Application.TodoItem.Commands.DeleteTodoItem;

public class DeleteTodoItemCommandHandler :
    AbstractRequestHandler<DeleteTodoItemCommand, StdResponse<PaginationModel<GetTodoItemListDto>>>
{
    public ITodoItemRepository TodoItemRepository { get; set; }

    public DeleteTodoItemCommandHandler(IHttpContextAccessor? httpContextAccessor,
        ITodoItemRepository todoItemRepository) : base(httpContextAccessor)
    {
        TodoItemRepository = todoItemRepository;
    }

    public override async Task<StdResponse<PaginationModel<GetTodoItemListDto>>> Handle(DeleteTodoItemCommand request,
        CancellationToken _)
    {
        if (!await TodoItemRepository.Exists(request.Id, _)) {
            return NotFoundMsg<PaginationModel<GetTodoItemListDto>>("Todo item notfound.");
        }

        var todoItem = await TodoItemRepository.Get(request.Id, _);

        await TodoItemRepository.Remove(request.Id, _);

        var todoListList = await Mediator!.Send(new GetTodoItemListQuery {
            TodoListId = todoItem!.TodoListId,
            Page = request.Page,
            PageSize = request.PageSize,
        }, _);
        todoListList.Message = "Todo item deleted.";

        return todoListList;
    }
}