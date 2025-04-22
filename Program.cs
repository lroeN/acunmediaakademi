using MilitaryStatusInquirer;

Console.Write("Yaşınızı giriniz: ");
string input = Console.ReadLine() ?? "";

if (!int.TryParse(input, out int age))
{
    Console.WriteLine("Lütfen geçerli bir yaş girin!");
    return;
}

Console.Write("Cinsiyetizi giriniz (erkek/kadın):");
string gender = Console.ReadLine() ?? "";



string result = MilitaryStatus.CheckMilitaryStatus(age, gender);
Console.WriteLine(result);
Console.ReadKey();
