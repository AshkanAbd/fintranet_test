using Application.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.TodoList.Commands.UpdateTodoList;

public class UpdateTodoListCommand : IRequest<StdResponse<UpdateTodoListDto>>
{
    [FromRoute] public long Id { get; set; }
    [FromForm] public string? Title { get; set; }
    [FromForm] public string? Color { get; set; }
}