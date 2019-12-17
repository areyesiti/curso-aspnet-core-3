using AutoMapper;
using CoreGram.Data;
using CoreGram.Data.Dto;
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
        private readonly IMapper _mapper;

        public UserProfileRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserProfileDto>> GetAll()
        {
            var model = await _context.UsersProfiles.ToListAsync();            
            return _mapper.Map<List<UserProfile>, List<UserProfileDto>>(model);
        }

        public async Task<UserProfileDto> GetById(int userId)
        {
            //var model = await _context.UsersProfiles.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var model = await _context.UsersProfiles.FirstOrDefaultAsync(x => x.Id == userId);

            if (model == null)
            {
                throw new Exception("Perfil de usuario no encontrado");
            }

            return _mapper.Map<UserProfileDto>(model);
        }

    }
}
