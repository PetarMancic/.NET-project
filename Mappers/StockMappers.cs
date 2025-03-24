using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
         public static StockDto ToStockDto(this Stock stockModel)
        {
            return new  StockDto
            {
                ID= stockModel.ID,
                Symbol= stockModel.Symbol,
                CompanyName= stockModel.CompanyName,
                Pruchase= stockModel.Pruchase,
                LastDiv= stockModel.LastDiv,
                Industry=stockModel.Industry,
                MarketCap= stockModel.MarketCap,
                Comments= stockModel.comments.Select(x=>x.ToCommentDto()).ToList()
               
            };
            
        }

            public static Stock ToStockFromCreateDto(this CreateStockReuqestDto stockDto)
            {
                 return new Stock 
                {
                Symbol= stockDto.Symbol,
                CompanyName= stockDto.CompanyName,
                Pruchase= stockDto.Pruchase,
                LastDiv= stockDto.LastDiv,
                Industry= stockDto.Industry,
                MarketCap= stockDto.MarketCap
               };
            }
        
    }
}