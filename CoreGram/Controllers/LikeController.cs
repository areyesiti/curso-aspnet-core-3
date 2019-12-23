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
    public class LikeController : Controller
    {
        private LikeRepository _repository;

        public LikeController(LikeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Añade o quita un like de un post
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<LikeDto>> Like([FromBody]LikeDto dto)
        {
            return Ok(await _repository.Like(dto));
        }
    }
}
