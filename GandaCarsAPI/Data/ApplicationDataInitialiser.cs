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
                    Email = "tom.debakker@hotmail.com"
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
                    BusChauffeur = bc,
                    TotaalAantalMinutenStationnement = 45
                  
                };

                Onderbreking o = new Onderbreking()
                {
                    StartDag = DateTime.Now.DayOfWeek,
                    EindDag = DateTime.Now.DayOfWeek,
                    StartUur = DateTime.Now.AddHours(2),
                    EindUur = DateTime.Now.AddHours(3),
                };
                d.Onderbrekingen.Add(o);

                bc.Diensten.Add(d);

                Dienst d2 = new Dienst()
                {
                    Naam = "dienst2",
                    StartUur = DateTime.Now.AddHours(-2),
                    EindUur = DateTime.Now,
                    StartDag = DateTime.Now.AddDays(2).DayOfWeek,
                    EindDag = DateTime.Now.AddDays(2).DayOfWeek,
                    BusChauffeur = bc,
                    
                };
                _dbContext.Diensten.Add(d2);
                bc.Diensten.Add(d2);
                _dbContext.SaveChanges();

            }
        }
                
    }
}
