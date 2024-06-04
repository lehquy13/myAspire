namespace LibraryManagement.Domain.Shared.Utilities;

public static class Helper
{
    public static T ToEnum<T>(this string value) where T : struct
    {
        var defaultValue = default(T);
        try   
        {   
            T res = (T)Enum.Parse(typeof(T), value, true);

            if (!Enum.IsDefined(typeof(T), res))
            {
                return defaultValue;
            }   
            
            return res;   
        }   
        catch   
        {   
            return defaultValue;   
        }   
    }
    
    public static T ToEnum<T>(this string value, T defaultValue) where T : struct
    {
        try   
        {   
            T res = (T)Enum.Parse(typeof(T), value, true);

            if (!Enum.IsDefined(typeof(T), res))
            {
                return defaultValue;
            }   
            
            return res;   
        }   
        catch   
        {   
            return defaultValue;   
        }   
    }

    public static bool CheckValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
}