using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentityServer.Data.Entities;

namespace IdentityServer.Data
{
   
    public class AppUserContext : IdentityDbContext<ApplicationUser>
    {
        public AppUserContext(DbContextOptions<AppUserContext> options) : base(options)
        {

        }

      

        protected override void OnModelCreating(ModelBuilder builder)
        {
           base.OnModelCreating(builder);
        }
    }

}
