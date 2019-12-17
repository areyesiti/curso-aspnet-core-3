using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGram.Data;
using CoreGram.Data.Dto;
using CoreGram.Data.Models;
using CoreGram.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreGram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private readonly FollowerRepository _repository;

        public FollowersController(FollowerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<FollowerInfoDto>>> GetFollowers(int userId)
        {
            return Ok(await _repository.GetFollowers(userId));
        }

        [HttpGet("Following/{userId}")]
        public async Task<ActionResult<IEnumerable<FollowerInfoDto>>> GetFollowings(int userId)
        {
            return Ok(await _repository.GetFollowings(userId));
        }

    }
}