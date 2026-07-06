using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebPage.Data;
using MyWebPage.Models;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace MyWebPage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProjectController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                if (project.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(project.ImageFile.FileName);
                    string extension = Path.GetExtension(project.ImageFile.FileName);
                    project.ImageUrl = "/uploads/projects/" + fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    
                    string path = Path.Combine(wwwRootPath + "/uploads/projects/", fileName + DateTime.Now.ToString("yymmssfff") + extension);
                    
                    Directory.CreateDirectory(Path.Combine(wwwRootPath, "uploads", "projects"));
                    
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await project.ImageFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (id != project.Id) return NotFound();
            if (ModelState.IsValid)
            {
                if (project.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(project.ImageFile.FileName);
                    string extension = Path.GetExtension(project.ImageFile.FileName);
                    project.ImageUrl = "/uploads/projects/" + fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    
                    string path = Path.Combine(wwwRootPath + "/uploads/projects/", fileName + DateTime.Now.ToString("yymmssfff") + extension);
                    
                    Directory.CreateDirectory(Path.Combine(wwwRootPath, "uploads", "projects"));
                    
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await project.ImageFile.CopyToAsync(fileStream);
                    }
                }
                
                _context.Update(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
