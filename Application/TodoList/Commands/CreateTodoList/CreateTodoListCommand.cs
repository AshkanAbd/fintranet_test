using Application.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.TodoList.Commands.CreateTodoList;

public class CreateTodoListCommand : IRequest<StdResponse<CreateTodoListDto>>
{
    [FromForm] public string? Title { get; set; }
    [FromForm] public string? Color { get; set; }
}