using Application.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.TodoList.Queries.GetTodoList;

public class GetTodoListQuery : IRequest<StdResponse<GetTodoListDto>>
{
    [FromRoute] public long Id { get; set; }
    [FromQuery] public int? Page { get; set; }
    [FromQuery] public int? PageSize { get; set; }
}