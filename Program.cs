using TransliterateWordsApp;

Console.Write("Bir kelime giriniz: ");
string word = Console.ReadLine() ?? "";

string reversedWord = transliterate.ReverseWord(word);
Console.WriteLine($"Ters çevrilmiş kelime: {reversedWord}");
Console.WriteLine("Çıkmak için herhangi bir tuşa basmanız yeterlidir.");
Console.ReadKey();