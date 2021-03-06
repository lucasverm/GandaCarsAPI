﻿using System;
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
        private readonly IBusChauffeurRepository _busChauffeurRepository;
        private readonly IOnderbrekingRepository _onderbrekingRepository;

        public DienstController(IDienstRepository dienstRepository,
            IBusChauffeurRepository busChauffeurRepository, IOnderbrekingRepository onderbrekingRepository)
        {
            _dienstRepository = dienstRepository;
            _busChauffeurRepository = busChauffeurRepository;
            _onderbrekingRepository = onderbrekingRepository;
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
            var that = this;
            if (!dto.Id.Equals(id))
            {
                return BadRequest("id's komen niet overeen!");
            }
            Dienst dienst = _dienstRepository.GetBy(id);
            dienst.StartUur = dto.StartUur;
            dienst.EindUur = dto.EindUur;
            dienst.Naam = dto.Naam;
            dienst.StartDag = dto.StartDag;
            dienst.EindDag = dto.EindDag;
            dienst.TotaalAantalMinutenStationnement = dto.TotaalAantalMinutenStationnement;
            if (dienst.BusChauffeur != null)
            {
                if (dienst.BusChauffeur.Id != dto.BusChauffeurId)
                {
                    BusChauffeur OudeBc = _busChauffeurRepository.GetBy(dienst.BusChauffeur.Id);
                    OudeBc.Diensten.Remove(OudeBc.Diensten.SingleOrDefault(d => d.Id == dienst.Id));
                    _busChauffeurRepository.Update(dienst.BusChauffeur);
                    _dienstRepository.SaveChanges();
                    BusChauffeur NieuweBc = _busChauffeurRepository.GetBy(dto.BusChauffeurId);
                    dienst.BusChauffeur = NieuweBc;
                    NieuweBc.Diensten.Add(dienst);
                    _busChauffeurRepository.Update(NieuweBc);
                }
            }
            else
            {
                BusChauffeur NieuweBc = _busChauffeurRepository.GetBy(dto.BusChauffeurId);
                dienst.BusChauffeur = NieuweBc;
                NieuweBc.Diensten.Add(dienst);
                _busChauffeurRepository.Update(NieuweBc);
            }
            string validatie = _dienstRepository.ValidateDienst(dienst);
            if (validatie != null)
            {
                return BadRequest(validatie);
            }
            string request = null;
            dto.Onderbrekingen.ForEach(onderbreking =>
            {
                request = this._dienstRepository.ValidateOnderbrekingMetDienst(dienst, onderbreking);
                if (request != null) return;
            });
            if (request != null) return BadRequest(request);
            _onderbrekingRepository.DeleteRange(dienst.Onderbrekingen);
            _onderbrekingRepository.SaveChanges();
            _onderbrekingRepository.AddRange(dto.Onderbrekingen);
            _onderbrekingRepository.SaveChanges();
            dienst.Onderbrekingen = dto.Onderbrekingen;
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
            
            dienst.StartDag = dto.StartDag;
            dienst.EindDag = dto.EindDag;
            dienst.TotaalAantalMinutenStationnement = dto.TotaalAantalMinutenStationnement;
            BusChauffeur bc = _busChauffeurRepository.GetBy(dto.BusChauffeurId);
            if (bc == null) BadRequest("De bus chauffeur met opgegeven id kon niet worden gevonden.");
            dienst.BusChauffeur = bc;
            string validatie = _dienstRepository.ValidateDienst(dienst);
            if (validatie != null)
            {
                return BadRequest(validatie);
            }
            string request = null;
            dto.Onderbrekingen.ForEach(onderbreking =>
            {
                request = this._dienstRepository.ValidateOnderbrekingMetDienst(dienst, onderbreking);
                if (request != null) return;
            });
            if (request != null) return BadRequest(request);
            dienst.Onderbrekingen.ForEach(t => t.Id = null);
            dienst.Onderbrekingen.AddRange(dto.Onderbrekingen);
            dienst.Onderbrekingen = dto.Onderbrekingen;
            _dienstRepository.Add(dienst);
            bc.Diensten.Add(dienst);
            _busChauffeurRepository.Update(bc);
            _dienstRepository.SaveChanges();
            return dienst;
        }
    }
}