using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Model;
using MyApi1.Model.DTO;

namespace MyApi1.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<City>>> GetCityList([FromQuery]int provinceId)
        {
            return await _context.City.Where(a=>a.ParentId == provinceId).ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<City>>> GetProvinceList()
        {
            return await _context.City.Where(a=>a.ParentId==0).ToListAsync();
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCityOrProvince(int id)
        {
            var city = await _context.City.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] int id, CityDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }


            var model = await _context.City.FindAsync(id);
            model.Id = dto.Id;
            model.Title = dto.Title;
            model.ParentId = dto.ParentId;

            _context.City.Update(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Cities
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult<CityDto>> Add([FromForm]CityDto dto,CancellationToken cancellationToken)
        {
            City city = new City()
            {
                Title = dto.Title,
                ParentId = dto.ParentId
            };

            await _context.City.AddAsync(city,cancellationToken);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<City>> Delete([FromQuery] int id)
        {
            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.City.Remove(city);
            await _context.SaveChangesAsync();

            return city;
        }

        private bool CityExists(int id)
        {
            return _context.City.Any(e => e.Id == id);
        }
    }
}
