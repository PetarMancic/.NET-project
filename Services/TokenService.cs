using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public readonly  SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config=config;
            _key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"])); 
        }
        public string CreateToken(AppUser user)
        {
           var claims= new List<Claim>();
           claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
           claims.Add(new Claim(JwtRegisteredClaimNames.GivenName,user.UserName) );
          

           var creds=new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

           //ovde cemo da generisemo token, taccnije kako ce da izgleda
           var tokenDescriptor= new SecurityTokenDescriptor
           {
            Subject=new ClaimsIdentity(claims),
            Expires= DateTime.Now.AddDays(7), //koliko ce da traje token 
            SigningCredentials= creds,
            Issuer=_config["JWT:Issuer"],
            Audience=_config["JWT:Audience"]
           };
        //dodatna metoda koja ce zapravo da kreira sam token 
        var tokenHandler= new JwtSecurityTokenHandler();

        var token=tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
        



        }
    }
}