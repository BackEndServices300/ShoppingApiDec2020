using Microsoft.AspNetCore.Mvc;
using ShoppingApi.Models.Curbside;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class CurbsideOrdersController : ControllerBase
    {
        private readonly ICurbsideCommands _curbsideCommands;
        private readonly ICurbsideLookups _curbsideLookups;

        public CurbsideOrdersController(ICurbsideCommands curbsideCommands, ICurbsideLookups curbsideLookups)
        {
            _curbsideCommands = curbsideCommands;
            _curbsideLookups = curbsideLookups;
        }

        [HttpPost("/curbsideorders")]
        public async Task<ActionResult> PlaceOrder([FromBody] PostCurbsideRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } else
            {
                GetCurbsideResponse response = await _curbsideCommands.PlaceOrder(request);
                return Created(new Uri($"http://localhost:3000/curbsideorders/{response.Id}"), response);
            }
        }

        [HttpGet("/curbsideorders/{id:int}")]
        public async Task<ActionResult> GetOrder(int id)
        {
            GetCurbsideResponse response = await _curbsideLookups.GetById(id);

            if(response == null)
            {
                return NotFound();
            } else
            {
                return Ok(response);
            }
        }
    }
}
