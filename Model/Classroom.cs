namespace UserRoles.Model;

public class Classroom
{
    public int Id { get; set; }
    public string Name { get; set; }  // Örnek: "1. Sınıf Full Stack Focus"

    // Bir sınıfta birden fazla öğrenci ve eğitmen olabilir
    public ICollection<ClassroomStudent> ClassroomStudents { get; set; }
    public ICollection<ClassroomInstructor> ClassroomInstructors { get; set; }
}