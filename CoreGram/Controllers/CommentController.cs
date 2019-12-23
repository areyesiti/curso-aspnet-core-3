using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreGram.Data;
using CoreGram.Data.Dto;
using CoreGram.Data.Models;
using CoreGram.Helpers;
using CoreGram.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreGram.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private CommentRepository _repository;

        public CommentController(CommentRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtiene todos los comentarios de un post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("{postId}")]
        public async Task<ActionResult<CommentDto>> GetByPost(int postId)
        {
            return Ok(await _repository.GetByPost(postId));
        }

        /// <summary>
        /// Crea un comentario asociado a un post
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CommentDto>> Create([FromBody]CommentDto dto)
        {
            return Ok(await _repository.Comment(dto));
        }

        /// <summary>
        /// Elimina un comentario asociado a un post
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete("{commentId}")]
        public async Task<ActionResult<CommentDto>> Delete(int commentId)
        {
            return Ok(await _repository.Delete(commentId));
        }
    }
}
