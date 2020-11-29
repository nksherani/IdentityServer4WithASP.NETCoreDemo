using System;
using System.Collections.Generic;

namespace MvcClient.Models
{
    public partial class ApiResourceClaims
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int ApiResourceId { get; set; }

        public virtual ApiResources ApiResource { get; set; }
    }
}
