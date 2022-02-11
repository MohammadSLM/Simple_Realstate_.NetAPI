using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Model;
using MyApi1.Model.DTO;
using static MyApi1.Extensions;

namespace MyApi1.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ZonesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ZonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Zones
        [HttpGet]
        public async Task<ActionResult<List<ZoneSelectDto>>> GetZones()
        {
            return await _context.Zones.Select(a => new ZoneSelectDto
            {
                Id = a.Id,
                Title = a.Title,
                Area = a.Area,
                Desc = a.Desc,
                VidUrl = a.VidUrl,
                CityId = a.CityId,
                City = new CityDetail
                {
                    CityId = a.CityId,
                    ProvinceId = (int)_context.City.SingleOrDefault(s => s.Id == a.CityId).ParentId,
                    CityTitle = _context.City.SingleOrDefault(x => x.Id == a.CityId).Title,
                    ProvinceTitle = _context.City.SingleOrDefault(x => x.Id == (int)_context.City.SingleOrDefault(s => s.Id == a.CityId).ParentId).Title
                },
                ProvinceId = _context.City.Where(b => b.Id == _context.City.Where(z => z.Id == a.CityId).Select(m => m.ParentId).SingleOrDefault()).Select(c => c.Id).SingleOrDefault(),
                Pic = BaseConfiguration.GetBaseUrl() + a.PicUrl

            }).OrderByDescending(r=>r.Id).ToListAsync();

        }

        // GET: api/Zones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ZoneSelectDto>> GetZone(int id,CancellationToken cancellationToken)
        {
            var zone = await _context.Zones.AsNoTracking().Select(a => new ZoneSelectDto
            {
                Id = a.Id,
                Title = a.Title,
                Area = a.Area,
                Desc = a.Desc,
                VidUrl = a.VidUrl,
                CityId = a.CityId,
                ProvinceId = _context.City.Where(b=>b.Id == _context.City.Where(z=>z.Id==a.CityId).Select(m=>m.ParentId).SingleOrDefault()).Select(c=>c.Id).SingleOrDefault(),
                Pic = BaseConfiguration.GetBaseUrl() + a.PicUrl

            }).SingleOrDefaultAsync(p => p.Id.Equals(id),cancellationToken);

            if (zone == null)
            {
                return NotFound();
            }

            return zone;
        }

        // PUT: api/Zones/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] ZoneDto dto, int id)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var model = await _context.Zones.SingleOrDefaultAsync(a => a.Id.Equals(id));
            if (model == null) return NotFound();

            model.Title = dto.Title;
            model.Desc = dto.Desc;
            model.Area = dto.Area;
            model.CityId = dto.CityId;
            model.VidUrl = dto.VidUrl;

            if (dto.Pic != null)
            {
                var fileAdd = "/Upload/Zone/" + dto.Pic.FileName;
                if (model.PicUrl != fileAdd)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + model.PicUrl.TrimStart('\\'));
                    FileInfo myfileinf = new FileInfo(path);
                    myfileinf.Delete();

                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Zone", dto.Pic.FileName);
                    using (var Stream = new FileStream(uploadpath, FileMode.Create))
                    {
                        dto.Pic.CopyTo(Stream);
                    }

                    model.PicUrl = fileAdd;
                }
            }
            _context.Zones.Update(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Zones
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ZoneDto>> Add([FromForm] ZoneDto dto , CancellationToken cancellationToken)
        {
            Zone zone = new Zone()
            {
                Title = dto.Title,
                Desc = dto.Desc,
                VidUrl = dto.VidUrl,
                CityId = dto.CityId,
                Area = dto.Area
            };
            if (dto.Pic != null && dto.Pic.Length > 0)
            {
                string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Zone", dto.Pic.FileName);
                using (var Stream = new FileStream(uploadpath, FileMode.Create))
                {
                    await dto.Pic.CopyToAsync(Stream);
                }

                zone.PicUrl = "/Upload/Zone/" + dto.Pic.FileName;
            }
            await _context.Zones.AddAsync(zone, cancellationToken);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetZone", new { id = zone.Id }, zone);
        }

        // DELETE: api/Zones/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Zone>> Delete(int id)
        {
            var zone = await _context.Zones.FindAsync(id);
            if (zone == null)
            {
                return NotFound();
            }
            if (zone.PicUrl != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + zone.PicUrl.TrimStart('\\'));
                FileInfo myfileinf = new FileInfo(path);
                myfileinf.Delete();
            }

            _context.Zones.Remove(zone);
            await _context.SaveChangesAsync();

            return zone;
        }

        private bool ZoneExists(int id)
        {
            return _context.Zones.Any(e => e.Id == id);
        }
    }
}
