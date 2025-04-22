Console.WriteLine("Bu bir toplama makinesidir. Sayıları toplar.");
Console.WriteLine("Sayıları girmeye başlayın. İşlemi bitirmek için 't' tuşuna basınız.");

double toplam = 0;
string input;

while ((input = Console.ReadLine()) != "t")
{
    if (double.TryParse(input, out double sayi)) toplam += sayi;
    else Console.WriteLine("Geçersiz giriş.");
}

Console.WriteLine($"Toplam: {toplam}");