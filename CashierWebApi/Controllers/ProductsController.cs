using CashierWebApi.BL;
using CashierWebApi.BL.Exceptions;
using CashierWebApi.BL.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CashierWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ProductsService service;

        public ProductsController(ProductsService service)
        {
            this.service = service;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<ProductApiDto>> Get(string search, string sortPrice)
        {
            bool sortAsc = sortPrice?.ToLower() == "asc";
            bool sortDesc = sortPrice?.ToLower() == "desc";
            return sortAsc || sortDesc ? 
                await service.GetAsync(search, sortAsc)
                : await service.GetAsync(search, null);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{barcode}")]
        public async Task<ActionResult<ProductApiDto>> Get(string barcode)
        {
            (ProductApiDto product, Exception ex) = await service.GetAsync(barcode);
            if (ex != null)
            {
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is KeyNotFoundException)
                {
                    return NotFound(ex.Message);
                }
            }
            return product;
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductApiDto product)
        {
            Exception ex = await service.CreateAsync(product);
            if (ex != null)
            {
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is KeyNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                if (ex is AlreadyExistsException)
                {
                    return Conflict(ex.Message);
                }
                return StatusCode(500);
            }
            return Ok();
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{barcode}")]
        public async Task<ActionResult> Put(string barcode, [FromBody] ProductApiDto product)
        {
            Exception ex = await service.UpdateAsync(barcode, product);
            if (ex != null)
            {
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is KeyNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500);
            }
            return Ok();
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{barcode}")]
        public async Task<ActionResult<ProductApiDto>> Delete(string barcode)
        {
            (var product, Exception ex) = await service.DeleteAsync(barcode);
            if (ex != null)
            {
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                if (ex is KeyNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                return StatusCode(500);
            }
            return Ok(product);
        }
    }
}
