using System;
using GandaCarsAPI.Data.Mappers;
using GandaCarsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GandaCarsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<BusChauffeur> BusChauffeurs { get; set; }
        public DbSet<Dienst> Diensten { get; set; }
        public DbSet<Feestdag> Feestdagen { get; set; }
        public DbSet<EffectieveDienst> EffectieveDiensten { get; set; }
        public DbSet<Instellingen> Instellingen { get; set; }
        public DbSet<Onderbreking> Onderbrekingen { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new BusChauffeurConfiguratie());
            builder.ApplyConfiguration(new DienstConfiguratie());
            builder.ApplyConfiguration(new EffectieveDienstConfiguratie());
        }
    }
}
