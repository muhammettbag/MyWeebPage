using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebPage.Data;
using MyWebPage.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyWebPage.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PersonalInfoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PersonalInfoController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var info = await _context.PersonalInfos.FirstOrDefaultAsync();
            if (info == null)
            {
                info = new PersonalInfo();
                _context.PersonalInfos.Add(info);
                await _context.SaveChangesAsync();
            }
            return View(info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PersonalInfo model, IFormFile? cvFile)
        {
            if (ModelState.IsValid)
            {
                if (cvFile != null && cvFile.Length > 0)
                {
                    // Sadece PDF kabul edelim (isteğe bağlı)
                    if (Path.GetExtension(cvFile.FileName).ToLower() == ".pdf")
                    {
                        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "cv");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + cvFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await cvFile.CopyToAsync(fileStream);
                        }

                        // Eski dosyayı silme işlemi eklenebilir
                        if (!string.IsNullOrEmpty(model.CvUrl))
                        {
                            var oldPath = Path.Combine(_env.WebRootPath, model.CvUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Exists(oldPath); // TODO: Delete
                        }

                        model.CvUrl = "/uploads/cv/" + uniqueFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lütfen sadece PDF formatında dosya yükleyin.");
                        return View(model);
                    }
                }

                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Kişisel bilgiler güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
