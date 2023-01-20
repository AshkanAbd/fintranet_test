using Application.Common;
using Application.Common.Response;
using Application.Common.Validation;
using Infrastructure.Repositories.TodoList;
using Microsoft.AspNetCore.Http;

namespace Application.TodoList.Commands;

public class CreateTodoListCommandHandler :
    AbstractRequestHandler<CreateTodoListCommand, StdResponse<CreateTodoListDto>>
{
    private ITodoListRepository TodoListRepository;

    public CreateTodoListCommandHandler(IHttpContextAccessor? httpContextAccessor,
        ITodoListRepository todoListRepository) : base(httpContextAccessor)
    {
        TodoListRepository = todoListRepository;
    }

    public override async Task<StdResponse<CreateTodoListDto>> Handle(CreateTodoListCommand request,
        CancellationToken _)
    {
        var validationResult = await new CreateTodoListValidator().StdValidateAsync(request, _);
        if (validationResult.Failed()) {
            return ValidationError<CreateTodoListDto>(validationResult.Messages());
        }

        var todoList = await TodoListRepository.Add(new Domain.Models.TodoList {
            Title = request.Title!,
            Color = request.Color!,
        }, _);

        return Ok(new CreateTodoListDto {
            Id = todoList.Id,
            Title = todoList.Title,
            Color = todoList.Color,
            CreatedAt = todoList.CreatedAt,
        });
    }
}