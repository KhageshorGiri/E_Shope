using ProductService.Application.Dtos;
using ProductService.Domain.Entities;

namespace ProductService.Application.MappingProfileExtension
{
    
    public static class ProductDtoMapping
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price
            };
        }
        
        public static Product ToProduct(this Product product)
        {
            return new() 
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price
            };

        }
    }

    public static class CrateProductDtoMapping
    {
        public static CreateProductDto ToCreateProductDto(this Product product)
        {
            return new()
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price
            };
        }

        public static Product ToProduct(this CreateProductDto product)
        {
            return new()
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price
            };

        }
    }

    public static class UpdateProductMapping
    {
        public static UpdateProductDto ToUpdateProductDto(this Product product)
        {
            return new()
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price
            };
        }

        public static Product ToProduct(this UpdateProductDto product)
        {
            return new()
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price
            };

        }
    }
}
