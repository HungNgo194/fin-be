using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Interfaces;
using Repositories.Models;
using Services.ApiResponse;
using Services.Interfaces;

namespace Services.Implements
{
    public class PortfolioService : IPortfolioService
    { 
        private readonly IPortfolioRepository _portfolioRepository;
        public PortfolioService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
            
        }

        public async Task<ApiResponseModel<Portfolio>> CreatePortfolio(Portfolio portfolio)
        {
            var response = new ApiResponseModel<Portfolio>()
            {
                Success = false,
                Data = null
            };

            if (await _portfolioRepository.IsDuplicateAsync(portfolio)) 
            {
                response.Message = "Stock is duplicated";
                return response;
            }

            var userModel = await _portfolioRepository.PostPortfolioAsync(portfolio);
            if (userModel is null)
            {
                response.Message = "User list is empty!";
                return response;
            }

            response.Data = userModel;
            response.Success = true;
            return response;
        }

        public async Task<ApiResponseModel<Portfolio>> DeletePortfolio(AppUser user, string symbol)
        {
            var response = new ApiResponseModel<Portfolio>()
            {
                Success = false,
                Data = null
            };

            if (!await _portfolioRepository.PortfolioExists(user, symbol))
            {
                response.Message = "Stock does not exist in the portfolio";
                return response;
            }

            var portModel = await _portfolioRepository.DeleteAsync(user, symbol);
            response.Success = true;
            response.Data = portModel;
            response.Message = "Delete Successful!";
            return response;
        }

        public async Task<ApiResponseModel<List<Stock>>> GetUserPortfolio(AppUser user)
        {
            var response = new ApiResponseModel<List<Stock>>() 
            { 
                Success = false,
                Data = null
            };

            var portModel = await _portfolioRepository.GetUserPortfolio(user);
            if (portModel is null)
            {
                response.Message = "Portfolio list is empty!";
                return response;
            }

            response.Data = portModel;
            response.Success = true;
            return response;
            
        }
    }
}
