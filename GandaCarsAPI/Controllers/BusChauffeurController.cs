using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GandaCarsAPI.DTO;
using GandaCarsAPI.Models;
using GandaCarsAPI.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GandaCarsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [AllowAnonymous]
    public class BusChauffeurController : Controller
    {
        private readonly IBusChauffeurRepository _busChauffeurRepository;
        public BusChauffeurController(IBusChauffeurRepository busChauffeurRepository)
        {
            _busChauffeurRepository = busChauffeurRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<BusChauffeur> GetChauffeurById(string id)
        {
            BusChauffeur i = _busChauffeurRepository.GetBy(id);
            if (i == null) return NotFound("De chauffeur met opgegeven id kon niet worden gevonden.");
            return i;
        }

        [HttpDelete("{id}")]
        public ActionResult<BusChauffeur> VerwijderChauffeur(string id)
        {
            BusChauffeur g = _busChauffeurRepository.GetBy(id);
            if (g == null)
            {
                return NotFound("Het item met opgegeven id kon niet worden gevonden.");
            }
            _busChauffeurRepository.Delete(g);
            _busChauffeurRepository.SaveChanges();
            return g;
        }

        [HttpGet("getAll")]
        public IEnumerable<BusChauffeur> GetBusChauffeurs()
        {
            return _busChauffeurRepository.GetAll();
        }

        [HttpPut("{id}")]
        public ActionResult<BusChauffeur> PutItem(string id, BusChauffeurDTO dto)
        {
            if (!dto.Id.Equals(id))
            {
                return BadRequest("id's komen niet overeen!");
            }

            BusChauffeur bc = _busChauffeurRepository.GetBy(id);
            bc.Voornaam = dto.Voornaam;
            bc.Achternaam = dto.Achternaam;
            bc.Uurloon = dto.Uurloon;

            _busChauffeurRepository.Update(bc);
            _busChauffeurRepository.SaveChanges();
            return bc;
        }

        [HttpPost]
        public ActionResult<BusChauffeur> VoegBusChauffeurToe(BusChauffeurDTO dto)
        {
            BusChauffeur bc = new BusChauffeur();
            bc.Voornaam = dto.Voornaam;
            bc.Achternaam = dto.Achternaam;
            bc.Uurloon = dto.Uurloon;
            _busChauffeurRepository.Add(bc);
            _busChauffeurRepository.SaveChanges();
            return bc;
        }
    }
}
