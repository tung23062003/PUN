using System;
using System.Text.RegularExpressions;

public class RegexHelper
{
    /// <summary>
    /// Determines whether the object is empty and returns true if it is empty
    /// </summary>
    /// <typeparam name="T">The type of object to validate</typeparam>
    /// <param name="data">Object to verify</param>        
    public static bool IsNullOrEmpty<T>(T data)
    {
        //If null
        if(data == null)
        {
            return true;
        }

        //if string is empty
        if(data.GetType() == typeof(String))
        {
            if(string.IsNullOrEmpty(data.ToString().Trim()))
            {
                return true;
            }
        }
        //if DBNull
        if(data.GetType() == typeof(DBNull))
        {
            return true;
        }

        //Not null
        return false;
    }
    public static bool IsEmail(string email)
    {
        if(IsNullOrEmpty(email))
        {
            return false;
        }

        email = email.Trim();
        string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
        return IsMatch(email, pattern);
    }
    public static bool IsInt(string number)
    {
        if(IsNullOrEmpty(number))
        {
            return false;
        }
        number = number.Trim();
        string pattern = @"^[0-9]+[0-9]*$";
        return IsMatch(number, pattern);
    }
    public static bool IsNumber(string number)
    {
        if(IsNullOrEmpty(number))
        {
            return false;
        }
        number = number.Trim();
        string pattern = @"^[0-9]+[0-9]*[.]?[0-9]*$";
        return RegexHelper.IsMatch(number, pattern);
    }
    /// <summary>
    /// Convert HTML to TEXT
    /// </summary>
    /// <param name="strHtml"></param>
    /// <returns></returns>
    public static string HtmlToTxt(string strHtml)
    {
        string[] aryReg ={
            @"<script[^>]*?>.*?</script>",
            @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
            @"([\r\n])[\s]+",
            @"&(quot|#34);",
            @"&(amp|#38);",
            @"&(lt|#60);",
            @"&(gt|#62);",
            @"&(nbsp|#160);",
            @"&(iexcl|#161);",
            @"&(cent|#162);",
            @"&(pound|#163);",
            @"&(copy|#169);",
            @"&#(\d+);",
            @"-->",
            @"<!--.*\n"
            };

        string newReg = aryReg[0];
        string strOutput = strHtml;
        for(int i = 0; i < aryReg.Length; i++)
        {
            Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
            strOutput = regex.Replace(strOutput, string.Empty);
        }

        strOutput.Replace("<", "");
        strOutput.Replace(">", "");
        strOutput.Replace("\r\n", "");

        return strOutput;
    }
    public static bool IsMatch(string input, string pattern)
    {
        return IsMatch(input, pattern, RegexOptions.IgnoreCase);
    }

    public static bool IsMatch(string input, string pattern, RegexOptions options)
    {
        return Regex.IsMatch(input, pattern, options);
    }
}