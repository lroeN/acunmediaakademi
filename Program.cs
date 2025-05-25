using System;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using UserRoles.Data;
using UserRoles.Model;

namespace UserRoles;

class Program
{
    private static ConsoleMenu _ogrenciMenu = new("Kullanıcı Menüsü");
    private static ConsoleMenu _egitmenMenu = new("Editör Menüsü");
    private static ConsoleMenu _adminMenu = new("Admin Menüsü");
    
    static void Main(string[] args)
    {
        using (var controlContext = new AppDbContext())
        {   
            if (!controlContext.Users.Any(u => u.Role == Roles.Admin))
            {
                Console.WriteLine("Henüz bir admin bulunmuyor. Bir admin oluşturuluyor...");
                var adminUser = new User
                {
                    Userame = "admin",
                    Password = Helper.HashPassword("admin123"),
                    Role = Roles.Admin
                };
                controlContext.Users.Add(adminUser);
                controlContext.SaveChanges();
                Console.Clear();
               // Admin: Kullanıcı adı: admin | Şifre: admin123
               
            }
        }
        
        Console.WriteLine("1- Giriş Yap");
        Console.WriteLine("2- Kayıt Ol");
        var secim = Console.ReadLine();

        if (secim == "2")
        {
            Register();
            return;
        }
        
        using var context = new AppDbContext();

        Console.Write("Kullanıcı adınız: ");
        var username = Console.ReadLine();

        Console.Write("Şifreniz: ");
        var password = Console.ReadLine();

        var hashedInputPassword = Helper.HashPassword(password);
        var user = context.Users.FirstOrDefault(u => u.Userame == username && u.Password == hashedInputPassword);

        if (user == null)
        {
            Console.WriteLine("Kullanıcı adı veya şifre hatalı!");
            return;
        }
        Console.Clear();
        Console.WriteLine($"Hoşgeldiniz, {user.Userame}!");
        
        SetupMenus();

        switch (user.Role)
        {
            case Roles.Ogrenci:
                _ogrenciMenu.Show(true);
                break;
            case Roles.Egitmen:
                _egitmenMenu.Show(true);
                break;
            case Roles.Admin:
                _adminMenu.Show(true);
                break;
            default:
                Console.WriteLine("Rol bulunamadı!");
                break;
            
            
        }
    }

    static void SetupMenus()
    {
        _ogrenciMenu
            .AddOption("Kullanıcı bilgilerim", () => Console.WriteLine("Bilgiler gösterilecek..."))
            .AddOption("Onay bekleyen yorumlarım", () => Console.WriteLine("Yorumlar gösterilecek..."));

        _egitmenMenu
            .AddOption("Onay bekleyen yorumlar", () => Console.WriteLine("Yorumlar gösterilecek..."))
            .AddOption("Tüm yorumlar", () => Console.WriteLine("Yorumlar gösterilecek..."))
            .AddOption("Yeni içerik ekleme", () => Console.WriteLine("İçerik ekleme..."));

        _adminMenu
            .AddOption("Tüm Kullanıcılar", () => ListAllUsers())
            .AddOption("Kullanıcı Rolünü Değiştir", ChangeUserRole)
            .AddMenu("Eğitmen Menüsü", () => _egitmenMenu.Show())
            .AddOption("Öğrenci Ekle", AddStudent)
            .AddOption("Eğitmen Ekle", AddInstructor)
            .AddOption("Sınıf Ekle", AddClass)
            .AddOption("Öğrenci Sınıfa Ata", AssignStudentToClass)
            .AddOption("Eğitmen Sınıfa Ata", AssignInstructorToClass)
            .AddOption("Öğrenci Güncelle", UpdateStudent)
            .AddOption("Eğitmen Güncelle", UpdateInstructor)
            .AddOption("Sınıf Güncelle", UpdateClassroom)
            .AddOption("Tüm içerikler", () => Console.WriteLine("İçerikler listelenecek..."));
    }

    static void Register()
    {
        using var context = new AppDbContext();

        Console.Write("Kullanıcı Adı: ");
        var username = Console.ReadLine();

        // Kullanıcı adı kontrolü (isteğe bağlı)
        if (context.Users.Any(u => u.Userame == username))
        {
            Console.WriteLine("Bu kullanıcı adı zaten alınmış.");
            return;
        }

        Console.Write("Şifre: ");
        var password = Console.ReadLine();
        var hashedPassword = Helper.HashPassword(password);

        var newUser = new User
        {
            Userame = username,
            Password = hashedPassword,
            Role = Roles.Ogrenci // Sadece öğrenci rolü veriyoruz
        };

        context.Users.Add(newUser);
        context.SaveChanges();
        Console.Clear();
        Console.WriteLine("Kayıt başarıyla tamamlandı. Rolünüz: Öğrenci");
    }

