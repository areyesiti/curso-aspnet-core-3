using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreGram.Data;
using CoreGram.Data.Dto;
using CoreGram.Data.Models;
using CoreGram.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CoreGram.Repositories
{
    public class LikeRepository
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public LikeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int GetByPost(int postId)
        {
            // Devolución del total de likes para un post con Linq
            // return _context.Likes.Count(x => x.PostId == postId);

            // Parámetros y ejecución de procedimiento almacenado para obtener el total de likes de un post

            // Definición del parámetro de salida total
            var pTotal = new SqlParameter()
            {
                ParameterName = "@total",
                Direction = ParameterDirection.Output,
                Value = 0,
                DbType = DbType.Int32
            };

            // Definición del parámetro de entrada postId
            var pPost = new SqlParameter()
            {
                ParameterName = "@postId",
                Value = postId,
                DbType = DbType.String
            };

            // Llamada al SP
            _context.Database.ExecuteSqlCommand("sp_GetLikesByPost @postId, @total OUTPUT", pPost, pTotal);

            // Conversión del resultado devuelto por el SP a un entero
            return Convert.ToInt32(pTotal.Value);
        }
    
        public async Task<LikeDto> Like(LikeDto dto)
        {
            // Obtención de un like para un usuario y un post con Linq
            //var model = await _context.Likes.FirstOrDefaultAsync(x => x.UserId == dto.UserId && x.PostId == dto.PostId);

            // Obtención de un like para un usuario y un post FromSql
            var model = await _context.Likes
                .FromSqlRaw("SELECT * FROM Likes WHERE UserId = {0} AND PostId = {1}", dto.UserId, dto.PostId).FirstOrDefaultAsync();

            if (model != null)
            {
                // Si existe el like lo eliminamos
                _context.Likes.Remove(model);
            }
            else
            {
                // Si no existe, mapeamos al objeto entidad y lo añadimos al contexto
                model = _mapper.Map<Like>(dto);
                _context.Likes.Add(model);
            }

            // Guardamos y mapeamos al dto de salida
            _context.SaveChangesAsync();
            return _mapper.Map<LikeDto>(model);
        }

    }
}
