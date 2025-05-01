namespace HangMan;

public static class Helper
{
    // Askı şeması sabit
    private static readonly string[] Askı = new string[]
    {
        "  _______     ",
        " |/      |    ",
        " |           ", 
        " |           ", 
        " |           ",
        " |           ",
        "_|___         "
    };

    public static List<string> LoadWords(string filePath)
    {
        return new List<string>(File.ReadAllLines(filePath));
    }

    public static string GetRandomWord(List<string> words)
    {
        Random rnd = new Random();
        return words[rnd.Next(words.Count)];
    }

    public static void CizAdam(int yanlisSayisi)
    {
        string kafa      = (yanlisSayisi >= 1) ? "  O  " : "     ";
        string govde     = (yanlisSayisi >= 2) ? "  |  " : "     ";
        string sagKol    = (yanlisSayisi >= 3) ? "/" : " ";
        string solKol    = (yanlisSayisi >= 4) ? "\\" : " ";
        string kollar    = $" {sagKol}|{solKol} ";

        string sagBacak  = (yanlisSayisi >= 5) ? "/" : " ";
        string solBacak  = (yanlisSayisi >= 6) ? "\\" : " ";
        string bacaklar  = $" {sagBacak} {solBacak} ";

        string[] adam = (string[])Askı.Clone();
        adam[2] = $" |     {kafa} ";
        adam[3] = $" |     {(yanlisSayisi >= 2 ? kollar : "     ")}";
        adam[4] = $" |     {(yanlisSayisi >= 5 ? bacaklar : "     ")}";

        foreach (var satir in adam)
            Console.WriteLine(satir);
    }

    public static string FormatKelime(string kelime)
    {
        string gosterim = "";
        foreach (char harf in kelime)
            gosterim += harf + " ";
        return gosterim;
    }
}