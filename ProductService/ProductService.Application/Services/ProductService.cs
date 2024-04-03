using ProductService.Application.Dtos;
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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(CreateProductDto product)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateProductDto product)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

       
    }
}
