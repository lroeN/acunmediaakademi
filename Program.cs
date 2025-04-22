using PriceDiscountCalculationMethod;

Console.WriteLine("Ürün fiyatını giriniz. (TL): ");
string priceInput = Console.ReadLine() ?? "";

if (!double.TryParse(priceInput, out double price))
{
    Console.WriteLine("Geçerli bir fiyat giriniz.");
    return;
}

Console.Write("İndirim oranını giriniz. (%) :");
string discountInput = Console.ReadLine() ?? "";

if (!double.TryParse(discountInput, out double discount))
{
    Console.WriteLine("Geçerli bir indirim oranı giriniz!");
    return;
}

string result = PriceDiscount.CalculateDiscounted(price, discount);
Console.WriteLine(result);
Console.WriteLine("Çıkmak için herhangi bir tuşa basmanız yeterlidir.");
Console.ReadKey();
