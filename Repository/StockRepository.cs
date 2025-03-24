using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {

 private readonly ApplicationDBContext _context; //atribut

       

        public StockRepository(ApplicationDBContext context)
        {
            _context= context;
        }

        public async  Task<Stock> CreateAsync(Stock StockModel)
        {
            await _context.AddAsync(StockModel);
            await _context.SaveChangesAsync();

            return StockModel;

        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel= await _context.Stock.FirstOrDefaultAsync(x=>x.ID==id);
            if(stockModel==null)
            {
                return null;
            }

            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;

        }

        public  Task<bool> DoesExist(int stockId)
        {
              return  _context.Stock.AnyAsync(x => x.ID == stockId);
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject queryobject)
        {
         var stocks= _context.Stock
                   .Include(s => s.comments) // Uključujemo komentare, bez include nece da se ucitaju komentari , lazy loading
                   .AsQueryable(); //

         if(!string.IsNullOrWhiteSpace(queryobject.CompanyName))
         {
            stocks= stocks.Where(s=>s.CompanyName.Contains(queryobject.CompanyName));
         }
         if(!string.IsNullOrWhiteSpace(queryobject.Symbol))
         {
            stocks= stocks.Where(s=>s.Symbol.Contains(queryobject.Symbol));
         }
         return await stocks.ToListAsync();
        }

     
        public async Task<Stock?> GetStockById(int id) //can return null
        {
           return await _context.Stock
                    .Include(s=>s.comments)
                    .FirstOrDefaultAsync(x=>x.ID==id);
           
        }

        public async Task<Stock?> GetStockBySymbol(string symbol)
        {
           return await _context.Stock.FirstOrDefaultAsync(stock=> stock.Symbol==symbol);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStock)
        {
           var existingModel=  await _context.Stock.FirstOrDefaultAsync(x=>x.ID==id);
           if(existingModel==null)
           {
            return null;
           }

                existingModel.Symbol= updateStock.Symbol;
                existingModel.MarketCap= updateStock.MarketCap;
                existingModel.Pruchase= updateStock.Pruchase;
                existingModel.Industry= updateStock.Industry;
                existingModel.LastDiv= updateStock.LastDiv;
                existingModel.CompanyName= updateStock.CompanyName;

                await _context.SaveChangesAsync();

                return existingModel;
        }
    }
}