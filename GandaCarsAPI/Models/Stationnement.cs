using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GandaCarsAPI.Models
{
    public class Stationnement
    {
        public String Id { get; set; }
        public int AantalMinuten { get; set; }
        public Decimal Percentage { get; set; }
        public Stationnement()
        {
        }
    }
}
