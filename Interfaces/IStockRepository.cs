using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject obj);
        Task<Stock?> GetStockById(int id); // ? cuz we gonna use FirstOrDefault and it can return  null
        Task<Stock?> GetStockBySymbol(string symbol);
        Task<Stock> CreateAsync(Stock StockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStock);
        Task<Stock?> DeleteAsync(int id);

        Task<bool> DoesExist(int stockId);
    }
}