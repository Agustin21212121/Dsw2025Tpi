using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Exceptions;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public class ProductsManagementService
    {
        private readonly IRepository _repository;

        public ProductsManagementService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductModel.ProductResponse> CreateProduct(ProductModel.ProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Sku))
                throw new ArgumentException("El SKU no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("El nombre no puede estar vacío.");

            if (request.CurrentUnitPrice <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor a cero.");

            if (request.StockQuantity < 0)
                throw new ArgumentException("La cantidad en stock no puede ser negativa.");

            var existing = await _repository.First<Product>(p => p.Sku == request.Sku);
            if (existing != null)
            {
                throw new DuplicatedEntityException($"Ya existe un producto con el SKU {request.Sku}");
            }

            // Mapear el DTO de entrada (request) a la entidad Product
            var product = new Product
            {
                Sku = request.Sku,
                InternalCode = request.InternalCode,
                Name = request.Name,
                Description = request.Description,
                CurrentUnitPrice = request.CurrentUnitPrice,
                StockQuantity = request.StockQuantity,
                IsActive = true
            };

            // Guardar en la base de datos
            var saved = await _repository.Add(product);

            // Mapear el resultado a ProductDto.Response
            return new ProductModel.ProductResponse(
                saved.Id,
                saved.Sku,
                saved.InternalCode,
                saved.Name,
                saved.Description,
                saved.CurrentUnitPrice,
                saved.StockQuantity,
                saved.IsActive
            );
        }

        public async Task<IEnumerable<ProductModel.ProductResponse>> GetProducts()
        {
            // Traemos todos los productos activos
            var products = await _repository.GetFiltered<Product>(p => p.IsActive);

            if (!products.Any())
                throw new EntityNotFoundException("No hay productos activos.");


            // Mapear la lista de entidades Product a lista de DTOs Response
            return products.Select(p => new ProductModel.ProductResponse(
                p.Id,
                p.Sku,
                p.InternalCode,
                p.Name,
                p.Description,
                p.CurrentUnitPrice,
                p.StockQuantity,
                p.IsActive
            ));
        }

        public async Task<ProductModel.ProductResponse?> GetProductById(Guid id)
        {
            var product = await _repository.GetById<Product>(id);

            if (product == null || !product.IsActive)
                throw new EntityNotFoundException($"Producto con ID {id} no encontrado.");

            return new ProductModel.ProductResponse(
                product.Id,
                product.Sku,
                product.InternalCode,
                product.Name,
                product.Description,
                product.CurrentUnitPrice,
                product.StockQuantity,
                product.IsActive
            );
        }

        public async Task<ProductModel.ProductResponse?> UpdateProduct(Guid id, ProductModel.ProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Sku))
                throw new ArgumentException("El SKU no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("El nombre no puede estar vacío.");

            if (request.CurrentUnitPrice <= 0)
                throw new ArgumentException("El precio unitario debe ser mayor a cero.");

            if (request.StockQuantity < 0)
                throw new ArgumentException("La cantidad en stock no puede ser negativa.");

            var product = await _repository.GetById<Product>(id);
            if (product == null || !product.IsActive)
                throw new EntityNotFoundException($"Producto con ID {id} no encontrado.");

            var existing = await _repository.First<Product>(p => p.Sku == request.Sku && p.Id != id);
            if (existing != null)
                throw new DuplicatedEntityException($"Ya existe un producto con el SKU {request.Sku}.");

            product.Sku = request.Sku;
            product.InternalCode = request.InternalCode;
            product.Name = request.Name;
            product.Description = request.Description;
            product.CurrentUnitPrice = request.CurrentUnitPrice;
            product.StockQuantity = request.StockQuantity;

            var updated = await _repository.Update(product);

            return new ProductModel.ProductResponse(
                updated.Id,
                updated.Sku,
                updated.InternalCode,
                updated.Name,
                updated.Description,
                updated.CurrentUnitPrice,
                updated.StockQuantity,
                updated.IsActive
            );
        }

        public async Task<bool> DeactivateProduct(Guid id)
        {
            var product = await _repository.GetById<Product>(id);
            if (product == null || !product.IsActive)
                throw new EntityNotFoundException($"Producto con ID {id} no encontrado.");


            product.IsActive = false;
            await _repository.Update(product);
            return true;
        }



    }
}
