using System;
using GandaCarsAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GandaCarsAPI.Data.Mappers
{
    public class StassionementConfiguratie : IEntityTypeConfiguration<Stassionement>
    {
        public void Configure(EntityTypeBuilder<Stassionement> builder)
        {
            
        }
    }
}