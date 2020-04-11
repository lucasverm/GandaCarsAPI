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
                    Voornaam = "lucas",
                    Achternaam = "Vermeulen",
                    Uurloon = 10
                };
                _dbContext.BusChauffeurs.Add(bc);
                _dbContext.SaveChanges();

                Dienst d = new Dienst()
                {
                    Naam = "dienst1",
                    StartUur = DateTime.Now,
                    EindUur = DateTime.Now.AddHours(2),
                    StartDag = DateTime.Now.DayOfWeek,
                    EindDag = DateTime.Now.AddDays(1).DayOfWeek,
                    BusChauffeur = bc
                };
                _dbContext.Diensten.Add(d);
                bc.Diensten.Add(d);

                Dienst d2 = new Dienst()
                {
                    Naam = "dienst2",
                    StartUur = DateTime.Now,
                    EindUur = DateTime.Now.AddHours(2),
                    StartDag = DateTime.Now.AddDays(2).DayOfWeek,
                    EindDag = DateTime.Now.AddDays(2).DayOfWeek,
                    BusChauffeur = bc
                };
                _dbContext.Diensten.Add(d2);
                bc.Diensten.Add(d2);
                _dbContext.SaveChanges();

            }
        }
                
    }
}
