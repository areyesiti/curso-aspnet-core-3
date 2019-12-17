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
    public class UserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserInfoDto>> GetAll()
        {
            var model = await _context.Users.ToListAsync();
            var result = _mapper.Map<List<User>, List<UserInfoDto>>(model);
            return result;
        }

        public async Task<UserInfoDto> GetById(int userId)
        {
            //var model = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var model = await _context.Users.Include(x => x.Profile).FirstOrDefaultAsync(x => x.Id == userId);

            if (model == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            var response = _mapper.Map<UserInfoDto>(model);
            return response;
        }

        public async Task<UserInfoDto> Update(int userId, UserDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado");
            }

            _context.Entry<User>(user).State = EntityState.Detached;

            var model = _mapper.Map<User>(dto);
            _context.Update(model);

            //_context.Entry(user).CurrentValues.SetValues(dto);

            //_context.Users.Update(user);

            await _context.SaveChangesAsync();

            return _mapper.Map<UserInfoDto>(model);

        }

    }
}
