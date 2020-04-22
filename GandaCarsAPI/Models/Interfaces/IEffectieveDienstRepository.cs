using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models.Interfaces
{
    public interface IEffectieveDienstRepository
    {
        EffectieveDienst GetBy(string id);
        IEnumerable<EffectieveDienst> GetAllVan(BusChauffeur bc);
        IEnumerable<EffectieveDienst> GetAllVan(BusChauffeur bc, String jaar, String week);
        IEnumerable<EffectieveDienst> DeleteAllVan(BusChauffeur bc, String jaar, String week);
        IEnumerable<EffectieveDienst> GetAllByMonth(BusChauffeur bc, String jaar, int maand);
        IEnumerable<EffectieveDienst> GetAll();
        void Add(EffectieveDienst ed);
        List<EffectieveDienst> AddRange(List<EffectieveDienst> ed);
        void DeleteRange(List<EffectieveDienst> ed);
        void Delete(EffectieveDienst ed);
        void Update(EffectieveDienst ed);
        void SaveChanges();
        
    }
}
