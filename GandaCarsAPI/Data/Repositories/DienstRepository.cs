using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;

namespace GandaCarsAPI.Data.Repositories
{
    public class DienstRepository : IDienstRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Dienst> _dienst;

        public void Add(Dienst dienst)
        {
            _dienst.Add(dienst);
        }

        public void Delete(Dienst dienst)
        {
            _dienst.Remove(dienst);
        }

        public IEnumerable<Dienst> GetAll()
        {
            return _dienst.ToList();

        }

        public Dienst GetBy(string id)
        {
            return _dienst.SingleOrDefault(r => r.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Dienst dienst)
        {
            _context.Update(dienst);
        }
    }
}
