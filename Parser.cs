// Decompiled with JetBrains decompiler
// Type: KFM_INCLUDE.Parser
// Assembly: KFM_INCLUDED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3EC1F72F-985F-4D38-A59E-99A47EC3416E
// Assembly location: C:\Users\s.zhaparov\Desktop\KFM_LOADER_new\KFM_INCLUDE\KFM_INCLUDED.exe

using KFM_INCLUDE.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace KFM_INCLUDE
{
  internal class Parser
  {
    public void parse(string filePath, DB db, string fileHash)
    {
      LoggerUtils loggerUtils = new LoggerUtils();
      List<KFM_INCLUDE_RECORD> KFM_INCLUDE_RecordList = new List<KFM_INCLUDE_RECORD>();
      List<KFM_INCLUDE_RECORD> KFM_INCLUDE_person_List = new List<KFM_INCLUDE_RECORD>();
      try
      {
        using (XmlReader reader = XmlReader.Create(filePath))
        {
          while (!reader.EOF)
          {
            if (reader.NodeType == XmlNodeType.Element)
            {
                            if (reader.Name.ToString() == "org")
                            {
                                XElement xelement1 = XNode.ReadFrom(reader) as XElement;
                                KFM_INCLUDE_RECORD kfmIncludeRecord = new KFM_INCLUDE_RECORD();
                                kfmIncludeRecord.recordType = "org";
                                kfmIncludeRecord.loadFileName = filePath;
                                kfmIncludeRecord.loadFileHash = fileHash;
                                foreach (XElement xelement in xelement1.Elements().ToList<XElement>())
                                {
                                    switch (xelement.Name.ToString())
                                    {
                                        case "added_to_list":
                                            if (xelement.Value != "" && xelement.Value != "None")
                                            {
                                                kfmIncludeRecord.added_to_list = DateTime.Parse(xelement.Value, (IFormatProvider)CultureInfo.GetCultureInfo("ru-ru"));
                                                break;
                                            }
                                            break;

                                        case "removed_from_list":
                                            if (xelement.Value != "" && xelement.Value != "None")
                                            {
                                                kfmIncludeRecord.removed_from_list = DateTime.Parse(xelement.Value, (IFormatProvider)CultureInfo.GetCultureInfo("ru-ru"));
                                                break;
                                            }
                                            break;
                                        case "org_name":
                                            kfmIncludeRecord.org_name = xelement.Value;
                                            break;
                                        case "org_iin":
                                            kfmIncludeRecord.iin = xelement.Value;
                                            break;
                                        case "note":
                                            kfmIncludeRecord.note = xelement.Value;
                                            break;
                                        case "num":

                                            kfmIncludeRecord.num = Convert.ToDecimal(xelement.Value);
                                            break;
                                    }
                                }
                                KFM_INCLUDE_RecordList.Add(kfmIncludeRecord);
                            }
                            else if(reader.Name.ToString() == "person") {

                                XElement xelement1 = XNode.ReadFrom(reader) as XElement;
                                KFM_INCLUDE_RECORD kfmIncludeRecord = new KFM_INCLUDE_RECORD();
                                kfmIncludeRecord.recordType = "person";
                                kfmIncludeRecord.loadFileName = filePath;
                                kfmIncludeRecord.loadFileHash = fileHash;
                                foreach (XElement xelement in xelement1.Elements().ToList<XElement>())
                                {
                                    switch (xelement.Name.ToString())
                                    {
                                        case "birthdate":
                                            if (xelement.Value != "" && xelement.Value != "None")
                                            {
                                                kfmIncludeRecord.birthdate = DateTime.Parse(xelement.Value, (IFormatProvider)CultureInfo.GetCultureInfo("ru-ru"));
                                                break;
                                            }
                                            break;                   
                                        case "iin":
                                            kfmIncludeRecord.iin = xelement.Value;
                                            break;
                                        case "note":
                                            kfmIncludeRecord.note = xelement.Value;
                                            break;
                                        case "lname":
                                            kfmIncludeRecord.lname = xelement.Value;
                                            break;
                                        case "fname":
                                            kfmIncludeRecord.fname = xelement.Value;
                                            break;
                                        case "mname":
                                            kfmIncludeRecord.mname = xelement.Value;
                                            break;

                                        case "num":

                                            kfmIncludeRecord.num = Convert.ToDecimal(xelement.Value);
                                            break;

                                        case "correction":

                                            kfmIncludeRecord.correction = xelement.Value;
                                            break;
                                    }
                                }
                                KFM_INCLUDE_person_List.Add(kfmIncludeRecord);

                            }
           
             
              else
                reader.Read();
            }
            else
              reader.Read();
          }
        }
        loggerUtils.writeInfoLog("process", "Parsing is over. Truncating tables... ", db, fileHash, "include.xml");
        if (!db.TruncateTables().Equals("ERROR"))
        {
          loggerUtils.writeInfoLog("process", "Inserting data to database...", db, fileHash, "include.xml");
          if (KFM_INCLUDE_RecordList.Count > 0)
            db.Insert_KFM_INCLUDE_record(KFM_INCLUDE_RecordList);

            if (KFM_INCLUDE_person_List.Count > 0)
                db.Insert_KFM_INCLUDE_record_person(KFM_INCLUDE_person_List);

                }
        loggerUtils.writeInfoLog("process", "Finished inserting data.", db, fileHash, "include.xml");
        loggerUtils.writeInfoLog("process", "Writing hash of new file into DB.", db, fileHash, "include.xml");
        if (db.InsertHash("KFM_INCLUDE", "include.xml", fileHash, "MD5").Equals("SUCCESS"))
          return;
        loggerUtils.writeInfoLog("process", "Error writing hash to DB for file.", db, fileHash, "include.xml");
      }
      catch (Exception ex)
      {
        loggerUtils.writeErrorLog(ex.ToString());
      }
    }
  }
}
