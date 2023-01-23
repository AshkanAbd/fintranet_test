using Application.Common.Response;
using Application.TodoList.Queries.GetTodoListList;
using Infrastructure.Common.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.TodoList.Commands.CreateTodoList;

public class CreateTodoListCommand : IRequest<StdResponse<PaginationModel<GetTodoListListDto>>>
{
    [FromForm] public string? Title { get; set; }
    [FromForm] public string? Color { get; set; }
    [FromQuery] public int? Page { get; set; }
    [FromQuery] public int? PageSize { get; set; }
}