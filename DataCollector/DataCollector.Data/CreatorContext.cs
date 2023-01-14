using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataCollector.Data
{
    public class CreatorContext : DbContext
    {
        public DbSet<CreatorEntity?> Creator { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=PLUTO-0101;Database=Creators;Trusted_Connection=True;Encrypt=False");
        }
    }
    
}
