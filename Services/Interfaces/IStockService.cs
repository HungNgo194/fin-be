using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.ApiResponse;
using Services.DTOs.Stock;
using Repositories.Helpers;

namespace Services.Interfaces
{
    public interface IStockService
    {
        Task<ApiResponseModel<IEnumerable<StockDTO>>> GetAllStocks(QueryObject query);
        Task<ApiResponseModel<StockDTO?>> GetStockById(int id);
        Task<ApiResponseModel<StockDTO?>> GetStockBySymbol(string symbol);
        Task<ApiResponseModel<StockDTO>> PostStock(CreateStockRequestDTO stockDTO);
        Task<ApiResponseModel<StockDTO?>> PutStock(int id, UpdateStockRequestDTO stockDTO);
        Task<ApiResponseModel<StockDTO?>> DeleteStock(int id);
    }
}
