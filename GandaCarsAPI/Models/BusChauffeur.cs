using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models
{
    public class BusChauffeur
    {
        public string Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public int Uurloon { get; set; }
        public string Email { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public List<Dienst> Diensten { get; set; }
        public BusChauffeur()
        {
            Diensten = new List<Dienst>();
        }
    }
}
