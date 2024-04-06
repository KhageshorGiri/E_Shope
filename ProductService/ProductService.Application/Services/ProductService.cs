using Microsoft.Extensions.Logging;
using ProductService.Application.Dtos;
using ProductService.Application.MappingProfileExtension;
using ProductService.Application.Messaging;
using ProductService.Application.ServiceInterfaces;
using ProductService.Domain.IRepositories;

namespace ProductService.Application.Services
{
    public class ProductsService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IEventPublisher _eventPublisher;
        private ILogger<ProductsService> _logger;
        private readonly string _eventTopic = "product_service_topic";

        public ProductsService(IProductRepository productRepositorycs, IEventPublisher eventPublisher, ILogger<ProductsService> logger)
        {
            _productRepository = productRepositorycs;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request Recived and Forwarding to Repository");
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);
            _logger.LogInformation("Product Service Called successfully for {0} service method", nameof(GetByIdAsync));
            return product?.ToProductDto();
        }

        public async Task<IEnumerable<ProductDto>?> GetAllAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request Recived and Forwarding to Repository");
            var allProducts = await _productRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Product Service Called successfully for {0} service method", nameof(GetAllAsync));
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

            _logger.LogInformation("Request Recived and Forwarding to Repository");
            await _productRepository.AddAsync(productToAdd, cancellationToken);
            _logger.LogInformation("Product Service Called successfully for {0} service method", nameof(AddAsync));
            await _eventPublisher.PublishAsync(_eventTopic, productToAdd.ToString());
            _logger.LogInformation("Product Service Event Published successfully for topic {0}, with {1} service method", _eventTopic, productToAdd.ToString());
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

            _logger.LogInformation("Request Recived and Forwarding to Repository");
            await _productRepository.UpdateAsync(productToUpdate, cancellationToken);
            _logger.LogInformation("Product Service Called successfully for {0} service method", nameof(UpdateAsync));
            await _eventPublisher.PublishAsync(_eventTopic, productToUpdate.ToString());
            _logger.LogInformation("Product Service Event Published successfully for topic {0}, with {1} service method", _eventTopic, productToUpdate.ToString());
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var productToDelete = await _productRepository.GetByIdAsync(id, cancellationToken);

            if (productToDelete == null)
            {
                throw new ArgumentOutOfRangeException($"Product with ID {id} not found.");
            }
            productToDelete.IsDeleted = true;

            _logger.LogInformation("Request Recived and Forwarding to Repository");
            await _productRepository.DeleteAsync(productToDelete, cancellationToken);
            _logger.LogInformation("Product Service Called successfully for {0} service method", nameof(DeleteAsync));
            await _eventPublisher.PublishAsync(_eventTopic, productToDelete.ToString());
            _logger.LogInformation("Product Service Event Published successfully for topic {0}, with {1} service method", _eventTopic, productToDelete.ToString());
        }


    }
}
