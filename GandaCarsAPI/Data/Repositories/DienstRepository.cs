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
            return _dienst.OrderBy(t => t.DagVanToevoegen).Include(t => t.BusChauffeur).Include(t => t.Onderbrekingen).ToList();

        }

        public Dienst GetBy(string id)
        {
            return _dienst.Include(t => t.BusChauffeur).Include(t => t.Onderbrekingen).SingleOrDefault(r => r.Id == id);
        }

        public String ValidateDienst(Dienst d)
        {
            if ((d.EindDag.GetHashCode() - d.StartDag.GetHashCode() != 1 && d.EindDag.GetHashCode() - d.StartDag.GetHashCode() != 0) && !(d.StartDag.GetHashCode() == 6 && d.EindDag.GetHashCode() == 0))
            {
                return "De data van de dienst zijn fout!";
            }
            if ((d.EindDag.GetHashCode() == d.StartDag.GetHashCode()) && (d.StartUur > d.EindUur))
            {
                return "De start tijd van de dienst valt voor de eind tijd!";
            }

            if (d.EindDag.GetHashCode() != d.StartDag.GetHashCode())
            {
                if (d.StartUur < d.EindUur)
                {
                    return "De dienst duurt te lang!";
                }

            }
            return null;
        }

        public String ValidateOnderbrekingMetDienst(Dienst d, Onderbreking o)
        {
            if (!(o.StartDag == d.StartDag || o.StartDag == d.EindDag))
            {
                return "Start dag van de onderbreking die start op " + o.StartDag + " om " +o.StartUur.TimeOfDay  + " komt niet overeen met dienst!";
            }
            else if (!(o.EindDag == d.StartDag || o.EindDag == d.EindDag))
            {
                return "Eind dag van onderbreking die start op " + o.StartDag + " om " + o.StartUur.TimeOfDay + " komt niet overeen met dienst!";
            }
            else if (o.EindDag.GetHashCode() - o.StartDag.GetHashCode() != 1 && o.EindDag.GetHashCode() - o.StartDag.GetHashCode() != 0)
            {
                return "De data van de onderbreking die start op " + o.StartDag + " om " + o.StartUur.TimeOfDay + " zijn fout!";
                //dienst op zelfde dag
            }
            else if (d.StartDag.GetHashCode() == d.EindDag.GetHashCode())
            {
                if (o.StartUur < d.StartUur)
                {
                    return "Het start uur van de onderbreking die start op " + o.StartDag + " om " + o.StartUur.TimeOfDay + " valt voor het start uur van de dienst.";
                }
                else if (o.EindUur > d.EindUur)
                {
                    return "Het eind uur van de onderbreking die start op " + o.StartDag + " om " + o.StartUur.TimeOfDay + " valt na het eind uur van de dienst.";
                }
                //dienst op 2 dagen
            }
            else if (d.StartDag.GetHashCode() != d.EindDag.GetHashCode())
            {
                if (o.StartDag.GetHashCode() == o.EindDag.GetHashCode() && o.StartUur > o.EindUur)
                {
                    return "De tijdstippen van de onderbreking die start op " + o.StartDag + " om " + o.StartUur.TimeOfDay + " zijn fout!";
                }
            }
            if (d.StartDag.GetHashCode() == o.StartDag.GetHashCode() && o.StartUur < d.StartUur)
            {
                return "Het start uur van de onderbreking die start op " + o.StartDag + " om " + o.StartUur.TimeOfDay + " valt voor het start uur van de dienst.";
            }
            else if (d.EindDag.GetHashCode() == o.EindDag.GetHashCode() && o.EindUur > d.EindUur)
            {
                return "Het eind uur van de onderbreking die start op " + o.StartDag + " om " + o.StartUur.TimeOfDay + " valt na het eind uur van de dienst.";
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
