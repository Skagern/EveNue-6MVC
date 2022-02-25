using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GruppNrSexMVC.Models;

namespace GruppNrSexMVC.Models
{
    public class SponsorContext : DbContext
    {
        public DbSet<Sponsor> Sponsors { get; set; }
        public SponsorContext(DbContextOptions options) : base(options)
        {

        }
        
    }
}


