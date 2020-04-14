using System;
using System.Linq;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GandaCarsAPI.Data.Repositories
{
    public class stationnementRepository: IstationnementRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Stationnement> _stationnementen;

        public stationnementRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _stationnementen = dbContext.stationnementen;
        }
        public stationnementRepository()
        {
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Stationnement Delete(Stationnement s)
        {
            _stationnementen.Remove(s);
            return s;
        }

        public Stationnement GetBy(string id)
        {
            return _stationnementen.SingleOrDefault(r => r.Id == id);
        }

        public void Update(Stationnement s)
        {
            _stationnementen.Update(s);
        }

        public void Add(Stationnement s)
        {
            s.Id = null;
            _stationnementen.Add(s);
        }

        
    }
}
