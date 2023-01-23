using Application.Common.Response;
using Application.TodoItem.Queries.GetTodoItemList;
using Infrastructure.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.TodoItem.Commands.DeleteTodoItem;

public class DeleteTodoItemCommand : IRequest<StdResponse<PaginationModel<GetTodoItemListDto>>>
{
    [FromRoute] public long Id { get; set; }
    [FromQuery] public int? Page { get; set; }
    [FromQuery] public int? PageSize { get; set; }
}