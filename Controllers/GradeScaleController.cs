using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DziennikOcen.Data;
using DziennikOcen.Models;

namespace DziennikOcen.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GradeScaleController : Controller
    {
        private readonly GradingSystemDbContext _context;

        public GradeScaleController(GradingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var scale = await _context.GradeScales.OrderBy(g => g.Value).ToListAsync();
            return View(scale);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GradeScale gradeScale)
        {
            if (!ModelState.IsValid) return View(gradeScale);

            var exists = await _context.GradeScales.AnyAsync(g => g.Value == gradeScale.Value);
            if (exists)
            {
                ModelState.AddModelError("Value", "Taka wartość oceny znajduje się już w skali.");
                return View(gradeScale);
            }

            _context.Add(gradeScale);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Ocena {gradeScale.Value} została pomyślnie dodana do skali.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var gradeScale = await _context.GradeScales.FindAsync(id);
            if (gradeScale == null) return NotFound();

            return View(gradeScale);
        }

        // POST: /GradeScale/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GradeScale gradeScale)
        {
            if (id != gradeScale.Id) return NotFound();

            if (!ModelState.IsValid) return View(gradeScale);

            var isAlreadyAssigned = await _context.StudentGrades.AnyAsync(g => g.GradeScaleId == id);
            if (isAlreadyAssigned)
            {
                ModelState.AddModelError(string.Empty, $"Nie można zmienić wartości {gradeScale.Value}, ponieważ studenci posiadają już tę ocenę w dzienniku! Możesz ją edytować tylko, gdy nie jest używana.");
                return View(gradeScale);
            }

            var exists = await _context.GradeScales.AnyAsync(g => g.Value == gradeScale.Value && g.Id != id);
            if (exists)
            {
                ModelState.AddModelError("Value", "Inna ocena w skali ma już przypisaną tę wartość.");
                return View(gradeScale);
            }

            try
            {
                _context.Update(gradeScale);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pomyślnie zaktualizowano wartość w skali ocen.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Wystąpił nieoczekiwany błąd bazy danych.");
                return View(gradeScale);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var gradeScale = await _context.GradeScales.FindAsync(id);
            if (gradeScale == null) return NotFound();

            var isAssignedToAnyGrade = await _context.StudentGrades.AnyAsync(g => g.GradeScaleId == id);
            if (isAssignedToAnyGrade)
            {
                TempData["ErrorMessage"] = $"Nie można usunąć oceny {gradeScale.Value}, ponieważ studenci posiadają już takie oceny cząstkowe w dzienniku!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.GradeScales.Remove(gradeScale);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Ocena {gradeScale.Value} została usunięta ze skali.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Wystąpił nieoczekiwany błąd bazy danych podczas próby usunięcia.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}