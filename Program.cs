//Finding Different Letters

using FindingDifferentLetters;


Console.WriteLine("Birinci kelimeyi giriniz:");
string letter1 = Console.ReadLine();

Console.WriteLine("İkinci kelimeyi giriniz:");
string letter2 = Console.ReadLine();

string differentLetters = Letter.FindingDiffrentLetter(letter1, letter2);

Console.Write("Farklı harfler: ");
for (int i = 0; i < differentLetters.Length; i++)
{
    Console.Write(differentLetters[i] + " ");
}
Console.WriteLine();
Console.ReadKey();
