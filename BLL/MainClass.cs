using System.Security.Cryptography;
using System.Text;
public class MainClass
{
    public enum enSaveMode { AddNew = 0, Update = 1 };
    public static string ComputeHash(string input)
{
    
    using (SHA256 sha256Hash = SHA256.Create())
    {
         
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}

}

