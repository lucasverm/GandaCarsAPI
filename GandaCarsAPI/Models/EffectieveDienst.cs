using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models
{
    public class EffectieveDienst
    {
        public string Id { get; set; }
        public string Naam { get; set; }
        public DateTime Start { get; set; }
        public DateTime Einde { get; set; }
        public BusChauffeur BusChauffeur { get; set; }
        public int TotaalAantalMinutenStationnement { get; set; }
        public int AndereMinuten { get; set; }
        public DateTime DagVanToevoegen { get; set; }
        public EffectieveDienst GerelateerdeDienst { get; set; }
        public List<Onderbreking> Onderbrekingen { get; set; }
        public EffectieveDienst()
        {
            DagVanToevoegen = DateTime.Now;
            Onderbrekingen = new List<Onderbreking>();
        }
    }
}
