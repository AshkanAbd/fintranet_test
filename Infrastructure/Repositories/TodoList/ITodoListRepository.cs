using Infrastructure.Common.Pagination;

namespace Infrastructure.Repositories.TodoList;

public interface ITodoListRepository
{
    public Task<Domain.Models.TodoList> Add(Domain.Models.TodoList todoList,
        CancellationToken cancellationToken = default);

    public Task<bool> Exists(long id, CancellationToken cancellationToken = default);

    public Task<Domain.Models.TodoList?> Get(long id, CancellationToken cancellationToken = default);

    public Task<PaginationModel<Domain.Models.TodoList>> GetWithPagination(int? page, int? pageSize,
        CancellationToken cancellationToken = default);

    public Task<Domain.Models.TodoList> Update(long id, Domain.Models.TodoList todoList,
        CancellationToken cancellationToken = default);

    public Task Remove(long id, CancellationToken cancellationToken = default);
}