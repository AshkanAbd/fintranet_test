using Application.Common.Response;
using Application.TodoItem.Queries.GetTodoItemList;
using Infrastructure.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.TodoItem.Commands.CreateTodoItem;

public class CreateTodoItemCommand : IRequest<StdResponse<PaginationModel<GetTodoItemListDto>>>
{
    [FromForm] public long TodoListId { get; set; }
    [FromForm] public string? Title { get; set; }
    [FromForm] public string? Note { get; set; }
    [FromForm] public int? Priority { get; set; }
    [FromQuery] public int? Page { get; set; }
    [FromQuery] public int? PageSize { get; set; }
}