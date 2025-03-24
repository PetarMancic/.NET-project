using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CreateStockReuqestDto
    {
        
        [Required]
        [MaxLength(10, ErrorMessage ="Symbol can not be over 10  characters!")]
         public string  Symbol {get; set; } = string.Empty;

        [Required]
        [MaxLength(10, ErrorMessage ="CompanyName can not be over 10  characters!")]
        public string CompanyName {get; set; }= string.Empty;
       
        [Required]
        [Range(0,1000000)]
        public decimal Pruchase {get; set;}
        
        [Required]
        [Range(0.001,1000000)]
         public decimal LastDiv {get; set;}

        [Required]
        [MaxLength(20, ErrorMessage ="Industry can not be over 20  characters!")]
         public string Industry {get; set;}= string.Empty;

        [Required]
        [Range(0,5000000000)]
         public long MarketCap  {get; set;}
    }
}