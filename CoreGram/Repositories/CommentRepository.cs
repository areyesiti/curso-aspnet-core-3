using AutoMapper;
using CoreGram.Data;
using CoreGram.Data.Dto;
using CoreGram.Data.Extensions;
using CoreGram.Data.Models;
using CoreGram.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Repositories
{
    public class CommentRepository
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public CommentRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CommentDto>> GetByPost(int postId)
        {
            // Creamos un obj de salida del tipo lista de CommentDto
            List<CommentDto> comments = new List<CommentDto>();

            // Obtenemos los comentarios para un determinado post
            List<PostComment> postsComments = await _context.PostsComments
                                                .Include(x => x.Comment)
                                                .Where(x => x.PostId == postId)
                                                .ToListAsync();

            // Recorremos los resultados y mapeamos mediante el método de extensión
            foreach (PostComment item in postsComments)
            {
                comments.Add(item.Comment.ToDto(item.PostId));
            }

            // Devolvemos la lista de comentarios mapeados
            return comments;
        }

        public async Task<CommentDto> Comment(CommentDto dto)
        { 
            // Creamos un nuevo comentario y lo añadimos al contexto
            Comment comment = new Comment();
            comment = _mapper.Map<Comment>(dto);
            comment.Date = DateTime.Now;
            _context.Comments.Add(comment);

            await _context.SaveChangesAsync();

            // Creamos un nuevo PostComment y lo añadimos al contexto
            PostComment postComment = new PostComment
            {
                PostId = dto.PostId,
                CommentId = comment.Id                
            };
            _context.PostsComments.Add(postComment);

            // Guardamos los cambios y devolvemos el dto de entrada
            await _context.SaveChangesAsync();
            return comment.ToDto(postComment.PostId);
        }

        public async Task<CommentDto> Delete(int commentId)
        {
            // Obtenemos un comentario y comprobamos que existe
            var model = await _context.Comments
                            .Include(x => x.PostsComments)
                                .ThenInclude(x => x.Post)
                            .FirstOrDefaultAsync(x => x.Id == commentId);

            if (model != null)
            {
                // Si existe lo eliminamos del contexto y guardamos
                _context.Comments.Remove(model);
                await _context.SaveChangesAsync();
                
                // Mapeamos la salida a su dto pasándole el id del post
                return model.ToDto(model.PostsComments.FirstOrDefault().PostId);
            }
            else
            {
                throw new NotFoundException("No se ha encontrado el comentario");
            }
        }
    }
}
