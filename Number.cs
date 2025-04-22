namespace SingleOrDoubleMethod;

public class Number
{
    public static string CheckSingleOrDouble(int number)
    {
        if (number % 2 == 0)
        {
            return "Girdiğiniz sayı çift bir sayıdır.";
        }
        else
        {
            return "Girdiğiniz sayı tek bir sayıdır.";
        }
    }
}