using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Linq;

namespace Craft.Net.Client
{
    public class LastLogin
    {
        private static readonly byte[] LastLoginSalt = new byte[] { 0x0c, 0x9d, 0x4a, 0xe4, 0x1e, 0x83, 0x15, 0xfc };
        private const string LastLoginPassword = "passwordfile";

        public static string LastLoginFile
        {
            get
            {
                return Path.Combine(DotMinecraft.GetDotMinecraftPath(), "lastlogin");
            }
        }

        public static LastLogin GetLastLogin()
        {
            return GetLastLogin(LastLoginFile);
        }

        public static LastLogin GetLastLogin(string lastLoginFile)
        {
            try
            {
                byte[] encryptedLogin = File.ReadAllBytes(lastLoginFile);
                PKCSKeyGenerator crypto = new PKCSKeyGenerator(LastLoginPassword, LastLoginSalt, 5, 1);
                ICryptoTransform cryptoTransform = crypto.Decryptor;
                byte[] decrypted = cryptoTransform.TransformFinalBlock(encryptedLogin, 0, encryptedLogin.Length);
                short userLength = IPAddress.HostToNetworkOrder(BitConverter.ToInt16(decrypted, 0));
                byte[] user = decrypted.Skip(2).Take(userLength).ToArray();
                byte[] password = decrypted.Skip(4 + userLength).ToArray();
                LastLogin result = new LastLogin();
                result.Username = System.Text.Encoding.UTF8.GetString(user);
                result.Password = System.Text.Encoding.UTF8.GetString(password);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static void SetLastLogin(LastLogin login)
        {
            byte[] decrypted = BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)login.Username.Length))
                .Concat(System.Text.Encoding.UTF8.GetBytes(login.Username))
                .Concat(BitConverter.GetBytes(IPAddress.NetworkToHostOrder((short)login.Password.Length)))
                .Concat(System.Text.Encoding.UTF8.GetBytes(login.Password)).ToArray();

            PKCSKeyGenerator crypto = new PKCSKeyGenerator(LastLoginPassword, LastLoginSalt, 5, 1);
            ICryptoTransform cryptoTransform = crypto.Encryptor;
            byte[] encrypted = cryptoTransform.TransformFinalBlock(decrypted, 0, decrypted.Length);
            if (File.Exists(LastLoginFile))
                File.Delete(LastLoginFile);
            using (Stream stream = File.Create(LastLoginFile))
                stream.Write(encrypted, 0, encrypted.Length);
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}