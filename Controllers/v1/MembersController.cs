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
using MyApi.Model.DTO;
using static MyApi1.Extensions;

namespace MyApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet("[action]")]
        public async Task<ActionResult<List<MemberSelectDto>>> GetMembers()
        {
            return await _context.Members.Select(a => new MemberSelectDto
            {
                Id = a.Id,
                FullName = a.FullName,
                Instagram = a.Instagram,
                PhoneNumber = a.PhoneNumber,
                Position = a.Position,
                WhatsApp = a.WhatsApp,
                Pic = BaseConfiguration.GetBaseUrl() + a.PicUrl

            }).OrderByDescending(v=>v.Id).ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<MemberSelectDto>>> GetMembersBy9()
        {
            var list = await _context.Members.OrderByDescending(a => a.Id).Take(9).Select(a => new MemberSelectDto
            {
                Id = a.Id,
                FullName = a.FullName,
                Instagram = a.Instagram,
                PhoneNumber = a.PhoneNumber,
                Position = a.Position,
                WhatsApp = a.WhatsApp,
                Pic = BaseConfiguration.GetBaseUrl() + a.PicUrl

            }).OrderByDescending(v => v.Id).ToListAsync();
            return list;
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberSelectDto>> GetMember(int id, CancellationToken cancellationToken)
        {
            var member = await _context.Members.AsNoTracking().Select(a => new MemberSelectDto
            {
                Id = a.Id,
                FullName = a.FullName,
                Instagram = a.Instagram,
                PhoneNumber = a.PhoneNumber,
                Position = a.Position,
                WhatsApp = a.WhatsApp,
                Pic = BaseConfiguration.GetBaseUrl() + a.PicUrl

            }).SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);

            if (member == null)
            {
                return NotFound();
            }

            return member;
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] MemberDto dto , int id)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var model = await _context.Members.SingleOrDefaultAsync(a => a.Id.Equals(id));
            if (model == null) return NotFound();

            model.FullName = dto.FullName;
            model.PhoneNumber = dto.PhoneNumber;
            model.Position = dto.Position;
            model.Instagram = dto.Instagram;
            model.WhatsApp = dto.WhatsApp;

            if (dto.Pic != null)
            {
                var fileAdd = "/Upload/Member/" + dto.Pic.FileName;
                if (model.PicUrl != fileAdd)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + model.PicUrl.TrimStart('\\'));
                    FileInfo myfileinf = new FileInfo(path);
                    myfileinf.Delete();

                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Member", dto.Pic.FileName);
                    using (var Stream = new FileStream(uploadpath, FileMode.Create))
                    {
                        await dto.Pic.CopyToAsync(Stream);
                    }

                    model.PicUrl = fileAdd;
                }
            }

            _context.Members.Update(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        // POST: api/Members
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MemberDto>> Add([FromForm] MemberDto dto,CancellationToken cancellationToken)
        {
            Member member = new Member()
            {
                FullName = dto.FullName,
                Position = dto.Position,
                PhoneNumber = dto.PhoneNumber,
                Instagram = dto.Instagram,
                WhatsApp = dto.WhatsApp
            };
            if (dto.Pic != null && dto.Pic.Length > 0)
            {
                string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Member", dto.Pic.FileName);
                using (var Stream = new FileStream(uploadpath, FileMode.Create))
                {
                    await dto.Pic.CopyToAsync(Stream);
                }

                member.PicUrl = "/Upload/Member/" + dto.Pic.FileName;
            }
            await _context.Members.AddAsync(member,cancellationToken);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Members/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Member>> Delete(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            if (member.PicUrl != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + member.PicUrl.TrimStart('\\'));
                FileInfo myfileinf = new FileInfo(path);
                myfileinf.Delete();
            }
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return member;
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }
}
