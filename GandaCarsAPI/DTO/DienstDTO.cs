using System;
namespace GandaCarsAPI.DTO
{
    public class DienstDTO
    {
        public string Id { get; set; }
        public string Naam { get; set; }
        public DateTime StartUur { get; set; }
        public DateTime EindUur { get; set; }
        public DayOfWeek Dag { get; set; }
        public DienstDTO() { }
    }
}
