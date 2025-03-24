using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{

    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {

        private readonly ApplicationDBContext _context; //atribut

        private readonly IStockRepository _stockRepo;

        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _context= context;
            _stockRepo= stockRepo;
        }

        [HttpGet]
        [Authorize]
        public  async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stocks= await _stockRepo.GetAllAsync(queryObject);
            var stocksDto= stocks.Select(s=> s.ToStockDto());

            return Ok(stocksDto);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> getById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock= await _stockRepo.GetStockById(id);

            if(stock==null)
            {
                return NotFound();

            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> createStock([FromBody] CreateStockReuqestDto stockDto)
        {

               // Ručno mapiranje (nije preporučljivo), ovo bismo kucali da nemamo toStockFromCreateDto
                // var stockModel = new Stock 
                // {
                //     Symbol = stockDto.Symbol,
                //     CompanyName = stockDto.CompanyName,
                //     Purchase = stockDto.Purchase,
                //     LastDiv = stockDto.LastDiv,
                //     Industry = stockDto.Industry,
                //     MarketCap = stockDto.MarketCap
                // };
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel= stockDto.ToStockFromCreateDto();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(getById), new {id= stockModel.ID}, stockModel.ToStockDto());
           



        }

        [HttpPut]
        [Route("{id:int}")]
        public  async Task<IActionResult> updateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

                 var stockModel= await  _stockRepo.UpdateAsync(id,updateDto);
                if(stockModel==null)
                {
                    return NotFound();
                }

                return Ok(stockModel.ToStockDto());
                
        }





        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> deleteStock([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel= await _stockRepo.DeleteAsync(id);
             if(stockModel==null)
                {
                    return NotFound();
                }
           
            return NoContent();
        }

    }
           

       
       

}