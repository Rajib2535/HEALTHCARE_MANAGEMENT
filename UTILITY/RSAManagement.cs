using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PemUtils;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace UTILITY
{
    public static class RSAManagement
    {


        public static void GenerateRsaKey(string version, IConfiguration _configuration, IHostEnvironment _env, ILogger _logger)
        {
            var rsa = RSA.Create();

            rsa.KeySize = _configuration.GetValue<int>("RSA:KeySize");
            var directory = Path.Combine(_env.ContentRootPath, _configuration.GetValue<string>("RSA:KeyLocation"), version);
            bool exists = Directory.Exists(directory);
            if (!exists)
                Directory.CreateDirectory(directory);
            var genpublicKey = Path.Combine(directory, @"publickey.pem");
            var genprivatekey = Path.Combine(directory, @"privatekey.pem");
            using (var fs = File.Create(genprivatekey))
            {
                using (var pem = new PemWriter(fs))
                {
                    pem.WritePrivateKey(rsa);
                }
            }

            using (var fs = File.Create(genpublicKey))
            {
                using (var pem = new PemWriter(fs))
                {
                    pem.WritePublicKey(rsa);
                }
            }
        }


        public static string EncryptData(string plainText, string version, IConfiguration _configuration, IHostEnvironment _env, ILogger _logger)
        {
            //string logPath = @"C:\Log\EncryptData.txt";
            //FileLogger.FileCreation("Step: EncryptData start "+ plainText, logPath);
            using (var rsa = GetRSACryptoProvider(version, true, _configuration, _env, _logger))
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                var cipherTextBytes = rsa.Encrypt(plainTextBytes, RSAEncryptionPadding.Pkcs1);
                var cipherText = Convert.ToBase64String(cipherTextBytes);
                //FileLogger.FileCreation("Step: EncryptData success " + plainText, logPath);
                return cipherText;
            }
        }


        public static string Decrypt(string plainText, string version, IConfiguration _configuration, IHostEnvironment _env, ILogger _logger)
        {
            using (var rsa = GetRSACryptoProvider(version, false, _configuration, _env, _logger))
            {

                var cipherTextBytes = Convert.FromBase64String(plainText);
                var plainTextBytes = rsa.Decrypt(cipherTextBytes, RSAEncryptionPadding.Pkcs1);
                plainText = Encoding.UTF8.GetString(plainTextBytes);
            }

            return plainText;
        }

        private static RSA GetRSACryptoProvider(string version, bool isPulicKey, IConfiguration _configuration, IHostEnvironment _env, ILogger _logger)
        {
            //string logPath = @"C:\Log\EncryptData.txt";
            _logger.Information("Step: GetRSACryptoProvider isPulicKey " + isPulicKey);
            _logger.Information($"ContentRootPath: {_env.ContentRootPath}, KeyLocation: {_configuration.GetValue<string>("RSA:KeyLocation")}, Version: {version}");

            var rsa = RSA.Create();

            try
            {

                var directory = Path.Combine(_env.ContentRootPath, _configuration.GetValue<string>("RSA:KeyLocation"), version, isPulicKey ? @"publickey.pem" : @"privatekey.pem");
                _logger.Information($"ContentRootPath: {_env.ContentRootPath}, KeyLocation: {_configuration.GetValue<string>("RSA:KeyLocation")}, Version: {version}");

                using (var Key = File.OpenRead(directory))
                {
                    using (var pem = new PemReader(Key))
                    {
                        var rsaParameters = pem.ReadRsaKey();
                        rsa.ImportParameters(rsaParameters);
                    }
                }
                return rsa;
            }
            catch (Exception ex)
            {
                _logger.Error("Step: GetRSACryptoProvider catch " + ex);

                return null;
            }
        }

        public static string QCashEncrypt(string textToEncrypt, string TdesKey, string TdesIV)
        {
            TripleDES tdes = TripleDES.Create();
            tdes.Key = Encoding.ASCII.GetBytes(TdesKey);
            tdes.IV = Encoding.ASCII.GetBytes(TdesIV);
            //tdes.Key = tdesKey;
            //tdes.IV = tdesIV;
            byte[] buffer = Encoding.ASCII.GetBytes(textToEncrypt);
            return Convert.ToBase64String(tdes.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }
        public static string QCashDecrypt(string textToDecrypt, string TdesKey, string TdesIV)
        {
            byte[] buffer = Convert.FromBase64String(textToDecrypt);
            TripleDES des = TripleDES.Create();
            des.Key = Encoding.ASCII.GetBytes(TdesKey);
            des.IV = Encoding.ASCII.GetBytes(TdesIV);
            return Encoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public static string TestRSAEncryptWithPublicKey(string text, IConfiguration _configuration, IHostEnvironment _env, ILogger _logger)
        {
            try
            {
                //Create a UnicodeEncoder to convert between byte array and string.
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                //Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] dataToEncrypt = ByteConverter.GetBytes(text);
                byte[] encryptedData;
                var cipherText = "";
                //Create a new instance of RSACryptoServiceProvider to generate
                //public and private key data.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024))
                {

                    //Pass the data to ENCRYPT, the public key information 
                    //(using RSACryptoServiceProvider.ExportParameters(false),
                    //and a boolean flag specifying no OAEP padding.


                    var directory = Path.Combine(_env.ContentRootPath, _configuration.GetValue<string>("RSA:KeyLocation"), "v2", @"privatekey.pem"); //@"publickey.pem"
                    using (var Key = File.OpenRead(directory))
                    {
                        using (var pem = new PemReader(Key))
                        {
                            var rsaParameters = pem.ReadRsaKey();
                            // rsa.ImportParameters(rsaParameters);
                            encryptedData = RSAEncrypt(dataToEncrypt, rsaParameters, false);
                            cipherText = Convert.ToBase64String(encryptedData);
                        }
                    }
                }
                return cipherText;
            }
            catch (ArgumentNullException)
            {
                //Catch this exception in case the encryption did
                //not succeed.
                return null;
            }
        }

        public static string TestRSADecryptWithPrivateKey(string plainText, IConfiguration _configuration, IHostEnvironment _env, ILogger _logger)
        {
            try
            {
                //Create a UnicodeEncoder to convert between byte array and string.
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                byte[] decryptedData;
                var cipherTextBytes = Convert.FromBase64String(plainText);
                //Create a new instance of RSACryptoServiceProvider to generate
                //public and private key data.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024))
                {

                    var directory1 = Path.Combine(_env.ContentRootPath, _configuration.GetValue<string>("RSA:KeyLocation"), "v2", @"publickey.pem"); //@"privatekey.pem"
                    using (var Key1 = File.OpenRead(directory1))
                    {
                        using (var pem1 = new PemReader(Key1))
                        {
                            var rsaParameters1 = pem1.ReadRsaKey();
                            // rsa.ImportParameters(rsaParameters);
                            decryptedData = RSADecrypt(cipherTextBytes, rsaParameters1, false);
                        }
                    }

                    var data = ByteConverter.GetString(decryptedData);
                    return data;
                }
            }
            catch (ArgumentNullException)
            {
                //Catch this exception in case the encryption did
                //not succeed.
                return null;
            }
        }


        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs
                    //toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {

                return null;
            }
        }

        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {

                return null;
            }
        }


        public static string SignData(string message, IConfiguration _configuration, IHostEnvironment _env, ILogger _logger)
        {
            //// The array to store the signed message in bytes
            byte[] signedBytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                //// Write the message to a byte array using UTF8 as the encoding.
                var encoder = new UTF8Encoding();
                byte[] originalData = encoder.GetBytes(message);

                try
                {
                    var directory1 = Path.Combine(_env.ContentRootPath, _configuration.GetValue<string>("RSA:KeyLocation"), "v2", @"privatekey.pem"); //@"privatekey.pem"
                    using (var Key1 = File.OpenRead(directory1))
                    {
                        using (var pem1 = new PemReader(Key1))
                        {
                            var rsaParameters1 = pem1.ReadRsaKey();
                            rsa.ImportParameters(rsaParameters1);
                            //// Sign the data, using SHA512 as the hashing algorithm 
                            signedBytes = rsa.SignData(originalData, CryptoConfig.MapNameToOID("SHA256"));
                        }
                    }
                }
                catch (CryptographicException e)
                {
                    return null;
                }
                finally
                {
                    //// Set the keycontainer to be cleared when rsa is garbage collected.
                    rsa.PersistKeyInCsp = false;
                }
            }
            //// Convert the a base64 string before returning
            return Convert.ToBase64String(signedBytes);
        }


        public static bool VerifyData(string originalMessage, string signedMessage, IConfiguration _configuration, IHostEnvironment _env, ILogger _logger)
        {
            bool success = false;
            using (var rsa = new RSACryptoServiceProvider())
            {
                var encoder = new UTF8Encoding();
                byte[] bytesToVerify = encoder.GetBytes(originalMessage);
                byte[] signedBytes = Convert.FromBase64String(signedMessage);
                try
                {
                    var directory1 = Path.Combine(_env.ContentRootPath, _configuration.GetValue<string>("RSA:KeyLocation"), "v2", @"publickey.pem"); //@"privatekey.pem"
                    using (var Key1 = File.OpenRead(directory1))
                    {
                        using (var pem1 = new PemReader(Key1))
                        {
                            var rsaParameters1 = pem1.ReadRsaKey();

                            rsa.ImportParameters(rsaParameters1);

                            success = rsa.VerifyData(bytesToVerify, CryptoConfig.MapNameToOID("SHA256"), signedBytes);

                        }
                    }
                }
                catch (CryptographicException e)
                {
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            return success;
        }


    }
}
