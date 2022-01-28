using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Id_And_Access_Processor
{
    //common store for all claims
    public class ClaimsStore 
    {
        public Int64 Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string Description { get; set; }
        public string UserOrRoleClaim { get; set; }
    }
}

