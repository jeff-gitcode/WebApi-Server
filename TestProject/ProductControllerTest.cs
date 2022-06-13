using Moq;
using System;
using Xunit;
using WebApi_Test;
using Microsoft.Extensions.Logging;
using WebApi_Test.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FluentAssertions;
using AutoFixture.Xunit2;
using WebApi_Test.Services;
using WebApi_Test.Models;
using System.Threading.Tasks;

namespace TestProject
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _productService;
        private readonly Mock<ILogger<ProductsController>> _logger;

        private readonly ProductsController _sut;
        public ProductControllerTest()
        {
            _logger = new Mock<ILogger<ProductsController>>();
            _productService = new Mock<IProductService>();

            _sut = new ProductsController(_logger.Object, _productService.Object);
        }

        [Fact]
        public async Task GetProducts_Should_Return_NotFound_When_GetProducts_IsEmpty()
        {
            var products = new Product[] { };

            _productService.Setup(r => r.GetProductsAsync()).ReturnsAsync(products);

            var result = await _sut.GetProducts();

            _productService.Verify(r => r.GetProductsAsync(), Times.Once);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetProducts_Should_Return_NotFound_When_GetProducts_ThrowsException()
        {
            _productService.Setup(r => r.GetProductsAsync()).Throws(new Exception());

            var result = await _sut.GetProducts();


            var statusCode = result.Result as ObjectResult;

            _productService.Verify(r => r.GetProductsAsync(), Times.Once);
            statusCode.StatusCode.Should().Be(500);
        }

        [Theory, AutoData]
        public async Task GetProducts_Should_Return_Result_When_GetProducts(Product[] products)
        {
            _productService.Setup(r => r.GetProductsAsync()).ReturnsAsync(products);

            var result = await _sut.GetProducts();

            _productService.Verify(r => r.GetProductsAsync(), Times.Once);
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ShippingCost_Should_Return()
        {
            _productService.Setup(r => r.ShippingCostAsync(It.IsAny<double>())).ReturnsAsync(10);
            var payload = new Shipping()
            {
                Total = 30
            };

            var result = await _sut.ShippingCost(payload);

            _productService.Verify(r => r.ShippingCostAsync(30), Times.Once);
            (result.Result as ObjectResult).Value.Should().Be(10);
        }

        [Fact]
        public async Task ShippingCost_Should_Return_NotFound_When_GetProducts_ThrowsException()
        {
            _productService.Setup(r => r.ShippingCostAsync(It.IsAny<double>())).Throws(new Exception());

            var payload = new Shipping()
            {
                Total = 10
            };

            var result = await _sut.ShippingCost(payload);


            var statusCode = result.Result as ObjectResult;

            _productService.Verify(r => r.ShippingCostAsync(10), Times.Once);
            statusCode.StatusCode.Should().Be(500);
        }

        [Theory, AutoData]
        public async Task PlaceOrder_Should_Return_Result(Order order)
        {
            _productService.Setup(r => r.PlaceOrderAsync(It.IsAny<Order>())).ReturnsAsync(true);

            var result = await _sut.PlaceOrder(order);

            _productService.Verify(r => r.PlaceOrderAsync(order), Times.Once);
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Theory, AutoData]
        public async Task PlaceOrder_Should_Return_InternalError_When_ThrowsException(Order order)
        {
            _productService.Setup(r => r.PlaceOrderAsync(It.IsAny<Order>())).Throws(new Exception());

            var result = await _sut.PlaceOrder(order);

            var statusCode = result.Result as ObjectResult;

            _productService.Verify(r => r.PlaceOrderAsync(order), Times.Once);
            statusCode.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task PlaceOrder_Should_Return_BadRequest_When_Request_Invalid()
        {
            _productService.Setup(r => r.PlaceOrderAsync(It.IsAny<Order>())).ReturnsAsync(true);

            var result = await _sut.PlaceOrder(null);

            result.Result.Should().BeOfType<BadRequestResult>();

        }
    }
}
