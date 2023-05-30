using Bizland.DAL;
using Bizland.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bizland.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
        List<Team>teams=_appDbContext.teams.ToList();
            return View(teams);
        }

      

    }
}