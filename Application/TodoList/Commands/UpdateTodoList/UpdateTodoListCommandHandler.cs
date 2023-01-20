using Application.Common;
using Application.Common.Pagination;
using Application.Common.Response;
using Application.Common.Validation;
using Application.TodoList.Queries.GetTodoListList;
using Infrastructure.Repositories.TodoList;
using Microsoft.AspNetCore.Http;

namespace Application.TodoList.Commands.UpdateTodoList;

public class UpdateTodoListCommandHandler :
    AbstractRequestHandler<UpdateTodoListCommand, StdResponse<PaginationModel<GetTodoListListDto>>>
{
    public ITodoListRepository TodoListRepository { get; }

    public UpdateTodoListCommandHandler(IHttpContextAccessor? httpContextAccessor,
        ITodoListRepository todoListRepository) : base(httpContextAccessor)
    {
        TodoListRepository = todoListRepository;
    }

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

        var todoListList = await Mediator!.Send(new GetTodoListListQuery {
            Page = request.Page,
            PageSize = request.PageSize,
        }, _);
        todoListList.Message = "Todo list updated.";

        return todoListList;
    }
}