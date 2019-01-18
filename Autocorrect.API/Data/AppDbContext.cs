using Autocorrect.API.Data.DbEntities;
using Autocorrect.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autocorrect.API.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){
        }
        public DbSet<SpecialWord> SpecialWords { get; set; }
        public DbSet<SuggestedWord> SuggestedWords { get; set; }
        public DbSet<Licenses> Licenses { get; set; }
    }
}
