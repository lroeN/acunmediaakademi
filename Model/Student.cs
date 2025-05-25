namespace UserRoles.Model;

public class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";

    public ICollection<ClassroomStudent> ClassroomStudents { get; set; }
}

public class ClassroomStudent
{
    public int ClassroomId { get; set; }
    public Classroom Classroom { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }
}
