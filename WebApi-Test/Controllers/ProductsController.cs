using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Test.Models;
using WebApi_Test.Services;

namespace WebApi_Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        [Route("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetProductsAsync();

                if (products.IsNotOrEmpty())
                {
                    return NotFound();
                }

                return Ok(products);

            }
            catch (Exception e)
            {
                _logger.LogError("FAILED: GetProducts - ${e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }



        [HttpPost]
        [Route("shipping")]
        public async Task<ActionResult<double>> ShippingCost(Shipping shipping)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var shippingCost = await _productService.ShippingCostAsync(shipping.Total);

                return Ok(shippingCost);

            }
            catch (Exception e)
            {
                _logger.LogError("FAILED: ShippingCost - ${e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpPost]
        [Route("checkout")]
        public async Task<ActionResult<double>> PlaceOrder(Order order)
        {
            try
            {
                if (!ModelState.IsValid || order == null)
                {
                    return BadRequest();
                }

                var result = await _productService.PlaceOrderAsync(order);

                return Ok(result);

            }
            catch (Exception e)
            {
                _logger.LogError("FAILED: PlaceOrder - ${e.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

    }
}
