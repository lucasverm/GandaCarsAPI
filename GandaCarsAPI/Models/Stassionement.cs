using System;
namespace GandaCarsAPI.Models
{
    public class Stassionement
    {
        public String Id { get; set; }
        public DateTime Beginuur { get; set; }
        public DateTime EindUur { get; set; }
        public Decimal Tarief { get; set; }
        public Stassionement()
        {
        }
    }
}
