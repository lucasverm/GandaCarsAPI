using System;
using GandaCarsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GandaCarsAPI.Data.Mappers
{
    public class EffectieveDienstConfiguratie : IEntityTypeConfiguration<EffectieveDienst>
    {
        public void Configure(EntityTypeBuilder<EffectieveDienst> builder)
        {
            builder.HasOne(t => t.GerelateerdeDienst).WithOne();
        }
    }
}