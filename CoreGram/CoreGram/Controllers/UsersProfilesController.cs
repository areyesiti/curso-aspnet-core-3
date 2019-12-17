using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGram.Data;
using CoreGram.Data.Models;
using CoreGram.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreGram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersProfilesController : ControllerBase
    {
        private readonly UserProfileRepository _repository;

        public UsersProfilesController(UserProfileRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetAll()
        {
            return Ok(await _repository.GetAll());
        }
        
        [HttpGet("{Id}")]
        public async Task<ActionResult<UserProfile>> GetById(int Id)
        {
            return Ok(await _repository.GetById(Id));
        }

    }
}