using TeacherAndStudentManagementSystem.Models;

static class Program
{
    static List<Teachers> teachers;
    static List<Student> students;
    static List<Classroom> classrooms;

    static void Main()
    {
        teachers = DataManager.LoadTeachers();
        classrooms = DataManager.LoadClassrooms();
        students = DataManager.LoadStudents(classrooms);

        if (!Login()) // Kullanıcı Adı: OrhanEkici Şifre: zorbisey
        {
            Console.WriteLine("Giriş başarısız. Programdan çıkılıyor.");
            return;
        }

        MainMenu();
    }

    static bool Login()
    {
        Console.Clear();
        Console.WriteLine(" Öğretmen Girişi");
        Console.Write("Kullanıcı Adı: ");
        string username = Console.ReadLine()!;
        Console.Write("Şifre: ");
        string password = Console.ReadLine()!;

        foreach (var t in teachers)
        {
            if (t.Username == username && t.Password == password)
                return true;
        }

        Console.WriteLine(" Hatalı kullanıcı adı veya şifre.");
        return false;
    }

    static void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Ana Menü ===");
            Console.WriteLine("1 - Öğrenci Yönetimi");
            Console.WriteLine("2 - Öğretmen Yönetimi");
            Console.WriteLine("3 - Sınıf Yönetimi");
            Console.WriteLine("0 - Çıkış");
            Console.Write("Seçiminiz: ");
            string secim = Console.ReadLine()!;

            switch (secim)
            {
                case "1": StudentMenu(); break;
                case "2": TeacherMenu(); break;
                case "3": ClassroomMenu(); break;
                case "0": return;
                default: Console.WriteLine("⚠ Geçersiz seçim."); break;
            }
        }
    }
    

    static void StudentMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(" Öğrenci Yönetimi");
            Console.WriteLine("1 - Listele");
            Console.WriteLine("2 - Ekle");
            Console.WriteLine("3 - Sil");
            Console.WriteLine("0 - Geri Dön");
            Console.Write("Seçiminiz: ");
            string secim = Console.ReadLine()!;

            switch (secim)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine(" Öğrenci Listesi:");
                    foreach (var s in students)
                        Console.WriteLine(s);
                    Console.ReadKey();
                    break;

                case "2":
                    AddStudent();
                    break;

                case "3":
                    DeleteStudent();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine(" Geçersiz seçim.");
                    break;
            }
        }
    }

    static void AddStudent()
    {
        Console.Clear();
        Console.WriteLine(" Öğrenci Ekle");
        Console.Write("Ad: ");
        string fname = Console.ReadLine()!;
        Console.Write("Soyad: ");
        string lname = Console.ReadLine()!;

        Console.Write("Öğrenci No (4 basamak): ");
        if (!int.TryParse(Console.ReadLine(), out int number) || number < 0 || number > 9999)
        {
            Console.WriteLine(" Öğrenci numarası 0-9999 arasında olmalıdır.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Sınıf seçin:");
        for (int i = 0; i < classrooms.Count; i++)
            Console.WriteLine($"{i + 1} - {classrooms[i].Name}");

        if (!int.TryParse(Console.ReadLine(), out int classIndex) || classIndex < 1 || classIndex > classrooms.Count)
        {
            Console.WriteLine(" Geçersiz sınıf seçimi.");
            Console.ReadKey();
            return;
        }

        var selectedClass = classrooms[classIndex - 1];
        students.Add(new Student(fname, lname, number, selectedClass));
        DataManager.SaveStudents(students);
        Console.WriteLine(" Öğrenci eklendi.");
        Console.ReadKey();
    }

    static void DeleteStudent()
    {
        Console.Clear();
        Console.WriteLine(" Öğrenci Sil");
        Console.Write("Silinecek öğrenci no: ");
        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine(" Geçersiz numara.");
            Console.ReadKey();
            return;
        }

        var student = students.Find(s => s.StudentNumber == number);
        if (student == null)
        {
            Console.WriteLine(" Öğrenci bulunamadı.");
        }
        else
        {
            students.Remove(student);
            DataManager.SaveStudents(students);
            Console.WriteLine(" Öğrenci silindi.");
        }
        Console.ReadKey();
    }

    static void TeacherMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(" Öğretmen Yönetimi");
            Console.WriteLine("1 - Listele");
            Console.WriteLine("2 - Ekle");
            Console.WriteLine("3 - Sil");
            Console.WriteLine("0 - Geri Dön");
            Console.Write("Seçiminiz: ");
            string secim = Console.ReadLine()!;

            switch (secim)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine(" Öğretmen Listesi:");
                    foreach (var t in teachers)
                        Console.WriteLine(t.Username);
                    Console.ReadKey();
                    break;

                case "2":
                    Console.Clear();
                    Console.Write("Yeni kullanıcı adı: ");
                    string uname = Console.ReadLine()!;
                    Console.Write("Şifre: ");
                    string pwd = Console.ReadLine()!;
                    teachers.Add(new Teachers(uname, pwd));
                    DataManager.SaveTeachers(teachers);
                    Console.WriteLine(" Öğretmen eklendi.");
                    Console.ReadKey();
                    break;

                case "3":
                    Console.Clear();
                    Console.Write("Silinecek kullanıcı adı: ");
                    string del = Console.ReadLine()!;
                    var teacher = teachers.Find(t => t.Username == del);
                    if (teacher != null)
                    {
                        teachers.Remove(teacher);
                        DataManager.SaveTeachers(teachers);
                        Console.WriteLine(" Öğretmen silindi.");
                    }
                    else
                    {
                        Console.WriteLine("Öğretmen bulunamadı.");
                    }
                    Console.ReadKey();
                    break;

                case "0": return;
                default: break;
            }
        }
    }

    static void ClassroomMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(" Sınıf Yönetimi");
            Console.WriteLine("1 - Listele");
            Console.WriteLine("2 - Ekle");
            Console.WriteLine("3 - Sil");
            Console.WriteLine("0 - Geri Dön");
            Console.Write("Seçiminiz: ");
            string secim = Console.ReadLine()!;

            switch (secim)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine(" Sınıf Listesi:");

                    foreach (var c in classrooms)
                    {
                        var studentCount = students.Count(s => s.Classroom.Name == c.Name);
                        Console.WriteLine($"{c.Name} Sınıfı: {studentCount} Öğrenci");
                    }
                    Console.ReadKey();
                    break;

                case "2":
                    Console.Clear();
                    Console.Write("Yeni sınıf adı: ");
                    string cname = Console.ReadLine()!;
                    classrooms.Add(new Classroom(cname));
                    DataManager.SaveClassrooms(classrooms);
                    Console.WriteLine(" Sınıf eklendi.");
                    Console.ReadKey();
                    break;

                case "3":
                    Console.Clear();
                    Console.Write("Silinecek sınıf adı: ");
                    string del = Console.ReadLine()!;
                    var classroom = classrooms.Find(c => c.Name == del);
                    if (classroom != null)
                    {
                        classrooms.Remove(classroom);
                        DataManager.SaveClassrooms(classrooms);
                        Console.WriteLine(" Sınıf silindi.");
                    }
                    else
                    {
                        Console.WriteLine("Sınıf bulunamadı.");
                    }
                    Console.ReadKey();
                    break;

                case "0": return;
                default: break;
            }
        }
    }
}
