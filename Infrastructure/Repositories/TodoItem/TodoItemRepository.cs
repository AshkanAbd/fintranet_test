using Application.Common.Pagination;
using Infrastructure.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.TodoItem;

public class TodoItemRepository : ITodoItemRepository
{
    private AppDbContext DbContext { get; set; }

    public TodoItemRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<Domain.Models.TodoItem> Add(Domain.Models.TodoItem todoItem,
        CancellationToken cancellationToken = default)
    {
        await DbContext.AddAsync(todoItem, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return todoItem;
    }

    public async Task<bool> Exists(long id, CancellationToken cancellationToken = default)
    {
        return await DbContext.TodoItems.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Domain.Models.TodoItem?> Get(long id, CancellationToken cancellationToken = default)
    {
        return await DbContext.TodoItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PaginationModel<Domain.Models.TodoItem>> GetWithPagination(long todoListId, int? page,
        int? pageSize, CancellationToken cancellationToken = default)
    {
        return await DbContext.TodoItems
            .AsNoTracking()
            .Where(x => x.TodoListId == todoListId)
            .OrderByDescending(x => x.Priority)
            .UsePaginationAsync(page, pageSize, cancellationToken);
    }

    public async Task<Domain.Models.TodoItem> Update(long id, Domain.Models.TodoItem todoItem,
        CancellationToken cancellationToken = default)
    {
        var item = (await LoadItem(id, cancellationToken))!;

        item.Title = todoItem.Title;
        item.Note = todoItem.Note;
        item.Priority = todoItem.Priority;

        if (todoItem.TodoListId != 0) {
            item.TodoListId = todoItem.TodoListId;
        }

        await DbContext.SaveChangesAsync(cancellationToken);
        return item;
    }

    public async Task Remove(long id, CancellationToken cancellationToken = default)
    {
        var item = (await LoadItem(id, cancellationToken))!;

        await DbContext.RemoveAsync(item, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Domain.Models.TodoItem?> LoadItem(long id, CancellationToken cancellationToken = default)
    {
        var trackedEntity = DbContext.ChangeTracker.Entries<Domain.Models.TodoItem>()
            .FirstOrDefault(x => x.Entity.Id == id);

        if (trackedEntity == null) {
            return await DbContext.TodoItems.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        return trackedEntity.Entity;
    }
}