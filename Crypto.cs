// Decompiled with JetBrains decompiler
// Type: KFM_INCLUDE.Crypto
// Assembly: KFM_INCLUDED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3EC1F72F-985F-4D38-A59E-99A47EC3416E
// Assembly location: C:\Users\s.zhaparov\Desktop\KFM_LOADER_new\KFM_INCLUDE\KFM_INCLUDED.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace KFM_INCLUDE
{
  public static class Crypto
  {
    private static LoggerUtils loggerUtils = new LoggerUtils();
    private const int Keysize = 256;
    private const int DerivationIterations = 1000;
    private const string passPhrase = "MAKV2SPBNI99212";
    private const string configPath = "UNListLoader.exe.config";

    public static void Encrypt()
    {
      string input = "";
      string connString = Crypto.GetConnString();
      if (connString == "")
        return;
      byte[] salt = Crypto.Generate256BitsOfRandomEntropy();
      byte[] rgbIV = Crypto.Generate256BitsOfRandomEntropy();
      byte[] bytes1 = Encoding.UTF8.GetBytes(connString);
      using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("MAKV2SPBNI99212", salt, 1000))
      {
        byte[] bytes2 = rfc2898DeriveBytes.GetBytes(32);
        using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
        {
          rijndaelManaged.BlockSize = 256;
          rijndaelManaged.Mode = CipherMode.CBC;
          rijndaelManaged.Padding = PaddingMode.PKCS7;
          using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes2, rgbIV))
          {
            using (MemoryStream memoryStream = new MemoryStream())
            {
              using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
              {
                cryptoStream.Write(bytes1, 0, bytes1.Length);
                cryptoStream.FlushFinalBlock();
                byte[] array = ((IEnumerable<byte>) ((IEnumerable<byte>) salt).Concat<byte>((IEnumerable<byte>) rgbIV).ToArray<byte>()).Concat<byte>((IEnumerable<byte>) memoryStream.ToArray()).ToArray<byte>();
                memoryStream.Close();
                cryptoStream.Close();
                input = Convert.ToBase64String(array);
              }
            }
          }
        }
      }
      Crypto.SetConnString(input, 1);
    }

    public static string Decrypt(bool dosave)
    {
      string connString = Crypto.GetConnString();
      string input = "";
      byte[] numArray1 = Convert.FromBase64String(connString);
      byte[] array1 = ((IEnumerable<byte>) numArray1).Take<byte>(32).ToArray<byte>();
      byte[] array2 = ((IEnumerable<byte>) numArray1).Skip<byte>(32).Take<byte>(32).ToArray<byte>();
      byte[] array3 = ((IEnumerable<byte>) numArray1).Skip<byte>(64).Take<byte>(numArray1.Length - 64).ToArray<byte>();
      using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("MAKV2SPBNI99212", array1, 1000))
      {
        byte[] bytes = rfc2898DeriveBytes.GetBytes(32);
        using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
        {
          rijndaelManaged.BlockSize = 256;
          rijndaelManaged.Mode = CipherMode.CBC;
          rijndaelManaged.Padding = PaddingMode.PKCS7;
          using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes, array2))
          {
            using (MemoryStream memoryStream = new MemoryStream(array3))
            {
              using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
              {
                byte[] numArray2 = new byte[array3.Length];
                int count = cryptoStream.Read(numArray2, 0, numArray2.Length);
                memoryStream.Close();
                cryptoStream.Close();
                input = Encoding.UTF8.GetString(numArray2, 0, count);
              }
            }
          }
        }
      }
      if (dosave)
        Crypto.SetConnString(input, 0);
      return input;
    }

    private static byte[] Generate256BitsOfRandomEntropy()
    {
      byte[] data = new byte[32];
      using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
        cryptoServiceProvider.GetBytes(data);
      return data;
    }

    private static string GetConnString()
    {
      try
      {
        XElement xelement1 = XDocument.Load("UNListLoader.exe.config").Root.Element((XName) "appSettings");
        string str = "";
        foreach (XElement xelement2 in xelement1.Elements().ToList<XElement>())
        {
          if (xelement2.Attribute((XName) "key").Value == "connection_string")
            str = xelement2.Attribute((XName) "value").Value;
        }
        return str;
      }
      catch (Exception ex)
      {
        Crypto.loggerUtils.writeErrorLog("GetConnString: " + ex.Message);
        return "";
      }
    }

    private static void SetConnString(string input, int is_encrypted)
    {
      try
      {
        XDocument xdocument = XDocument.Load("UNListLoader.exe.config");
        foreach (XElement xelement in xdocument.Root.Element((XName) "appSettings").Elements().ToList<XElement>())
        {
          if (xelement.Attribute((XName) "key").Value == "connection_string")
            xelement.Attribute((XName) "value").Value = input;
          else if (xelement.Attribute((XName) "key").Value == nameof (is_encrypted))
            xelement.Attribute((XName) "value").Value = is_encrypted.ToString();
        }
        xdocument.Save("UNListLoader.exe.config");
      }
      catch (Exception ex)
      {
        Crypto.loggerUtils.writeErrorLog("SetConnString: " + input + ", " + ex.Message);
      }
    }
  }
}
