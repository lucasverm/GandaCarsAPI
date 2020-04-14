using System;
namespace GandaCarsAPI.DTO
{
    public class BusChauffeurDTO
    {
        public string Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Email { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public int Uurloon { get; set; }
    }
}
