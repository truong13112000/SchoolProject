using System.Security.Cryptography;
using System.Text;

namespace SchoolProject.Controllers
{
    public static class HashCodeAndDecodePassWord
    {
        public static string Key = "abc@@asklas";
        public static string HashCodePassword(string password)
        {

            //MÃ HÓA 

            //if (string.IsNullOrEmpty(password)){
            //    return "";
            //}

            //password += Key;
            //var passWordByte = Encoding.UTF8.GetBytes(password);
            //return Convert.ToBase64String(passWordByte);


            // Mã hóa Md5
             
            //string hash = "NewPassWord2022";
            //byte[] bytes = Encoding.UTF8.GetBytes(password);

            //MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();  
            //TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();

            //tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            //tripleDES.Mode = CipherMode.ECB;

            //ICryptoTransform transform = tripleDES.CreateEncryptor();
            //byte[] result = transform.TransformFinalBlock(bytes, 0, bytes.Length);
            //return Convert.ToBase64String(result);


            // Mã hóa SHD256

            var newpass = SHA256.Create();
            byte[] bytes = newpass.ComputeHash(Encoding.UTF8.GetBytes(password));

            var sb = new StringBuilder();
            for(int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string DeCodePassword(string password)
        {
            //if (string.IsNullOrEmpty(password))
            //{
            //    return "";
            //}

            //var base64EmcodeByte = Convert.FromBase64String(password);
            //var result = Encoding.UTF8.GetString(base64EmcodeByte);
            //result = result.Substring(0, result.Length - base64EmcodeByte.Length);
            //return result;
            string hash = "NewPassWord2022";
            byte[] bytes = Convert.FromBase64String(password);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripleDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripleDES.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(bytes, 0, bytes.Length);

            return UTF8Encoding.UTF8.GetString(result);
        }
    }
}
