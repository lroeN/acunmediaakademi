namespace TransliterateWordsApp;

public class transliterate
{
    public static string ReverseWord(string word)
    {
        string reversedWord = "";

        for (int i = word.Length - 1; i >= 0; i--)
        {
            reversedWord += word[i];
        }
        return reversedWord;
    }
}