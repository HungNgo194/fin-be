using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Helpers;
using Repositories.Interfaces;
using Repositories.Models;
using Services.DTOs.Comment;
using Services.DTOs.Stock;
using Services.Extensions;
using Services.Implements;
using Services.Interfaces;
using Services.Mappers;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IStockService _stockService;
        private readonly IFMPService _fMPService;
        private readonly UserManager<AppUser> _userManager;
        public CommentsController(ICommentService commentService, UserManager<AppUser> userManager, IStockService stockService, IFMPService fMPService)
        {
            _commentService = commentService;
            _userManager = userManager;
            _stockService = stockService;
            _fMPService = fMPService;
        }

        // GET: api/Comments
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetComments([FromQuery] CommentQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _commentService.GetAllComments(query);
            if (response.Success)
            {
                return Ok(response.Data);
            }
            return BadRequest(response.Message);
        }

        // GET: api/Comments/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _commentService.GetCommentById(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> PostComment([FromBody] CreateCommentDTO commentDTO, [FromRoute] string symbol)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stockResponse = await _stockService.GetStockBySymbol(symbol);
            
            if (!stockResponse.Success || stockResponse.Data is null) 
            {
                var httpResponse = await _fMPService.FindStockBySymbolAsync(symbol);
                if (!httpResponse.Success || httpResponse.Data is null) 
                {
                    return BadRequest(httpResponse.Message);
                } else
                {
                    stockResponse = await _stockService.PostStock(httpResponse.Data.ToCreateDTOFromStock());
                }
            } 

            if (!stockResponse.Success || stockResponse.Data is null) { return BadRequest(stockResponse.Message); }

            

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var response = await _commentService.PostComment(commentDTO, stockResponse.Data.Id, appUser);
            if (!response.Success || response.Data is null) { return BadRequest(response.Message); }

            

            return CreatedAtAction("GetComment", new { id = response.Data.Id }, response.Data);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _commentService.DeleteComment(id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return NoContent();
        }

    }
}
