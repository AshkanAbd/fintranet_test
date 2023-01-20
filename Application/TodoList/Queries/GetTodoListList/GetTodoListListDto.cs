namespace Application.TodoList.Queries.GetTodoListList;

public class GetTodoListListDto
{
    public long Id { get; set; }
    public string Title { get; set; } = "";
    public string Color { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}