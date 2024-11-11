using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;
using Services.ApiResponse;

namespace Services.Interfaces
{
    public interface IFMPService
    {
        Task<ApiResponseModel<Stock>> FindStockBySymbolAsync(string symbol);
    }
}
