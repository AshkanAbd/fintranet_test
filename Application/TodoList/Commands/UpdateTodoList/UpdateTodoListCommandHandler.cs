using Application.Common;
using Application.Common.Response;
using Application.Common.Validation;
using Infrastructure.Repositories.TodoList;
using Microsoft.AspNetCore.Http;

namespace Application.TodoList.Commands.UpdateTodoList;

public class UpdateTodoListCommandHandler :
    AbstractRequestHandler<UpdateTodoListCommand, StdResponse<UpdateTodoListDto>>
{
    public ITodoListRepository TodoListRepository { get; }

    public UpdateTodoListCommandHandler(IHttpContextAccessor? httpContextAccessor,
        ITodoListRepository todoListRepository) : base(httpContextAccessor)
    {
        TodoListRepository = todoListRepository;
    }

    public override async Task<StdResponse<UpdateTodoListDto>> Handle(UpdateTodoListCommand request,
        CancellationToken _)
    {
        var validationResult = await new UpdateTodoListValidator().StdValidateAsync(request, _);
        if (validationResult.Failed()) {
            return ValidationError<UpdateTodoListDto>(validationResult.Messages());
        }

        if (!await TodoListRepository.Exists(request.Id, _)) {
            return NotFoundMsg<UpdateTodoListDto>();
        }

        var todoList = (await TodoListRepository.Get(request.Id, _))!;
        todoList.Title = request.Title!;
        todoList.Color = request.Color!;

        todoList = await TodoListRepository.Update(request.Id, todoList, _);

        return Ok(new UpdateTodoListDto {
            Id = todoList.Id,
            Title = todoList.Title,
            Color = todoList.Color,
            CreatedAt = todoList.CreatedAt,
            UpdatedAt = todoList.UpdatedAt,
        });
    }
}