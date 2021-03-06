﻿using System;
using System.Collections.Generic;

namespace GandaCarsAPI.Models.Interfaces
{
    public interface IDienstRepository
    {
        Dienst GetBy(string id);
        IEnumerable<Dienst> GetAll();
        String ValidateDienst(Dienst d);
        String ValidateOnderbrekingMetDienst(Dienst d, Onderbreking o);
        void Add(Dienst bc);
        void Delete(Dienst dienst);
        void Update(Dienst dienst);
        void SaveChanges();
    }
}
