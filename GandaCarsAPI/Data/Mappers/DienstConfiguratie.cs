using System;
using GandaCarsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GandaCarsAPI.Data.Mappers
{
    public class DienstConfiguratie : IEntityTypeConfiguration<Dienst>
    {
        public void Configure(EntityTypeBuilder<Dienst> builder)
        {
            builder.Property(g => g.Naam).IsRequired();
        }
    }
}