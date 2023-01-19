using Application.Common.Pagination;
using Infrastructure.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.TodoList;

public class TodoListRepository : ITodoListRepository
{
    public AppDbContext DbContext { get; set; }

    public TodoListRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<Domain.Models.TodoList> Add(Domain.Models.TodoList todoList,
        CancellationToken cancellationToken = default)
    {
        await DbContext.AddAsync(todoList, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return todoList;
    }

    public async Task<Domain.Models.TodoList?> Get(long id, CancellationToken cancellationToken = default)
    {
        return await DbContext.TodoLists.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PaginationModel<Domain.Models.TodoList>> GetWithPagination(int? page, int? pageSize,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.TodoLists.UsePaginationAsync(page, pageSize, cancellationToken);
    }

    public async Task<Domain.Models.TodoList> Update(long id, Domain.Models.TodoList todoList,
        CancellationToken cancellationToken = default)
    {
        var todoList1 = (await LoadItem(id, cancellationToken))!;

        todoList1.Title = todoList.Title;
        todoList1.Color = todoList.Color;

        await DbContext.SaveChangesAsync(cancellationToken);

        return todoList1;
    }

    public async Task Remove(long id, CancellationToken cancellationToken = default)
    {
        var todoList1 = (await LoadItem(id, cancellationToken))!;

        await DbContext.RemoveAsync(todoList1, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Domain.Models.TodoList?> LoadItem(long id, CancellationToken cancellationToken = default)
    {
        var trackedEntity = DbContext.ChangeTracker.Entries<Domain.Models.TodoList>()
            .FirstOrDefault(x => x.Entity.Id == id);

        if (trackedEntity == null) {
            return await DbContext.TodoLists.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        return trackedEntity.Entity;
    }
}