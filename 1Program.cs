// Decompiled with JetBrains decompiler
// Type: KFM_INCLUDE.Catalog
// Assembly: KFM_INCLUDED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3EC1F72F-985F-4D38-A59E-99A47EC3416E
// Assembly location: C:\Users\s.zhaparov\Desktop\KFM_LOADER_new\KFM_INCLUDE\KFM_INCLUDED.exe

using NLog;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace KFM_INCLUDE
{
  internal class Catalog
  {
    private static Logger Logger = LogManager.GetCurrentClassLogger();

    public static void DownloadInclude()
    {
      ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback) ((_param1, _param2, _param3, _param4) => true);
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      string str1 = ConfigurationManager.AppSettings["proxy"].ToString();
      string str2 = ConfigurationManager.AppSettings["ip"].ToString();
      string str3 = ConfigurationManager.AppSettings["port"].ToString();
      string userName = ConfigurationManager.AppSettings["username"].ToString();
      string password = ConfigurationManager.AppSettings["password"].ToString();
      string domain = ConfigurationManager.AppSettings["domain"].ToString();
      string appSetting1 = ConfigurationManager.AppSettings["url"];
      string appSetting2 = ConfigurationManager.AppSettings["AuthLogin"];
      string appSetting3 = ConfigurationManager.AppSettings["AuthPassword"];
      ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((sender, certificate, chain, sslPolicyErrors) => true);
      WebClientEx webClientEx1 = new WebClientEx();
      webClientEx1.Encoding = Encoding.UTF8;
      using (WebClientEx webClientEx2 = webClientEx1)
      {
        try
        {
          if (str1.ToUpper() == "TRUE")
            webClientEx2.Proxy = (IWebProxy) new WebProxy(str2 + ":" + str3, true)
            {
              Credentials = (ICredentials) new NetworkCredential(userName, password, domain)
            };
          if (System.IO.File.Exists("include.xml"))
            System.IO.File.Delete("include.xml");
          NameValueCollection data = new NameValueCollection()
          {
            {
              "action",
              "auth/formLogin"
            },
            {
              "username",
              appSetting2
            },
            {
              "password",
              appSetting3
            }
          };
         // webClientEx2.UploadValues("https://afmrk.gov.kz/assets/components/office/action.php", data);
          

                    //    WebClient.DownloadFile(Source, @".\Downloads\" + FileName);

                    XDocument.Parse(webClientEx2.DownloadString(appSetting1)).Save(".\\include.xml");
        }
        catch (WebException ex)
        {
          Catalog.Logger.Debug("Download Status: " + ex?.ToString());
        }
      }
    }

    private class Program
    {
      private static void Main(string[] args)
      {
        LoggerUtils loggerUtils = new LoggerUtils();
        DB db = (DB) null;
        string str1 = "";
        try
        {
          if (args.Length != 0)
          {
            if (args[0].ToUpper() == "-ENC")
            {
              loggerUtils.writeInfoLog("Encrypting started..");
              Crypto.Encrypt();
              loggerUtils.writeInfoLog("Encrypting finished.");
              return;
            }
            if (args[0].ToUpper() == "-DEC")
            {
              loggerUtils.writeInfoLog("Decrypting started..");
              Crypto.Decrypt(true);
              loggerUtils.writeInfoLog("Decrypting finished.");
              return;
            }
          }
          db = new DB();
         // loggerUtils.writeInfoLog("start", "Start working...", db, "", "include.xml");
          string str2 = ConfigurationManager.AppSettings["url"].ToString();
          string tableName = "TERRORISTS_KFM";
        //  loggerUtils.writeInfoLog("process", "Downloading file from - " + str2, db, "", "include.xml");
          Catalog.DownloadInclude();
          loggerUtils.writeInfoLog("process", "Download finished.", db, "", "include.xml");
          loggerUtils.writeInfoLog("process", "Retrieving the hash of - include.xml", db, "", "include.xml");
          using (MD5 md5 = MD5.Create())
          {
            using (FileStream fileStream = System.IO.File.OpenRead("include.xml"))
              str1 = BitConverter.ToString(md5.ComputeHash((Stream) fileStream)).Replace("-", "").ToLowerInvariant();
          }
          loggerUtils.writeInfoLog("process", "Hash of file - include.xml - " + str1, db, str1, "include.xml");
          loggerUtils.writeInfoLog("process", "Checking the hash in DB.", db, str1, "include.xml");
          if (!db.CheckHash("KFM_INCLUDE", str1, "MD5").Equals("SUCCESS"))
          {
            loggerUtils.writeInfoLog("process", "There is no file with this hash in DB..", db, str1, "include.xml");
            new Parser().parse("include.xml", db, str1);
            loggerUtils.writeInfoLog("process", "Zip and Archive the parsed file...", db, str1, "include.xml");
            string str3 = Catalog.Program.CompressFile("include.xml", str1);
            if (!Directory.Exists("Archive"))
              Directory.CreateDirectory("Archive");
            string str4 = "Archive/" + Path.GetFileNameWithoutExtension(str3) + "_" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + str1 + "_.zip";
            if (System.IO.File.Exists(str4))
              System.IO.File.Delete(str4);
            System.IO.File.Move(str3, str4);
            if (System.IO.File.Exists("include.zip"))
              System.IO.File.Delete("include.zip");
          }
          else
            loggerUtils.writeInfoLog("process", "File include.xml with hash " + str1 + " already exists in DB", db, str1, "include.xml");
          loggerUtils.writeInfoLog("process", "GatherStatistics", db, str1, "include.xml");
          db.GatherStatistics();
          loggerUtils.writeInfoLog("process", "FinalizeLoading", db, str1, "include.xml");
          db.FinalizeLoading("include.xml", str1);
          loggerUtils.writeInfoLog("process", "SendNotifications", db, str1, "include.xml");
          db.SendNotifications("include.xml", str1);
          loggerUtils.writeInfoLog("process", "Refresh table", db, str1, "active.xml");
          //db.RefreshTable(tableName);
          loggerUtils.writeInfoLog("finish", "Finalization", db, str1, "include.xml");
          db.CloseDatabase();
        }
        catch (Exception ex)
        {
          loggerUtils.writeInfoLog("process", ex.ToString(), db, str1, "include.xml");
        }
      }

      private static string CompressFile(string sourceFileName, string hash)
      {
        using (ZipArchive destination = ZipFile.Open(Path.ChangeExtension(sourceFileName, ".zip"), ZipArchiveMode.Create))
          destination.CreateEntryFromFile(sourceFileName, Path.GetFileName(sourceFileName));
        return Path.ChangeExtension(sourceFileName, ".zip");
      }
    }
  }
}
