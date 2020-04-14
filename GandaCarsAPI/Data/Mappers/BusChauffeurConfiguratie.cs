using System;
using GandaCarsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GandaCarsAPI.Data.Mappers
{
    public class BusChauffeurConfiguratie : IEntityTypeConfiguration<BusChauffeur>
    {
        public void Configure(EntityTypeBuilder<BusChauffeur> builder)
        {
            builder.Property(g => g.Voornaam).IsRequired();
            builder.HasMany(g => g.Diensten).WithOne();
        }
    }
}
