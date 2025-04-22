using SingleOrDoubleMethod;

Console.Write("Bir sayı giriniz: ");
string input = Console.ReadLine();

if (!int.TryParse(input, out int number))
{
    Console.WriteLine("Lütfen geçerli bir sayı giriniz! ");
    return;
}

string result = Number.CheckSingleOrDouble(number);
Console.WriteLine(result);
Console.WriteLine("Çıkmak için herhangi bir tuşa basmanız yeterlidir.");
Console.ReadKey();
