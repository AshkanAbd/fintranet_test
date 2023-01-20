using Application.Common.Pagination;
using Application.Common.Response;
using Application.TodoItem.Queries.GetTodoItemList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.TodoItem.Commands.UpdateTodoItem;

public class UpdateTodoItemCommand : IRequest<StdResponse<PaginationModel<GetTodoItemListDto>>>
{
    [FromRoute] public long Id { get; set; }
    [FromForm] public string? Title { get; set; }
    [FromForm] public string? Note { get; set; }
    [FromForm] public int? Priority { get; set; }
    [FromQuery] public int? Page { get; set; }
    [FromQuery] public int? PageSize { get; set; }
}