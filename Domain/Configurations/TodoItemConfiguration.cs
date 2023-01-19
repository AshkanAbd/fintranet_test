using Domain.Common;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations;

public class TodoItemConfiguration : AbstractModelMap<TodoItem>
{
    public override void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.TodoList)
            .WithMany(x => x.TodoItems)
            .HasForeignKey(x => x.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}