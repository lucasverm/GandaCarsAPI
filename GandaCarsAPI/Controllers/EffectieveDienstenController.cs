using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GandaCarsAPI.DTO;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GandaCarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [AllowAnonymous]
    public class EffectieveDienstenController : Controller
    {
        private readonly IDienstRepository _dienstRepository;
        private readonly IEffectieveDienstRepository _effectieveDienstRepository;
        private readonly IBusChauffeurRepository _busChauffeurRepository;
        private readonly IstationnementRepository _stationnementRepository;

        public EffectieveDienstenController(IEffectieveDienstRepository effectieveDienstRepository, IDienstRepository dienstRepository,
            IBusChauffeurRepository busChauffeurRepository, IstationnementRepository stationnementRepository)
        {
            _effectieveDienstRepository = effectieveDienstRepository;
            _dienstRepository = dienstRepository;
            _busChauffeurRepository = busChauffeurRepository;
            _stationnementRepository = stationnementRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<EffectieveDienst> GetEffectieveDienstById(string id)
        {
            EffectieveDienst i = _effectieveDienstRepository.GetBy(id);
            if (i == null) return NotFound("De effectieve dienst met opgegeven id kon niet worden gevonden.");
            return i;
        }

        [HttpPost("{jaar}/{week}/{busChauffeurId}")]
        public ActionResult<IEnumerable<EffectieveDienst>> PostEffectieveDiensten(List<EffectieveDienstDTO> ed, string jaar, string week, string busChauffeurId)
        {
            BusChauffeur bc = _busChauffeurRepository.GetBy(busChauffeurId);
            if (bc == null) return BadRequest("De buschauffeur met opgegeven id kon niet worden gevonden.");
            string req = "";
            var teVerwijderen = _effectieveDienstRepository.GetAllVan(bc, jaar, week).ToList();
            _effectieveDienstRepository.DeleteRange(teVerwijderen);
            _effectieveDienstRepository.SaveChanges();
            var effectieveDienstenToUpload = new List<EffectieveDienst>();
            ed.ForEach(dienst =>
            {
                EffectieveDienst d = new EffectieveDienst();
                d.BusChauffeur = _busChauffeurRepository.GetBy(dienst.BusChauffeurId);
                if (d.BusChauffeur == null) req = "De buschauffeur met kon niet worden gevonden.";
                dienst.Stationnementen.ForEach(s =>
                {
                    _stationnementRepository.Delete(_stationnementRepository.GetBy(s.Id));
                    s.Id = null;
                    d.Stationnementen.Add(s);
                });
                d.Stationnementen = dienst.Stationnementen;
                d.Naam = dienst.Naam;
                d.Start = dienst.Start;
                d.Eind = dienst.Einde;
                effectieveDienstenToUpload.Add(d);
            });
            _effectieveDienstRepository.AddRange(effectieveDienstenToUpload);
            _effectieveDienstRepository.SaveChanges();
            if (req != "") return BadRequest(req);
            return effectieveDienstenToUpload;
        }


        [HttpGet("{jaar}/{week}/{busChauffeurId}")]
        public ActionResult<IEnumerable<EffectieveDienst>> GetEffectievieDienstenVanChauffeur(string jaar, string week, string busChauffeurId)
        {
            BusChauffeur bc = _busChauffeurRepository.GetBy(busChauffeurId);
            if (bc == null) return BadRequest("De buschauffeur met opgegeven id kon niet worden gevonden.");
            return _effectieveDienstRepository.GetAllVan(bc, jaar, week).ToList();
        }

        [HttpGet("GetEffectievieDienstenNullDienstenNaarEffectieveDiensten/{jaar}/{week}/{busChauffeurId}")]
        public ActionResult<IEnumerable<EffectieveDienst>> GetEffectievieDienstenNullZetDienstenNaarEffectieveDienstenOm(string jaar, string week, string busChauffeurId)
        {
            //eerste dag van week berekenen
            DateTime jan1 = new DateTime(Int32.Parse(jaar), 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var weekNum = Int32.Parse(week);
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            var eersteDagVanWeek = result.AddDays(-4);

            BusChauffeur bc = _busChauffeurRepository.GetBy(busChauffeurId);
            if (bc == null) return BadRequest("De buschauffeur met opgegeven id kon niet worden gevonden.");
            List<EffectieveDienst> i = _effectieveDienstRepository.GetAllVan(bc, jaar, week).ToList();
            if (i.Count == 0)
            {
                bc.Diensten.ForEach(dienst =>
                {
                    EffectieveDienst ed = new EffectieveDienst();
                    ed.BusChauffeur = bc;
                    ed.Naam = dienst.Naam;
                    ed.Stationnementen = dienst.Stationnementen;
                    ed.Start = eersteDagVanWeek.AddDays(dienst.StartDag.GetHashCode()).AddHours(dienst.StartUur.Hour).AddMinutes(dienst.StartUur.Minute);
                    ed.Eind = eersteDagVanWeek.AddDays(dienst.EindDag.GetHashCode()).AddHours(dienst.EindUur.Hour).AddMinutes(dienst.EindUur.Minute);
                    _effectieveDienstRepository.Add(ed);
                });
                _effectieveDienstRepository.SaveChanges();
                return _effectieveDienstRepository.GetAllVan(bc, jaar, week).ToList();
            }
            else
            {
                return i;
            }
            
        }

        [HttpDelete("{id}")]
        public ActionResult<EffectieveDienst> VerwijderDienst(string id)
        {
            EffectieveDienst g = _effectieveDienstRepository.GetBy(id);
            if (g == null)
            {
                return NotFound("Het item met opgegeven id kon niet worden gevonden.");
            }
            _effectieveDienstRepository.Delete(g);
            _effectieveDienstRepository.SaveChanges();
            return g;
        }

        [HttpGet("getAll")]
        public IEnumerable<EffectieveDienst> GetDiensts()
        {
            return _effectieveDienstRepository.GetAll();
        }
    }
}
