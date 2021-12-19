using CashierWebApi.BL;
using CashierWebApi.BL.Exceptions;
using CashierWebApi.BL.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CashierWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        BillsService service;

        public BillsController(BillsService service)
        {
            this.service = service;
        }

        // GET: api/<BillsController>
        [HttpGet]
        public async Task<IEnumerable<BillApiDto>> Get(int? statusId, string orderBy, bool orderAsc)
        {
            return await service.GetAllAsync(statusId, orderBy?.ToLower(), orderAsc);
        }

        // GET: api/<BillsController>/statuses
        [HttpGet("statuses")]
        public async Task<IEnumerable<BillStatusApiDto>> GetStatuses()
        {
            return await service.GetAllStatusesAsync();
        }

        // GET api/<BillsController>/5
        [HttpGet("{number}")]
        public async Task<ActionResult<BillApiDto>> Get(uint number)
        {
            var bill = await service.GetAsync(number);
            if (bill == null)
            {
                return NotFound();
            }
            return Ok(bill);
        }

        // POST api/<BillsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] IEnumerable<ItemApiDto> items)
        {
            var ex = await service.CreateAsync(items);
            if (ex != null)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        // PUT api/<BillsController>/5/cancel
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<BillApiDto>> PutCancel(uint id)
        {
            (var bill, var ex) = await service.CancelAsync(id);
            if (bill == null || ex is KeyNotFoundException)
            {
                return NotFound(ex?.Message);
            }
            if (ex is SaveChangesException)
            {
                return StatusCode(500, ex.Message);
            }
            return bill;
        }
    }
}
