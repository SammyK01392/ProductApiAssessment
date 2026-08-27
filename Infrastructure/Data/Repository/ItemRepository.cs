using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repository
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Item>> GetByProductIdAsync(int productId) =>
            await _dbSet.AsNoTracking()
                         .Where(i => i.ProductId == productId)
                         .ToListAsync();
    }
}
