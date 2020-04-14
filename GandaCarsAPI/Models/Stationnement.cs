using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GandaCarsAPI.Models
{
    public class Stationnement
    {
        public String Id { get; set; }
        public DateTime StartUur { get; set; }
        public DateTime EindUur { get; set; }
        public Decimal Tarief { get; set; }
        public DayOfWeek Dag { get; set; }
        public Stationnement()
        {
        }
    }
}
