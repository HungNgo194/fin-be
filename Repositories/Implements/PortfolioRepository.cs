using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implements
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public PortfolioRepository(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _dbContext.Portfolio.Where(u => u.AppUserId == user.Id)
                                             .Select(stock => new Stock
                                             {
                                                 Id = stock.StockId,
                                                 Symbol = stock.Stock.Symbol,
                                                 Comments = stock.Stock.Comments,
                                                 CompanyName = stock.Stock.CompanyName,
                                                 Industry = stock.Stock.Industry,
                                                 LastDiv = stock.Stock.LastDiv,
                                                 MarketCap = stock.Stock.MarketCap,
                                                 Purchase = stock.Stock.Purchase
                                             }).ToListAsync();
        }

        public async Task<Portfolio> PostPortfolioAsync(Portfolio portfolio)
        {
            await _dbContext.Portfolio.AddAsync(portfolio);
            await _dbContext.SaveChangesAsync();
            return portfolio;
        }

        public async Task<bool> IsDuplicateAsync(Portfolio portfolio)
        {
            var port = await _dbContext.Portfolio.FirstOrDefaultAsync(x => x.AppUserId == portfolio.AppUserId && x.StockId == portfolio.StockId);
            if (port != null)
            {
                return true;
            }
            return false;
        }

        public async Task<Portfolio?> DeleteAsync(AppUser user, string symbol)
        {
            var portModel = await _dbContext.Portfolio.FirstOrDefaultAsync(p => p.AppUserId == user.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());

            if (portModel is null) return null;

            _dbContext.Portfolio.Remove(portModel);
            await _dbContext.SaveChangesAsync();

            return portModel;
        }

        public Task<bool> PortfolioExists(AppUser user, string symbol)
        {
            return _dbContext.Portfolio.AnyAsync(p => p.AppUserId == user.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());
        }
    }
}
