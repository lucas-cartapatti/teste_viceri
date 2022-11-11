using HeroAcademyApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeroAcademyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroisSuperpoderesController : ControllerBase
    {
        private readonly heroacademydbContext _dbContext;
        public HeroisSuperpoderesController(heroacademydbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        [HttpGet]
        public string Get()
        {
            var data = _dbContext.HeroisSuperpoderes.ToList();

            return string.Empty;
        }

        //Heroi {id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                List<HeroisSuperpoderes> result = await _dbContext.HeroisSuperpoderes.Where(x => x.HeroiId == id).ToListAsync(); //Retorna lista de super herois
                if(result is not null)
                {
                    foreach (var item in result)
                    {
                        item.Superpoder = await _dbContext.Superpoderes.FindAsync(item.SuperpoderId);    
                    }
                }

                return Ok(result);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }
    }
}
