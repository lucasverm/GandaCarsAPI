using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GandaCarsAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace GandaCarsAPI.Data
{
    public class ApplicationDataInitialiser
    {

        private readonly ApplicationDbContext _dbContext;
        

        public ApplicationDataInitialiser(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeData()
        {

            _dbContext.Database.EnsureDeleted();

            if (_dbContext.Database.EnsureCreated())
            {

                BusChauffeur bc = new BusChauffeur()
                {
                    Voornaam = "Tom",
                    Achternaam = "De Bakker",
                    Uurloon = 11,
                    GeboorteDatum = DateTime.Now,
                    Email = "parick@p.com"
                };
                _dbContext.BusChauffeurs.Add(bc);
                _dbContext.SaveChanges();

                Dienst d = new Dienst()
                {
                    Naam = "dienst1",
                    StartUur = DateTime.Now,
                    EindUur = DateTime.Now.AddHours(-3),
                    StartDag = DateTime.Now.DayOfWeek,
                    EindDag = DateTime.Now.AddDays(1).DayOfWeek,
                    BusChauffeur = bc
                };
                _dbContext.Diensten.Add(d);
                bc.Diensten.Add(d);

                Stationnement s = new Stationnement()
                {
                    StartUur = DateTime.Now.AddHours(1),
                    EindUur = DateTime.Now.AddHours(1).AddMinutes(5),
                    Tarief = 100,
                    Dag = DateTime.Now.AddDays(2).DayOfWeek
                };
                

                Dienst d2 = new Dienst()
                {
                    Naam = "dienst2",
                    StartUur = DateTime.Now.AddHours(-2),
                    EindUur = DateTime.Now,
                    StartDag = DateTime.Now.AddDays(2).DayOfWeek,
                    EindDag = DateTime.Now.AddDays(2).DayOfWeek,
                    BusChauffeur = bc,
                    
                };
                d2.stationnementen.Add(s);
                _dbContext.Diensten.Add(d2);
                bc.Diensten.Add(d2);
                _dbContext.SaveChanges();

            }
        }
                
    }
}
