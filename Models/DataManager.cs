using TeacherAndStudentManagementSystem.Models;

public static class DataManager
{
    public static string TeacherFile = "teachers.txt";
    public static string StudentFile = "students.txt";
    public static string ClassroomFile = "classrooms.txt";

    public static List<Teachers> LoadTeachers()
    {
        var teachers = new List<Teachers>();
        if (!File.Exists(TeacherFile)) return teachers;

        foreach (var line in File.ReadAllLines(TeacherFile))
        {
            var parts = line.Split(',');
            if (parts.Length == 2)
                teachers.Add(new Teachers(parts[0], parts[1]));
        }

        return teachers;
    }

    public static void SaveTeachers(List<Teachers> teachers)
    {
        var lines = new List<string>();
        foreach (var t in teachers)
        {
            lines.Add($"{t.Username},{t.Password}");
        }
        File.WriteAllLines(TeacherFile, lines);
    }

    public static List<Classroom> LoadClassrooms()
    {
        var classrooms = new List<Classroom>();
        if (!File.Exists(ClassroomFile)) return classrooms;

        foreach (var line in File.ReadAllLines(ClassroomFile))
        {
            if (!string.IsNullOrWhiteSpace(line))
                classrooms.Add(new Classroom(line.Trim()));
        }

        return classrooms;
    }

    public static void SaveClassrooms(List<Classroom> classrooms)
    {
        var lines = new List<string>();
        foreach (var c in classrooms)
        {
            lines.Add(c.Name);
        }
        File.WriteAllLines(ClassroomFile, lines);
    }

    public static List<Student> LoadStudents(List<Classroom> classrooms)
    {
        var students = new List<Student>();
        if (!File.Exists(StudentFile)) return students;

        foreach (var line in File.ReadAllLines(StudentFile))
        {
            var parts = line.Split(',');
            if (parts.Length == 4)
            {
                string fname = parts[0];
                string lname = parts[1];
                if (!int.TryParse(parts[2], out int number)) continue;
                string className = parts[3];

                var cls = classrooms.Find(c => c.Name == className);
                if (cls != null)
                {
                    students.Add(new Student(fname, lname, number, cls));
                }
            }
        }

        return students;
    }

    public static void SaveStudents(List<Student> students)
    {
        var lines = new List<string>();
        foreach (var s in students)
        {
            lines.Add($"{s.Firstname},{s.Lastname},{s.StudentNumber},{s.Classroom.Name}");
        }
        File.WriteAllLines(StudentFile, lines);
    }
}