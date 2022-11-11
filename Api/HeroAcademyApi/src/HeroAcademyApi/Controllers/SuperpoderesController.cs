using HeroAcademyApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeroAcademyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperpoderesController : ControllerBase
    {
        private readonly heroacademydbContext _dbContext;
        public SuperpoderesController(heroacademydbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Superpoderes = await _dbContext.Superpoderes.ToListAsync();

                if(Superpoderes == null)
                    throw new Exception("Non super powers found, please add at least 1 register");
                return Ok(Superpoderes);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
        }
    }
}
