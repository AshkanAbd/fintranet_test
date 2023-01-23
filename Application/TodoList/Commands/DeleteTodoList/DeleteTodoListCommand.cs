using Application.Common.Response;
using Application.TodoList.Queries.GetTodoListList;
using Infrastructure.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.TodoList.Commands.DeleteTodoList;

public class DeleteTodoListCommand : IRequest<StdResponse<PaginationModel<GetTodoListListDto>>>
{
    [FromRoute] public long Id { get; set; }
    [FromQuery] public int? Page { get; set; }
    [FromQuery] public int? PageSize { get; set; }
}