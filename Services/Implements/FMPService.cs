using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repositories.Models;
using Services.ApiResponse;
using Services.DTOs.Stock;
using Services.Interfaces;
using Services.Mappers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Services.Implements
{
    public class FMPService : IFMPService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        public FMPService(HttpClient httpClient, IConfiguration config)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<ApiResponseModel<Stock>> FindStockBySymbolAsync(string symbol)
        {
            var response = new ApiResponseModel<Stock>()
            {
                Success = false,
                Data = null
            };
            try
            {
               
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock = tasks[0];
                    if (stock != null)
                    {
                        response.Success = true;
                        response.Data = stock.ToStockFromFMP();
                        return response;
                    }
                    response.Message = "Not found!";
                    return response;
                }
                response.Message = "Error has occured. Status code: " + result.StatusCode;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Error has occured: " + ex.Message;
                return response;
            }
        }
    }
}
