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
            return _dienst.OrderBy(t => t.DagVanToevoegen).Include(t => t.BusChauffeur).ToList();

        }

        public Dienst GetBy(string id)
        {
            return _dienst.Include(t => t.BusChauffeur).SingleOrDefault(r => r.Id == id);
        }

        public String ValidateDienst(Dienst d)
        {
            if ((d.EindDag.GetHashCode() - d.StartDag.GetHashCode() != 1 && d.EindDag.GetHashCode() - d.StartDag.GetHashCode() != 0) && !(d.StartDag.GetHashCode() == 6 && d.EindDag.GetHashCode() == 0))
            {
                return "De data van de dienst zijn fout!";
            }
            if ((d.EindDag.GetHashCode() == d.StartDag.GetHashCode()) && (d.StartUur > d.EindUur))
            {
                return "De start tijd valt voor de eind tijd!";
            }

            if (d.EindDag.GetHashCode() != d.StartDag.GetHashCode())
            {
                if(d.StartUur < d.EindUur)
                {
                    return "De dienst duurt te lang!";
                }
                
            }
            return null;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Dienst dienst)
        {
            _dienst.Update(dienst);
        }
    }
}
