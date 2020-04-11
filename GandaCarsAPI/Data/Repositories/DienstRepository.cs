using System;
using System.Collections.Generic;
using System.Linq;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GandaCarsAPI.Data.Repositories
{
    public class DienstRepository : IDienstRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Dienst> _dienst;

        public DienstRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _dienst = dbContext.Diensten;
        }

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
            return _dienst.Include(t => t.BusChauffeur).ToList();

        }

        public Dienst GetBy(string id)
        {
            return _dienst.Include(t => t.BusChauffeur).SingleOrDefault(r => r.Id == id);
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
