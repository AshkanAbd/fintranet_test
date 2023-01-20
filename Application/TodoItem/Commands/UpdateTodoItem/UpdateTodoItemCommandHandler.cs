using Application.Common;
using Application.Common.Pagination;
using Application.Common.Response;
using Application.Common.Validation;
using Application.TodoItem.Queries.GetTodoItemList;
using Infrastructure.Repositories.TodoItem;
using MediatR;

namespace Application.TodoItem.Commands.UpdateTodoItem;

public class UpdateTodoItemCommandHandler :
    AbstractRequestHandler<UpdateTodoItemCommand, StdResponse<PaginationModel<GetTodoItemListDto>>>
{
    private ITodoItemRepository TodoItemRepository { get; }
    private IMediator Mediator { get; }

    public UpdateTodoItemCommandHandler(ITodoItemRepository todoItemRepository, IMediator mediator)
    {
        TodoItemRepository = todoItemRepository;
        Mediator = mediator;
    }

    public override async Task<StdResponse<PaginationModel<GetTodoItemListDto>>> Handle(UpdateTodoItemCommand request,
        CancellationToken _)
    {
        var validationResult = await new UpdateTodoItemValidator().StdValidateAsync(request, _);
        if (validationResult.Failed()) {
            return ValidationError<PaginationModel<GetTodoItemListDto>>(validationResult.Messages());
        }

        if (!await TodoItemRepository.Exists(request.Id, _)) {
            return NotFoundMsg<PaginationModel<GetTodoItemListDto>>("Todo item notfound.");
        }

        var todoItem = await TodoItemRepository.Update(request.Id, new Domain.Models.TodoItem {
            Title = request.Title!,
            Note = request.Note!,
            Priority = request.Priority!.Value,
        }, _);

        var todoListList = await Mediator.Send(new GetTodoItemListQuery {
            TodoListId = todoItem.TodoListId,
            Page = request.Page,
            PageSize = request.PageSize,
        }, _);
        todoListList.Message = "Todo item updated.";

        return todoListList;
    }
}