using Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using DbContext = SoftDeletes.Core.DbContext;

namespace Domain.Models;

[EntityTypeConfiguration(typeof(TodoItemConfiguration))]
public class TodoItem : SoftDeletes.ModelTools.ModelExtension
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Note { get; set; }
    public int Priority { get; set; }
    public long TodoListId { get; set; }

    public TodoList TodoList { get; set; }

    public override Task OnSoftDeleteAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override void OnSoftDelete(DbContext context)
    {
    }

    public override Task LoadRelationsAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override void LoadRelations(DbContext context)
    {
    }
}