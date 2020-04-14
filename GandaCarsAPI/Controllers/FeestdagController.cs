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
    public class FeestdagController : Controller
    {

        private readonly IFeestdagRepository _feestdagRepository;

        public FeestdagController(IFeestdagRepository feestdagRepository)
        {
            _feestdagRepository = feestdagRepository;
        }

        [HttpGet("getAll")]
        public IEnumerable<Feestdag> GetAll()
        {
            return _feestdagRepository.GetAll();
        }

        [HttpPost]
        public ActionResult<IEnumerable<Feestdag>> AddFden(List<FeestdagDTO> dto)
        {
            List<Feestdag> feestdagen = new List<Feestdag>();
            dto.ForEach(d =>
            {
                Feestdag fd = new Feestdag();
                fd.Dag = d.Dag;
                fd.Naam = d.Naam;
                feestdagen.Add(fd);
            });
            _feestdagRepository.DeleteRanger(_feestdagRepository.GetAll());
            _feestdagRepository.AddRange(feestdagen);
            _feestdagRepository.SaveChanges();
            return feestdagen;
        }
        
    }
}
