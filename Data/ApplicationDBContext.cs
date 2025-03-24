using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet <Stock> Stock {get; set;}
        public DbSet <Comment> Comments {get; set;}
        public DbSet<Portfolio> Portfolios {get; set;}

        // public DbSet<User> Users {get; set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        //mapiranje M:N veze
        //1. dodati primarni kljuc, kombinacija 2 fk 
        //2. modelirati Portfolio -> AppUser
        //3. modelirati Portoflio -> Stock 

      //1.
        builder.Entity<Portfolio>()
                .HasKey(p=> new { p.AppUserId, p.stockId});

        //2.
        builder.Entity<Portfolio>()
        .HasOne(p=> p.AppUser) //Portfolio ima jednog AppUsera
        .WithMany(ap=> ap.Portfolios)  // APpUser moze da ima vise portfolia
        .HasForeignKey(p=> p.AppUserId);

        //3.
        builder.Entity<Portfolio>() 
        .HasOne(p=> p.Stock)// portfolio moze da ima jedan stock 
        .WithMany(st=> st.Portofolios)  //stock moze da ima vise portfolia
        .HasForeignKey(p=> p.stockId);  //strani kljuc 

            List<IdentityRole> roles= new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="ADMIN"
                },
                 new IdentityRole
                {
                    Name="User",
                    NormalizedName="USER"
                }
                
            };
            builder.Entity<IdentityRole>().HasData(roles);


        }





    }
}