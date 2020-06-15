using System;
namespace GandaCarsAPI.Models.Interfaces
{
    public interface IInstellingenRepository
    {
        void Update(Instellingen instellingen);
        Instellingen GetInstellingen();
        void SaveChanges();
    }
}
