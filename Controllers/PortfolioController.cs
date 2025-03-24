using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepo;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo,IPortfolioRepository portfolio)
        {
            _userManager = userManager;
            _stockRepository= stockRepo;
            _portfolioRepo= portfolio;
        }

        [HttpGet("api/GetPortfolio")]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username= User.GetUsername();
            var appUser= await _userManager.FindByNameAsync(username);
            var userPortfolio= await _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost("api/createPortfolio")]
        [Authorize]
        public async Task<IActionResult> CreatePortfolio(string stockSymbol)
        {
            var username= User.GetUsername(); //nadjemo username
            var AppUser= await _userManager.FindByNameAsync(username);

            var stock= _stockRepository.GetStockBySymbol(stockSymbol);

            var portfolioObject= new Portfolio
            {
                stockId= stock.Id,
                AppUserId= AppUser.Id
            };

            //await _portfolioRepo.createPortfolio
            await _portfolioRepo.CreatePortfolio(portfolioObject);

            if(portfolioObject==null)
            {
                return StatusCode(500, "Could not create ");
            }
            return Ok(portfolioObject);

        }

        [HttpDelete("api/deletePortfolio")]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio( string StockSymbol)
        {
            var username= User.GetUsername();
            var AppUser= await _userManager.FindByNameAsync(username);

            var userPortfolio= await _portfolioRepo.GetUserPortfolio(AppUser);

            var filterStock= userPortfolio
                            .Where(userPort=> userPort.Symbol.ToLower()== StockSymbol.ToLower())
                            .ToList();

            if(filterStock.Count()==1)
            {
                await _portfolioRepo.DeletePortfolio(StockSymbol,AppUser);
            }
            else
            {
                return BadRequest("Stock is not in ur portfolio!");
            }

            return Ok();

        }
 
    }
}