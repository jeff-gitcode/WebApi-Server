using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi_Test.Controllers;
using WebApi_Test.Models;
using WebApi_Test.Services;
using Xunit;

namespace TestProject
{
    public class ProductServiceTest
    {
        private readonly Mock<ILogger<ProductService>> _logger;

        private readonly ProductService _sut;

        public ProductServiceTest()
        {
            _logger = new Mock<ILogger<ProductService>>();

            _sut = new ProductService(_logger.Object);
        }

        [Fact]
        public async Task GetProductsAsync_Should_Return_Value()
        {
            var result = await _sut.GetProductsAsync();


            result.Should().BeOfType<Product[]>();

        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(50, 20)]
        [InlineData(100, 20)]
        public async Task ShippingCostAsync_Should_Return_Value(double total, double expected)
        {
            var result = await _sut.ShippingCostAsync(total);


            result.Should().Be(expected);

        }

        [Theory, AutoData]
        public async Task PlaceOrderAsync_Should_Return_Value(Order order)
        {
            var result = await _sut.PlaceOrderAsync(order);


            result.Should().BeTrue();

        }
    }
}
