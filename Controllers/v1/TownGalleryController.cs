using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Model;
using MyApi1.Model.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyApi1.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TownGalleryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TownGalleryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TownGallery>>> GetTownGallery(int townId)
        {
            return await _context.TownGalleries.Where(a => a.TownId.Equals(townId)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TownGallery>> GetImage(int id)
        {
            var image = await _context.TownGalleries.FindAsync(id);

            if (image is null)
            {
                return NotFound();
            }

            return image;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TownGalleryDto>> Add([FromForm] TownGalleryDto dto,CancellationToken cancellationToken)
        {

            if (dto != null && dto.Pic.Length > 0)
            {
                string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "TownUtm", dto.Pic.FileName);
                using (var Stream = new FileStream(uploadpath, FileMode.Create))
                {
                    await dto.Pic.CopyToAsync(Stream);
                }

                TownGallery TownGallery = new TownGallery()
                {
                    TownId = dto.TownId,
                    PicUrl = "/Upload/TownUtm/" + dto.Pic.FileName
                };
                await _context.TownGalleries.AddAsync(TownGallery, cancellationToken);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest();
            }


            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<TownGallery>> Delete(int id)
        {
            var TownGallery = await _context.TownGalleries.FindAsync(id);
            if (TownGallery == null)
            {
                return NotFound();
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + TownGallery.PicUrl.TrimStart('\\'));
            FileInfo myfileinf = new FileInfo(path);
            myfileinf.Delete();

            _context.TownGalleries.Remove(TownGallery);
            await _context.SaveChangesAsync();

            return TownGallery;
        }
    }
}
