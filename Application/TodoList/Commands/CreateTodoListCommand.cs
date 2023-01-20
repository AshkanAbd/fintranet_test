using Application.Common.Response;
using MediatR;

namespace Application.TodoList.Commands;

public class CreateTodoListCommand : IRequest<StdResponse<CreateTodoListDto>>
{
    public string? Title { get; set; }
    public string? Color { get; set; }
}