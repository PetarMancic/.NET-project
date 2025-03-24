using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {

        private readonly ApplicationDBContext _context;

        public PortfolioRepository(ApplicationDBContext applicationDBContext)
        {
            _context=applicationDBContext;
        }

        public async Task<Portfolio> CreatePortfolio(Portfolio portfolio)
        {
             _context.Portfolios.Add(portfolio);
              await _context.SaveChangesAsync();
              return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(string symbol, AppUser appUser)
        {
            var portfolioModel= _context.Portfolios.FirstOrDefault(port=> port.AppUserId==appUser.Id && port.Stock.Symbol.ToLower()== symbol);
            if(portfolioModel== null)
            {
                return null;
            }
            _context.Portfolios.Remove(portfolioModel);
             await _context.SaveChangesAsync();
             return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(p=> p.AppUserId==user.Id)
                .Select(stock=> new Stock
                {
                    ID= stock.stockId,
                    Symbol= stock.Stock.Symbol,
                    CompanyName= stock.Stock.CompanyName,
                    Pruchase= stock.Stock.Pruchase,
                    LastDiv= stock.Stock.LastDiv,
                    Industry= stock.Stock.Industry,
                    MarketCap= stock.Stock.MarketCap
                }).ToListAsync();
        }

        
    }
}