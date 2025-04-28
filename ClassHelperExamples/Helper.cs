namespace ClassHelperExamples;

public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Gender { get; set; }

    public int GetAge()
    {
        return CalculateAge();
    }

    private int CalculateAge()
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - BirthDate.Year;
        if (today < BirthDate.AddYears(age))
        {
            age--;
        }
        return age;
    }
}

public static class Helper
{
    public static int AskOption(string title, string[] options)
    {
        Console.WriteLine(title);
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }

        Console.Write("\nSeçiminiz: ");
        var input = Console.ReadLine() ?? "";

        if (int.TryParse(input, out int selected) && selected >= 1 && selected <= options.Length)
        {
            return selected;
        }

        Console.WriteLine("Geçersiz seçim! Devam etmek için bir tuşa basın...");
        Console.ReadKey();
        return AskOption(title, options);
    }
}