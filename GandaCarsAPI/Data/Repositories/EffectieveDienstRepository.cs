using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GandaCarsAPI.Data.Repositories
{
    public class EffectieveDienstRepository : IEffectieveDienstRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<EffectieveDienst> _effectieveDiensten;


        public EffectieveDienstRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _effectieveDiensten = dbContext.EffectieveDiensten;

        }

        public void Add(EffectieveDienst ed)
        {
            _effectieveDiensten.Add(ed);
        }

        public List<EffectieveDienst> AddRange(List<EffectieveDienst> ed)
        {
            _effectieveDiensten.AddRange(ed);
            return ed;
        }

        public List<EffectieveDienst> GetAllByDatum(DateTime date)
        {
            return _effectieveDiensten.OrderBy(t => t.DagVanToevoegen).Where(d => d.Start == date || d.Eind == date).Include(t => t.BusChauffeur).ToList();
        }

        public void Delete(EffectieveDienst ed)
        {
            _effectieveDiensten.Remove(ed);
        }

        public IEnumerable<EffectieveDienst> GetAllVan(BusChauffeur bc)
        {
            return _effectieveDiensten.OrderBy(t => t.DagVanToevoegen).Where(t => t.BusChauffeur == bc).Include(t => t.BusChauffeur).Include(t => t.GerelateerdeDienst).ToList();
        }

        public IEnumerable<EffectieveDienst> GetAllVan(BusChauffeur bc, String jaar, String week)
        {
            //get weeknummer
            CultureInfo myCI = new CultureInfo("nl-BE");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            int jaartal = Int32.Parse(jaar);
            int weekNummer = Int32.Parse(week);
            var uitvoer = this.GetAllVan(bc);
            uitvoer = uitvoer.Where(t => myCal.GetWeekOfYear(t.Start, myCWR, myFirstDOW) == weekNummer && t.Start.Year == jaartal).ToList();
            return uitvoer;
        }

        public IEnumerable<EffectieveDienst> DeleteAllVan(BusChauffeur bc, String jaar, String week)
        {
            var toDelete = this.GetAllVan(bc, jaar, week).ToList();
            toDelete.ForEach(t =>
            {
                if (this.GetBy(t.Id) != null)
                {
                    var gerelateerd = t.GerelateerdeDienst;
                    t.GerelateerdeDienst = null;
                    this.Update(t);
                    this.SaveChanges();
                    this.Delete(t);
                    this.SaveChanges();
                    if (gerelateerd != null)
                    {
                        this.Delete(gerelateerd);
                        this.SaveChanges();
                    }
                }
            });

            return toDelete;
        }

        public IEnumerable<EffectieveDienst> GetAllByMonth(BusChauffeur bc, String jaar, int maand)
        {
            int jaartal = Int32.Parse(jaar);
            var uitvoer = this.GetAllVan(bc);
            return uitvoer.Where(t => t.Start.Month == maand && t.Start.Year == jaartal).ToList();
        }

        public IEnumerable<EffectieveDienst> GetAll()
        {
            return _effectieveDiensten.OrderBy(t => t.DagVanToevoegen).Include(t => t.BusChauffeur).Include(t => t.GerelateerdeDienst).ToList();
        }

        public EffectieveDienst GetBy(string id)
        {
            return _effectieveDiensten.Include(t => t.BusChauffeur).Include(t => t.GerelateerdeDienst).FirstOrDefault(d => d.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(EffectieveDienst dienst)
        {
            _effectieveDiensten.Update(dienst);
        }

        public void DeleteRange(List<EffectieveDienst> ed)
        {


        }
    }
}
