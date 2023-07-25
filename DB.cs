// Decompiled with JetBrains decompiler
// Type: KFM_INCLUDE.DB
// Assembly: KFM_INCLUDED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3EC1F72F-985F-4D38-A59E-99A47EC3416E
// Assembly location: C:\Users\s.zhaparov\Desktop\KFM_LOADER_new\KFM_INCLUDE\KFM_INCLUDED.exe

using KFM_INCLUDE.Model;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace KFM_INCLUDE
{
  internal class DB
  {
    private static Logger log = LogManager.GetCurrentClassLogger();
    private OracleConnection conn;
    private OracleCommand command;
    private LoggerUtils loggerUtils;
    private string isEncrypted = ConfigurationManager.AppSettings["is_encrypted"].ToString();
    private string connectionString = ConfigurationManager.AppSettings["connection_string"].ToString();
        private string pkg_name = ConfigurationManager.AppSettings["pkg_name"].ToString();

        public DB()
    {
      this.loggerUtils = new LoggerUtils();
      if (this.isEncrypted == "1")
        this.connectionString = Crypto.Decrypt(false);
      this.conn = new OracleConnection(this.connectionString);
      this.conn.Open();
    }

    public string TruncateTables()
    {
      OracleParameter oracleParameter1 = (OracleParameter) null;
      OracleParameter p_table = (OracleParameter)null;

            if (this.conn.State.ToString().Equals("Open"))
      {
        this.command = new OracleCommand(pkg_name + ".truncate_tables", this.conn);
        this.command.BindByName = true;
        this.command.CommandType = CommandType.StoredProcedure;

            /*    p_table = new OracleParameter("p_table", OracleDbType.Varchar2);
                p_table.Direction = ParameterDirection.Input;
                p_table.Value = "KFM_INCLUDE";
                this.command.Parameters.Add(p_table);*/

                oracleParameter1 = new OracleParameter("p_status", OracleDbType.Varchar2);
        oracleParameter1.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter1);
        this.command.Parameters["p_status"].Size = (int) byte.MaxValue;
        OracleParameter oracleParameter2 = new OracleParameter("p_error_code", OracleDbType.Varchar2);
        oracleParameter2.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter2);
        this.command.Parameters["p_error_code"].Size = (int) byte.MaxValue;
        OracleParameter oracleParameter3 = new OracleParameter("p_error_message", OracleDbType.Varchar2);
        oracleParameter3.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter3);
        this.command.Parameters["p_error_message"].Size = (int) byte.MaxValue;
        this.command.ExecuteNonQuery();
        if (oracleParameter1.Value.ToString() == "ERROR")
          this.loggerUtils.writeInfoLog(oracleParameter1.Value.ToString() + "-" + oracleParameter2.Value.ToString() + oracleParameter3.Value.ToString());
      }
      return oracleParameter1.Value.ToString();
    }

    public string CheckHash(
      string p_list_type,
      string p_load_file_hash,
      string p_load_file_hash_method)
    {
      OracleParameter oracleParameter = (OracleParameter) null;
      if (this.conn.State.ToString().Equals("Open"))
      {
        this.command = new OracleCommand("pkg_main.check_hash", this.conn);
        this.command.BindByName = true;
        this.command.CommandType = CommandType.StoredProcedure;
        this.command.Parameters.Add(nameof (p_list_type), OracleDbType.Varchar2, (object) p_list_type, ParameterDirection.Input);
        this.command.Parameters.Add(nameof (p_load_file_hash), OracleDbType.Varchar2, (object) p_load_file_hash, ParameterDirection.Input);
        this.command.Parameters.Add(nameof (p_load_file_hash_method), OracleDbType.Varchar2, (object) p_load_file_hash_method, ParameterDirection.Input);
        this.command.Parameters.Add("p_id_no", OracleDbType.Decimal, ParameterDirection.Output);
        this.command.Parameters.Add("p_load_sysdate", OracleDbType.Date, ParameterDirection.Output);
        this.command.Parameters.Add("p_load_file_name", OracleDbType.Varchar2, ParameterDirection.Output);
        this.command.Parameters["p_load_file_name"].Size = (int) byte.MaxValue;
        oracleParameter = new OracleParameter("p_status", OracleDbType.Varchar2);
        oracleParameter.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter);
        this.command.Parameters["p_status"].Size = (int) byte.MaxValue;
        this.command.Parameters.Add("p_error_code", OracleDbType.Varchar2, ParameterDirection.Output);
        this.command.Parameters["p_error_code"].Size = (int) byte.MaxValue;
        this.command.Parameters.Add("p_error_message", OracleDbType.Varchar2, ParameterDirection.Output);
        this.command.Parameters["p_error_message"].Size = (int) byte.MaxValue;
        this.command.ExecuteNonQuery();
      }
            return oracleParameter.Value.ToString();
            //return "";
          
    }

    public string InsertHash(
      string p_list_type,
      string p_load_file_name,
      string p_load_file_hash,
      string p_load_file_hash_method)
    {
      OracleParameter oracleParameter = (OracleParameter) null;
      if (this.conn.State.ToString().Equals("Open"))
      {
        this.command = new OracleCommand( "pkg_main.ins_log_loaded_file", this.conn);
        this.command.BindByName = true;
        this.command.CommandType = CommandType.StoredProcedure;
        this.command.Parameters.Add(nameof (p_list_type), OracleDbType.Varchar2, (object) p_list_type, ParameterDirection.Input);
        this.command.Parameters.Add(nameof (p_load_file_name), OracleDbType.Varchar2, (object) p_load_file_name, ParameterDirection.Input);
        this.command.Parameters.Add(nameof (p_load_file_hash), OracleDbType.Varchar2, (object) p_load_file_hash, ParameterDirection.Input);
        this.command.Parameters.Add(nameof (p_load_file_hash_method), OracleDbType.Varchar2, (object) p_load_file_hash_method, ParameterDirection.Input);
        this.command.Parameters.Add("p_id_no", OracleDbType.Decimal, ParameterDirection.Output);
        oracleParameter = new OracleParameter("p_status", OracleDbType.Varchar2);
        oracleParameter.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter);
        this.command.Parameters["p_status"].Size = (int) byte.MaxValue;
        this.command.Parameters.Add("p_error_code", OracleDbType.Varchar2, ParameterDirection.Output);
        this.command.Parameters["p_error_code"].Size = (int) byte.MaxValue;
        this.command.Parameters.Add("p_error_message", OracleDbType.Varchar2, ParameterDirection.Output);
        this.command.Parameters["p_error_message"].Size = (int) byte.MaxValue;
        this.command.ExecuteNonQuery();
      }
               return oracleParameter.Value.ToString();
            
        }

    public void Insert_KFM_INCLUDE_record(List<KFM_INCLUDE_RECORD> KFM_INCLUDE_RecordList)
    {
      if (!this.conn.State.ToString().Equals("Open"))
        return;



            foreach (KFM_INCLUDE_RECORD record in KFM_INCLUDE_RecordList)
            {

                try
                {


                    this.command = new OracleCommand("insert into KFM_INCLUDE ( record_type  , NUM,  full_name,  IIN,NOTE, added_to_list, removed_from_list,   LOAD_FILE_NAME,LOAD_FILE_HASH) values ( :record_type, :num , :full_name, :iin,:note, :added_to_list, :removed_from_list,:load_file_name,:load_file_hash)", this.conn);


                    this.command.Parameters.Add("record_type", OracleDbType.Varchar2, record.recordType, ParameterDirection.Input);
                    this.command.Parameters.Add("NUM", OracleDbType.Decimal, record.num, ParameterDirection.Input);

               
                   
                        this.command.Parameters.Add("full_name", OracleDbType.Varchar2, record.org_name, ParameterDirection.Input);
                    

                    this.command.Parameters.Add("IIN", OracleDbType.Varchar2, record.iin, ParameterDirection.Input);
                    this.command.Parameters.Add("NOTE", OracleDbType.Varchar2, record.note, ParameterDirection.Input);
                    this.command.Parameters.Add("added_to_list", OracleDbType.Date, record.added_to_list, ParameterDirection.Input);
                    this.command.Parameters.Add("removed_from_list", OracleDbType.Date, record.removed_from_list, ParameterDirection.Input);
                    this.command.Parameters.Add("LOAD_FILE_NAME", OracleDbType.Varchar2, record.loadFileName, ParameterDirection.Input);
                    this.command.Parameters.Add("LOAD_FILE_HASH", OracleDbType.Varchar2, record.loadFileHash, ParameterDirection.Input);


                    this.command.ExecuteNonQuery();
                    this.command.Dispose();

                }
                catch (OracleException ex)
                {
                    log.Info(ex.ToString());
                }

            }


        }


        public void Insert_KFM_INCLUDE_record_person(List<KFM_INCLUDE_RECORD> KFM_INCLUDE_RecordList)
        {
            if (!this.conn.State.ToString().Equals("Open"))
                return;



            foreach (KFM_INCLUDE_RECORD record in KFM_INCLUDE_RecordList)
            {

                try
                {


                    this.command = new OracleCommand("insert into KFM_INCLUDE ( record_type  , NUM,  lname, fname, mname ,  IIN,NOTE, birthdate, correction,   LOAD_FILE_NAME,LOAD_FILE_HASH) values ( :record_type, :num , :lname, :fname, :mname  , :iin, :note, :birthdate, :correction,:load_file_name,:load_file_hash)", this.conn);


                    this.command.Parameters.Add("record_type", OracleDbType.Varchar2, record.recordType, ParameterDirection.Input);
                    this.command.Parameters.Add("NUM", OracleDbType.Decimal, record.num, ParameterDirection.Input);



                    this.command.Parameters.Add("lname", OracleDbType.Varchar2, record.lname, ParameterDirection.Input);
                    this.command.Parameters.Add("fname", OracleDbType.Varchar2, record.fname, ParameterDirection.Input);
                    this.command.Parameters.Add("mname", OracleDbType.Varchar2, record.mname, ParameterDirection.Input);



                    this.command.Parameters.Add("IIN", OracleDbType.Varchar2, record.iin, ParameterDirection.Input);
                    this.command.Parameters.Add("NOTE", OracleDbType.Varchar2, record.note, ParameterDirection.Input);
                    this.command.Parameters.Add("birthdate", OracleDbType.Date, record.birthdate, ParameterDirection.Input);
                    this.command.Parameters.Add("correction", OracleDbType.Varchar2, record.correction, ParameterDirection.Input);
                    this.command.Parameters.Add("LOAD_FILE_NAME", OracleDbType.Varchar2, record.loadFileName, ParameterDirection.Input);
                    this.command.Parameters.Add("LOAD_FILE_HASH", OracleDbType.Varchar2, record.loadFileHash, ParameterDirection.Input);


                    this.command.ExecuteNonQuery();
                    this.command.Dispose();

                }
                catch (OracleException ex)
                {
                    log.Info(ex.ToString());
                }

            }


        }



        public string InsertLog(
      string process,
      string p_load_file_hash,
      string p_load_file_name,
      string comment)
    {
      OracleParameter oracleParameter1 = (OracleParameter) null;
      if (this.conn.State.ToString().Equals("Open"))
      {
        this.command = new OracleCommand("pkg_main.ins_log_action", this.conn);
        this.command.BindByName = true;
        this.command.CommandType = CommandType.StoredProcedure;
        this.command.Parameters.Add("p_list_type", OracleDbType.Varchar2, (object) "KFM_INCLUDE", ParameterDirection.Input);
        this.command.Parameters.Add("p_load_process", OracleDbType.Varchar2, (object) process, ParameterDirection.Input);
        this.command.Parameters.Add("p_load_comment", OracleDbType.Varchar2, (object) comment, ParameterDirection.Input);
        this.command.Parameters.Add(nameof (p_load_file_name), OracleDbType.Varchar2, (object) p_load_file_name, ParameterDirection.Input);
        this.command.Parameters.Add(nameof (p_load_file_hash), OracleDbType.Varchar2, (object) p_load_file_hash, ParameterDirection.Input);
        this.command.Parameters.Add("p_id_no", OracleDbType.Decimal, ParameterDirection.Output);
        oracleParameter1 = new OracleParameter("p_status", OracleDbType.Varchar2);
        oracleParameter1.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter1);
        this.command.Parameters["p_status"].Size = (int) byte.MaxValue;
        this.command.Parameters.Add("p_error_code", OracleDbType.Varchar2, ParameterDirection.Output);
        this.command.Parameters["p_error_code"].Size = (int) byte.MaxValue;
        OracleParameter oracleParameter2 = new OracleParameter("p_error_message", OracleDbType.Varchar2);
        oracleParameter2.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter2);
        this.command.Parameters["p_error_message"].Size = (int) byte.MaxValue;
        this.command.ExecuteNonQuery();
      }
             return oracleParameter1.Value.ToString();

        
    }

    public string GatherStatistics()
    {
      OracleParameter oracleParameter1 = (OracleParameter) null;
            OracleParameter p_table = (OracleParameter)null;
            if (this.conn.State.ToString().Equals("Open"))
      {
        this.command = new OracleCommand(pkg_name + ".gather_statistics", this.conn);
        this.command.BindByName = true;
        this.command.CommandType = CommandType.StoredProcedure;

             /*   p_table = new OracleParameter("p_table", OracleDbType.Varchar2);
                p_table.Direction = ParameterDirection.Input;
                p_table.Value = "KFM_INCLUDE";
                this.command.Parameters.Add(p_table);*/

                oracleParameter1 = new OracleParameter("p_status", OracleDbType.Varchar2);
        oracleParameter1.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter1);
        this.command.Parameters["p_status"].Size = (int) byte.MaxValue;
        OracleParameter oracleParameter2 = new OracleParameter("p_error_code", OracleDbType.Varchar2);
        oracleParameter2.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter2);
        this.command.Parameters["p_error_code"].Size = (int) byte.MaxValue;
        OracleParameter oracleParameter3 = new OracleParameter("p_error_message", OracleDbType.Varchar2);
        oracleParameter3.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter3);
        this.command.Parameters["p_error_message"].Size = (int) byte.MaxValue;
        this.command.ExecuteNonQuery();
        if (oracleParameter1.Value.ToString() == "ERROR")
          this.loggerUtils.writeInfoLog(oracleParameter1.Value.ToString() + "-" + oracleParameter2.Value.ToString() + oracleParameter3.Value.ToString());
      }
      return oracleParameter1.Value.ToString();
    }

    public string FinalizeLoading(string p_load_file_name, string p_load_file_hash)
    {
      OracleParameter oracleParameter = (OracleParameter) null;

            OracleParameter p_table = (OracleParameter)null;
            if (this.conn.State.ToString().Equals("Open"))
      {
        this.command = new OracleCommand(pkg_name + ".finalize_loading", this.conn);
        this.command.BindByName = true;
        this.command.CommandType = CommandType.StoredProcedure;

                /*p_table = new OracleParameter("p_table", OracleDbType.Varchar2);
                p_table.Direction = ParameterDirection.Input;
                p_table.Value = "KFM_INCLUDE";
                this.command.Parameters.Add(p_table);*/

                this.command.Parameters.Add(nameof (p_load_file_name), OracleDbType.Varchar2, (object) p_load_file_name, ParameterDirection.Input);
        this.command.Parameters.Add(nameof (p_load_file_hash), OracleDbType.Varchar2, (object) p_load_file_hash, ParameterDirection.Input);
        oracleParameter = new OracleParameter("p_status", OracleDbType.Varchar2);
        oracleParameter.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter);
        this.command.Parameters["p_status"].Size = (int) byte.MaxValue;
        this.command.Parameters.Add("p_error_code", OracleDbType.Varchar2, ParameterDirection.Output);
        this.command.Parameters["p_error_code"].Size = (int) byte.MaxValue;
        this.command.Parameters.Add("p_error_message", OracleDbType.Varchar2, ParameterDirection.Output);
        this.command.Parameters["p_error_message"].Size = (int) byte.MaxValue;
        this.command.ExecuteNonQuery();
      }
      return oracleParameter.Value.ToString();
    }

    public string SendNotifications(string p_load_file_name, string p_load_file_hash)
    {
      OracleParameter oracleParameter = (OracleParameter) null;

            OracleParameter p_table = (OracleParameter)null;

            if (this.conn.State.ToString().Equals("Open"))
      {
        this.command = new OracleCommand(pkg_name + ".send_notifications", this.conn);
        this.command.BindByName = true;
        this.command.CommandType = CommandType.StoredProcedure;

             /*   p_table = new OracleParameter("p_table", OracleDbType.Varchar2);
                p_table.Direction = ParameterDirection.Input;
                p_table.Value = "KFM_INCLUDE";
                this.command.Parameters.Add(p_table);*/

                this.command.Parameters.Add(nameof (p_load_file_name), OracleDbType.Varchar2, (object) p_load_file_name, ParameterDirection.Input);
        this.command.Parameters.Add(nameof (p_load_file_hash), OracleDbType.Varchar2, (object) p_load_file_hash, ParameterDirection.Input);
        oracleParameter = new OracleParameter("p_status", OracleDbType.Varchar2);
        oracleParameter.Direction = ParameterDirection.Output;
        this.command.Parameters.Add(oracleParameter);
        this.command.Parameters["p_status"].Size = (int) byte.MaxValue;
        this.command.Parameters.Add("p_error_code", OracleDbType.Varchar2, ParameterDirection.Output);
        this.command.Parameters["p_error_code"].Size = (int) byte.MaxValue;
        this.command.Parameters.Add("p_error_message", OracleDbType.Varchar2, ParameterDirection.Output);
        this.command.Parameters["p_error_message"].Size = (int) byte.MaxValue;
        this.command.ExecuteNonQuery();
      }
      return oracleParameter.Value.ToString();
    }

    public void RefreshTable(string tableName)
    {
      if (!this.conn.State.ToString().Equals("Open"))
        return;
      this.command = new OracleCommand("pkg_loaders.refresh_black_list", this.conn);
      this.command.BindByName = true;
      this.command.CommandType = CommandType.StoredProcedure;
      this.command.Parameters.Add("in_table_name", OracleDbType.Varchar2, (object) tableName, ParameterDirection.Input);
      this.command.Parameters.Add("out_error", OracleDbType.Varchar2, ParameterDirection.Output);
      this.command.ExecuteNonQuery();
    }

    public void CloseDatabase()
    {
      this.conn.Close();
      this.conn.Dispose();
    }
  }
}
