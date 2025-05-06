namespace TeacherAndStudentManagementSystem.Models;

public class Classroom
{
    public string Name { get; set; }

    public Classroom(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}