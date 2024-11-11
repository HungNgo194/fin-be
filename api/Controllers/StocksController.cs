using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Models;
using Services.DTOs.Stock;
using Repositories.Helpers;
using Services.Interfaces;
using Services.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StocksController(IStockService stockService)
        {
            _stockService = stockService;
        }

        // GET: api/Stocks
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetStocks([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _stockService.GetAllStocks(query);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }

        // GET: api/Stocks/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStock([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _stockService.GetStockById(id);
            if (!response.Success)
            {
                return BadRequest(response.Message); 
            }
            return Ok(response.Data);
        }

        // GET: api/Stocks/5
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetBySymbol([FromRoute] string symbol)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _stockService.GetStockBySymbol(symbol);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        // PUT: api/Stocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutStock([FromRoute] int id, [FromBody] UpdateStockRequestDTO stockDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _stockService.PutStock(id, stockDTO);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Data);
        }

        // POST: api/Stocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostStock([FromBody] CreateStockRequestDTO stockDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _stockService.PostStock(stockDTO);
            if (!response.Success || response.Data is null) 
            {
                return BadRequest(response.Message);
            }

            return CreatedAtAction("GetStock", new { id = response.Data.Id }, response.Data);
        }

        // DELETE: api/Stocks/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await  _stockService.DeleteStock(id);
            if (!response.Success) { return BadRequest(response.Message); }

            return NoContent();
        }
    }
}
