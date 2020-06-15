using System;
namespace GandaCarsAPI.Models
{
    public class Onderbreking
    {
        public String Id { get; set; }
        public DateTime StartUur { get; set; }
        public DateTime EindUur { get; set; }
        public DayOfWeek StartDag { get; set; }
        public DayOfWeek EindDag { get; set; }
        public DateTime EffectieveStart { get; set; }
        public DateTime EffectiefEinde{ get; set; }
        public Onderbreking()
        {
        }
    }
}
