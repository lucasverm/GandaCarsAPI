using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            return _effectieveDiensten.Where(d => d.Start == date || d.Eind == date).Include(t => t.BusChauffeur).Include(t => t.Stationnementen).ToList();
        }

        public void DeleteRange(List<EffectieveDienst> ed)
        {
            _effectieveDiensten.RemoveRange(ed);
        }

        public void Delete(EffectieveDienst ed)
        {
            _effectieveDiensten.Remove(ed);
        }

        public IEnumerable<EffectieveDienst> GetAllVan(BusChauffeur bc)
        {
            return _effectieveDiensten.Where(t => t.BusChauffeur == bc).Include(t => t.BusChauffeur).Include(t => t.Stationnementen).ToList();
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
            return this.GetAllVan(bc).Where(t => (t.Start.Year == jaartal || t.Eind.Year == jaartal) && (myCal.GetWeekOfYear(t.Start, myCWR, myFirstDOW) == weekNummer || myCal.GetWeekOfYear(t.Eind, myCWR, myFirstDOW) == weekNummer)).ToList();  
        }


        public IEnumerable<EffectieveDienst> GetAll()
        {
            return _effectieveDiensten.Include(t => t.BusChauffeur).Include(t => t.Stationnementen).ToList();
        }

        public EffectieveDienst GetBy(string id)
        {
            return _effectieveDiensten.Include(t => t.BusChauffeur).Include(t => t.Stationnementen).FirstOrDefault(d => d.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(EffectieveDienst dienst)
        {
            _effectieveDiensten.Update(dienst);
        }
    }
}
