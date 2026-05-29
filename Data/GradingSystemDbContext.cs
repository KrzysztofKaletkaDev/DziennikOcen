using Microsoft.EntityFrameworkCore;
using DziennikOcen.Models;
using Microsoft.AspNetCore.Identity;

namespace DziennikOcen.Data
{
    public class GradingSystemDbContext : DbContext
    {
        public GradingSystemDbContext(DbContextOptions<GradingSystemDbContext> options) : base(options)
        {
        }
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<GradeScale> GradeScales { get; set; } = null!;
        public DbSet<AssessmentType> AssessmentTypes { get; set; } = null!;
        public DbSet<StudentGrade> StudentGrades { get; set; } = null!;
        public DbSet<GradeClassification> GradeClassifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasIndex(r => r.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Student>().HasIndex(s => s.StudentNumber).IsUnique();
            modelBuilder.Entity<Course>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<GradeScale>().HasIndex(g => g.Value).IsUnique();
            modelBuilder.Entity<AssessmentType>().HasIndex(a => a.Name).IsUnique();

            modelBuilder.Entity<StudentGrade>()
                .HasOne(g => g.Course)
                .WithMany(c => c.Grades)
                .HasForeignKey(g => g.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentGrade>()
                .HasOne(g => g.GradeScale)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.GradeScaleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentGrade>()
                .HasOne(g => g.AssessmentType)
                .WithMany(a => a.Grades)
                .HasForeignKey(g => g.AssessmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentGrade>()
                .HasOne(g => g.Lecturer)
                .WithMany(u => u.GivenGrades)
                .HasForeignKey(g => g.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Lecturer" }
            );

            modelBuilder.Entity<GradeScale>().HasData(
                new GradeScale { Id = 1, Value = 2.0m },
                new GradeScale { Id = 2, Value = 3.0m },
                new GradeScale { Id = 3, Value = 3.5m },
                new GradeScale { Id = 4, Value = 4.0m },
                new GradeScale { Id = 5, Value = 4.5m },
                new GradeScale { Id = 6, Value = 5.0m }
            );

            var hasher = new PasswordHasher<User>();
            var defaultAdmin = new User
            {
                Id = 1,
                FirstName = "System",
                LastName = "Administrator",
                Email = "admin@dziennik.pl",
                RoleId = 1,
                PasswordHash = string.Empty
            };
            defaultAdmin.PasswordHash = hasher.HashPassword(defaultAdmin, "Admin123!");

            modelBuilder.Entity<User>().HasData(defaultAdmin);
        }
    }
}
