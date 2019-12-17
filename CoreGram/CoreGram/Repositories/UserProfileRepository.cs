using CoreGram.Data;
using CoreGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Repositories
{
    public class UserProfileRepository
    {
        private readonly DataContext _context;

        public UserProfileRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserProfile>> GetAll()
        {
            return await _context.UsersProfiles.ToListAsync();            
        }

        public async Task<UserProfile> GetById(int userId)
        {
            //var model = await _context.UsersProfiles.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var model = await _context.UsersProfiles.FirstOrDefaultAsync(x => x.Id == userId);

            if (model == null)
            {
                throw new Exception("Perfil de usuario no encontrado");
            }

            return model;
        }

    }
}
