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
using Repositories.Models;
using Services.Extensions;
using Services.Interfaces;
using Services.Mappers;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfoliosController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockService _stockService;
        private readonly IPortfolioService _portfolioService;
        private readonly IFMPService _fMPService;
        public PortfoliosController(UserManager<AppUser> userManager, IStockService stockService, IPortfolioService portfolioService, IFMPService fMPService)
        {
            _userManager = userManager;
            _stockService = stockService;
            _portfolioService = portfolioService;
            _fMPService = fMPService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioService.GetUserPortfolio(appUser);

            if (!userPortfolio.Success)
            {
                return BadRequest(userPortfolio.Message);
            }

            return Ok(userPortfolio.Data);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostPorfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var responseStock = await _stockService.GetStockBySymbol(symbol);
            if (!responseStock.Success || responseStock.Data is null)
            {
                var httpResponse = await _fMPService.FindStockBySymbolAsync(symbol);
                if (!httpResponse.Success || httpResponse.Data is null)
                {
                    return BadRequest(httpResponse.Message);
                }
                else
                {
                    responseStock = await _stockService.PostStock(httpResponse.Data.ToCreateDTOFromStock());
                }
            }

            if (!responseStock.Success || responseStock.Data is null) { return BadRequest(responseStock.Message); }


            var portfolio = new Portfolio()
            {
                AppUserId = appUser.Id,
                StockId = responseStock.Data.Id,

            };


            var response = await _portfolioService.CreatePortfolio(portfolio);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Created();

        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var responseStock = await _stockService.GetStockBySymbol(symbol);
            if (!responseStock.Success || responseStock.Data is null)
            {
                return BadRequest(responseStock.Message);
            }

            var response = await _portfolioService.DeletePortfolio(appUser, symbol);
            if (!response.Success) { return BadRequest(response.Message); }


            return Ok(response.Message);
        }
    }
}
