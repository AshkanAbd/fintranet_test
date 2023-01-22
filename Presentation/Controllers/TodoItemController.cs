using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoItem;
using Application.TodoItem.Commands.CreateTodoItem;
using Application.TodoItem.Commands.DeleteTodoItem;
using Application.TodoItem.Commands.UpdateTodoItem;
using Application.TodoItem.Queries.GetTodoItemList;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers;

[ApiExplorerSettings(GroupName = "api")]
public class TodoItemController : ControllerExtension
{
    public TodoItemController(ITodoItemService todoItemService)
    {
        TodoItemService = todoItemService;
    }

    private ITodoItemService TodoItemService { get; }

    [HttpGet("todoList/{TodoListId}/todoItem")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoItemListDto>>>> GetTodoItemList(
        [FromQuery] GetTodoItemListQuery query,
        CancellationToken _
    )
    {
        return FormatResponse(await TodoItemService.GetTodoItemList(query, _));
    }

    [HttpPost("todoItem")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoItemListDto>>>> CreateTodoItem(
        [FromForm] CreateTodoItemCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await TodoItemService.CreateTodoItem(command, _));
    }

    [HttpPut("todoItem/{Id}")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoItemListDto>>>> UpdateTodoItem(
        [FromForm] UpdateTodoItemCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await TodoItemService.UpdateTodoItem(command, _));
    }

    [HttpDelete("todoItem/{Id}")]
    public async Task<ActionResult<StdResponse<PaginationModel<GetTodoItemListDto>>>> DeleteTodoItem(
        [FromForm] DeleteTodoItemCommand command,
        CancellationToken _
    )
    {
        return FormatResponse(await TodoItemService.DeleteTodoItem(command, _));
    }
}