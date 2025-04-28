namespace ClassHelperExamples;

public static class StudentHelper
{
    public static void ListStudents(List<Student> students)
    {
        Console.Clear();
        Console.WriteLine("ÖĞRENCİ LİSTESİ\n");

        if (students.Count == 0)
        {
            Console.WriteLine("Henüz kayıtlı öğrenci yok.");
            return;
        }

        for (int i = 0; i < students.Count; i++)
        {
            var student = students[i];
            Console.WriteLine($"{i + 1}. {student.FirstName} {student.LastName} | {student.Gender} | {student.BirthDate.ToString("dd.MM.yyyy")} | {student.GetAge()} yaşında");
        }
    }

    public static void AddStudent(List<Student> students)
    {
        Console.Clear();
        Console.WriteLine("ÖĞRENCİ EKLEME\n");

        Console.Write("Adı: ");
        var firstName = Console.ReadLine() ?? "";

        Console.Write("Soyadı: ");
        var lastName = Console.ReadLine() ?? "";

        Console.Write("Cinsiyeti: ");
        var gender = Console.ReadLine() ?? "";

        Console.Write("Doğum Tarihi (yyyy-MM-dd formatında olmasına dikkat ediniz!): ");
        var birthDateInput = Console.ReadLine() ?? "";

        if (!DateOnly.TryParse(birthDateInput, out var birthDate))
        {
            Console.WriteLine("Hatalı doğum tarihi formatı!");
            return;
        }

        var newStudent = new Student
        {
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            BirthDate = birthDate
        };

        students.Add(newStudent);
        Console.WriteLine("\nÖğrenci başarıyla eklendi!");
    }

    public static void DeleteStudent(List<Student> students)
    {
        Console.Clear();
        Console.WriteLine("ÖĞRENCİ SİLME\n");

        if (students.Count == 0)
        {
            Console.WriteLine("Silinecek öğrenci yok.");
            return;
        }

        for (int i = 0; i < students.Count; i++)
        {
            var student = students[i];
            Console.WriteLine($"{i + 1}. {student.FirstName} {student.LastName}");
        }

        Console.Write("\nSilmek istediğin öğrencinin numarasını gir: ");
        var input = Console.ReadLine() ?? "";

        if (!int.TryParse(input, out int index) || index < 1 || index > students.Count)
        {
            Console.WriteLine("Geçersiz seçim!");
            return;
        }

        var removedStudent = students[index - 1];
        students.RemoveAt(index - 1);
        Console.WriteLine($"\n{removedStudent.FirstName} {removedStudent.LastName} başarıyla silindi!");
    }
}
