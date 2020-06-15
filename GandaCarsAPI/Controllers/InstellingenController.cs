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
    public class InstellingenController : Controller
    {

        private readonly IInstellingenRepository _instellingenRepository;

        public InstellingenController(IInstellingenRepository instellingenRepository)
        {
            _instellingenRepository = instellingenRepository;
        }

        [HttpGet]
        public ActionResult<Instellingen> GetInstellingById()
        {
            return _instellingenRepository.GetInstellingen();
        }

        [HttpPut]
        public ActionResult<Instellingen> PutInstellingen(InstellingenDTO dto)
        {
            Instellingen i = _instellingenRepository.GetInstellingen();
            i.AantalMinutenAdministratieveTijdVoorDienst = dto.AantalMinutenAdministratieveTijdVoorDienst;
            i.Stelsel = dto.Stelsel;
            _instellingenRepository.Update(i);
            _instellingenRepository.SaveChanges();
            return i;
        }

    }
}
