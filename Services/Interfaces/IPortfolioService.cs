using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;
using Services.ApiResponse;

namespace Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<ApiResponseModel<List<Stock>>> GetUserPortfolio(AppUser user);
        Task<ApiResponseModel<Portfolio>> CreatePortfolio(Portfolio portfolio);
        Task<ApiResponseModel<Portfolio>> DeletePortfolio(AppUser user, string symbol);
    }
}
