using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreGram.Data;
using CoreGram.Data.Dto;
using CoreGram.Data.Models;
using CoreGram.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CoreGram.Repositories
{
    public class FollowerRepository
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public FollowerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<FollowerInfoDto> GetFollowers(int userId)
        {
            // Obtenemos el usuario y comprobamos que existe
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                throw new NotFoundException("El usuario no existe");
            }       

            // Obtenemos los seguidores de un usuario incluyendo el perfil de usuario de estos y ordenando por Login
            var model = _context.Followers
                .Where(x => x.UserId == userId)
                .Include(x => x.UserFollower)
                    .ThenInclude(x => x.Profile)                
                .OrderBy(x => x.UserFollower.Login)
                .ToList();

            // Mapeamos la lista con automapper
            return _mapper.Map<List<Follower>, List<FollowerInfoDto>>(model);            
        }

        public IEnumerable<FollowerInfoDto> GetFollowings(int userId)
        {
            // Obtenemos el usuario y comprobamos que existe
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                throw new NotFoundException("El usuario no existe");
            }

            // Obtenemos los seguidos de un usuario incluyendo el perfil de usuario de estos y ordenando por Login
            var model = _context.Followers
                .Where(x => x.FollowerId == userId)
                .Include(x => x.UserFollowing)
                    .ThenInclude(x => x.Profile)
                .OrderBy(x => x.UserFollowing.Login)
                .ToList();

            // Mapeamos la lista con automapper
            return _mapper.Map<List<Follower>, List<FollowerInfoDto>>(model);
        }

        public async Task<FollowerDto> Create(FollowerDto dto)
        {
            
            // Obtenemos la entidad follower para un usuario y un follower
            var follower = await _context.Followers.FindAsync(dto.UserId, dto.FollowerId);

            // Comprobamos si ya un usuario ya sigue a otro usuario
            if (follower != null)
            {
                throw new NotFoundException("Ya sigues a este usuario");
            }

            // Comprobamos si se pretende seguir un usuario a si mismo
            if (dto.UserId == dto.FollowerId)
            {
                throw new NotFoundException("No puedes seguirte a ti mismo");
            }

            // Mapeamos el dto a la entidad, la añadimos al contexto, guardamos y devolvemos el dto de followers
            var model = _mapper.Map<Follower>(dto);
            _context.Followers.Add(model);
            await _context.SaveChangesAsync();
            return _mapper.Map<FollowerDto>(model);
        }

        public async Task<FollowerDto> Delete(int userId, int followerId)
        {
            // Obtenemos y comprobamos si existe la entidad followers para un usuario y un follower
            var model = await _context.Followers.FindAsync(userId, followerId);
            if (model != null)
            {
                // Si es así la eliminamos, guardamos y mapeamos la respuesta
                _context.Followers.Remove(model);
                await _context.SaveChangesAsync();
                return _mapper.Map<FollowerDto>(model);
            }
            else
            {
                throw new NotFoundException("No se ha encontrado el usuario seguido indicado");
            }
        }
    }
}
