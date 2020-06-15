using System;
using System.Collections.Generic;
using System.Linq;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GandaCarsAPI.Data.Repositories
{
    public class BusChauffeurRepository : IBusChauffeurRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<BusChauffeur> _busChauffeurs;

        public BusChauffeurRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _busChauffeurs = dbContext.BusChauffeurs;
        }

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
            return _busChauffeurs.OrderBy(t => t.Achternaam).Include(t => t.Diensten).ToList();
        }

        public BusChauffeur GetBy(string id)
        {
            return _busChauffeurs.Include(t => t.Diensten).ThenInclude(t => t.Onderbrekingen).SingleOrDefault(r => r.Id == id);
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
