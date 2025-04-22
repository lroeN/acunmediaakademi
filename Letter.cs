namespace FindingDifferentLetters;

public class Letter
{
    public static string FindingDiffrentLetter(string letter1, string letter2)
    {
        string farkli = "";
        string l1 = letter1.ToLower();
        string l2 = letter2.ToLower();

        for (int i = 0; i < l1.Length; i++)
        {
            string harf = l1[i].ToString();
            if (!VarMi(l2, harf) && !VarMi(farkli, harf))
            {
                farkli += harf;
            }
        }

        for (int i = 0; i < l2.Length; i++)
        {
            string harf = l2[i].ToString();
            if (!VarMi(l1, harf) && !VarMi(farkli, harf))
            {
                farkli += harf;
            }
        }

        return farkli;
    }

    static bool VarMi(string letter, string harf)
    {
        for (int i = 0; i < letter.Length; i++)
        {
            if (letter[i].ToString() == harf)
            {
                return true;
            }
        }
        return false;
    }
}