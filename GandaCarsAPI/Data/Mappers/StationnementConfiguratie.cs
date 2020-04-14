using System;
using GandaCarsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GandaCarsAPI.Data.Mappers
{
    public class stationnementConfiguratie : IEntityTypeConfiguration<Stationnement>
    {
        public void Configure(EntityTypeBuilder<Stationnement> builder)
        {
            builder
             .Property(f => f.Id)
             .ValueGeneratedOnAdd();
        }
    }
}