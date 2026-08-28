using Application.DTOs;
using Application.DTOs.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IItemService
    {
        Task<ItemDto> GetByIdAsync(int id);
        Task<IReadOnlyList<ItemDto>> GetAllAsync();
        Task<IReadOnlyList<ItemDto>> GetByProductIdAsync(int productId);
        Task<ItemDto> CreateAsync(CreateItemDto dto);
        Task UpdateAsync(int id, UpdateItemDto dto);
        Task DeleteAsync(int id);
    }
}
