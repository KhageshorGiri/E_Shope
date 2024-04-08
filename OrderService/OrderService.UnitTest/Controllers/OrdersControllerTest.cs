using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.API.Controllers;
using OrderService.Application.Dtos;
using OrderService.Application.ServiceInterfaces;

namespace OrderService.UnitTest.Controllers
{
    
    public class OrdersControllerTest
    {
        private readonly Random rand = new Random();
        private readonly Mock<IOrderService> serviceStub = new Mock<IOrderService>();
        private readonly Mock<ILogger<OrdersController>> logger = new();


        [Fact]
        public async Task Get_WhenUnExistingOrdere_ReturnNotFound()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            serviceStub.Setup(service => service.GetOrderByIdAsync(It.IsAny<int>(), cancellationToken))
                .ReturnsAsync((OrderDto?)null);

            var controller = new OrdersController(serviceStub.Object, logger.Object);

            // Act
            var actionResult = await controller.Get(rand.Next(1000), cancellationToken);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Get_WithExistingOrder_ReturnExpectedOrder()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var expectedResult = new[] { RandomOrderDto(), RandomOrderDto(), RandomOrderDto() };

            serviceStub.Setup(service => service.GetAllAsync(cancellationToken))
                .ReturnsAsync(expectedResult);


            var controller = new OrdersController(serviceStub.Object, logger.Object);

            // Act
            var actionResult = await controller.Get(cancellationToken);
            var result = actionResult as OkObjectResult;
            // Assert
            result?.Value.Should().NotBeNull();
            result?.Value.Should().BeEquivalentTo(expectedResult,
                optins => optins.ComparingByMembers<OrderDto>());
        }

        [Fact]
        public async Task Post_WithOrderToAdd_ReturnResponseOk()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var orderToAdd = CreateRandomOrderDto();

            var controller = new OrdersController(serviceStub.Object, logger.Object);

            // Act
            var actionResult = await controller.Post(orderToAdd, cancellationToken);
            var result = actionResult as OkObjectResult;

            // Assert
            result?.Value.Should().BeEquivalentTo("Added");
            actionResult.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task Post_InvalidOrder_ReturnsBadRequest()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var orderToAdd = CreateRandomOrderDto();
            orderToAdd.ProductName = null;

            var controller = new OrdersController(serviceStub.Object, logger.Object);
            controller.ModelState.AddModelError("Name", "Name is required");
            controller.ModelState.AddModelError("Price", "Price must be positive");

            // Act
            var result = await controller.Post(orderToAdd, cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Put_WhenNonExistingOrder_ReturnsNoContent()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var existingOrder = UpdateRandomOrderDto();

            serviceStub.Setup(service => service.GetOrderByIdAsync(It.IsAny<int>(), cancellationToken))
                .ReturnsAsync((OrderDto?)null);

            var controller = new OrdersController(serviceStub.Object, logger.Object);

            // Act
            var result = await controller.Put(It.IsAny<int>(), existingOrder, cancellationToken);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Put_WhenExistingOrder_ReturnsOk()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var existingOrder = UpdateRandomOrderDto();
            var expectedOrder = RandomOrderDto();

            serviceStub.Setup(service => service.GetOrderByIdAsync(It.IsAny<int>(), cancellationToken))
                .ReturnsAsync(expectedOrder);

            var controller = new OrdersController(serviceStub.Object, logger.Object);

            // Act
            var result = await controller.Put(It.IsAny<int>(), existingOrder, cancellationToken);
            var okResult = result as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            okResult?.Value.Should().Be("Updated");
        }

        [Fact]
        public async Task Put_InvalidOrder_ReturnsBadRequest()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var OrderToUpdate = UpdateRandomOrderDto();
            OrderToUpdate.ProductName = null;

            var controller = new OrdersController(serviceStub.Object, logger.Object);
            controller.ModelState.AddModelError("Name", "Name is required");
            controller.ModelState.AddModelError("Price", "Price must be positive");

            // Act
            var result = await controller.Put(It.IsAny<int>(), OrderToUpdate, cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Delete_OrderDoesNotExist_ReturnsNoContent()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            serviceStub.Setup(service => service.GetOrderByIdAsync(It.IsAny<int>(), cancellationToken))
                              .ReturnsAsync((OrderDto?)null); // Assuming product does not exist

            var controller = new OrdersController(serviceStub.Object, logger.Object);

            // Act
            var result = await controller.Delete(It.IsAny<int>(), cancellationToken);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_OrderExists_ReturnsOk()
        {
            // Arrange
            var cancellationToken = new CancellationToken();
            var expectedPrduct = RandomOrderDto();
            serviceStub.Setup(service => service.GetOrderByIdAsync(It.IsAny<int>(), cancellationToken))
                              .ReturnsAsync(expectedPrduct);

            serviceStub.Setup(service => service.DeleteAsync(It.IsAny<int>(), cancellationToken))
                              .Returns(Task.CompletedTask);

            var controller = new OrdersController(serviceStub.Object, logger.Object);

            // Act
            var result = await controller.Delete(It.IsAny<int>(), cancellationToken);
            var okResult = result as OkObjectResult;

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            okResult?.Value.Should().Be("Deleted");
        }

        #region Private Helper Function

        private OrderDto RandomOrderDto()
        {
            return new()
            {
                Id = rand.Next(100),
                ProductName = "Test Order",
                OrderDescription = "Test Description",
                OrderDate = DateTime.UtcNow,
                OrderDeliveryDate = DateTime.UtcNow
            };
        }
        private CreateOrderDto CreateRandomOrderDto()
        {
            return new()
            {
                ProductName = "Test Order",
                OrderDescription = "Test Description",
                OrderDate = DateTime.UtcNow,
                OrderDeliveryDate = DateTime.UtcNow
            };
        }
        private UpdateOrderDto UpdateRandomOrderDto()
        {
            return new()
            {
                ProductName = "Test Order",
                OrderDescription = "Test Description",
                OrderDate = DateTime.UtcNow,
                OrderDeliveryDate = DateTime.UtcNow
            };
        }

        #endregion Private Helper Function
    }
}
