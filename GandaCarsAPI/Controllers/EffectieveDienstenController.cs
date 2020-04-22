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

        public EffectieveDienstenController(IEffectieveDienstRepository effectieveDienstRepository, IDienstRepository dienstRepository,
            IBusChauffeurRepository busChauffeurRepository)
        {
            _effectieveDienstRepository = effectieveDienstRepository;
            _dienstRepository = dienstRepository;
            _busChauffeurRepository = busChauffeurRepository;
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
            TimeSpan span = new TimeSpan(1, 0, 0, 0);
            ed.ForEach(d =>
            {
                if (d.Start > d.Einde) req = "De start van de dienst '" + d.Naam + "' valt voor het einde!";
                if ((d.Einde - d.Start) > span) req = "De dienst '" + d.Naam + "' duurt te lang!";
            });
            if (req != "") return BadRequest(req);
            _effectieveDienstRepository.DeleteAllVan(bc, jaar, week).ToList();
            var effectieveDienstenToUpload = new List<EffectieveDienst>();
            ed.ForEach(dienst =>
            {
                EffectieveDienst hoofdDienst = new EffectieveDienst();
                hoofdDienst.BusChauffeur = bc;
                hoofdDienst.TotaalAantalMinutenStationnement = dienst.TotaalAantalMinutenStationnement;
                hoofdDienst.Naam = dienst.Naam;
                if (dienst.Start.DayOfWeek == dienst.Einde.DayOfWeek)
                {
                    hoofdDienst.Start = dienst.Start;
                    hoofdDienst.Eind = dienst.Einde;
                    effectieveDienstenToUpload.Add(hoofdDienst);
                }
                else
                {
                    EffectieveDienst gerelateerdeDienst = new EffectieveDienst();
                    hoofdDienst.Start = dienst.Start;
                    hoofdDienst.Eind = dienst.Start.AddHours(-dienst.Start.Hour).AddMinutes(-dienst.Start.Minute).AddDays(1); gerelateerdeDienst.BusChauffeur = bc;
                    gerelateerdeDienst.Naam = dienst.Naam;
                    gerelateerdeDienst.Start = dienst.Einde.AddHours(-dienst.Einde.Hour).AddMinutes(-dienst.Einde.Minute);
                    gerelateerdeDienst.Eind = dienst.Einde;
                    _effectieveDienstRepository.Add(gerelateerdeDienst);
                    hoofdDienst.GerelateerdeDienst = gerelateerdeDienst;
                    _effectieveDienstRepository.Add(hoofdDienst);
                    _effectieveDienstRepository.SaveChanges();
                    gerelateerdeDienst.GerelateerdeDienst = hoofdDienst;
                    _effectieveDienstRepository.Update(gerelateerdeDienst);

                }
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
            var uitvoer = _effectieveDienstRepository.GetAllVan(bc, jaar, week).ToList();
            return uitvoer;
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
            var eersteDagVanWeek = result.AddDays(-3);

            BusChauffeur bc = _busChauffeurRepository.GetBy(busChauffeurId);
            if (bc == null) return BadRequest("De buschauffeur met opgegeven id kon niet worden gevonden.");
            List<EffectieveDienst> i = _effectieveDienstRepository.GetAllVan(bc, jaar, week).ToList();
            if (i.Count == 0)
            {
                bc.Diensten.ForEach(dienst =>
                {
                    if (dienst.StartDag == dienst.EindDag)
                    {
                        EffectieveDienst ed = new EffectieveDienst();
                        ed.BusChauffeur = bc;
                        ed.Naam = dienst.Naam;
                        ed.TotaalAantalMinutenStationnement = dienst.TotaalAantalMinutenStationnement;
                        ed.Start = eersteDagVanWeek.AddDays(dienst.StartDag.GetHashCode() == 0 ? 6 : dienst.StartDag.GetHashCode() - 1).AddHours(dienst.StartUur.Hour).AddMinutes(dienst.StartUur.Minute);
                        ed.Eind = eersteDagVanWeek.AddDays(dienst.EindDag.GetHashCode() == 0 ? 6 : dienst.EindDag.GetHashCode() - 1).AddHours(dienst.EindUur.Hour).AddMinutes(dienst.EindUur.Minute);
                        _effectieveDienstRepository.Add(ed);
                    }
                    else
                    {
                        EffectieveDienst edStart = new EffectieveDienst();
                        EffectieveDienst edEind = new EffectieveDienst();
                        edStart.BusChauffeur = bc;
                        edStart.Naam = dienst.Naam;
                        edStart.TotaalAantalMinutenStationnement = dienst.TotaalAantalMinutenStationnement;
                        var startDag = dienst.StartDag.GetHashCode();
                        var eindDag = dienst.EindDag.GetHashCode();
                        if (startDag == 0)
                        {
                            eindDag = 7;
                            startDag = 6;
                        }
                        else if (startDag == 6)
                        {
                            eindDag = 6;
                            startDag = startDag - 1;
                        }

                        else
                        {
                            startDag = startDag - 1;
                            eindDag = eindDag - 1;
                        }
                        edStart.Start = eersteDagVanWeek.AddDays(startDag).AddHours(dienst.StartUur.Hour).AddMinutes(dienst.StartUur.Minute);
                        edStart.Eind = eersteDagVanWeek.AddDays(startDag).AddDays(1);

                        edEind.BusChauffeur = bc;
                        edEind.Naam = dienst.Naam;

                        edEind.Start = eersteDagVanWeek.AddDays(eindDag);
                        edEind.Eind = eersteDagVanWeek.AddDays(eindDag).AddHours(dienst.EindUur.Hour).AddMinutes(dienst.EindUur.Minute);
                        _effectieveDienstRepository.Add(edEind);
                        edStart.GerelateerdeDienst = edEind;
                        _effectieveDienstRepository.Add(edStart);
                        _effectieveDienstRepository.SaveChanges();
                        edEind.GerelateerdeDienst = edStart;
                        _effectieveDienstRepository.Update(edEind);

                    }
                });
                _effectieveDienstRepository.SaveChanges();
                var uitvoer = _effectieveDienstRepository.GetAllVan(bc, jaar, week).ToList();
                return uitvoer;
            }
            else
            {
                return i;
            }

        }

        [HttpDelete("verwijderDiensten/{jaar}/{week}/{busChauffeurId}")]
        public ActionResult<List<EffectieveDienst>> VerwijderDienst(string busChauffeurId, string jaar, string week)
        {
            BusChauffeur bc = _busChauffeurRepository.GetBy(busChauffeurId);
            if (bc == null) return BadRequest("Het buschauffeur met opgegeven id kon niet worden gevonden.");
            var uitvoer = _effectieveDienstRepository.DeleteAllVan(bc, jaar, week).ToList();
            return uitvoer;
        }

        [HttpGet("getAll")]
        public IEnumerable<EffectieveDienst> GetDiensts()
        {
            return _effectieveDienstRepository.GetAll();
        }

        [HttpGet("getByMonth/{jaar}/{maand}/{busChauffeurId}")]
        public ActionResult<IEnumerable<EffectieveDienst>> GetByMonth(string busChauffeurId, string jaar, int maand)
        {
            BusChauffeur bc = _busChauffeurRepository.GetBy(busChauffeurId);
            if (bc == null) return BadRequest("Het buschauffeur met opgegeven id kon niet worden gevonden.");
            return _effectieveDienstRepository.GetAllByMonth(bc, jaar, maand).ToList();
        }
    }
}
