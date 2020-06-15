using System;
using System.Collections.Generic;
using System.Linq;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GandaCarsAPI.Data.Repositories
{
    public class InstellingenRepository : IInstellingenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Instellingen> _instellingen;

        public InstellingenRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _instellingen = dbContext.Instellingen;
        }

        public void Update(Instellingen instellingen)
        {
            _instellingen.Update(instellingen);
        }

        public Instellingen GetInstellingen()
        {
            var instellingen = _instellingen.FirstOrDefault();
            if(instellingen == null)
            {
                _instellingen.Add(new Instellingen());
                this.SaveChanges();
                return _instellingen.FirstOrDefault();
            }
            return instellingen;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
