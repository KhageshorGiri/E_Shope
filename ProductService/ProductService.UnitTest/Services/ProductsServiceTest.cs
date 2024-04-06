using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ProductService.Application.Dtos;
using ProductService.Application.MappingProfileExtension;
using ProductService.Application.Messaging;
using ProductService.Application.Services;
using ProductService.Domain.Entities;
using ProductService.Domain.IRepositories;

namespace ProductService.UnitTest.Services
{
    public class ProductsServiceTest
    {
        private readonly Random rand = new Random();
        private readonly Mock<IProductRepository> repoMock = new Mock<IProductRepository>();
        private readonly Mock<ILogger<ProductsService>> logger = new();
        private readonly Mock<IEventPublisher> eventPublisher = new();


        [Fact]
        public async Task GetByIdAsync_WhenUnExistingProduct_ReturnNull()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            repoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), cancellationToken))
                .ReturnsAsync((Product?)null);
            
            var service = new ProductsService(repoMock.Object, eventPublisher.Object,  logger.Object);

            // Act
            var serviceResponse = await service.GetByIdAsync(It.IsAny<int>(), cancellationToken);

            // Assert
            serviceResponse.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingProducts_ReturnExpectedProducts()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var expectedProduct = ExpectedProductDto();
            repoMock.Setup(repo => repo.GetByIdAsync(expectedProduct.Id, cancellationToken))
                .ReturnsAsync(expectedProduct);

            var service = new ProductsService(repoMock.Object, eventPublisher.Object, logger.Object);

            // Act
            var serviceResponse = await service.GetByIdAsync(expectedProduct.Id, cancellationToken);

            // Assert
            serviceResponse.Should().NotBeNull();
            serviceResponse.Should().BeEquivalentTo(expectedProduct.ToProductDto(), option =>
                            option.ComparingByMembers<ProductDto>());
        }

        [Fact]
        public async Task AddAsync_WithProductToAdd_ReturnVoid()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var newProduct = CreateRandomProductDto();

            var service = new ProductsService(repoMock.Object, eventPublisher.Object, logger.Object);

            // Act
            await service.AddAsync(newProduct, cancellationToken);

            // Assert
            repoMock.Verify(repo => repo.AddAsync(It.IsAny<Product>(), cancellationToken), Times.Once);
            eventPublisher.Verify(
                publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenUnExistingProduct_ReturnArgumentOutOfRangeException()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var productToUpdate = ExpectedProductDto();

            repoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), cancellationToken))
                            .ReturnsAsync((Product?)null);

            var service = new ProductsService(repoMock.Object, eventPublisher.Object, logger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.UpdateAsync(productToUpdate.Id, productToUpdate.ToUpdateProductDto(), cancellationToken));
        }

        [Fact]
        public async Task UpdateAsync_WhenProductToUpdated_ReturnVoid()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var productToUpdate = ExpectedProductDto();

            repoMock.Setup(repo => repo.GetByIdAsync(productToUpdate.Id, cancellationToken))
                           .ReturnsAsync(productToUpdate);

            var service = new ProductsService(repoMock.Object, eventPublisher.Object, logger.Object);

            // Act
            await service.UpdateAsync(productToUpdate.Id, productToUpdate.ToUpdateProductDto(), cancellationToken);

            // Assert
            repoMock.Verify(repo => repo.UpdateAsync(It.IsAny<Product>(), cancellationToken), Times.Once);
            eventPublisher.Verify(
                publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenUnExistingProduct_ReturnArgumentOutOfRangeException()
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            repoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), cancellationToken))
                            .ReturnsAsync((Product?)null);

            var service = new ProductsService(repoMock.Object, eventPublisher.Object, logger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteAsync(It.IsAny<int>(), cancellationToken));
        }

        [Fact]
        public async Task DeleteAsync_WhenProductToDelete_ReturnVoid()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var prodctToDelete = ExpectedProductDto();

            repoMock.Setup(repo => repo.GetByIdAsync(prodctToDelete.Id, cancellationToken))
                            .ReturnsAsync(prodctToDelete);

            var service = new ProductsService(repoMock.Object, eventPublisher.Object, logger.Object);

            // Act
            await service.DeleteAsync(prodctToDelete.Id, cancellationToken);

            // Act & Assert
            repoMock.Verify(repo => repo.DeleteAsync(It.IsAny<Product>(), cancellationToken), Times.Once);
            eventPublisher.Verify(
               publisher => publisher.PublishAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        #region Private Helper Function

        private Product ExpectedProductDto()
        {
            return new()
            {
                Id = rand.Next(100),
                ProductName = "Test Prduct",
                ProductDescription = "Test Description",
                Price = rand.Next(1000)
            };
        }
        private CreateProductDto CreateRandomProductDto()
        {
            return new()
            {
                ProductName = "Test Prduct",
                ProductDescription = "Test Description",
                Price = rand.Next(1000)
            };
        }
        private ProductDto RandomProductDto()
        {
            return new()
            {
                Id = rand.Next(10),
                ProductName = "Test Prduct",
                ProductDescription = "Test Description",
                Price = rand.Next(1000)
            };
        }

        #endregion Private Helper Function
    }
}
