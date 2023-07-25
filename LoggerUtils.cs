// Decompiled with JetBrains decompiler
// Type: KFM_INCLUDE.LoggerUtils
// Assembly: KFM_INCLUDED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3EC1F72F-985F-4D38-A59E-99A47EC3416E
// Assembly location: C:\Users\s.zhaparov\Desktop\KFM_LOADER_new\KFM_INCLUDE\KFM_INCLUDED.exe

using NLog;
using System;

namespace KFM_INCLUDE
{
  internal class LoggerUtils
  {
    public static Logger logger;

    public LoggerUtils()
    {
      try
      {
        LoggerUtils.logger = LogManager.GetCurrentClassLogger();
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
      }
    }

    public void writeInfoLog(string text)
    {
      LoggerUtils.logger.Info(text);
      Console.WriteLine(text);
    }

    public void writeErrorLog(string text)
    {
      LoggerUtils.logger.Error(text);
      Console.WriteLine(text);
    }

    public void writeInfoLog(
      string process,
      string text,
      DB db,
      string fileHash,
      string fileName)
    {
      LoggerUtils.logger.Info(text);
      Console.WriteLine(text);
      db.InsertLog(process, fileHash, fileName, text);
    }

    public void writeErrorLog(string text, DB db)
    {
      LoggerUtils.logger.Error(text);
      Console.WriteLine(text);
    }
  }
}
