using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DziennikOcen.Data;
using DziennikOcen.Models;


namespace DziennikOcen.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly GradingSystemDbContext _context;

        public UserController(GradingSystemDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .ToListAsync();

            return View(users);
        }
        public async Task<IActionResult> CreateAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var roles = await _context.Roles.ToListAsync();
                ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name");
                return View(model);
            }

            var emailExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Ten adres e-mail jest już zajęty.");
                var roles = await _context.Roles.ToListAsync();
                ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name");
                return View(model);
            }

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                RoleId = model.RoleId
            };

            var hasher = new PasswordHasher<User>();
            newUser.PasswordHash = hasher.HashPassword(newUser, model.Password);

            _context.Add(newUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var model = new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleId = user.RoleId,
                IsActive = user.IsActive
            };

            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name", model.RoleId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                var roles = await _context.Roles.ToListAsync();
                ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name", model.RoleId);
                return View(model);
            }

            var emailExists = await _context.Users.AnyAsync(u => u.Email == model.Email && u.Id != id);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Ten adres e-mail jest już zajęty przez innego użytkownika.");
                var roles = await _context.Roles.ToListAsync();
                ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name", model.RoleId);
                return View(model);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (user.RoleId == 1 && model.RoleId != 1)
            {
                var totalAdmins = await _context.Users.CountAsync(u => u.RoleId == 1);

                if (totalAdmins <= 1)
                {
                    ModelState.AddModelError("RoleId", "Nie można zmienić roli tego użytkownika. Jest on ostatnim administratorem w systemie.");

                    var roles = await _context.Roles.ToListAsync();
                    ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name", model.RoleId);
                    return View(model);
                }
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.RoleId = model.RoleId;
            user.IsActive = model.IsActive;

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var hasher = new PasswordHasher<User>();
                user.PasswordHash = hasher.HashPassword(user, model.NewPassword);
            }

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Dane użytkownika {user.FirstName} {user.LastName} zostały pomyślnie zaktualizowane.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Wystąpił nieoczekiwany błąd bazy danych podczas zapisu.");
                var roles = await _context.Roles.ToListAsync();
                ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name", model.RoleId);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            if (user.RoleId == 1 || (user.Role != null && user.Role.Name == "Admin"))
            {
                var totalAdmins = await _context.Users.CountAsync(u => u.RoleId == 1);

                if (totalAdmins <= 1)
                {
                    TempData["ErrorMessage"] = $"Operacja zablokowana! Użytkownik {user.FirstName} {user.LastName} jest ostatnim administratorem w systemie. Nie możesz go usunąć.";
                    return RedirectToAction(nameof(Index));
                }

                try
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Administrator {user.FirstName} {user.LastName} został trwale usunięty z bazy danych.";
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = $"Nie można trwale usunąć administratora {user.FirstName} {user.LastName}, ponieważ posiada on powiązania historyczne w bazie danych (np. wystawione oceny). Zmień mu najpierw rolę na Prowadzącego, a następnie go zdeaktywuj.";
                }

            }
            else
            {
                user.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Prowadzący {user.FirstName} {user.LastName} został pomyślnie zdeaktywowany i przeniesiony do archiwum.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.IsActive = true;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Konto prowadzącego {user.FirstName} {user.LastName} zostało pomyślnie przywrócone.";
            return RedirectToAction(nameof(Index));
        }
    }
}