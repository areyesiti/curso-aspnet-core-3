using CoreGram.Data;
using CoreGram.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Repositories
{
    public class UserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();            
        }

        public async Task<User> GetById(int userId)
        {
            //var model = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var model = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (model == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            return model;
        }

    }
}
