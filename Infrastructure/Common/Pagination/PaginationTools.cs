using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common.Pagination;

public static class PaginationTools
{
    private static void NormalizePagination(ref int? page, ref int? pageSize)
    {
        if (page <= 0) {
            page = 1;
        }

        if (pageSize <= 0) {
            pageSize = 10;
        }
    }

    public static async Task<PaginationModel<T>> UsePaginationAsync<T>(this IQueryable<T> query, int? page = null,
        int? pageSize = null, CancellationToken cancellationToken = default)
    {
        NormalizePagination(ref page, ref pageSize);
        var iPage = page ?? 1;
        var iPageSize = pageSize ?? 1;
        var count = await query.CountAsync(cancellationToken);

        query = query.Skip((iPage - 1) * iPageSize).Take(iPageSize);

        return new PaginationModel<T> {
            List = await query.ToListAsync(cancellationToken),
            PageCount = (int) Math.Ceiling(count / (double) iPageSize),
            CurrentPage = iPage,
            CurrentPageSize = iPageSize,
            Total = count
        };
    }

    public static PaginationModel<T> UsePagination<T>(this IQueryable<T> query, int? page = null,
        int? pageSize = null)
    {
        NormalizePagination(ref page, ref pageSize);
        var iPage = page ?? 1;
        var iPageSize = pageSize ?? 1;
        var count = query.Count();

        if (iPageSize != -1) {
            query = query.Skip((iPage - 1) * iPageSize).Take(iPageSize);
        }
        else {
            iPageSize = count == 0 ? 1 : count;
        }

        return new PaginationModel<T> {
            List = query.ToList(),
            PageCount = (int) Math.Ceiling(count / (double) iPageSize),
            CurrentPage = iPage,
            CurrentPageSize = iPageSize,
            Total = count
        };
    }
}