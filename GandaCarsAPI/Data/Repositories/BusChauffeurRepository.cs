using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;

namespace GandaCarsAPI.Data.Repositories
{
    public class BusChauffeurRepository : IBusChauffeurRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<BusChauffeur> _busChauffeurs;

        public void Add(BusChauffeur bc)
        {
            _busChauffeurs.Add(bc);
        }

        public void Delete(BusChauffeur bc)
        {
            //_GebruikerItems.RemoveRange(_GebruikerItems.Where(t => t.Item.Id == Item.Id).ToList());
            _busChauffeurs.Remove(bc);
        }

        public IEnumerable<BusChauffeur> GetAll()
        {
            return _busChauffeurs.ToList();

        }

        public BusChauffeur GetBy(string id)
        {
            return _busChauffeurs.SingleOrDefault(r => r.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(BusChauffeur bc)
        {
            _context.Update(bc);
        }
    }
}
