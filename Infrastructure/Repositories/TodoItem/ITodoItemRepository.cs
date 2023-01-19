using Application.Common.Pagination;

namespace Infrastructure.Repositories.TodoItem;

public interface ITodoItemRepository
{
    public Task<Domain.Models.TodoItem> Add(Domain.Models.TodoItem todoItem,
        CancellationToken cancellationToken = default);

    public Task<Domain.Models.TodoItem?> Get(long id, CancellationToken cancellationToken = default);

    public Task<PaginationModel<Domain.Models.TodoItem>> GetWithPagination(int? page, int? pageSize,
        CancellationToken cancellationToken = default);

    public Task<Domain.Models.TodoItem> Update(long id, Domain.Models.TodoItem todoItem,
        CancellationToken cancellationToken = default);

    public Task Remove(long id, CancellationToken cancellationToken = default);
}