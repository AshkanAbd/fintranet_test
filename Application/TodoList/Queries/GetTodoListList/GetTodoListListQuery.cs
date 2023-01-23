using Application.Common.Response;
using Infrastructure.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.TodoList.Queries.GetTodoListList;

public class GetTodoListListQuery : IRequest<StdResponse<PaginationModel<GetTodoListListDto>>>
{
    [FromQuery] public int? Page { get; set; }
    [FromQuery] public int? PageSize { get; set; }
}