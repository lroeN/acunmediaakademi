namespace VatCalculationMethodApp;

public class Helper
{
    public static double CalculateVat(double price, double rate)
    {
        return price * rate / 100;
    }
    
    public static double CalculatePriceVat(double price, double rate)
    {
        return price + CalculateVat(price, rate);
    }
    
    public static (double vat, double total) CalculateBoth(double price, double rate)
    {
        double vat = CalculateVat(price, rate);
        double total = price + vat;
        return (vat, total);
    }
}