using System.Linq;
using System.Threading.Tasks;
using LMSA.Projects.DAL.Contexts;
using LMSA.Projects.DAL.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace LMSA.Projects.Controllers
{
    [Route("/projects")]
    public class ProjectsController : Controller
    {
        private readonly ProjectsDbContext _context;

        readonly IPublishEndpoint _publishEndpoint;

        public ProjectsController(ProjectsDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> OnGet()
        {
            return Json(_context.Projects.ToArray());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> OnGet(int id)
        {
            var proj = await _context.Projects.FindAsync(id);
            if (proj == null)
                return BadRequest();
            else
                return Json(proj);
        }
        [HttpPost]
        public async Task<IActionResult> OnPost([FromBody] Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            await _publishEndpoint.Publish<Project>(new
            {
                Value = project
            });

            return Created(Url.ActionLink(nameof(OnGet), null, new
            {
                id = project.Id
            }), project);
        }
        [HttpPut]
        public async Task<IActionResult> OnPut([FromBody] Project project)
        {
            var cur = await _context.Projects.FindAsync(project.Id);

            if (cur == null)
                return BadRequest();
            else
            {
                cur.Title = project.Title;
                cur.Description = project.Description;

                var result = _context.Projects.Update(cur);
                await _context.SaveChangesAsync();

                return Json(result);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> OnDelete(int id)
        {
            Project project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                try
                {
                    _context.Projects.Remove(project);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                catch
                {
                    return StatusCode(501);
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}