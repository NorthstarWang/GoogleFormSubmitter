using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace M8OU.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public DateTime GenerateTime { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public FormHistory FormHistory { get; set; }
    }
}
