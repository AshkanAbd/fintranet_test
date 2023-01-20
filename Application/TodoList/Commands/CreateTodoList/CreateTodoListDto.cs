namespace Application.TodoList.Commands.CreateTodoList;

public class CreateTodoListDto
{
    public long Id { get; set; }
    public string Title { get; set; } = "";
    public string Color { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}