using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using ProductService.API.Controllers;
using ProductService.Application.Dtos;
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
        public async Task Get_WhenUnExistingProduct_ReturnNotFound()
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
        public async Task Get_WithExistingProducts_ReturnExpectedProducts()
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
        public async Task Post_WithProductToAdd_ReturnResponseOk()
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
        public async Task Post_InvalidProduct_ReturnsBadRequest()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var productToAdd = CreateRandomProductDto();
            productToAdd.ProductName = null;

            var controller = new ProductsController(serviceStub.Object, logger.Object);
            controller.ModelState.AddModelError("Name", "Name is required");
            controller.ModelState.AddModelError("Price", "Price must be positive");

            // Act
            var result = await controller.Post(productToAdd, cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Put_WhenNonExistingProduct_ReturnsNoContent()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var existingProduct = UpdateRandomProductDto();

            serviceStub.Setup(service => service.GetByIdAsync(It.IsAny<int>(), cancellationToken))
                .ReturnsAsync((ProductDto?)null);

            var controller = new ProductsController(serviceStub.Object, logger.Object);

            // Act
            var result = await controller.Put(It.IsAny<int>(), existingProduct, cancellationToken);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Put_WhenExistingProduct_ReturnsOk()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var existingProduct = UpdateRandomProductDto();
            var expectedProduct = RandomProductDto();

            serviceStub.Setup(service => service.GetByIdAsync(It.IsAny<int>(), cancellationToken))
                .ReturnsAsync(expectedProduct);

            var controller = new ProductsController(serviceStub.Object, logger.Object);

            // Act
            var result = await controller.Put(It.IsAny<int>(), existingProduct, cancellationToken);
            var okResult = result as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            okResult?.Value.Should().Be("Updated");
        }

        [Fact]
        public async Task Put_InvalidProduct_ReturnsBadRequest()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var productToUpdate = UpdateRandomProductDto();
            productToUpdate.ProductName = null;

            var controller = new ProductsController(serviceStub.Object, logger.Object);
            controller.ModelState.AddModelError("Name", "Name is required");
            controller.ModelState.AddModelError("Price", "Price must be positive");

            // Act
            var result = await controller.Put(It.IsAny<int>(), productToUpdate, cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Delete_ProductDoesNotExist_ReturnsNoContent()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            serviceStub.Setup(service => service.GetByIdAsync(It.IsAny<int>(), cancellationToken))
                              .ReturnsAsync((ProductDto?)null); // Assuming product does not exist

            var controller = new ProductsController(serviceStub.Object, logger.Object);

            // Act
            var result = await controller.Delete(It.IsAny<int>(), cancellationToken);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ProductExists_ReturnsOk()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var expectedPrduct = RandomProductDto();
            serviceStub.Setup(service => service.GetByIdAsync(It.IsAny<int>(), cancellationToken))
                              .ReturnsAsync(expectedPrduct);

            serviceStub.Setup(service => service.DeleteAsync(It.IsAny<int>(), cancellationToken))
                              .Returns(Task.CompletedTask);

            var controller = new ProductsController(serviceStub.Object, logger.Object);

            // Act
            var result = await controller.Delete(It.IsAny<int>(), cancellationToken);
            var okResult = result as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            okResult?.Value.Should().Be("Deleted.");
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
