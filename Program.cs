using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static string userFile = "users.txt";
    static string dataDirectory = "UserData";

    static void Main(string[] args)
    {
        Directory.CreateDirectory(dataDirectory);
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Giriş Yap\n2. Kayıt Ol\n3. Çıkış");
            Console.Write("Seçiminiz: ");
            var choice = Console.ReadLine();
            if (choice == "1") LoginUser();
            else if (choice == "2") RegisterUser();
            else if (choice == "3") break;
        }
    }

    static void RegisterUser()
    {
        Console.Clear();
        Console.Write("Kullanıcı adı: ");
        var username = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Kullanıcı adı boş olamaz. Bir tuşa basın.");
            Console.ReadKey();
            return;
        }

        Console.Write("Şifre: ");
        var password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Şifre boş olamaz. Bir tuşa basın.");
            Console.ReadKey();
            return;
        }

        if (File.ReadAllLines(userFile).Any(line => line.Split(',')[0] == username))
        {
            Console.WriteLine("Bu kullanıcı adı zaten var. Bir tuşa basın.");
            Console.ReadKey();
            return;
        }

        var encryptedPassword = EncryptionHelper.Encrypt(password);
        File.AppendAllText(userFile, $"{username},{encryptedPassword}{Environment.NewLine}");
        Console.WriteLine("Kayıt başarılı. Bir tuşa basın.");
        Console.ReadKey();
    }

    static void LoginUser()
    {
        Console.Clear();
        Console.Write("Kullanıcı adı: ");
        var username = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Kullanıcı adı boş olamaz. Bir tuşa basın.");
            Console.ReadKey();
            return;
        }

        Console.Write("Şifre: ");
        var password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Şifre boş olamaz. Bir tuşa basın.");
            Console.ReadKey();
            return;
        }

        try
        {
            var user = File.ReadAllLines(userFile).FirstOrDefault(line => line.Split(',')[0] == username);
            if (user == null)
            {
                Console.WriteLine("Kullanıcı bulunamadı. Bir tuşa basın.");
                Console.ReadKey();
                return;
            }

            var encryptedPassword = user.Split(',')[1];
            if (EncryptionHelper.Decrypt(encryptedPassword) != password)
            {
                Console.WriteLine("Hatalı şifre. Bir tuşa basın.");
                Console.ReadKey();
                return;
            }

            MainMenu(username);
        }
        catch
        {
            Console.WriteLine("Giriş yapılırken bir hata oluştu. Bir tuşa basın.");
            Console.ReadKey();
        }
    }

    static void MainMenu(string username)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Yeni Kayıt Ekle\n2. Kayıtları Listele\n3. Kayıt Ara ve Düzenle\n4. Tüm Kayıtları Sil\n5. Çıkış");
            Console.Write("Seçiminiz: ");
            var input = Console.ReadLine();
            if (input == "1") AddNewEntry(username);
            else if (input == "2") ListEntries(username);
            else if (input == "3") SearchAndEditEntry(username);
            else if (input == "4") DeleteAllEntries(username);
            else if (input == "5") break;
        }
    }

    static void AddNewEntry(string username)
    {
        Console.Clear();
        Console.Write("Günlüğe bir isim verin: ");
        var entryTitle = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(entryTitle))
        {
            Console.WriteLine("Başlık boş olamaz. Bir tuşa basın.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Günlük içeriğini girin: ");
        var content = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(content))
        {
            Console.WriteLine("İçerik boş olamaz. Bir tuşa basın.");
            Console.ReadKey();
            return;
        }

        var date = DateTime.Now.ToString("dd-MM-yyyy");
        var entryPath = Path.Combine(dataDirectory, $"{username}_entries.txt");
        var encryptedContent = EncryptionHelper.Encrypt($"{entryTitle}|{content}|{date}|{date}");
        File.AppendAllText(entryPath, encryptedContent + Environment.NewLine);
        Console.WriteLine("Kayıt eklendi. Bir tuşa basın.");
        Console.ReadKey();
    }

    static void ListEntries(string username)
    {
        Console.Clear();
        var entryPath = Path.Combine(dataDirectory, $"{username}_entries.txt");
        if (!File.Exists(entryPath))
        {
            Console.WriteLine("Kayıt bulunamadı.");
        }
        else
        {
            var entries = File.ReadAllLines(entryPath);
            foreach (var line in entries)
            {
                var decrypted = EncryptionHelper.Decrypt(line);
                var parts = decrypted.Split('|');
                Console.WriteLine($"{parts[0]} - {parts[2]} (Güncellendi: {parts[3]})\n{parts[1]}\n-------------");
            }
        }
        Console.WriteLine("Bir tuşa basın.");
        Console.ReadKey();
    }

    static void SearchAndEditEntry(string username)
    {
        Console.Clear();
        var entryPath = Path.Combine(dataDirectory, $"{username}_entries.txt");
        if (!File.Exists(entryPath))
        {
            Console.WriteLine("Kayıt bulunamadı.");
            Console.ReadKey();
            return;
        }

        var entries = File.ReadAllLines(entryPath).ToList();
        Console.Write("Aranacak kayıt tarihi (dd-MM-yyyy): ");
        var searchDate = Console.ReadLine();
        if (!DateTime.TryParseExact(searchDate, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out var _))
        {
            Console.WriteLine("Geçersiz tarih formatı. Lütfen doğru formatta bir tarih girin. (dd-MM-yyyy)");
            Console.ReadKey();
            return;
        }

        bool found = false;

        for (int i = 0; i < entries.Count; i++)
        {
            var decrypted = EncryptionHelper.Decrypt(entries[i]);
            var parts = decrypted.Split('|');
            if (parts[2].StartsWith(searchDate))
            {
                Console.WriteLine($"Başlık: {parts[0]}\nTarih: {parts[2]}\nİçerik: {parts[1]}\n-------------");
                Console.WriteLine("Bu kaydı düzenlemek ister misiniz? (e/h): ");
                if (Console.ReadLine().ToLower() == "e")
                {
                    Console.WriteLine("Yeni içerik eklemek için yazın (mevcut içerik: {0}): ", parts[1]);
                    var newContent = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(newContent))
                    {
                        Console.WriteLine("İçerik boş olamaz. Bir tuşa basın.");
                        Console.ReadKey();
                        return;
                    }
                    
                    parts[1] += " " + newContent;
                    
                    var updated = $"{parts[0]}|{parts[1]}|{parts[2]}|{DateTime.Now:dd-MM-yyyy}";
                    entries[i] = EncryptionHelper.Encrypt(updated);
                    File.WriteAllLines(entryPath, entries);

                    Console.WriteLine("Kayıt güncellendi.");
                    Console.ReadKey();
                    found = true;
                    break;
                }
            }
        }

        if (!found)
        {
            Console.WriteLine("Eşleşen kayıt bulunamadı. Bir tuşa basın.");
            Console.ReadKey();
        }
    }

    static void DeleteAllEntries(string username)
    {
        Console.Clear();
        Console.Write("Tüm kayıtları silmek istediğinize emin misiniz? (e/h): ");
        if (Console.ReadLine().ToLower() == "e")
        {
            var entryPath = Path.Combine(dataDirectory, $"{username}_entries.txt");
            if (File.Exists(entryPath)) File.Delete(entryPath);
            Console.WriteLine("Tüm kayıtlar silindi.");
        }
        else
        {
            Console.WriteLine("İşlem iptal edildi.");
        }
        Console.ReadKey();
    }
}

public static class EncryptionHelper
{
    private static readonly string key = "GunlukSifre12345";

    public static string Encrypt(string input)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        using (var md5 = MD5.Create()) keyBytes = md5.ComputeHash(keyBytes);
        using (var tripleDES = TripleDES.Create())
        {
            tripleDES.Key = keyBytes;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            using (var encryptor = tripleDES.CreateEncryptor())
            {
                byte[] result = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                return Convert.ToBase64String(result);
            }
        }
    }

    public static string Decrypt(string input)
    {
        byte[] inputBytes = Convert.FromBase64String(input);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        using (var md5 = MD5.Create()) keyBytes = md5.ComputeHash(keyBytes);
        using (var tripleDES = TripleDES.Create())
        {
            tripleDES.Key = keyBytes;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            using (var decryptor = tripleDES.CreateDecryptor())
            {
                byte[] result = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                return Encoding.UTF8.GetString(result);
            }
        }
    }
}