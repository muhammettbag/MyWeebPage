using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebPage.Data;
using MyWebPage.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebPage.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var personalInfo = _context.PersonalInfos.FirstOrDefault();
        var projects = _context.Projects.ToList();
        var experiences = _context.Experiences.OrderByDescending(e => e.Id).ToList();

        var viewModel = new HomeViewModel
        {
            PersonalInfo = personalInfo ?? new PersonalInfo(),
            Projects = projects ?? new List<Project>(),
            Experiences = experiences ?? new List<Experience>()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
