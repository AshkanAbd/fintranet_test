using Application.TodoItem.Queries.GetTodoItemList;
using Infrastructure.Common.Pagination;

namespace Application.TodoList.Queries.GetTodoList;

public class GetTodoListDto
{
    public long Id { get; set; }
    public string Title { get; set; } = "";
    public string Color { get; set; } = "";
    public PaginationModel<GetTodoItemListDto>? TodoItemList { get; set; }
}