﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using Data.Models;
using API.Repositories;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAddressesController : ControllerBase
    {
        private readonly OrderAddressRepo _repo;

        public OrderAddressesController(OrderAddressRepo repo)
        {
            _repo = repo;
        }


        // GET: api/OrderAddresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderAddress>>> GetOrderAddresses()
        {
            await _repo.GetAllOrderAddress();
            return Ok();
        }

        // GET: api/OrderAddresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderAddress>> GetOrderAddress(Guid id)
        {
          await _repo.GetOrderAddressById(id);
            return Ok();
        }
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderAddress>> GetOrderAddressByOrderId(Guid orderId)
        {
            await _repo.GetOrderAddressByOrderId(orderId);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderAddress(OrderAddress orderAddress)
        {
            await _repo.Update(orderAddress);
            return Ok();
        }

        // POST: api/OrderAddresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderAddress>> PostOrderAddress(OrderAddress orderAddress)
        {
            await _repo.Create(orderAddress);
            return Ok();
        }

        // DELETE: api/OrderAddresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderAddress(Guid id)
        {
            await _repo.Delete(id);
            return Ok();
        }

        
    }
}
