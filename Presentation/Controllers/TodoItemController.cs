using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoItem.Commands.CreateTodoItem;
using Application.TodoItem.Commands.DeleteTodoItem;
using Application.TodoItem.Commands.UpdateTodoItem;
using Application.TodoItem.Queries.GetTodoItemList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;

[ApiExplorerSettings(GroupName = "api")]
public class TodoItemController : ControllerExtension
{
    private IMediator Mediator { get; }

    public TodoItemController(IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet("todoList/{TodoListId}/todoItem")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoItemListDto>>>> GetTodoItem(
        [FromQuery] GetTodoItemListQuery query,
        CancellationToken _
    )
    {
        return FormatResponse(await Mediator.Send(query, _));
    }

    [HttpPost("todoItem")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoItemListDto>>>> CreateTodoItem(
        [FromForm] CreateTodoItemCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await Mediator.Send(command, _));
    }

    [HttpPut("todoItem/{Id}")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoItemListDto>>>> UpdateTodoItem(
        [FromForm] UpdateTodoItemCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await Mediator.Send(command, _));
    }

    [HttpDelete("todoItem/{Id}")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoItemListDto>>>> DeleteTodoItem(
        [FromForm] DeleteTodoItemCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await Mediator.Send(command, _));
    }
}