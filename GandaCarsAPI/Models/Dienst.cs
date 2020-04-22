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
        public DateTime DagVanToevoegen { get; set; }
        public BusChauffeur BusChauffeur { get; set; }
        public List<Stationnement> Stationnementen { get; set; }
        public Dienst()
        {
            Stationnementen = new List<Stationnement>();
            DagVanToevoegen = DateTime.Now;
        }
    }
}
