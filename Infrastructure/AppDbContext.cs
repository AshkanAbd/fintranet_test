using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : SoftDeletes.Core.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<TodoList> TodoLists { get; set; } = null!;
}