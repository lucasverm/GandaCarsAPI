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
        private readonly IstationnementRepository _stationnementRepository;

        public DienstController(IDienstRepository dienstRepository,
            IBusChauffeurRepository busChauffeurRepository, IstationnementRepository stationnementRepository)
        {
            _dienstRepository = dienstRepository;
            _busChauffeurRepository = busChauffeurRepository;
            _stationnementRepository = stationnementRepository;
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
            dienst.stationnementen.ForEach(s =>
            {
                _stationnementRepository.Delete(s);
            });
            string req = null;
            dto.stationnementen.ForEach(s =>
            {
                req = this.ValidateStationnement(s, dienst);
                _stationnementRepository.Add(s);
            });
            if (req != null) return BadRequest(req);
            dienst.stationnementen = dto.stationnementen;

            string validatie = _dienstRepository.ValidateDienst(dienst);
            if (validatie != null)
            {
                return BadRequest(validatie);
            }
            _dienstRepository.Update(dienst);
            _dienstRepository.SaveChanges();
            return dienst;

        }

        private string ValidateStationnement(Stationnement s, Dienst d)
        {
            string req = null;
            if (d.StartDag != d.EindDag)
            {
                if (!(s.Dag == d.StartDag || s.Dag == d.EindDag))
                {
                    req = "Het stationnement dat begint om " + s.StartUur.ToShortTimeString() + " is niet correct!";
                }
                if (s.Dag == d.StartDag)
                {
                    if (d.StartUur >= s.StartUur)
                    {
                        req = "Het stationnement dat begint om " + s.StartUur.ToShortTimeString() + " is niet correct!";
                    }

                    if (s.StartUur >= s.EindUur)
                    {
                        req = "Het stationnement dat begint om " + s.StartUur.ToShortTimeString() + " is niet correct!";
                    }
                }

                if (s.Dag == d.EindDag)
                {
    
                    if (d.EindUur <= s.EindUur)
                    {
                        req = "Het stationnement dat begint om " + s.StartUur.ToShortTimeString() + " is niet correct!";
                    }

                    if (s.StartUur >= s.EindUur)
                    {
                        req = "Het stationnement dat begint om " + s.StartUur.ToShortTimeString() + " is niet correct!";
                    }
                }

            }
            else
            {
                if (s.Dag != d.StartDag)
                {
                    req = "Het stationnement dat begint om " + s.StartUur.ToShortTimeString() + " is niet correct!";
                }
                if (d.StartUur >= s.StartUur || d.EindUur <= s.EindUur)
                {
                    req = "Het stationnement dat begint om " + s.StartUur.ToShortTimeString() + " valt niet binnen de dienst.";
                }

                if (s.StartUur > s.EindUur)
                {
                    req = "De uren van het stationnement dat begint om " + s.StartUur.ToShortTimeString() + " zijn niet correct.";
                }
            }
            return req;
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
            BusChauffeur bc = _busChauffeurRepository.GetBy(dto.BusChauffeurId);
            if (bc == null) BadRequest("De bus chauffeur met opgegeven id kon niet worden gevonden.");
            dienst.BusChauffeur = bc;
            string validatie = _dienstRepository.ValidateDienst(dienst);
            if (validatie != null)
            {
                return BadRequest(validatie);
            }
            string req = null;
            dto.stationnementen.ForEach(s =>
            {
                req = this.ValidateStationnement(s, dienst);
                _stationnementRepository.Add(s);
            });
            if (req != null) return BadRequest(req);
            dienst.stationnementen = dto.stationnementen;
            _dienstRepository.Add(dienst);
            bc.Diensten.Add(dienst);
            _busChauffeurRepository.Update(bc);
            _dienstRepository.SaveChanges();
            return dienst;
        }
    }
}