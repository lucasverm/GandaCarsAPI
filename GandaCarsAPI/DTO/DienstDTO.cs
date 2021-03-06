﻿using System;
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
        public int TotaalAantalMinutenStationnement { get; set; }
        public List<Onderbreking> Onderbrekingen { get; set; }
        public DienstDTO() { }
    }
}
