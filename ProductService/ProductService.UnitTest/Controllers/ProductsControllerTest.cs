using Castle.Components.DictionaryAdapter.Xml;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using ProductService.API.Controllers;
using ProductService.Application.Dtos;
using ProductService.Application.MappingProfileExtension;
using ProductService.Application.ServiceInterfaces;
using ProductService.Domain.Entities;

namespace ProductService.UnitTest.Controllers
{
    public class ProductsControllerTest
    {
        private readonly Random rand = new Random();
        private readonly Mock<IProductService> serviceStub = new Mock<IProductService>();
        private readonly Mock<ILogger<ProductsController>> logger = new();


        [Fact]
        public async Task GetProduct_WhenUnExistingProduct_ReturnNotFound()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            serviceStub.Setup(service => service.GetByIdAsync(It.IsAny<int>(), cancellationToken))
                .ReturnsAsync((ProductDto?)null);

            var controller = new ProductsController(serviceStub.Object, logger.Object);

            // Act
            var actionResult = await controller.Get(rand.Next(1000), cancellationToken);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetAllProducts_WithExistingProducts_ReturnExpectedProducts()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var expectedResult = new[] { RandomProductDto(), RandomProductDto (), RandomProductDto ()};

            serviceStub.Setup(service => service.GetAllAsync(cancellationToken))
                .ReturnsAsync(expectedResult);
                

            var controller = new ProductsController(serviceStub.Object, logger.Object);

            // Act
            var actionResult = await controller.Get(cancellationToken);
            var result = actionResult as OkObjectResult;
            // Assert
            result?.Value.Should().NotBeNull();
            result?.Value.Should().BeEquivalentTo(expectedResult,
                optins => optins.ComparingByMembers<ProductDto>());
        }

        [Fact]
        public async Task AddProduct_WithProductToAdd_ReturnResponseOk()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var productToAdd = CreateRandomProductDto();

            var controller = new ProductsController(serviceStub.Object, logger.Object);

            // Act
            var actionResult = await controller.Post(productToAdd, cancellationToken);
            var result = actionResult as OkObjectResult;

            // Assert
            result.Value.Should().BeEquivalentTo("Success.");
            actionResult.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task AddProduct_WithInvalidProduct_ReturnResponseBadRequest()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var productToAdd = CreateRandomProductDto();
            productToAdd.ProductName = null;
            
            var controller = new ProductsController(serviceStub.Object, logger.Object);

            // Act
            var actionResult = await controller.Post(productToAdd, cancellationToken);
            var result = actionResult as OkObjectResult;

            // Assert
            actionResult.Should().BeOfType<BadRequestObjectResult>();
            var modelStateErrors = result?.Value as ModelStateDictionary;
            modelStateErrors.Should().NotBeEmpty();
        }

        #region Private Helper Function

        private ProductDto RandomProductDto()
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
        private UpdateProductDto UpdateRandomProductDto()
        {
            return new()
            {
                ProductName = "Test Prduct",
                ProductDescription = "Test Description",
                Price = rand.Next(1000)
            };
        }

        #endregion Private Helper Function
    }
}
