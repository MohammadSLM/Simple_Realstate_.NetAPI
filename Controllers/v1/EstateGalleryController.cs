using Microsoft.AspNetCore.Authorization;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyApi1.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EstateGalleryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstateGalleryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstateGallery>>> GetEstateGallery(int estateId)
        {
            return await _context.EstateGalleries.Where(a=>a.EstateId.Equals(estateId)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EstateGallery>> GetImage(int id)
        {
            var image = await _context.EstateGalleries.FindAsync(id);

            if (image is null)
            {
                return NotFound();
            }

            return image;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<EstateGalleryDto>> Add(EstateGalleryDto dto,CancellationToken cancellationToken)
        {

            if (dto != null && dto.Pic.Length > 0)
            {


                string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Estate", dto.Pic.FileName);
                using (var Stream = new FileStream(uploadpath, FileMode.Create))
                {
                    await dto.Pic.CopyToAsync(Stream);
                }

                EstateGallery estateGallery = new EstateGallery()
                {
                    EstateId = dto.EstateId,
                    PicUrl = "/Upload/Estate/" + dto.Pic.FileName
                };
                await _context.EstateGalleries.AddAsync(estateGallery,cancellationToken);
                await _context.SaveChangesAsync();
            }


            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<EstateGallery>> Delete(int id)
        {
            var estateGallery = await _context.EstateGalleries.FindAsync(id);
            if (estateGallery == null)
            {
                return NotFound();
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + estateGallery.PicUrl.TrimStart('\\'));
            FileInfo myfileinf = new FileInfo(path);
            myfileinf.Delete();

            _context.EstateGalleries.Remove(estateGallery);
            await _context.SaveChangesAsync();

            return estateGallery;
        }

    }
}
