namespace MilitaryStatusInquirer;

public class MilitaryStatus
{
    public static string CheckMilitaryStatus(int age, string gender)
    {
        if (gender.ToLower() == "kadın")
        {
            return "Kadınlar için askerlik zorunlu değildir.";
        }

        if (age >= 20)
        {
            return "Askerlik yaşı gelmiştir.";
        }

        return "Askerlik yaşı henüz gelmemiştir.";
    }
}