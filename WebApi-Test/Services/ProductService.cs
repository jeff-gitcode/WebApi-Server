using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Test.Models;

namespace WebApi_Test.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();

        Task<double> ShippingCostAsync(double total);

        Task<bool> PlaceOrderAsync(Order order);
    }

    public class ProductService: IProductService
    {
        private readonly ILogger<ProductService> _logger;

        public ProductService(ILogger<ProductService> logger = null)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
                var product1 = new Product
                {
                    ID = 1,
                    Title = "Casaco maneiro",
                    Price = 290.9
                };

                var product2 = new Product
                {
                    ID = 2,
                    Title = "Óculos barato",
                    Price = 20.9
                };

                var product3 = new Product
                {
                    ID = 3,
                    Title = "Tênis da moda",
                    Price = 500.1
                };

                var products = new Product[]
                {
                product1, product2, product3
                };

                return products;

            }
            catch (Exception)
            {
                _logger.LogError("FAILED: GetProductsAsync");
                throw;
            }
        }

        public async Task<double> ShippingCostAsync(double total)
        {
            var shippingCost = 10;

            try
            {
                if (total >= 50)
                {
                    shippingCost = 20;
                }

                return shippingCost;

            }
            catch (Exception)
            {
                _logger.LogError("FAILED: ShippingCostAsync");
                throw;
            }
        }

        public async Task<bool> PlaceOrderAsync(Order order)
        {
            try
            {
                var result = order;

                return true;
            }
            catch (Exception)
            {
                _logger.LogError("FAILED: PlaceOrderAsync");
                throw;
            }

            return false;

        }
    }
}
