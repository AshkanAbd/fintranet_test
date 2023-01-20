using Application.Common;
using Application.Common.Pagination;
using Application.Common.Response;
using Application.Common.Validation;
using Application.TodoList.Queries.GetTodoListList;
using Infrastructure.Repositories.TodoList;
using Microsoft.AspNetCore.Http;

namespace Application.TodoList.Commands.CreateTodoList;

public class CreateTodoListCommandHandler :
    AbstractRequestHandler<CreateTodoListCommand, StdResponse<PaginationModel<GetTodoListListDto>>>
{
    private ITodoListRepository TodoListRepository;

    public CreateTodoListCommandHandler(IHttpContextAccessor? httpContextAccessor,
        ITodoListRepository todoListRepository) : base(httpContextAccessor)
    {
        TodoListRepository = todoListRepository;
    }

    public override async Task<StdResponse<PaginationModel<GetTodoListListDto>>> Handle(CreateTodoListCommand request,
        CancellationToken _)
    {
        var validationResult = await new CreateTodoListValidator().StdValidateAsync(request, _);
        if (validationResult.Failed()) {
            return ValidationError<PaginationModel<GetTodoListListDto>>(validationResult.Messages());
        }

        await TodoListRepository.Add(new Domain.Models.TodoList {
            Title = request.Title!,
            Color = request.Color!,
        }, _);

        var todoListList = await Mediator!.Send(new GetTodoListListQuery {
            Page = request.Page,
            PageSize = request.PageSize,
        }, _);
        todoListList.Message = "Todo list created.";

        return todoListList;
    }
}