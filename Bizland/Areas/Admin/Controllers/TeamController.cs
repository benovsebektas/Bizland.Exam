using Bizland.DAL;
using Bizland.Models;
using Bizland.Untilities.Extensions;
using Bizland.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bizland.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Team> teams = _context.teams.ToList();
            return View(teams);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Team team = _context.teams.Find(Id);

            if (team == null)
            {
                return NotFound();

            }
            string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", team.ImagePath);
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }
            _context.teams.Remove(team);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }






        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamCreateVM member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!member.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{member.Photo.FileName} It should be in Photo Type ");
                return View();
            }
            if (!member.Photo.CheckFileSize(500))
            {
                ModelState.AddModelError("Photo", $"{member.Photo.FileName} - Not more than 500kb");
                return View();
            }

            string root = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team");
            string fileName = await member.Photo.SaveAsync(root);
            Team teamMember = new Team()
            {
                Name = member.Name,
                ImagePath = fileName,
                Description = member.Description
            };
            await _context.teams.AddAsync(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }










        public async Task<IActionResult> Update(int id)
        {
            Team member = await _context.teams.FindAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Team member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Team? result = await _context.teams.FirstOrDefaultAsync(t => t.Id == member.Id);  
            if (result is null)
            {
                TempData["Exists"] = "This Member is not in the database";
                return RedirectToAction(nameof(Index));
            }
            result.Name = member.Name;
            result.Description = member.Description;
            result.ImagePath = member.ImagePath;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




    }




}
