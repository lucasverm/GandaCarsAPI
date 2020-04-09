using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

            }
        }
                
    }
}
