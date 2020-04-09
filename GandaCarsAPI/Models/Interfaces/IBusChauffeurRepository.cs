using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models.Interfaces
{
    public interface IBusChauffeurRepository
    {
        BusChauffeur GetBy(string id);
        IEnumerable<BusChauffeur> GetAll();
        void Add(BusChauffeur bc);
        void Delete(BusChauffeur bc);
        void Update(BusChauffeur bc);
        void SaveChanges();
    }
}
