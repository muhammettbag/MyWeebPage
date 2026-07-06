using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebPage.Data;
using MyWebPage.Models;
using System.Linq;

namespace MyWebPage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ExperienceController : Controller
    {
        private readonly AppDbContext _context;

        public ExperienceController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var experiences = _context.Experiences.OrderByDescending(e => e.Id).ToList();
            return View(experiences);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Experience experience)
        {
            if (ModelState.IsValid)
            {
                _context.Experiences.Add(experience);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(experience);
        }

        public IActionResult Edit(int id)
        {
            var experience = _context.Experiences.Find(id);
            if (experience == null)
            {
                return NotFound();
            }
            return View(experience);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Experience experience)
        {
            if (ModelState.IsValid)
            {
                _context.Experiences.Update(experience);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(experience);
        }

        public IActionResult Delete(int id)
        {
            var experience = _context.Experiences.Find(id);
            if (experience != null)
            {
                _context.Experiences.Remove(experience);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
