using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Helpers;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetStocksAsync(QueryObject query);
        Task<Stock?> GetStockAsync(int id);

        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock> PostStockAsync(Stock stock);
        Task<Stock?> PutStockAsync(int id, Stock stock);
        Task<Stock?> DeleteStockAsync(int id);
        Task<bool> StockExists(int id);
    }
}
