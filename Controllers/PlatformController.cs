using Microsoft.AspNetCore.Mvc;
using RedisAPI.Data;
using RedisAPI.Models;

namespace RedisAPI.Controllers
{
    
    [ApiController]
    [Route("api/Platform")]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;

        public PlatformController(IPlatformRepo platformRepo)
        {
            _platformRepo = platformRepo;
        }

        [HttpGet(Name ="GetAllPlatforms")]
        public ActionResult GetAllPlatforms()
        {
            var platforms = _platformRepo.GetPlatforms();

            if(platforms != null)
            {
                return Ok(platforms);
            }

            return NotFound();
        }

        [HttpGet("{id}", Name ="GetPlatformById")]
        public ActionResult<Platform> GetPlatformById(string id)
        {
            var platform = _platformRepo.GetPlatformById(id);

            if(platform != null)
            {
                return Ok(platform);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<Platform> CreatePlatforms(Platform platform)
        {
            _platformRepo.CreatePlatform(platform);

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platform.Id}, platform);
        }


    }
}