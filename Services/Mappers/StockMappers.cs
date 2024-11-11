using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;
using Services.DTOs.Stock;

namespace Services.Mappers
{
    public static class StockMappers
    {
        public static StockDTO ToStockDTO(this Stock stockModel) {
            return new StockDTO()
            {
                CompanyName = stockModel.CompanyName,
                Id = stockModel.Id,
                Industry = stockModel.Industry,
                LastDiv = stockModel.LastDiv,
                MarketCap = stockModel.MarketCap,
                Purchase = stockModel.Purchase,
                Symbol = stockModel.Symbol,
                Comments = stockModel.Comments.Select(c => c.ToCommentDTO()).ToList()
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequestDTO stockModel)
        {
            return new Stock()
            {
                CompanyName = stockModel.CompanyName,
                Industry = stockModel.Industry,
                LastDiv = stockModel.LastDiv,
                MarketCap = stockModel.MarketCap,
                Purchase = stockModel.Purchase,
                Symbol = stockModel.Symbol,

            };
        }

        public static Stock ToStockFromUpdateDTO(this UpdateStockRequestDTO stockModel)
        {
            return new Stock()
            {
                CompanyName = stockModel.CompanyName,
                Industry = stockModel.Industry,
                LastDiv = stockModel.LastDiv,
                MarketCap = stockModel.MarketCap,
                Purchase = stockModel.Purchase,
                Symbol = stockModel.Symbol,

            };
        }

        public static Stock ToStockFromFMP(this FMPStock fmpStock)
        {
            return new Stock()
            {
                CompanyName = fmpStock.companyName,
                Industry = fmpStock.industry,
                LastDiv = (decimal) fmpStock.lastDiv,
                MarketCap =  fmpStock.mktCap,
                Purchase = (decimal) fmpStock.price,
                Symbol = fmpStock.symbol,

            };
        }

        public static CreateStockRequestDTO ToCreateDTOFromStock(this Stock stockModel)
        {
            return new CreateStockRequestDTO()
            {
                CompanyName = stockModel.CompanyName,
                Industry = stockModel.Industry,
                LastDiv = stockModel.LastDiv,
                MarketCap = stockModel.MarketCap,
                Purchase = stockModel.Purchase,
                Symbol = stockModel.Symbol,

            };
        }
    }
}
