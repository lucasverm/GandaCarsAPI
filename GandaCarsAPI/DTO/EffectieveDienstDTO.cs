using System;
using System.Collections.Generic;
using GandaCarsAPI.Models;

namespace GandaCarsAPI.DTO
{
    public class EffectieveDienstDTO
    {
        public string Id { get; set; }
        public string Naam { get; set; }
        public DateTime Start { get; set; }
        public DateTime Einde { get; set; }
        public String BusChauffeurId { get; set; }
        public int AndereMinuten { get; set; }
        public int TotaalAantalMinutenStationnement { get; set; }
        public List<Onderbreking> Onderbrekingen { get; set; }
        public EffectieveDienstDTO() { }
    }
}
