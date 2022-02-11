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
using MyApi1.Model;
using MyApi1.Model.DTO;
using static MyApi1.Extensions;

namespace MyApi1.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Settings
        [HttpGet]
        public async Task<ActionResult<SettingDto>> GetSetting(CancellationToken cancellationToken)
        {
            return await _context.Setting.AsNoTracking().OrderByDescending(s=>s.Id).Select(a=> new SettingDto
            {
             Id = a.Id,
             BannerPicUrl1Url = BaseConfiguration.GetBaseUrl() + a.BannerPicUrl1,
             BannerPicUrl2Url = BaseConfiguration.GetBaseUrl() + a.BannerPicUrl2,
             CustomerCount = a.CustomerCount,
             ProjectCount = a.ProjectCount,
             TownCount = a.TownCount,
             ZoneCount = a.ZoneCount
            }).FirstOrDefaultAsync(cancellationToken);
        }

        // PUT: api/Settings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] SettingDto dto , int id)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var model = await _context.Setting.SingleOrDefaultAsync(a => a.Id.Equals(id));
            if (model == null) return NotFound();

            model.CustomerCount = dto.CustomerCount;
            model.ProjectCount = dto.ProjectCount;
            model.TownCount = dto.TownCount;
            model.ZoneCount = dto.ZoneCount;

            if (dto.BannerPicUrl1 != null)
            {
                var fileAdd = "/Upload/" + dto.BannerPicUrl1.FileName;
                if (model.BannerPicUrl1 != fileAdd)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + model.BannerPicUrl1.TrimStart('\\'));
                    FileInfo myfileinf = new FileInfo(path);
                    myfileinf.Delete();

                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Estate", dto.BannerPicUrl1.FileName);
                    using (var Stream = new FileStream(uploadpath, FileMode.Create))
                    {
                        dto.BannerPicUrl1.CopyTo(Stream);
                    }

                    model.BannerPicUrl1 = fileAdd;
                }
            }

            if (dto.BannerPicUrl2 != null)
            {
                var fileAdd = "/Upload/" + dto.BannerPicUrl2.FileName;
                if (model.BannerPicUrl2 != fileAdd)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + model.BannerPicUrl2.TrimStart('\\'));
                    FileInfo myfileinf = new FileInfo(path);
                    myfileinf.Delete();

                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Estate", dto.BannerPicUrl2.FileName);
                    using (var Stream = new FileStream(uploadpath, FileMode.Create))
                    {
                        dto.BannerPicUrl2.CopyTo(Stream);
                    }

                    model.BannerPicUrl2 = fileAdd;
                }
            }

            _context.Setting.Update(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Settings
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Setting>> Add([FromForm] SettingDto dto,CancellationToken cancellationToken)
        {
            Setting setting = new Setting
            {
                CustomerCount = dto.CustomerCount,
                TownCount = dto.TownCount,
                ProjectCount = dto.ProjectCount,
                ZoneCount = dto.ZoneCount
            };

            if (dto.BannerPicUrl1 != null && dto.BannerPicUrl1.Length > 0)
            {
                string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", dto.BannerPicUrl1.FileName);
                using (var Stream = new FileStream(uploadpath, FileMode.Create))
                {
                    await dto.BannerPicUrl1.CopyToAsync(Stream);
                }

                setting.BannerPicUrl1 = "/Upload/" + dto.BannerPicUrl1.FileName;
            }

            if (dto.BannerPicUrl2 != null && dto.BannerPicUrl2.Length > 0)
            {
                string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", dto.BannerPicUrl2.FileName);
                using (var Stream = new FileStream(uploadpath, FileMode.Create))
                {
                    await dto.BannerPicUrl2.CopyToAsync(Stream);
                }

                setting.BannerPicUrl2 = "/Upload/" + dto.BannerPicUrl2.FileName;
            }

            await _context.Setting.AddAsync(setting, cancellationToken);
            await _context.SaveChangesAsync();

            return Ok(setting);
        }

        private bool SettingExists(int id)
        {
            return _context.Setting.Any(e => e.Id == id);
        }
    }
}
