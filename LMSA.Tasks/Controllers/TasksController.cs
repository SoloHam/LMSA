using System.Linq;
using System.Threading.Tasks;
using LMSA.Tasks.DAL.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace LMSA.Tasks.Controllers
{
    [Route("/tasks")]
    public class TasksController : Controller
    {
        private readonly TasksDbContext _context;

        public TasksController(TasksDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> OnGet()
        {
            return Json(_context.Tasks.ToArray());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> OnGet(int id)
        {
            var task = _context.Tasks.FindAsync(id);

            if (task == null)
                return BadRequest();
            else
                return Json(task);
        }

        [HttpPost]
        public async Task<IActionResult> OnPost([FromBody] DAL.Models.Task task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return Created(Url.ActionLink(nameof(OnGet), null, new { id = task.Id }), task);
        }

        [HttpPut]
        public async Task<IActionResult> OnPut([FromBody] DAL.Models.Task task)
        {
            var cur = await _context.Tasks.FindAsync(task.Id);
            if (cur == null)
                return BadRequest();
            else
            {
                cur.Title = task.Title;
                cur.Description = task.Description;
                cur.IsComplete = task.IsComplete;

                _context.Tasks.Update(cur);
                await _context.SaveChangesAsync();

                return Json(cur);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> OnDelete(int id)
        {
            DAL.Models.Task task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return BadRequest();
            else
            {
                try
                {
                    _context.Tasks.Remove(task);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                catch
                {
                    return BadRequest();
                }
            }
        }
    }
}