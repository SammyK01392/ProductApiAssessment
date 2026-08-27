using Application.DTOs;
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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithItemsAsync(id)
                ?? throw new NotFoundException(nameof(Product), id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<PagedResultDto<ProductDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var products = await _unitOfWork.Products.GetAllWithItemsAsync(pageNumber, pageSize);
            var totalCount = await _unitOfWork.Products.CountAsync();

            return new PagedResultDto<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(products),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Product), id);

            product.ProductName = dto.ProductName;
            product.ModifiedBy = dto.ModifiedBy;
            product.ModifiedOn = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Product), id);

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
