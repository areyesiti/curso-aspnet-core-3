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
    public class PostRepository
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        private readonly FollowerRepository _followerRepository;
        private readonly LikeRepository _likesRepository;

        public PostRepository(DataContext context, IMapper mapper, 
                              FollowerRepository followerRepository,
                              LikeRepository likesRepository)
        {
            _context = context;
            _mapper = mapper;
            _followerRepository = followerRepository;
            _likesRepository = likesRepository;
        }

        public async Task<IEnumerable<PostInfoDto>> GetAll()
        {
            // Obtenemos todos los posts de todos los usuarios
            var model = await _context.Posts
                            .Include(x => x.User)
                            .Include(x => x.Likes)
                            .Include(x => x.PostsComments)
                                .ThenInclude(x => x.Comment)
                                    .ThenInclude(x => x.User)
                            .OrderByDescending(x => x.Date).ToListAsync();

            // Mapeamos la lista de posts
            return PostInfoDtoMapper(model);
        }

        public async Task<IEnumerable<PostInfoDto>> GetByUser(int userId)
        {
            // Obtenemos los posts de un usuario en concreto
            var model = await _context.Posts
                            .Where(x => x.UserId == userId)
                            .Include(x => x.User)
                            .Include(x => x.Likes)
                            .Include(x => x.PostsComments)
                                .ThenInclude(x => x.Comment)
                                    .ThenInclude(x => x.User)
                            .OrderByDescending(x => x.Date).ToListAsync();

            // Mapeamos la lista de posts
            return PostInfoDtoMapper(model);
        }

        public async Task<IEnumerable<PostInfoDto>> GetByFollowings(int userId)
        {
            // Obtenemos una lista de ids de los que usuarios que sigue el usuario
            var followersIds = _followerRepository.GetFollowings(userId).Select(x => x.UserId).ToArray();              

            // Obtenemos los posts de los usuarios que sigue el usuario
            var model = await _context.Posts.Where(x => followersIds.Contains(x.UserId))
                                    .Include(x => x.User)
                                    .Include(x => x.Likes)
                                    .Include(x => x.PostsComments)
                                        .ThenInclude(x => x.Comment)
                                            .ThenInclude(x => x.User)
                                    .OrderByDescending(x => x.Date).ToListAsync();

            // Mapeamos la lista de posts
            return PostInfoDtoMapper(model);

        }

        public async Task<PostDto> Create(PostDto dto)
        {
            // Mapeamos el dto a la entidad, la añadimos al contexto, guardamos y devolvemos el dto mapeado
            var model = _mapper.Map<Post>(dto);
            _context.Posts.Add(model);
            await _context.SaveChangesAsync();
            return _mapper.Map<PostDto>(model);
        }

        public async Task<PostDto> Delete(int postId)
        {
            // Obtenemos el post y comprobamos que existe
            var model = await _context.Posts.FindAsync(postId);
            if (model != null)
            {
                // Eliminamos el post del contexto, guardamos y devolvemos el objeto devuelto mapeado
                _context.Posts.Remove(model);
                await _context.SaveChangesAsync();
                return _mapper.Map<PostDto>(model);
            }
            else
            {
                throw new NotFoundException("No se ha encontrado la publicación");
            }
        }

        // Mapeador personalizado para devolver la lista de posts
        private List<PostInfoDto> PostInfoDtoMapper(List<Post> posts)
        {
            // Creamos el objeto lista que vamos a devolver
            List<PostInfoDto> result = new List<PostInfoDto>();

            // Recorremos la lista de post obtenida por parámetros
            foreach (var post in posts)
            {
                // Creamos y asignamos propiedados a un nuevo PostInfoDto
                PostInfoDto item = new PostInfoDto
                {
                    Id = post.Id,
                    User = _mapper.Map<UserInfoDto>(post.User),
                    Picture = post.Picture,
                    Comments = new List<CommentInfoDto>()
                };

                // Obtenemos el total de likes desde el repositorio de likes
                item.Likes = _likesRepository.GetByPost(item.Id);

                // Obtenemos el total de comentarios desde PostComments
                item.TotalComments = post.PostsComments.Count();

                // Componemos el CommentInfoDto de los comentarios que se añadira al dto de PostInfoDto
                foreach (PostComment postComment in post.PostsComments)
                {
                    CommentInfoDto comment = new CommentInfoDto
                    {
                        Id = postComment.Comment.Id,
                        Login = postComment.Comment.User.Login,
                        Remark = postComment.Comment.Remark
                    };
                    item.Comments.Add(comment);
                }
                result.Add(item);
            }

            return result;
        }
    }
}
