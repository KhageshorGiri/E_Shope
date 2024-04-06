using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.DataContext;
using ProductService.Infrastructure.Repository;

namespace ProductService.UnitTest.Repositories
{
    public class ProductRepositoryTest
    {

        private readonly Random rand = new Random();
        private DbContextOptionsBuilder<ProductServiceDbContext> optionsBuilder = new DbContextOptionsBuilder<ProductServiceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            

        private ProductServiceDbContext? dbContext;

        [Fact]
        public async Task GetByIdAsync_WhenExistingProduct_ReturnProduct()
        {
            // Arrange
            dbContext = new ProductServiceDbContext(optionsBuilder.Options);
            var expectedProdcut = CreateRandomProduct();

            dbContext.Products.Add(expectedProdcut);
            await dbContext.SaveChangesAsync();
            var repository = new ProductRepository(dbContext);

            //Act
            var result = await repository.GetByIdAsync(expectedProdcut.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProdcut.Id, result.Id);
            result.Should().BeEquivalentTo(expectedProdcut, option =>
                            option.ComparingByMembers<Product>());
        }

        [Fact]
        public async Task GetByIdAsync_WhenUnExistingProduct_ReturnNull()
        {
            // Arrange
            dbContext = new ProductServiceDbContext(optionsBuilder.Options);
            var repository = new ProductRepository(dbContext);

            // Act
            var result = await repository.GetByIdAsync(It.IsAny<int>());


            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_WhenExistingProduct_ReturnProductList()
        {
            // Arrange
            dbContext = new ProductServiceDbContext(optionsBuilder.Options);
            var expectedProdcut = new[] { CreateRandomProduct(), CreateRandomProduct() };

            dbContext.Products.Add(expectedProdcut[0]);
            dbContext.Products.Add(expectedProdcut[1]);
            await dbContext.SaveChangesAsync();
            var repository = new ProductRepository(dbContext);

            //Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expectedProdcut, option =>
                            option.ComparingByMembers<Product>());
        }

        [Fact]
        public async Task GetAllAsync_WhenUnExistingProduct_ReturnEmptyList()
        {
            // Arrange
            dbContext = new ProductServiceDbContext(optionsBuilder.Options);
            var repository = new ProductRepository(dbContext);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task AddAsync_WhenNewProduct_ReturnNothng()
        {
            // Arrange
            dbContext = new ProductServiceDbContext(optionsBuilder.Options);
            var prodcutToAdd = CreateRandomProduct();

            var repository = new ProductRepository(dbContext);

            //Act
            await repository.AddAsync(prodcutToAdd);

            // Assert
            Assert.Single(dbContext.Products);
        }

        [Fact]
        public async Task UpdateAsync_WhenNewProduct_ReturnNothng()
        {
            // Arrange
            dbContext = new ProductServiceDbContext(optionsBuilder.Options);
            var prodcutToUpdate = UpdateRandomProduct();

            var repository = new ProductRepository(dbContext);

            //Act
            await repository.UpdateAsync(prodcutToUpdate);

            // Assert
            Assert.Single(dbContext.Products);
        }

        [Fact]
        public async Task DeleteAsync_WhenNewProduct_ReturnNothng()
        {
            // Arrange
            dbContext = new ProductServiceDbContext(optionsBuilder.Options);
            var prodcutToDelete = RandomProduct();

            var repository = new ProductRepository(dbContext);

            //Act
            await repository.AddAsync(prodcutToDelete);

            // Assert
            Assert.Single(dbContext.Products);
        }

        #region Private Helper Function

        private Product RandomProduct()
        {
            return new()
            {
                Id = rand.Next(100),
                ProductName = "Test Prduct",
                ProductDescription = "Test Description",
                Price = rand.Next(1000)
            };
        }
        private Product CreateRandomProduct()
        {
            return new()
            {
                ProductName = "Test Prduct",
                ProductDescription = "Test Description",
                Price = rand.Next(1000)
            };
        }
        private Product UpdateRandomProduct()
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
