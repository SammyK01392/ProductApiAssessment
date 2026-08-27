using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Infrastructure.Data;

namespace ProductApiAssessment.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IItemService, ItemService>();

            // AutoMapper 16+ syntax: config action comes first, then assemblies
            services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

            services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();

            return services;
        }
    }
}