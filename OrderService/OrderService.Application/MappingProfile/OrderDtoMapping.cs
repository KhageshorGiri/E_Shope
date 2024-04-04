using OrderService.Application.Dtos;
using OrderService.Domain.Entities;
using System.Runtime.CompilerServices;

namespace OrderService.Application.MappingProfile
{
    public static class OrderDtoMapping
    {
        public static OrderDto ToOrderDto(this Order order)
        {
            return new()
            {
                Id = order.Id,
                ProductName = order.ProductName,
                OrderDescription = order.OrderDescription,
                OrderDate = order.OrderDate,
                OrderDeliveryDate = order.OrderDeliveryDate
            };
        }

        public static Order ToOrder(this OrderDto order)
        {
            return new()
            {
                Id = order.Id,
                ProductName = order.ProductName,
                OrderDescription = order.OrderDescription,
                OrderDate = order.OrderDate,
                OrderDeliveryDate = order.OrderDeliveryDate
            };
        }
    }

    public static class CreateOrderDtoMapping
    {
        public static CreateOrderDto ToCreateOrderDto(this Order order)
        {
            return new()
            {
                ProductName = order.ProductName,
                OrderDescription = order.OrderDescription,
                OrderDate = order.OrderDate,
                OrderDeliveryDate = order.OrderDeliveryDate
            };
        }

        public static Order ToOrder(this CreateOrderDto order)
        {
            return new()
            {
                ProductName = order.ProductName,
                OrderDescription = order.OrderDescription,
                OrderDate = order.OrderDate,
                OrderDeliveryDate = order.OrderDeliveryDate
            };
        }
    }

    public static class UpdateOrderDtoMapping
    {
        public static UpdateOrderDto ToUpdateOrderDto(this Order order)
        {
            return new()
            {
                ProductName = order.ProductName,
                OrderDescription = order.OrderDescription,
                OrderDate = order.OrderDate,
                OrderDeliveryDate = order.OrderDeliveryDate
            };
        }

        public static Order ToOrder(this UpdateOrderDto order)
        {
            return new()
            {
                ProductName = order.ProductName,
                OrderDescription = order.OrderDescription,
                OrderDate = order.OrderDate,
                OrderDeliveryDate = order.OrderDeliveryDate
            };
        }
    }
}
