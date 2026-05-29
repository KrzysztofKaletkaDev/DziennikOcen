using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DziennikOcen.Data;
using DziennikOcen.Models;

namespace DziennikOcen.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GradeClassificationController : Controller
    {
        private readonly GradingSystemDbContext _context;

        public GradeClassificationController(GradingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var classifications = await _context.GradeClassifications.OrderBy(c => c.Name).ToListAsync();
            return View(classifications);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GradeClassification classification)
        {
            if (!ModelState.IsValid) return View(classification);

            var exists = await _context.GradeClassifications
                .AnyAsync(c => c.Name.ToLower() == classification.Name.ToLower());

            if (exists)
            {
                ModelState.AddModelError("Name", "Taka klasyfikacja ocen już istnieje w systemie.");
                return View(classification);
            }

            _context.Add(classification);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Klasyfikacja '{classification.Name}' została pomyślnie dodana.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var classification = await _context.GradeClassifications.FindAsync(id);
            if (classification == null) return NotFound();

            return View(classification);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GradeClassification classification)
        {
            if (id != classification.Id) return NotFound();

            if (!ModelState.IsValid) return View(classification);

            var isAlreadyAssigned = await _context.StudentGrades.AnyAsync(g => g.GradeClassificationId == id);
            if (isAlreadyAssigned)
            {
                ModelState.AddModelError(string.Empty, $"Nie można zmienić nazwy '{classification.Name}', ponieważ studenci posiadają już przypisane oceny z tej kategorii!");
                return View(classification);
            }

            var exists = await _context.GradeClassifications
                .AnyAsync(c => c.Name.ToLower() == classification.Name.ToLower() && c.Id != id);

            if (exists)
            {
                ModelState.AddModelError("Name", "Inna klasyfikacja ma już taką samą nazwę.");
                return View(classification);
            }

            try
            {
                _context.Update(classification);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pomyślnie zaktualizowano nazwę klasyfikacji.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Wystąpił nieoczekiwany błąd bazy danych.");
                return View(classification);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var classification = await _context.GradeClassifications.FindAsync(id);
            if (classification == null) return NotFound();

            try
            {
                _context.GradeClassifications.Remove(classification);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Klasyfikacja '{classification.Name}' została usunięta ze słownika.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = $"Nie można usunąć kategorii '{classification.Name}', ponieważ studenci posiadają w dzienniku oceny cząstkowe z tego typu zadania!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}