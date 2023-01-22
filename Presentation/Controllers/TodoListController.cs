using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoList;
using Application.TodoList.Commands.CreateTodoList;
using Application.TodoList.Commands.DeleteTodoList;
using Application.TodoList.Commands.UpdateTodoList;
using Application.TodoList.Queries.GetTodoList;
using Application.TodoList.Queries.GetTodoListList;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;

[ApiExplorerSettings(GroupName = "api")]
public class TodoListController : ControllerExtension
{
    public TodoListController(ITodoListService todoListService)
    {
        TodoListService = todoListService;
    }

    private ITodoListService TodoListService { get; }

    [HttpGet("todoList")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoListListDto>>>> GetTodoListList(
        [FromQuery] GetTodoListListQuery query,
        CancellationToken _
    )
    {
        return FormatResponse(await TodoListService.GetTodoListList(query, _));
    }

    [HttpGet("todoList/{Id:long}")]
    public async Task<ActionResult<StdResponse<GetTodoListDto>>> GetTodoList(
        [FromQuery] GetTodoListQuery query,
        CancellationToken _
    )
    {
        return FormatResponse(await TodoListService.GetTodoList(query, _));
    }

    [HttpPost("todoList")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoListListDto>>>> CreateTodoList(
        [FromForm] CreateTodoListCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await TodoListService.CreateTodoList(command, _));
    }

    [HttpPut("todoList/{Id}")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoListListDto>>>> UpdateTodoList(
        [FromForm] UpdateTodoListCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await TodoListService.UpdateTodoList(command, _));
    }

    [HttpDelete("todoList/{Id}")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoListListDto>>>> DeleteTodoList(
        [FromForm] DeleteTodoListCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await TodoListService.DeleteTodoList(command, _));
    }
}