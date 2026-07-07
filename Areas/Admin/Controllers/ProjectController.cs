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
                if (project.ImageFiles != null && project.ImageFiles.Count > 0)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string uploadsFolder = Path.Combine(wwwRootPath, "uploads", "projects");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    foreach (var file in project.ImageFiles)
                    {
                        string extension = Path.GetExtension(file.FileName);
                        string fileName = Guid.NewGuid().ToString() + extension;
                        string path = Path.Combine(uploadsFolder, fileName);
                        
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        
                        project.ProjectImages.Add(new ProjectImage { ImageUrl = "/uploads/projects/" + fileName });
                    }
                    
                    if (project.ProjectImages.Count > 0)
                    {
                        project.ImageUrl = project.ProjectImages.First().ImageUrl;
                    }
                }
                // Fallback for single image upload if used
                else if (project.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string extension = Path.GetExtension(project.ImageFile.FileName);
                    string fileName = Guid.NewGuid().ToString() + extension;
                    
                    project.ImageUrl = "/uploads/projects/" + fileName;
                    
                    string uploadsFolder = Path.Combine(wwwRootPath, "uploads", "projects");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    string path = Path.Combine(uploadsFolder, fileName);
                    
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await project.ImageFile.CopyToAsync(fileStream);
                    }
                    project.ProjectImages.Add(new ProjectImage { ImageUrl = project.ImageUrl });
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
                var existingProject = await _context.Projects
                    .Include(p => p.ProjectImages)
                    .FirstOrDefaultAsync(p => p.Id == project.Id);
                    
                if (existingProject == null) return NotFound();

                // Update properties
                existingProject.Title = project.Title;
                existingProject.Summary = project.Summary;
                existingProject.Technologies = project.Technologies;
                existingProject.LiveLink = project.LiveLink;

                // Handle multiple image uploads
                if (project.ImageFiles != null && project.ImageFiles.Count > 0)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string uploadsFolder = Path.Combine(wwwRootPath, "uploads", "projects");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    foreach (var file in project.ImageFiles)
                    {
                        string extension = Path.GetExtension(file.FileName);
                        string fileName = Guid.NewGuid().ToString() + extension;
                        string path = Path.Combine(uploadsFolder, fileName);
                        
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        
                        existingProject.ProjectImages.Add(new ProjectImage { ImageUrl = "/uploads/projects/" + fileName });
                    }
                    
                    if (string.IsNullOrEmpty(existingProject.ImageUrl) && existingProject.ProjectImages.Count > 0)
                    {
                        existingProject.ImageUrl = existingProject.ProjectImages.First().ImageUrl;
                    }
                }
                else if (project.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string extension = Path.GetExtension(project.ImageFile.FileName);
                    string fileName = Guid.NewGuid().ToString() + extension;
                    
                    string newImageUrl = "/uploads/projects/" + fileName;
                    
                    string uploadsFolder = Path.Combine(wwwRootPath, "uploads", "projects");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    string path = Path.Combine(uploadsFolder, fileName);
                    
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await project.ImageFile.CopyToAsync(fileStream);
                    }
                    existingProject.ImageUrl = newImageUrl;
                    existingProject.ProjectImages.Add(new ProjectImage { ImageUrl = newImageUrl });
                }
                
                _context.Update(existingProject);
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
