using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models.Interfaces
{
    public interface IstationnementRepository
    {
        Stationnement GetBy(string id);
        void Update(Stationnement s);
        void Add(Stationnement s);
        void SaveChanges();
        Stationnement Delete(Stationnement s);
    }
}
