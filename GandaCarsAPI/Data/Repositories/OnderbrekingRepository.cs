using System;
using System.Collections.Generic;
using System.Linq;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GandaCarsAPI.Data.Repositories
{
    public class OnderbrekingRepository : IOnderbrekingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Onderbreking> _onderbrekingen;

        public OnderbrekingRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _onderbrekingen = dbContext.Onderbrekingen;
        }

        public void Add(Onderbreking ond)
        {
            _onderbrekingen.Add(ond);
        }

        public void AddRange(List<Onderbreking> ond)
        {
            _onderbrekingen.AddRange(ond);
        }

        public void Delete(Onderbreking ond)
        {
            _onderbrekingen.Remove(ond);
        }

        public void DeleteRange(List<Onderbreking> ond)
        {
            _onderbrekingen.RemoveRange(ond);
        }

        public IEnumerable<Onderbreking> GetAll()
        {
            return _onderbrekingen.ToList();
        }

        public Onderbreking GetBy(string id)
        {
            return _onderbrekingen.FirstOrDefault(t => t.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Onderbreking ond)
        {
            _context.Update(ond);
        }
    }
}
