using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Model;
using MyApi1.Model;
using MyApi1.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyApi1.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Project
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Project.ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectView()
        {
            return await _context.Project.OrderByDescending(a => a.Id).Take(3).ToListAsync();
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var Project = await _context.Project.FindAsync(id);

            if (Project == null)
            {
                return NotFound();
            }

            return Project;
        }

        // PUT: api/Project/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] ProjectDto dto, int id)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var model = await _context.Project.SingleOrDefaultAsync(a => a.Id.Equals(id));
            if (model == null) return NotFound();

            model.Title = dto.Title;
            model.VidLink = dto.VidLink;

            _context.Project.Update(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Project
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProjectDto>> Add([FromForm] ProjectDto dto,CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                Project Project = new Project()
                {
                    Title = dto.Title,
                    VidLink = dto.VidLink
                };

                await _context.Project.AddAsync(Project,cancellationToken);
                await _context.SaveChangesAsync();

                return Ok();
            }
            else return BadRequest("دیتای نادرست");
        }

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Project>> Delete(int id)
        {
            var Project = await _context.Project.FindAsync(id);
            if (Project == null)
            {
                return NotFound();
            }

            _context.Project.Remove(Project);
            await _context.SaveChangesAsync();

            return Project;
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.Id == id);
        }
    }
}
