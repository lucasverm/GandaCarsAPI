using System;
namespace GandaCarsAPI.Models
{
    public class Instellingen
    {
        public string Id { get; set; }
        public int AantalMinutenAdministratieveTijdVoorDienst { get; set; }
        public int Stelsel { get; set; }
        public Instellingen()
        {
            Stelsel = 1;
            AantalMinutenAdministratieveTijdVoorDienst = 15;
        }
    }
}
