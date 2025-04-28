using ClassHelperExamples;

var me = new Student
{
    FirstName = "Orhan",
    LastName = "Ekici",
    BirthDate = new DateOnly(1989, 3, 17),
    Gender = "Erkek"
};

var students = new List<Student> { me };

while (true)
{
    Console.Clear();
    Console.WriteLine("Öğrenci Yönetim Sistemi\n".ToUpper());
    var inputSelection = Helper.AskOption("Yapmak istediğin işlemi seç", ["Listele", "Ekle", "Sil", "Çıkış"]);

    if (inputSelection == 1)
    {
        StudentHelper.ListStudents(students);
    }
    else if (inputSelection == 2)
    {
        StudentHelper.AddStudent(students);
    }
    else if (inputSelection == 3)
    {
        StudentHelper.DeleteStudent(students);
    }
    else
    {
        Console.Clear();
        Console.WriteLine("Hoşçakalın...");
        Thread.Sleep(1000);
        break;
    }

    Console.WriteLine("\nMenüye dönmek için bir tuşa basın.");
    Console.ReadKey(true);
}