    public static void ChangeUserRole()
    {
        using var context = new AppDbContext();

        Console.Write("Rolünü değiştirmek istediğiniz kullanıcının adını giriniz: ");
        var username = Console.ReadLine();

        var user = context.Users.FirstOrDefault(u => u.Userame == username);
        if (user == null)
        {
            Console.WriteLine("Kullanıcı bulunamadı!");
            return;
        }

        Console.WriteLine("Yeni rolü seçin:");
        Console.WriteLine("1. Öğrenci");
        Console.WriteLine("2. Eğitmen");
        Console.WriteLine("3. Admin");

        var input = Console.ReadLine();
        Roles newRole;
        switch (input)
        {
            case "1":
                newRole = Roles.Ogrenci;
                break;
            case "2":
                newRole = Roles.Egitmen;
                break;
            case "3":
                newRole = Roles.Admin;
                break;
            default:
                Console.WriteLine("Geçersiz seçim.");
                return;
        }

        user.Role = newRole;
        context.SaveChanges();
        Console.Clear();
        Console.WriteLine($"{username} kullanıcısının rolü {newRole} olarak değiştirildi.");
    }

    static void ListAllUsers()
    {
        using var context = new AppDbContext();
        var users = context.Users.ToList();

        foreach (var u in users)
        {
            Console.WriteLine($"ID: {u.Id} | Kullanıcı Adı: {u.Userame} | Rol: {u.Role}");
        }
    }

    static void AddStudent()
    {
        using var context = new AppDbContext();

        Console.Write("Öğrencinin Adı: ");
        var firstName = Console.ReadLine();

        Console.Write("Öğrencinin Soyadı: ");
        var lastName = Console.ReadLine();

        var newStudent = new Student
        {
            FirstName = firstName,
            LastName = lastName
        };

        context.Students.Add(newStudent);
        context.SaveChanges();

        Console.Clear();
        Console.WriteLine($"Öğrenci eklendi: {newStudent.FullName}");
    }

    static void AddInstructor()
    {
        using var context = new AppDbContext();
        Console.Write("Eğitmen Adı Soyadı: ");
        var name = Console.ReadLine();

        var instructor = new Instructor { Name = name };
        context.Instructors.Add(instructor);
        context.SaveChanges();
        Console.Clear();
        Console.WriteLine("Eğitmen başarıyla eklendi.");
    }

    static void AddClass()
    {
        using var context = new AppDbContext();
        Console.Write("Sınıf Adı: ");
        var name = Console.ReadLine();

        var cls = new Classroom { Name = name };
        context.Classrooms.Add(cls);
        context.SaveChanges();
        Console.Clear();
        Console.WriteLine("Sınıf başarıyla eklendi.");
    }

