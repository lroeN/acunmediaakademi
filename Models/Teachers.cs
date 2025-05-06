namespace TeacherAndStudentManagementSystem.Models;

public class Teachers
{
    public string Username { get; set; }
    public string Password { get; set; }

    public Teachers(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
