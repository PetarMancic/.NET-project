using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    //Test1234@Password!
    //user1234@example.com
    //Petar
    [ApiController]
    [Route("api/account")]
    public class AccountController: ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
     public AccountController(UserManager<AppUser> userManager,ITokenService tokenService, SignInManager<AppUser> signInManager)
     {
        _userManager= userManager;
        _tokenService= tokenService;
        _signInManager= signInManager;
     }   


    [HttpPost("register")]
     public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
     {
       Console.WriteLine("Vrednost  za registe rje ",registerDto==null);

        try{
    if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser= new AppUser
            {
                UserName=registerDto.Username,
                Email=registerDto.Email
            };

            var createdUser= await _userManager.CreateAsync(appUser,registerDto.Password);

            if(createdUser.Succeeded)
            {
                var roleResult= await _userManager.AddToRoleAsync(appUser,"User");
                if(roleResult.Succeeded)
                {
                    return Ok(
                        new NewUserDto{
                            Username=appUser.UserName,
                            Email=appUser.Email,
                            Token= _tokenService.CreateToken(appUser)

                        }
                    );
                }
                else
                {
                    return StatusCode(500,roleResult.Errors);

                }
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }

        }
            catch(Exception ex)
            {
                return  StatusCode(500,ex);
            }
        
    }  

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login (LoginDto loginDto)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user= await _userManager.Users.FirstOrDefaultAsync(x=> x.UserName==loginDto.Username.ToLower());
        if(user==null)
        {   
            return Unauthorized("Invalid  username"); 
        }

        var result= await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if(!result.Succeeded)
        {
            return Unauthorized("Invalid username/password! ");
        }
        return Ok(
            new NewUserDto{
                Username= user.UserName,
                Email=user.Email,
                Token= _tokenService.CreateToken(user)
            }
        );
    } 
}
    
}
