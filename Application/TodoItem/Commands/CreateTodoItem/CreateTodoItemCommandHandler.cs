using Application.Common;
using Application.Common.Pagination;
using Application.Common.Response;
using Application.Common.Validation;
using Application.TodoItem.Queries.GetTodoItemList;
using Infrastructure.Repositories.TodoItem;
using Infrastructure.Repositories.TodoList;
using Microsoft.AspNetCore.Http;

namespace Application.TodoItem.Commands.CreateTodoItem;

public class CreateTodoItemCommandHandler :
    AbstractRequestHandler<CreateTodoItemCommand, StdResponse<PaginationModel<GetTodoItemListDto>>>
{
    public ITodoItemRepository TodoItemRepository { get; }
    public ITodoListRepository TodoListRepository { get; set; }

    public CreateTodoItemCommandHandler(IHttpContextAccessor? httpContextAccessor,
        ITodoItemRepository todoItemRepository, ITodoListRepository todoListRepository) : base(httpContextAccessor)
    {
        TodoItemRepository = todoItemRepository;
        TodoListRepository = todoListRepository;
    }

    public override async Task<StdResponse<PaginationModel<GetTodoItemListDto>>> Handle(CreateTodoItemCommand request,
        CancellationToken _)
    {
        var validationResult = await new CreateTodoItemValidator().StdValidateAsync(request, _);
        if (validationResult.Failed()) {
            return ValidationError<PaginationModel<GetTodoItemListDto>>(validationResult.Messages());
        }

        if (!await TodoListRepository.Exists(request.TodoListId, _)) {
            return NotFoundMsg<PaginationModel<GetTodoItemListDto>>("Todo list notfound.");
        }

        await TodoItemRepository.Add(new Domain.Models.TodoItem {
            Title = request.Title!,
            Note = request.Note!,
            Priority = request.Priority!.Value,
            TodoListId = request.TodoListId,
        }, _);

        var todoListList = await Mediator!.Send(new GetTodoItemListQuery {
            TodoListId = request.TodoListId,
            Page = request.Page,
            PageSize = request.PageSize,
        }, _);
        todoListList.Message = "Todo item created.";

        return todoListList;
    }
}