using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using DziennikOcen.Data;
using DziennikOcen.Models;

namespace DziennikOcen.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class GradeController : Controller
    {
        private readonly GradingSystemDbContext _context;

        public GradeController(GradingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            var allGrades = await _context.StudentGrades
                .Include(g => g.Course)
                .Include(g => g.GradeScale)
                .Include(g => g.GradeClassification)
                .Include(g => g.Lecturer)
                .Where(g => g.StudentId == id)
                .ToListAsync();

            ViewBag.Student = student;
            return View(allGrades);
        }

        public async Task<IActionResult> Manage(int? id, int? courseId)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            var courses = await _context.Courses.ToListAsync();
            var scales = await _context.GradeScales.OrderBy(s => s.Value).ToListAsync();
            var classifications = await _context.GradeClassifications.OrderBy(c => c.Name).ToListAsync();

            ViewBag.Student = student;
            ViewBag.Courses = new SelectList(courses, "Id", "Name", courseId);
            ViewBag.Scales = new SelectList(scales, "Id", "Value");
            ViewBag.Classifications = new SelectList(classifications, "Id", "Name");
            ViewBag.SelectedCourseId = courseId;

            List<StudentGrade> filteredGrades = new List<StudentGrade>();

            if (courseId.HasValue)
            {
                filteredGrades = await _context.StudentGrades
                    .Include(g => g.GradeScale)
                    .Include(g => g.GradeClassification)
                    .Include(g => g.Lecturer)
                    .Where(g => g.StudentId == id && g.CourseId == courseId.Value)
                    .ToListAsync();
            }

            return View(filteredGrades);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGrade(StudentGrade grade)
        {
            var loggedUserEmail = User.Identity?.Name;
            var lecturer = await _context.Users.FirstOrDefaultAsync(u => u.Email == loggedUserEmail);

            if (lecturer != null)
            {
                grade.LecturerId = lecturer.Id;
            }

            ModelState.Remove("Student");
            ModelState.Remove("Course");
            ModelState.Remove("Lecturer");
            ModelState.Remove("GradeScale");
            ModelState.Remove("GradeClassification");
            ModelState.Remove("LecturerId");
            ModelState.Remove("Id");
            ModelState.Remove("AssessmentType");

            if (ModelState.IsValid)
            {
                _context.Add(grade);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ocena została pomyślnie dopisana studentowi.";
            }
            else
            {
                var errorMessages = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                TempData["ErrorMessage"] = $"Nie udało się dodać oceny. Szczegóły błędów: {errorMessages}";
            }

            return RedirectToAction(nameof(Manage), new { id = grade.StudentId, courseId = grade.CourseId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var grade = await _context.StudentGrades.FindAsync(id);
            if (grade == null) return NotFound();

            int studentId = grade.StudentId;
            int courseId = grade.CourseId;

            var loggedUserEmail = User.Identity?.Name;
            var currentLecturer = await _context.Users.FirstOrDefaultAsync(u => u.Email == loggedUserEmail);

            if (currentLecturer == null || grade.LecturerId != currentLecturer.Id)
            {
                TempData["ErrorMessage"] = "Brak uprawnień! Tę ocenę może usunąć wyłącznie wykładowca, który ją wystawił.";
                return RedirectToAction(nameof(Manage), new { id = studentId, courseId = courseId });
            }

            _context.StudentGrades.Remove(grade);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ocena cząstkowa została usunięta z dziennika.";
            return RedirectToAction(nameof(Manage), new { id = studentId, courseId = courseId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGrade(int id, int GradeScaleId, int GradeClassificationId)
        {
            var grade = await _context.StudentGrades.FindAsync(id);
            if (grade == null) return NotFound();

            var loggedUserEmail = User.Identity?.Name;
            var currentLecturer = await _context.Users.FirstOrDefaultAsync(u => u.Email == loggedUserEmail);

            if (currentLecturer == null || grade.LecturerId != currentLecturer.Id)
            {
                TempData["ErrorMessage"] = "Błąd uprawnień! Możesz edytować tylko własne oceny.";
                return RedirectToAction(nameof(Manage), new { id = grade.StudentId, courseId = grade.CourseId });
            }

            grade.GradeScaleId = GradeScaleId;
            grade.GradeClassificationId = GradeClassificationId;

            _context.Update(grade);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ocena została pomyślnie zaktualizowana.";
            return RedirectToAction(nameof(Manage), new { id = grade.StudentId, courseId = grade.CourseId });
        }
    }
}