    static void AssignStudentToClass()
    {
        using var context = new AppDbContext();

        Console.WriteLine("Öğrenciler:");
        foreach (var s in context.Students.ToList())
            Console.WriteLine($"{s.Id} - {s.FullName}");

        Console.Write("Öğrenci ID'si seçin: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        Console.WriteLine("Sınıflar:");
        foreach (var c in context.Classrooms.ToList())
            Console.WriteLine($"{c.Id} - {c.Name}");

        Console.Write("Sınıf ID'si seçin: ");
        if (!int.TryParse(Console.ReadLine(), out int classId))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        var existing = context.ClassroomStudents.FirstOrDefault(cs => cs.StudentId == studentId && cs.ClassroomId == classId);
        if (existing != null)
        {
            Console.WriteLine("Öğrenci zaten bu sınıfa atanmış.");
            return;
        }

        var assignment = new ClassroomStudent
        {
            StudentId = studentId,
            ClassroomId = classId
        };

        context.ClassroomStudents.Add(assignment);
        context.SaveChanges();
        Console.Clear();
        Console.WriteLine("Öğrenci sınıfa başarıyla atandı.");
    }

    static void AssignInstructorToClass()
    {
        using var context = new AppDbContext();

        Console.WriteLine("Eğitmenler:");
        foreach (var i in context.Instructors.ToList())
            Console.WriteLine($"{i.Id} - {i.Name}");

        Console.Write("Eğitmen ID'si seçin: ");
        if (!int.TryParse(Console.ReadLine(), out int instructorId))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        Console.WriteLine("Sınıflar:");
        foreach (var c in context.Classrooms.ToList())
            Console.WriteLine($"{c.Id} - {c.Name}");

        Console.Write("Sınıf ID'si seçin: ");
        if (!int.TryParse(Console.ReadLine(), out int classId))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        var existing = context.ClassroomInstructors
            .FirstOrDefault(ci => ci.InstructorId == instructorId && ci.ClassroomId == classId);

        if (existing != null)
        {
            Console.WriteLine("Eğitmen zaten bu sınıfa atanmış.");
            return;
        }

        var assignment = new ClassroomInstructor
        {
            InstructorId = instructorId,
            ClassroomId = classId
        };

        context.ClassroomInstructors.Add(assignment);
        context.SaveChanges();
        Console.Clear();
        Console.WriteLine("Eğitmen sınıfa başarıyla atandı.");

    }
    
    static void UpdateStudent()
{
    using var context = new AppDbContext();

    Console.WriteLine("Güncellenecek Öğrenciler:");
    foreach (var s in context.Students.ToList())
        Console.WriteLine($"{s.Id} - {s.FullName}");

    Console.Write("Güncellemek istediğiniz öğrenci ID'sini girin: ");
    if (!int.TryParse(Console.ReadLine(), out int studentId))
    {
        Console.WriteLine("Geçersiz ID");
        return;
    }

    var student = context.Students.FirstOrDefault(s => s.Id == studentId);
    if (student == null)
    {
        Console.WriteLine("Öğrenci bulunamadı.");
        return;
    }

    Console.Write("Yeni Ad: ");
    var newFirstName = Console.ReadLine();

    Console.Write("Yeni Soyad: ");
    var newLastName = Console.ReadLine();

    student.FirstName = newFirstName;
    student.LastName = newLastName;

    context.SaveChanges();
    Console.Clear();
    Console.WriteLine("Öğrenci bilgisi güncellendi.");
}

    static void UpdateInstructor()
    {
        using var context = new AppDbContext();

        Console.WriteLine("Güncellenecek Eğitmenler:");
        foreach (var i in context.Instructors.ToList())
            Console.WriteLine($"{i.Id} - {i.Name}");

        Console.Write("Güncellemek istediğiniz eğitmen ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out int instructorId))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        var instructor = context.Instructors.FirstOrDefault(i => i.Id == instructorId);
        if (instructor == null)
        {
            Console.WriteLine("Eğitmen bulunamadı.");
            return;
        }

        Console.Write("Yeni Ad Soyad: ");
        var newName = Console.ReadLine();

        instructor.Name = newName;
        context.SaveChanges();

        Console.WriteLine("Eğitmen bilgisi güncellendi.");
    }

    static void UpdateClassroom()
    {
        using var context = new AppDbContext();

        Console.WriteLine("Güncellenecek Sınıflar:");
        foreach (var c in context.Classrooms.ToList())
            Console.WriteLine($"{c.Id} - {c.Name}");

        Console.Write("Güncellemek istediğiniz sınıf ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out int classId))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        var classroom = context.Classrooms.FirstOrDefault(c => c.Id == classId);
        if (classroom == null)
        {
            Console.WriteLine("Sınıf bulunamadı.");
            return;
        }

        Console.Write("Yeni Sınıf Adı: ");
        var newName = Console.ReadLine();

        classroom.Name = newName;
        context.SaveChanges();

        Console.Clear();
        Console.WriteLine("Sınıf bilgisi güncellendi.");
    }
    
    static void AddUser(User currentUser)
    {
        using var context = new AppDbContext();

        Console.Write("Yeni kullanıcı adı: ");
        var username = Console.ReadLine();

        Console.Write("Şifre: ");
        var password = Console.ReadLine();

        Roles roleToAssign = Roles.Ogrenci;

        // Sadece adminse rol seçimi göster
        if (currentUser.Role == Roles.Admin)
        {
            Console.WriteLine("Yeni Rolü Seçin:");
            Console.WriteLine("1 - Admin");
            Console.WriteLine("2 - Öğretmen");
            Console.WriteLine("3 - Öğrenci");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    roleToAssign = Roles.Admin;
                    break;
                case "2":
                    roleToAssign = Roles.Egitmen;
                    break;
                default:
                    roleToAssign = Roles.Ogrenci;
                    break;
            }
        }
        else
        {
            Console.WriteLine("Otomatik olarak Öğrenci rolü atanacak.");
        }

        var newUser = new User
        {
            Userame = username,
            Password = Helper.HashPassword(password),
            Role = roleToAssign
        };

        context.Users.Add(newUser);
        context.SaveChanges();
        Console.Clear();
        Console.WriteLine($"Kullanıcı başarıyla oluşturuldu. Rolü: {roleToAssign}");
    }
    
    static void DeleteStudent()
    {
        using var context = new AppDbContext();

        Console.WriteLine("Silinecek Öğrenciler:");
        foreach (var s in context.Students.ToList())
            Console.WriteLine($"{s.Id} - {s.FullName}");

        Console.Write("Silmek istediğiniz öğrenci ID'sini girin: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Geçersiz ID");
            return;
        }

        var student = context.Students.FirstOrDefault(s => s.Id == studentId);
        if (student == null)
        {
            Console.WriteLine("Öğrenci bulunamadı.");
            return;
        }

        context.Students.Remove(student);
        context.SaveChanges();
        Console.Clear();
        Console.WriteLine("Öğrenci başarıyla silindi.");
    }
    
    static void ListStudents()
    {
        using var context = new AppDbContext();

        var students = context.Students.ToList();

        if (!students.Any())
        {
            Console.WriteLine("Kayıtlı öğrenci yok.");
            return;
        }

        Console.WriteLine("Tüm Öğrenciler:");
        foreach (var s in students)
        {
            Console.WriteLine($"ID: {s.Id} | Ad Soyad: {s.FullName}");
        }
    }

}
