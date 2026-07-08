using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Common
{
    public class PaginatedList<T>
    {
        public IReadOnlyList<T> Items { get; }

        public int PageIndex { get; }

        public int TotalPages { get; }

        public int TotalCount { get; }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        private PaginatedList(
            IReadOnlyList<T> items,
            int count,
            int pageIndex,
            int pageSize)
        {
            Items = items;

            TotalCount = count;

            PageIndex = pageIndex;

            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source,
            int pageIndex,
            int pageSize,
            CancellationToken ct = default)
        {
            var count = await source.CountAsync(ct);

            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PaginatedList<T>(
                items,
                count,
                pageIndex,
                pageSize);
        }
        
    }
}
