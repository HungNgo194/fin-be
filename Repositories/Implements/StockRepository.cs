using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repositories.Helpers;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implements
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(s => s.Id == id);
            if (stock == null)
            {
                return null;
            }
            _context.Stock.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stock.Include(c => c.Comments).ThenInclude(c => c.AppUser).FirstOrDefaultAsync(s => s.Symbol.ToLower() == symbol.ToLower());
        }

        public async Task<Stock?> GetStockAsync(int id)
        {
            return await _context.Stock.Include(c => c.Comments).ThenInclude(c => c.AppUser).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Stock>> GetStocksAsync(QueryObject query)
        {
            var stocks = _context.Stock.Include(c => c.Comments).ThenInclude(c => c.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);  
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();

        }

        public async Task<Stock> PostStockAsync(Stock stock)
        {
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> PutStockAsync(int id, Stock stockModel)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(s => s.Id == id);
            if (stock == null)
            {
                return null;
            }

            stock.LastDiv = stockModel.LastDiv;
            stock.MarketCap = stockModel.MarketCap;
            stock.Purchase = stockModel.Purchase;
            stock.Industry = stockModel.Industry;
            stock.CompanyName = stockModel.CompanyName;
            stock.Symbol = stockModel.Symbol;

            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<bool> StockExists(int id)
        {
            return await _context.Stock.AnyAsync(s => s.Id == id);
        }
    }
}
