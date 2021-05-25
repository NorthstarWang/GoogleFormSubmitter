using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using M8OU.Models;

namespace M8OU.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<FormHistory> FormHistories { get; set; }
        public DbSet<Report> Reports { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
