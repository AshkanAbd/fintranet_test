namespace Application.TodoItem.Queries.GetTodoItemList;

public class GetTodoItemListDto
{
    public long Id { get; set; }
    public string Title { get; set; } = "";
    public string Note { get; set; } = "";
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}