using System;
namespace GandaCarsAPI.Models
{
    public class Dienst
    {
        public string Id { get; set; }
        public string Naam { get; set; }
        public DateTime StartUur { get; set; }
        public DateTime EindUur { get; set; }
        public Dienst()
        {
        }
    }
}
