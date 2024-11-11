using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> PostPortfolioAsync(Portfolio portfolio);
        Task<bool> IsDuplicateAsync(Portfolio portfolio);
        Task<Portfolio?> DeleteAsync(AppUser user, string symbol);
        Task<bool> PortfolioExists(AppUser user, string symbol);
    }
}
