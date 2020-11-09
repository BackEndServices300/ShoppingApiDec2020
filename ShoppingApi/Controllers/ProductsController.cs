using Microsoft.AspNetCore.Mvc;
using ShoppingApi.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class ProductsController : ControllerBase
    {
        private readonly ILookupProducts _products;
        private readonly IProductCommands _productCommands;

        public ProductsController(ILookupProducts products, IProductCommands productCommands)
        {
            _products = products;
            _productCommands = productCommands;
        }

        [HttpPost("/products")]
        public async Task<ActionResult> AddProduct([FromBody] PostProductRequest productToAdd)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } else
            {
                GetProductDetailsResponse response = await _productCommands.AddProduct(productToAdd);
                return CreatedAtRoute("products#getbyid", new { id = response.Id }, response);
            }
        }

        [HttpGet("/products/{id:int}", Name ="products#getbyid")]
        public async Task<ActionResult> GetAProductById(int id)
        {
            GetProductDetailsResponse response = await _products.GetById(id);

            if(response == null)
            {
                return NotFound();
            } else
            {
                return Ok(response);
            }
        }


        [HttpGet("/products")]
        public async Task<ActionResult> GetProducts([FromQuery] string category = null)
        {
            if (category == null)
            {
                GetProductsResponse response = await _products.GetSummary();
                return Ok(response);
            } else
            {

                GetProductListSummary response = await _products.GetSummaryList(category);
                return Ok(response);
            }
        }
    }
}
