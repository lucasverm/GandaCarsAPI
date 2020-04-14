using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models.Interfaces
{
    public interface IFeestdagRepository
    {
        IEnumerable<Feestdag> GetAll();
        void AddRange(IEnumerable<Feestdag> fden);
        void DeleteRanger(IEnumerable<Feestdag> fden);
        void SaveChanges();
    }
}
