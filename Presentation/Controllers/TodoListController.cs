using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoList.Commands.CreateTodoList;
using Application.TodoList.Commands.DeleteTodoList;
using Application.TodoList.Commands.UpdateTodoList;
using Application.TodoList.Queries.GetTodoList;
using Application.TodoList.Queries.GetTodoListList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;

[ApiExplorerSettings(GroupName = "api")]
public class TodoListController : ControllerExtension
{
    private IMediator Mediator { get; }

    public TodoListController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet("todoList")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoListListDto>>>> GetTodoListList(
        [FromQuery] GetTodoListListQuery query,
        CancellationToken _
    )
    {
        return FormatResponse(await Mediator.Send(query, _));
    }

    [HttpGet("todoList/{Id:long}")]
    public async Task<ActionResult<StdResponse<GetTodoListDto>>> GetTodoList(
        [FromQuery] GetTodoListQuery query,
        CancellationToken _
    )
    {
        return FormatResponse(await Mediator.Send(query, _));
    }

    [HttpPost("todoList")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoListListDto>>>> CreateTodoList(
        [FromForm] CreateTodoListCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await Mediator.Send(command, _));
    }

    [HttpPut("todoList/{Id}")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoListListDto>>>> UpdateTodoList(
        [FromForm] UpdateTodoListCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await Mediator.Send(command, _));
    }

    [HttpDelete("todoList/{Id}")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoListListDto>>>> DeleteTodoList(
        [FromForm] DeleteTodoListCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await Mediator.Send(command, _));
    }
}