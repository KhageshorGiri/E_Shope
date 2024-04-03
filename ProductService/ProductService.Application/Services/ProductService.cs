using ProductService.Application.Dtos;
using ProductService.Application.MappingProfileExtension;
using ProductService.Application.ServiceInterfaces;
using ProductService.Domain.IRepositories;
using System.Threading;

namespace ProductService.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepositorycs _productRepository;

        public ProductService(IProductRepositorycs productRepositorycs)
        {
            _productRepository = productRepositorycs;
        }

        public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
            return product?.ToProductDto();
        }

        public async Task<IEnumerable<ProductDto>?> GetAllAsync(CancellationToken cancellationToken)
        {
            var allProducts = await _productRepository.GetAllAsync(cancellationToken);
            return allProducts?.Select(product => product.ToProductDto());
        }

        public async Task AddAsync(CreateProductDto product, CancellationToken cancellationToken)
        {
            var productToAdd = product.ToProduct();

            productToAdd.CreatedBy = 1;
            productToAdd.ModifiedBy = 1;
            productToAdd.IsDeleted = false;
            productToAdd.CreatedDate = DateTime.UtcNow.Date;
            productToAdd.ModifiedDate = DateTime.UtcNow.Date;

            await _productRepository.AddAsync(productToAdd, cancellationToken);
        }

        public async Task UpdateAsync(int id, UpdateProductDto product, CancellationToken cancellationToken)
        {
            var productToUpdate = await _productRepository.GetByIdAsync(id, cancellationToken);

            if (productToUpdate == null)
            {
                throw new ArgumentOutOfRangeException($"Product with ID {id} not found.");
            }

            productToUpdate.ProductName = product.ProductName;
            productToUpdate.ProductDescription = product.ProductDescription;
            productToUpdate.Price = product.Price;
            productToUpdate.ModifiedBy = 1;
            productToUpdate.ModifiedDate = DateTime.UtcNow.Date;

            await _productRepository.UpdateAsync(productToUpdate, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var productToDelete = await _productRepository.GetByIdAsync(id, cancellationToken);

            if (productToDelete == null)
            {
                throw new ArgumentOutOfRangeException($"Product with ID {id} not found.");
            }
            productToDelete.IsDeleted = true;
            await _productRepository.DeleteAsync(productToDelete, cancellationToken);
        }

       
    }
}
