using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
       Task<List<Stock>> GetUserPortfolio(AppUser user);
       Task<Portfolio> CreatePortfolio(Portfolio portfolio);
       Task<Portfolio> DeletePortfolio(string symbol, AppUser appUser);
    }
}