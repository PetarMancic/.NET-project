using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;

namespace api.Dtos.Stock
{
    public class StockDto
    {
        public int ID {get; set; }    
        public string  Symbol {get; set; } = string.Empty;
        public string CompanyName {get; set; }= string.Empty;     
        public decimal Pruchase {get; set;}
         public decimal LastDiv {get; set;}
         public string Industry {get; set;}= string.Empty;
         public long MarketCap  {get; set;}

         public List<CommentDto> Comments {get; set;}

    }
}