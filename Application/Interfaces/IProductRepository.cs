using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetByIdWithItemsAsync(int id);
        Task<IReadOnlyList<Product>> GetAllWithItemsAsync(int pageNumber, int pageSize);
        Task<int> CountAsync();
    }
}
