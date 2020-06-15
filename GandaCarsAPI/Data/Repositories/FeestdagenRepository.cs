using System;
using System.Collections.Generic;
using System.Linq;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GandaCarsAPI.Data.Repositories
{
    public class FeestdagRepository : IFeestdagRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Feestdag> _feestdagen;

        public FeestdagRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _feestdagen = dbContext.Feestdagen;
        }

        public void AddRange(IEnumerable<Feestdag> fden)
        {
            _feestdagen.AddRange(fden);
        }

        public void DeleteRanger(IEnumerable<Feestdag> fden)
        {
            _feestdagen.RemoveRange(fden);
        }

        public IEnumerable<Feestdag> GetAll()
        {
            return _feestdagen
                .OrderBy(t => t.Dag).ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
