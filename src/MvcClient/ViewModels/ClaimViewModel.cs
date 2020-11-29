
using MvcClient.Models;

namespace MvcClient.ViewModels
{
    public class ClaimViewModel
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public ClaimViewModel(AspNetUserClaims claim)
        {
            this.Id = claim.Id;
            this.ClaimType = claim.ClaimType;
            this.ClaimValue = claim.ClaimValue;
        }
    }

}
