using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class TownsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TownsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Towns
        [HttpGet("[action]")]
        public async Task<ActionResult<List<TownSelectDto>>> GetTowns([FromQuery] TownCommonModelAsync model)
        {
            var result = await _context.Towns
                .Skip((model.Page - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(a => new TownSelectDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Desc = a.Desc,
                    Lang = a.Lang,
                    Lat = a.Lat,
                    Peyment = a.Peyment,
                    VidUrl = a.VidUrl,
                    UtmPic = a.UtmPicUrl,
                    City = new CityDetail
                    {
                        CityId = a.CityId,
                        ProvinceId = a.ProvinceId,
                        CityTitle = _context.City.SingleOrDefault(x=>x.Id==a.CityId).Title,
                        ProvinceTitle = _context.City.SingleOrDefault(x => x.Id == a.ProvinceId).Title
                    },
                    GalleryPics = _context.TownGalleries.Where(b => b.TownId == a.Id).ToList(),
                    //Pics = _context.TownGalleries.Where(b => b.TownId == a.Id).Select(a => a.PicUrl).ToList()
                }).OrderByDescending(a => a.Id)
                .ToListAsync();

            foreach (var item in result)
            {
                foreach (var pic in item.GalleryPics)
                {
                    var url = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                    item.Pics.Add(url);
                    pic.PicUrl = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                }
            }


            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<TownSelectDto>>> GetTownsBy9()
        {
            var list = await _context.Towns.Select(a => new TownSelectDto
            {
                Id = a.Id,
                Title = a.Title,
                Desc = a.Desc,
                Lang = a.Lang,
                Lat = a.Lat,
                Peyment = a.Peyment,
                VidUrl = a.VidUrl,
                UtmPic = a.UtmPicUrl,
                City = new CityDetail
                {
                    CityId = a.CityId,
                    ProvinceId = a.ProvinceId,
                    CityTitle = _context.City.SingleOrDefault(x => x.Id == a.CityId).Title,
                    ProvinceTitle = _context.City.SingleOrDefault(x => x.Id == a.ProvinceId).Title
                },
                GalleryPics = _context.TownGalleries.Where(b => b.TownId == a.Id).ToList(),
                //Pics = _context.TownGalleries.Where(b => b.TownId == a.Id).Select(a => a.PicUrl).ToList()
            }).OrderByDescending(a => a.Id)
                .Take(9)
                .ToListAsync();

            foreach (var item in list)
            {
                foreach (var pic in item.GalleryPics)
                {
                    var url = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                    item.Pics.Add(url);
                    pic.PicUrl = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                }
            }

            return list;
        }

        // GET: api/Towns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Town>> GetTown(int id, CancellationToken cancellationToken)
        {
            var res = await _context.Towns.AsNoTracking().Select(a => new TownSelectDto
            {
                Id = a.Id,
                Title = a.Title,
                Desc = a.Desc,
                Lat = a.Lat,
                Lang = a.Lang,
                Peyment = a.Peyment,
                VidUrl = a.VidUrl,
                UtmPic = BaseConfiguration.GetBaseUrl() + a.UtmPicUrl,
                CityName = a.City.Title,
                City = new CityDetail
                {
                    CityId = a.CityId,
                    ProvinceId = a.ProvinceId,
                    CityTitle = _context.City.SingleOrDefault(x => x.Id == a.CityId).Title,
                    ProvinceTitle = _context.City.SingleOrDefault(x => x.Id == a.ProvinceId).Title
                },
                GalleryPics = _context.TownGalleries.Where(b => b.TownId == a.Id).ToList(),
                //Pics = _context.TownGalleries.Where(b => b.TownId == a.Id).Select(a => a.PicUrl).ToList()

            }).SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            foreach (var pic in res.GalleryPics)
            {
                var url = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
                res.Pics.Add(url);
                pic.PicUrl = BaseConfiguration.GetBaseUrl() + pic.PicUrl;
            }

            if (res == null)
            {
                return NotFound();
            }

            return Ok(res);
        }

        // PUT: api/Towns/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] TownDto dto, int id)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var model = await _context.Towns.Include(a => a.TownGalleries).SingleOrDefaultAsync(a => a.Id == id);
            if (model == null) return NotFound();

            model.Title = dto.Title;
            model.Desc = dto.Desc;
            model.CityId = dto.CityId;
            model.ProvinceId = dto.ProvinceId;
            model.Lang = dto.Lang;
            model.Lat = dto.Lat;
            model.Peyment = dto.Peyment;
            model.VidUrl = dto.VidUrl;

            if (dto.UtmPic != null)
            {
                var fileAdd = "/Upload/TownUtm/" + dto.UtmPic.FileName;
                if (fileAdd != model.UtmPicUrl)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + model.UtmPicUrl.TrimStart('\\'));
                    FileInfo myfileinf = new FileInfo(path);
                    myfileinf.Delete();

                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "TownUtm", dto.UtmPic.FileName);
                    using (var Stream = new FileStream(uploadpath, FileMode.Create))
                    {
                        dto.UtmPic.CopyTo(Stream);
                    }

                    model.UtmPicUrl = "/Upload/TownUtm/" + dto.UtmPic.FileName;
                }
            }
            _context.Towns.Update(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Towns
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TownDto>> Add([FromForm] TownDto dto , CancellationToken cancellationToken)
        {
            Town town = new Town()
            {
                Title = dto.Title,
                Desc = dto.Desc,
                Peyment = dto.Peyment,
                VidUrl = dto.VidUrl,
                Lang = dto.Lang,
                Lat = dto.Lat,
                ProvinceId = dto.ProvinceId,
                CityId = dto.CityId
            };
            if (dto.UtmPic != null && dto.UtmPic.Length > 0)
            {
                string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "TownUtm", dto.UtmPic.FileName);
                using (var Stream = new FileStream(uploadpath, FileMode.Create))
                {
                    await dto.UtmPic.CopyToAsync(Stream);
                }

                town.UtmPicUrl = "/Upload/TownUtm/" + dto.UtmPic.FileName;
            }

            await _context.Towns.AddAsync(town, cancellationToken);
            await _context.SaveChangesAsync();

            if (dto.GalleryPics.Any())
            {
                foreach (var item in dto.GalleryPics)
                {
                    if (item != null && item.Length > 0)
                    {


                        string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "TownUtm", item.FileName);
                        using (var Stream = new FileStream(uploadpath, FileMode.Create))
                        {
                            await item.CopyToAsync(Stream);
                        }

                        TownGallery townGallery = new TownGallery()
                        {
                            TownId = town.Id,
                            PicUrl = "/Upload/TownUtm/" + item.FileName
                        };
                        await _context.TownGalleries.AddAsync(townGallery, cancellationToken);
                        await _context.SaveChangesAsync();

                    }
                }
            }

            return Ok();
        }

        // DELETE: api/Towns/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Town>> Delete(int id)
        {
            var town = await _context.Towns.FindAsync(id);
            if (town == null)
            {
                return NotFound();
            }

            _context.Towns.Remove(town);

            var gallery = _context.TownGalleries.Where(a => a.TownId.Equals(id)).ToList();
            foreach (var item in gallery)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + item.PicUrl.TrimStart('\\'));
                FileInfo myfileinf = new FileInfo(path);
                myfileinf.Delete();

                _context.TownGalleries.Remove(item);
            }
            await _context.SaveChangesAsync();

            return town;
        }

        private bool TownExists(int id)
        {
            return _context.Towns.Any(e => e.Id == id);
        }
    }
}
