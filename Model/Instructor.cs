namespace UserRoles.Model;

public class Instructor
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Bir eğitmen birden fazla sınıfa atanabilir
    public ICollection<ClassroomInstructor> ClassroomInstructors { get; set; }
}

public class ClassroomInstructor
{
    public int ClassroomId { get; set; }
    public Classroom Classroom { get; set; }

    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; }
}