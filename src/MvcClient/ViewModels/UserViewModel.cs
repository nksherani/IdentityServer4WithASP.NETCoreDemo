using IdentityModel;
using MvcClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient.ViewModels
{

    public partial class UserViewModel
    {
        public UserViewModel()
        {
            AspNetUserClaims = new List<ClaimViewModel>();
           
        }
        public UserViewModel(AspNetUsers user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.EmailConfirmed = user.EmailConfirmed;
            this.PhoneNumber = user.PhoneNumber;
            this.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            this.TwoFactorEnabled = user.TwoFactorEnabled;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.IsEnabled = user.IsEnabled;

            this.AspNetUserClaims = user.AspNetUserClaims.Select(x => new ClaimViewModel(x)).ToList();

            var claim = user.AspNetUserClaims.Where(x => x.UserId == user.Id &&
                        x.ClaimType == JwtClaimTypes.Role &&
                        x.ClaimValue == "admin")?.FirstOrDefault();
            if (claim == null)
                this.IsAdmin = false;
            else
                this.IsAdmin = true;
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsAdmin { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsEnabled { get; set; }

        public virtual List<ClaimViewModel> AspNetUserClaims { get; set; }
        
    }

}
