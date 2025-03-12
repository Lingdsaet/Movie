using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Models;
using Movies.Repository;

namespace Movies.ControllerUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsUserController : ControllerBase
    {
        private readonly ActorRepository _actorRepository;

        public ActorsUserController(ActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        // GET: api/ActorsUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
            var actor = await _actorRepository.Actor.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return actor;
        }
    }
}
