using VatCalculationMethodApp;

class WatApp
{
    static int Read(string message)
    {
        Console.Write(message);
        return int.Parse(Console.ReadLine() ?? "0");
    }

    static string GetResultText(double price, double rate)
    {
        var (vat, total) = Helper.CalculateBoth(price, rate);
        return $"KDV: {vat} TL\nToplam: {total} TL";
    }

    static int Main()
    {
        double price = Read("Ürün fiyatı: ");
        double rate = Read("KDV oranı (%): ");

        string result = GetResultText(price, rate);
        Console.WriteLine(result);

        return 0; 
    }
}


// Hocama Not: :))
// Şu yüzden main kullandım sade bir şekilde de yazabilirdim ama "static int Main()" kullanarak kodumu kısaltmış oldum.
// Öbür türlü kod uzun olacaktı. Sürekli hepsini belirtecektim.
// Eğer yazmasaydım ve uzun halini yazsaydım zaten aslında maine yazıyorum sadece gözükmüyordu ama böyle daha kısa oldu.