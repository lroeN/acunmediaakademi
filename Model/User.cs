namespace UserRoles.Model;



public class User
{
    public int Id { get; set; }
    public string Userame { get; set; }
    public string Password { get; set; }
    public Roles Role { get; set; } = Roles.Ogrenci;
    public  DateTime CreatedAt { get; set; } = DateTime.Now;
    //myUser.Role==Roles.Admin kontrolünün veri tabanına eklenmesi
}

