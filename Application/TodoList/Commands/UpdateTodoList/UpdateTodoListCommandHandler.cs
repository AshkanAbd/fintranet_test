using Application.Common;
using Application.Common.Response;
using Application.Common.Validation;
using Application.TodoList.Queries.GetTodoListList;
using Infrastructure.Common.Pagination;
using Infrastructure.Repositories.TodoList;
using MediatR;

namespace Application.TodoList.Commands.UpdateTodoList;

public class UpdateTodoListCommandHandler :
    AbstractRequestHandler<UpdateTodoListCommand, StdResponse<PaginationModel<GetTodoListListDto>>>
{
    public UpdateTodoListCommandHandler(ITodoListRepository todoListRepository, IMediator mediator)
    {
        TodoListRepository = todoListRepository;
        Mediator = mediator;
    }

    private ITodoListRepository TodoListRepository { get; }
    private IMediator Mediator { get; }

    public override async Task<StdResponse<PaginationModel<GetTodoListListDto>>> Handle(UpdateTodoListCommand request,
        CancellationToken _)
    {
        var validationResult = await new UpdateTodoListValidator().StdValidateAsync(request, _);
        if (validationResult.Failed()) {
            return ValidationError<PaginationModel<GetTodoListListDto>>(validationResult.Messages());
        }

        if (!await TodoListRepository.Exists(request.Id, _)) {
            return NotFoundMsg<PaginationModel<GetTodoListListDto>>("Todo list notfound.");
        }

        await TodoListRepository.Update(request.Id, new Domain.Models.TodoList {
            Title = request.Title!,
            Color = request.Color!,
        }, _);

        var todoListList = await Mediator.Send(new GetTodoListListQuery {
            Page = request.Page,
            PageSize = request.PageSize,
        }, _);
        todoListList.Message = "Todo list updated.";

        return todoListList;
    }
}