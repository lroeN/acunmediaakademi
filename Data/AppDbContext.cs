using Microsoft.EntityFrameworkCore;
using UserRoles.Model;

namespace UserRoles.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }

    public DbSet<ClassroomStudent> ClassroomStudents { get; set; }
    public DbSet<ClassroomInstructor> ClassroomInstructors { get; set; }

    


    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=localhost;Database=UserRolesDb;User Id=*****; Password=******;TrustServerCertificate=true");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ClassroomStudent ara tablosu composite key
        modelBuilder.Entity<ClassroomStudent>()
            .HasKey(cs => new { cs.ClassroomId, cs.StudentId });

        modelBuilder.Entity<ClassroomStudent>()
            .HasOne(cs => cs.Classroom)
            .WithMany(c => c.ClassroomStudents)
            .HasForeignKey(cs => cs.ClassroomId);

        modelBuilder.Entity<ClassroomStudent>()
            .HasOne(cs => cs.Student)
            .WithMany(s => s.ClassroomStudents)
            .HasForeignKey(cs => cs.StudentId);

        // ClassroomInstructor ara tablosu composite key
        modelBuilder.Entity<ClassroomInstructor>()
            .HasKey(ci => new { ci.ClassroomId, ci.InstructorId });

        modelBuilder.Entity<ClassroomInstructor>()
            .HasOne(ci => ci.Classroom)
            .WithMany(c => c.ClassroomInstructors)
            .HasForeignKey(ci => ci.ClassroomId);

        modelBuilder.Entity<ClassroomInstructor>()
            .HasOne(ci => ci.Instructor)
            .WithMany(i => i.ClassroomInstructors)
            .HasForeignKey(ci => ci.InstructorId);

        // Sınıflar için başlangıç verisi (seed data)
        modelBuilder.Entity<Classroom>().HasData(
            new Classroom { Id = 1, Name = "1. Sınıf Full Stack Focus" },
            new Classroom { Id = 2, Name = "2. Sınıf Girişimcilik" },
            new Classroom { Id = 3, Name = "3. Sınıf Pazarlama" }
        );
    }
}
