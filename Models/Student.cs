namespace TeacherAndStudentManagementSystem.Models;

public class Student
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public int StudentNumber { get; set; }
    public Classroom Classroom { get; set; }

    public Student(string firstname, string lastname, int studentNumber, Classroom classroom)
    {
        Firstname = firstname;
        Lastname = lastname;
        StudentNumber = studentNumber;
        Classroom = classroom;
    }

    public override string ToString()
    {
        return $"{StudentNumber} - {Firstname} - {Lastname} ({Classroom})";
    }
}