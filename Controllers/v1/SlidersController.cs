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

namespace MyApi1.Controllers
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SlidersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Sliders
        [HttpGet("[action]")]
        public async Task<ActionResult<List<SliderSelectDto>>> GetSliders()
        {
            return await _context.Sliders.Select(a => new SliderSelectDto
            {
                Id = a.Id,
                Title = a.Title,
                Link = a.Link,
                Desc = a.Desc,
                Order = a.Order,
                Pic = BaseConfiguration.GetBaseUrl() + a.PicUrl

            }).ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<SliderSelectDto>>> GetSlidersView()
        {
            return await _context.Sliders.OrderByDescending(a => a.Id).Take(3).Select(a => new SliderSelectDto
            {
                Id = a.Id,
                Title = a.Title,
                Link = a.Link,
                Desc = a.Desc,
                Order = a.Order,
                Pic = BaseConfiguration.GetBaseUrl() + a.PicUrl

            }).ToListAsync();
        }

        // GET: api/Sliders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SliderSelectDto>> GetSlider(int id,CancellationToken cancellationToken)
        {
            var slider = await _context.Sliders.AsNoTracking().Select(a => new SliderSelectDto
            {
                Id = a.Id,
                Title = a.Title,
                Link = a.Link,
                Desc = a.Desc,
                Order = a.Order,
                Pic = BaseConfiguration.GetBaseUrl() + a.PicUrl

            }).SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            if (slider == null)
            {
                return NotFound();
            }

            return slider;
        }

        // PUT: api/Sliders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> Update([FromForm] SliderDto dto, int id)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var model = await _context.Sliders.SingleOrDefaultAsync(a => a.Id.Equals(id));
            if (model == null) return NotFound();

            model.Title = dto.Title;
            model.Link = dto.Link;
            model.Order = dto.Order;

            if (dto.Pic != null)
            {
                var fileAdd = "/Upload/Estate/" + dto.Pic.FileName;
                if (model.PicUrl != fileAdd)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + model.PicUrl.TrimStart('\\'));
                    FileInfo myfileinf = new FileInfo(path);
                    myfileinf.Delete();

                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Estate", dto.Pic.FileName);
                    using (var Stream = new FileStream(uploadpath, FileMode.Create))
                    {
                        dto.Pic.CopyTo(Stream);
                    }

                    model.PicUrl = fileAdd;
                }
            }

            _context.Sliders.Update(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Sliders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult<SliderDto>> Add([FromForm] SliderDto dto,CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                Slider slider = new Slider()
                {
                    Title = dto.Title,
                    Desc = dto.Desc,
                    Link = dto.Link,
                    Order = dto.Order
                };

                if (dto.Pic != null && dto.Pic.Length > 0)
                {
                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Slider", dto.Pic.FileName);
                    using (var Stream = new FileStream(uploadpath, FileMode.Create))
                    {
                        await dto.Pic.CopyToAsync(Stream);
                    }

                    slider.PicUrl = "/Upload/Slider/" + dto.Pic.FileName;
                }

                await _context.Sliders.AddAsync(slider,cancellationToken);
                await _context.SaveChangesAsync();

                return Ok();
            }
            else return BadRequest("دیتای نادرست");
        }

        // DELETE: api/Sliders/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Slider>> Delete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }

            if (slider.PicUrl != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + slider.PicUrl.TrimStart('\\'));
                FileInfo myfileinf = new FileInfo(path);
                myfileinf.Delete();
            }
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return slider;
        }

        private bool SliderExists(int id)
        {
            return _context.Sliders.Any(e => e.Id == id);
        }
    }
}
