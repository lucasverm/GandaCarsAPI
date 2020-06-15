using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models.Interfaces
{
    public interface IOnderbrekingRepository
    {
        Onderbreking GetBy(string id);
        IEnumerable<Onderbreking> GetAll();
        void Add(Onderbreking ond);
        void Delete(Onderbreking ond);
        void AddRange(List<Onderbreking> ond);
        void DeleteRange(List<Onderbreking> ond);
        void Update(Onderbreking ond);
        void SaveChanges();
    }
}
