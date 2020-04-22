using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models
{
    public class EffectieveDienst
    {
        public string Id { get; set; }
        public string Naam { get; set; }
        public DateTime Start { get; set; }
        public DateTime Eind { get; set; }
        public BusChauffeur BusChauffeur { get; set; }
        public List<Stationnement> Stationnementen { get; set; }
        public DateTime DagVanToevoegen { get; set; }
        public EffectieveDienst GerelateerdeDienst { get; set; }
        public EffectieveDienst()
        {
            Stationnementen = new List<Stationnement>();
            DagVanToevoegen = DateTime.Now;
        }
    }
}
