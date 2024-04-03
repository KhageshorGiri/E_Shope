using ProductService.Application.Dtos;
using ProductService.Application.MappingProfileExtension;
using ProductService.Application.ServiceInterfaces;
using ProductService.Domain.IRepositories;

namespace ProductService.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepositorycs _productRepository;

        public ProductService(IProductRepositorycs productRepositorycs)
        {
            _productRepository = productRepositorycs;
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product?.ToProductDto();
        }

        public async Task<IEnumerable<ProductDto>?> GetAllAsync()
        {
            var allProducts = await _productRepository.GetAllAsync();
            return allProducts?.Select(product => product.ToProductDto());
        }

        public async Task AddAsync(CreateProductDto product)
        {
            var productToAdd = product.ToProduct();

            productToAdd.CreatedBy = 1;
            productToAdd.ModifiedBy = 1;
            productToAdd.IsDeleted = false;
            productToAdd.CreatedDate = DateTime.UtcNow.Date;
            productToAdd.ModifiedDate = DateTime.UtcNow.Date;

            await _productRepository.AddAsync(productToAdd);
        }

        public async Task UpdateAsync(int id, UpdateProductDto product)
        {
            var productToUpdate = await _productRepository.GetByIdAsync(id);

            if (productToUpdate == null)
            {
                throw new ArgumentOutOfRangeException($"Product with ID {id} not found.");
            }

            productToUpdate.ProductName = product.ProductName;
            productToUpdate.ProductDescription = product.ProductDescription;
            productToUpdate.Price = product.Price;
            productToUpdate.ModifiedBy = 1;
            productToUpdate.ModifiedDate = DateTime.UtcNow.Date;

            await _productRepository.UpdateAsync(productToUpdate);
        }

        public async Task DeleteAsync(int id)
        {
            var productToDelete = await _productRepository.GetByIdAsync(id);

            if (productToDelete == null)
            {
                throw new ArgumentOutOfRangeException($"Product with ID {id} not found.");
            }
            productToDelete.IsDeleted = true;
            await _productRepository.DeleteAsync(productToDelete);
        }

       
    }
}
