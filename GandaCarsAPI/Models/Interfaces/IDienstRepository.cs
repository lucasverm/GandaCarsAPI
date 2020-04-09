using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models.Interfaces
{
    public interface IDienstRepository
    {
        Dienst GetBy(string id);
        IEnumerable<Dienst> GetAll();
        void Add(Dienst bc);
        void Delete(Dienst dienst);
        void Update(Dienst dienst);
        void SaveChanges();
    }
}
