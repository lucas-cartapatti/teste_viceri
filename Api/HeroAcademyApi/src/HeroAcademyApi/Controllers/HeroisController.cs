using HeroAcademyApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeroAcademyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroisController : ControllerBase
    {
        private readonly heroacademydbContext _dbContext;
        public HeroisController(heroacademydbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Herois> data = await _dbContext.Herois.ToListAsync(); //Retorna lista de super herois
                foreach(var heroi in data)
                {
                    heroi.HeroisSuperpoderes = await _dbContext.HeroisSuperpoderes.Where(x =>x.HeroiId == heroi.Id).ToListAsync();
                    foreach(var poder in heroi.HeroisSuperpoderes)
                    {
                        poder.Superpoder = await _dbContext.Superpoderes.FindAsync(poder.SuperpoderId);
                    }
                }
                
                if (data is null)
                    throw new Exception ("data is null, non SuperHero found");

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error : {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            string msg = string.Empty;
            try
            {

                var data = await _dbContext.Herois.FindAsync(id); //Retorna um único super heroi

                if (data is null)
                    throw new Exception("Non superhero found");

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error : {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(Herois heroi)
        {
            foreach (var item in heroi.HeroisSuperpoderes)
            {
                item.Superpoder = null;
            }
            try
            {
                await _dbContext.Database.BeginTransactionAsync();
                var addHeroi = await _dbContext.AddAsync(heroi);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();

                return this.StatusCode(StatusCodes.Status200OK, addHeroi.Entity);
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Herois heroi)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();
                var updHeroi = _dbContext.Update(heroi);
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();

                return this.StatusCode(StatusCodes.Status200OK, updHeroi.Entity);
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _dbContext.Database.BeginTransactionAsync();
                int delHeroi = await _dbContext.Database.ExecuteSqlRawAsync("delete from HeroisSuperpoderes where HeroiId = {0}; delete from Herois where id = {0}", id); //Retorna a quantidade de rows afetadas

                var delHeroiPoder = await _dbContext.HeroisSuperpoderes.Where(x => x.HeroiId.Equals(id)).ToListAsync();
                if (delHeroiPoder != null)
                    _dbContext.RemoveRange(delHeroiPoder);

                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();

                return this.StatusCode(StatusCodes.Status200OK, $"{id} Deleted from Database");
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }
    }
}
