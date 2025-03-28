using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{

    [Table("Stocks")]
    public class Stock
    {
        public int ID {get; set; }
        
        public string  Symbol {get; set; } = string.Empty;

        public string CompanyName {get; set; }= string.Empty;
        [Column (TypeName ="decimal(18,2)")]

        public decimal Pruchase {get; set;}
         [Column (TypeName ="decimal(18,2)")]

         public decimal LastDiv {get; set;}

         public string Industry {get; set;}= string.Empty;

         public long MarketCap  {get; set;}

         public List<Comment> comments{get; set;} = new List<Comment> ();

        public List<Portfolio> Portofolios{ get; set; } =new List<Portfolio> ();

    }
}