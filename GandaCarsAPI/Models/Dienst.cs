using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models
{
    public class Dienst
    {
        public string Id { get; set; }
        public string Naam { get; set; }
        public DateTime StartUur { get; set; }
        public DateTime EindUur { get; set; }
        public DayOfWeek StartDag { get; set; }
        public DayOfWeek EindDag { get; set; }
        public BusChauffeur BusChauffeur { get; set; }
        public List<Stationnement> stationnementen { get; set; }
        public Dienst()
        {
            stationnementen = new List<Stationnement>();
        }
    }
}
