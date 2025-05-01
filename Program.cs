using HangMan;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        bool tekrarOyna = true;
        int toplamIpucuSayisi = 0; // Toplam ipucu hakkı

        while (tekrarOyna)
        {
            Console.Clear();
            Console.WriteLine("Adam Asmaca Oyununa Hoş Geldiniz!\n");

            var words = Helper.LoadWords("Words.txt");
            string kelime = Helper.GetRandomWord(words).ToUpper();
            string gizliKelime = new string('_', kelime.Length);
            string kullanilanHarfler = "";
            int yanlisSayisi = 0;
            int ipucuSayisi = toplamIpucuSayisi; // Yeni oyun başladığında önceki ipuçları aktarılacak

            while (yanlisSayisi < 6 && gizliKelime.Contains('_'))
            {
                Console.Clear();
                Helper.CizAdam(yanlisSayisi);
                Console.WriteLine($"\nKelime: {Helper.FormatKelime(gizliKelime)}");
                Console.WriteLine("Kullanılan Harfler: " + kullanilanHarfler);
                Console.WriteLine($"İpucu Hakkınız: {ipucuSayisi}");

                Console.Write("\nBir harf tahmin edin (IPUCU: '1', ÇIKIŞ: ESC): ");
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                    return;

                if (key.KeyChar == '1')
                {
                    if (ipucuSayisi > 0)
                    {
                        // Henüz çözülmemiş harflerden birini ipucu olarak ver
                        char ipucuHarf = kelime.First(c => !gizliKelime.Contains(c));
                        Console.WriteLine($"\nİpucu: Kelimede '{ipucuHarf}' harfi var.");
                        ipucuSayisi--;
                        Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Hiç ipucu hakkınız yok.");
                        Console.ReadKey();
                        continue;
                    }
                }

                char tahmin = Char.ToUpper(key.KeyChar);

                if (!char.IsLetter(tahmin))
                {
                    Console.WriteLine("Lütfen sadece harf giriniz.");
                    Console.ReadKey();
                    continue;
                }

                if (kullanilanHarfler.Contains(tahmin))
                {
                    Console.WriteLine("Bu harfi zaten kullandınız.");
                    Console.ReadKey();
                    continue;
                }

                kullanilanHarfler += tahmin + " ";

                if (kelime.Contains(tahmin))
                {
                    char[] guncelle = gizliKelime.ToCharArray();
                    bool harfBulundu = false;
                    for (int i = 0; i < kelime.Length; i++)
                    {
                        if (kelime[i] == tahmin)
                        {
                            guncelle[i] = tahmin;
                            harfBulundu = true;
                        }
                    }
                    gizliKelime = new string(guncelle);

                    if (harfBulundu)
                    {
                        Console.WriteLine("Doğru tahmin!");
                        ipucuSayisi++; // Her doğru harf için ipucu ekleniyor
                    }
                }
                else
                {
                    yanlisSayisi++;
                    Console.WriteLine($"Yanlış tahmin! Kalan hakkınız: {6 - yanlisSayisi}");
                }

                Console.ReadKey();
            }

            // Eğer kelime doğru çözüldüyse, ipucu hakkını biriktir
            if (!gizliKelime.Contains('_'))
            {
                toplamIpucuSayisi += ipucuSayisi; // Yeni kelimede kullanılmak üzere ipucu hakkı ekleniyor
                Console.Clear();
                Helper.CizAdam(yanlisSayisi);
                Console.WriteLine($"\nTebrikler! Kelimeyi doğru bildiniz: {kelime}");
            }
            else
            {
                Console.Clear();
                Helper.CizAdam(yanlisSayisi);
                Console.WriteLine("\nAdamı astın! Oyun bitti.");
                Console.WriteLine("Doğru kelime: " + kelime);
            }

            // Yeni oyunda birikmiş ipucu hakkını aktar
            Console.Write("\nTekrar oynamak ister misiniz? (E/H): ");
            string cevap = Console.ReadLine().ToUpper();
            tekrarOyna = (cevap == "E");
        }
    }
}
