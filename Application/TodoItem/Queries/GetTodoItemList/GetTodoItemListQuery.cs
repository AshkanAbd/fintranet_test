using Application.Common.Pagination;
using Application.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.TodoItem.Queries.GetTodoItemList;

public class GetTodoItemListQuery : IRequest<StdResponse<PaginationModel<GetTodoItemListDto>>>
{
    [FromRoute] public long TodoListId { get; set; }
    [FromQuery] public int? Page { get; set; }
    [FromQuery] public int? PageSize { get; set; }
}