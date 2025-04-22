namespace PriceDiscountCalculationMethod;

public class PriceDiscount
{
    public static string CalculateDiscounted(double price, double discount)
    {
        if (discount < 0 || discount > 100)
        {
            return "Geçersiz indirim oranı! (0-100 arasında olmalı)";
        }

        double discountAmount = price * discount / 100;
        double discountedPrice = price - discountAmount;

        return
            $"Orijinal Fiyat: {price:0} TL\n" +
            $"İndirim Oranı: %{discount:0}\n" +
            $"İndirim Tutarı: {discountAmount:0} TL\n" +
            $"İndirimli Fiyat: {discountedPrice:0} TL";
    }
}