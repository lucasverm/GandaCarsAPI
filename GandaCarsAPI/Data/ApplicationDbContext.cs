using System;
using GandaCarsAPI.Data.Mappers;
using GandaCarsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GandaCarsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<BusChauffeur> BusChauffeurs { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new BusChauffeurConfiguratie());

        }
    }
}
