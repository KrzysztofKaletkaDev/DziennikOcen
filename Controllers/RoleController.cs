using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DziennikOcen.Data;
using DziennikOcen.Models;

namespace DziennikOcen.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly GradingSystemDbContext _context;

        public RoleController(GradingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role role)
        {
            if (!ModelState.IsValid) return View(role);

            var roleExists = await _context.Roles.AnyAsync(r => r.Name.ToLower() == role.Name.ToLower());
            if (roleExists)
            {
                ModelState.AddModelError("Name", "Taka rola systemowa już istnieje.");
                return View(role);
            }

            _context.Add(role);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Nowa rola '{role.Name}' została pomyślnie dodana.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            if (id == 1 || id == 2)
            {
                TempData["ErrorMessage"] = "Modyfikacja ról fabrycznych (Admin, Lecturer) jest bezwzględnie zablokowana ze względów bezpieczeństwa kodu!";
                return RedirectToAction(nameof(Index));
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Role role)
        {
            if (id != role.Id) return NotFound();

            if (id == 1 || id == 2)
            {
                TempData["ErrorMessage"] = "Operacja zablokowana!";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid) return View(role);

            try
            {
                _context.Update(role);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Nazwa roli została zaktualizowana.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Name", "Rola o takiej nazwie już istnieje.");
                return View(role);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 1 || id == 2)
            {
                TempData["ErrorMessage"] = "Nie możesz usunąć roli wbudowanej w silnik autoryzacji aplikacji.";
                return RedirectToAction(nameof(Index));
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();

            try
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Rola '{role.Name}' została usunięta.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = $"Nie można usunąć roli '{role.Name}', ponieważ są do niej przypisani użytkownicy.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}