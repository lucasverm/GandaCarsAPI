using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GandaCarsAPI.DTO;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GandaCarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [AllowAnonymous]
    public class DienstController : Controller
    {
        private readonly IDienstRepository _dienstRepository;
        public DienstController(IDienstRepository dienstRepository)
        {
            _dienstRepository = dienstRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<Dienst> GetDienstById(string id)
        {
            Dienst i = _dienstRepository.GetBy(id);
            if (i == null) return NotFound("De dienst met opgegeven id kon niet worden gevonden.");
            return i;
        }

        [HttpDelete("{id}")]
        public ActionResult<Dienst> VerwijderDienst(string id)
        {
            Dienst g = _dienstRepository.GetBy(id);
            if (g == null)
            {
                return NotFound("Het item met opgegeven id kon niet worden gevonden.");
            }
            _dienstRepository.Delete(g);
            _dienstRepository.SaveChanges();
            return g;
        }

        [HttpGet("getAll")]
        public IEnumerable<Dienst> GetDiensts()
        {
            return _dienstRepository.GetAll();
        }

        [HttpPut("{id}")]
        public ActionResult<Dienst> PutItem(string id, DienstDTO dto)
        {
            if (!dto.Id.Equals(id))
            {
                return BadRequest("id's komen niet overeen!");
            }

            Dienst dienst = _dienstRepository.GetBy(id);
            dienst.StartUur = dto.StartUur;
            dienst.EindUur = dto.EindUur;
            dienst.Naam = dto.Naam;

            _dienstRepository.Update(dienst);
            _dienstRepository.SaveChanges();
            return dienst;
        }

        [HttpPost]
        public ActionResult<Dienst> VoegDienstToe(DienstDTO dto)
        {
            Dienst dienst = new Dienst();
            dienst.StartUur = dto.StartUur;
            dienst.EindUur = dto.EindUur;
            dienst.Naam = dto.Naam;
            _dienstRepository.Add(dienst);
            _dienstRepository.SaveChanges();
            return dienst;
        }
    }
}