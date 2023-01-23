using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using DbContext = SoftDeletes.Core.DbContext;

namespace Domain.Models;

[EntityTypeConfiguration(typeof(TodoListConfiguration))]
public class TodoList : SoftDeletes.ModelTools.ModelExtension
{
    public long Id { get; set; }
    [MaxLength(30)] [Required] public string Title { get; set; }

    [Column(TypeName = "varchar(6)")]
    [Required]
    public string Color { get; set; }

    public IEnumerable<TodoItem> TodoItems { get; set; }

    public override async Task OnSoftDeleteAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        await context.RemoveRangeAsync(TodoItems, cancellationToken);
    }

    public override void OnSoftDelete(DbContext context)
    {
        context.RemoveRange(TodoItems);
    }

    public override async Task LoadRelationsAsync(DbContext context, CancellationToken cancellationToken = default)
    {
        await context.Entry(this)
            .Collection(x => x.TodoItems)
            .LoadAsync(cancellationToken);
    }

    public override void LoadRelations(DbContext context)
    {
        context.Entry(this)
            .Collection(x => x.TodoItems)
            .Load();
    }
}