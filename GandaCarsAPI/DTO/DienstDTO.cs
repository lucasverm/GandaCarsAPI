using System;
using System.Collections.Generic;
using GandaCarsAPI.Models;

namespace GandaCarsAPI.DTO
{
    public class DienstDTO
    {
        public string Id { get; set; }
        public string Naam { get; set; }
        public DateTime StartUur { get; set; }
        public DateTime EindUur { get; set; }
        public DayOfWeek StartDag { get; set; }
        public DayOfWeek EindDag { get; set; }
        public String BusChauffeurId { get; set; }
        public List<Stationnement> stationnementen { get; set; }
        public DienstDTO() { }
    }
}
