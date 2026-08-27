using Application.DTOs;
using Application.DTOs.Item;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ItemDto> GetByIdAsync(int id)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Item), id);

            return _mapper.Map<ItemDto>(item);
        }

        public async Task<IReadOnlyList<ItemDto>> GetAllAsync()
        {
            var items = await _unitOfWork.Items.GetAllAsync();
            return _mapper.Map<List<ItemDto>>(items);
        }

        public async Task<IReadOnlyList<ItemDto>> GetByProductIdAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId)
                ?? throw new NotFoundException(nameof(Product), productId);

            var items = await _unitOfWork.Items.GetByProductIdAsync(productId);
            return _mapper.Map<List<ItemDto>>(items);
        }

        public async Task<ItemDto> CreateAsync(CreateItemDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId)
                ?? throw new NotFoundException(nameof(Product), dto.ProductId);

            var item = _mapper.Map<Item>(dto);

            await _unitOfWork.Items.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ItemDto>(item);
        }

        public async Task UpdateAsync(int id, UpdateItemDto dto)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Item), id);

            item.Quantity = dto.Quantity;

            _unitOfWork.Items.Update(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _unitOfWork.Items.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Item), id);

            _unitOfWork.Items.Remove(item);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
