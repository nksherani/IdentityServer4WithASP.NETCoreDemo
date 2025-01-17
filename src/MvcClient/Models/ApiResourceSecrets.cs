﻿using System;
using System.Collections.Generic;

namespace MvcClient.Models
{
    public partial class ApiResourceSecrets
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public int ApiResourceId { get; set; }

        public virtual ApiResources ApiResource { get; set; }
    }
}
