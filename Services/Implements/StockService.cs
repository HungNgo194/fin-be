using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.ApiResponse;
using Services.DTOs.Stock;
using Repositories.Helpers;
using Services.Interfaces;
using Services.Mappers;

namespace Services.Implements
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepo;
        private readonly ICommentRepository _commentRepo;
        public StockService(IStockRepository stockRepo, ICommentRepository commentRepo)
        {
            _stockRepo = stockRepo;
            _commentRepo = commentRepo;
        }

        public async Task<ApiResponseModel<StockDTO?>> DeleteStock(int id)
        {
            var response = new ApiResponseModel<StockDTO?>()
            {
                Success = false,
                Data = null
            };

            try
            {
                var stock = await _stockRepo.DeleteStockAsync(id);
                if (stock == null)
                {
                    response.Message = "Stock does not exist!";
                    return response;
                }

                response.Success = true;
                response.Data = stock.ToStockDTO();
                response.Message = "Delete successfully!";
            }
            catch (Exception ex)
            {
                    response.Message = "An error has occurred! " + ex.Message;
                    response.Success = false;
            }

            return response;
        }


        public async Task<ApiResponseModel<StockDTO?>> GetStockById(int id)
        {
            var response = new ApiResponseModel<StockDTO?>()
            {
                Success = false,
                Data = null,
            };
            var stock = await _stockRepo.GetStockAsync(id);
            if (stock == null)
            {
                response.Message = "Stock does not exist!";
                return response;
            }

            response.Success = true;
            response.Data = stock.ToStockDTO();

            return response;
        }

        public async Task<ApiResponseModel<IEnumerable<StockDTO>>> GetAllStocks(QueryObject query)
        {
            
            var stocks = await _stockRepo.GetStocksAsync(query);
            var response = new ApiResponseModel<IEnumerable<StockDTO>>()
            {
                Success = false,
                Data = null
            };

            if (stocks is null)
            {
                response.Message = "List of stocks is empty";
                return response;
            } 
            response.Success = true;
            response.Data = stocks.Select(s => s.ToStockDTO());
            return response;
        }

        public async Task<ApiResponseModel<StockDTO>> PostStock(CreateStockRequestDTO stockDTO)
        {
            var response = new ApiResponseModel<StockDTO>()
            {
                Success = false,
                Data = null,
            };
            var stock = await _stockRepo.PostStockAsync(stockDTO.ToStockFromCreateDTO());
            response.Success = true;
            response.Data = stock.ToStockDTO();
            return response;
        }

        public async Task<ApiResponseModel<StockDTO?>> PutStock(int id, UpdateStockRequestDTO stockDTO)
        {
            var response = new ApiResponseModel<StockDTO?>()
            {
                Success = false,
                Data = null,
            };
            if (!await _stockRepo.StockExists(id))
            {
                response.Message = "Stock does not exist!";
                return response;
            }
            var stock = await _stockRepo.PutStockAsync(id, stockDTO.ToStockFromUpdateDTO());
            response.Success = true;
            response.Data = stock == null ? null : stock.ToStockDTO();
            return response;
        }

        public async Task<ApiResponseModel<StockDTO?>> GetStockBySymbol(string symbol)
        {
            var response = new ApiResponseModel<StockDTO?>()
            {
                Success = false,
                Data = null,
            };
            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                response.Message = "Stock does not exist!";
                return response;
            }

            response.Success = true;
            response.Data = stock.ToStockDTO();

            return response;
        }
    }
}
