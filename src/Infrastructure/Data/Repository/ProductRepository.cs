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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Product?> GetByIdWithItemsAsync(int id) =>
            await _dbSet.Include(p => p.Items)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IReadOnlyList<Product>> GetAllWithItemsAsync(int pageNumber, int pageSize) =>
            await _dbSet.Include(p => p.Items)
                         .AsNoTracking()
                         .OrderBy(p => p.Id)
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();

        public async Task<int> CountAsync() => await _dbSet.CountAsync();

      
    }
}
