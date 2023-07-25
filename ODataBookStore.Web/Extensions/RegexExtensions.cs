using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ODataBookStore.Web.Extensions;

public static class RegexExtensions
{

    private static readonly string PhoneRegex = "^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}$";

    public static bool IsValidEmail(this string email)
    {
        try
        {
            var m = new MailAddress(email);
            return true;
        }
        catch (FormatException ex)
        {
            return false;
        }
    }

    public static bool IsValidPhoneNumber(this string phone)
    {
        try
        {
            return Regex.IsMatch(phone, PhoneRegex);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
