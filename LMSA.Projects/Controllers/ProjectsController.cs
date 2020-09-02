using System.Linq;
using System.Threading.Tasks;
using LMSA.Projects.DAL.Contexts;
using LMSA.Projects.DAL.Models;
using LMSA.Shared;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace LMSA.Projects.Controllers
{
    [Route("/projects")]
    public class ProjectsController : Controller
    {
        private readonly ProjectsDbContext _context;
        private readonly RabbitMQManager rabbitMQ;
        readonly IPublishEndpoint _publishEndpoint;

        public ProjectsController(ProjectsDbContext context, RabbitMQManager rabbitMQ)
        {
            _context = context;
            this.rabbitMQ = rabbitMQ;
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

            //await _publishEndpoint.Publish<Project>(new
            //{
            //    Value = project
            //});
            var channel = rabbitMQ.GetChannel; 

            byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes("Project Created!");
            channel.BasicPublish("lmsa", "", null, messageBodyBytes);

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