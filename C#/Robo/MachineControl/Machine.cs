﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MachineControl
{
    public enum MC_WORKING_CONDITIONS
    {
        NONE,
        RO_ON_WORK = 1,
        TANK_FULL = 2,
        SAND_FILTER_BACKWASH = 3,
        CARBON_FILTER_BACKWASH = 4,
        FLUSHING = 5,
        NO_NETWORK = 6,
        ERROR_1 = 7,
        ERROR_2 = 8
    }
    public enum MCFormat_ERROR { NULL, FORMAT_MISMATCH, FORMAT_NOT_ACCEPTED, INVALID_FORMAT, SEPARATOR_NOT_FOUND, PARAMETER_COUNT }
    public class MCStatus
    {
        public MCStatus(String _status,List<Parameter> getAllStatus,String format)
        {
            try
            {
                StatusFormat = format;
                GetAllStatusParameters = getAllStatus;
                Error = true;
                ErrorMessage = MCFormat_ERROR.NULL;
                String fmt = StatusFormat;
                String extraSymbol = "";
                inputString = _status;
                parameters = GetAllStatusParameters;
                String tmp = fmt;
                bool flag = false;
                if (fmt.StartsWith("{"))
                {
                    flag = true;
                }
                if (!flag)
                {
                    int firstIndex = fmt.IndexOf("{");
                    if (firstIndex > 0)
                    {
                        extraSymbol = fmt.Substring(0, firstIndex);
                        if (inputString.StartsWith(extraSymbol))
                        {
                            tmp = inputString.Replace(extraSymbol, "").Trim();
                        }
                        else
                        {
                            ErrorMessage = MCFormat_ERROR.FORMAT_NOT_ACCEPTED;
                        }
                    }
                    else
                    {
                        ErrorMessage = MCFormat_ERROR.INVALID_FORMAT;
                    }
                }
                if (ErrorMessage == MCFormat_ERROR.NULL)
                {
                    String divider = "";
                    try
                    {
                        int fi = fmt.IndexOf("}");
                        int ti = fmt.IndexOf("{", fi + 1);
                        divider = fmt.Substring(fi + 1, ti - fi - 1);
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = MCFormat_ERROR.SEPARATOR_NOT_FOUND;
                    }
                    if (ErrorMessage == MCFormat_ERROR.NULL)
                    {
                        String _fmt = fmt;
                        String[] inArr = tmp.Split(new String[] { divider }, StringSplitOptions.None);
                        if (inArr.Length == parameters.Count)
                        {
                            for (int i = 0; i < inArr.Length; i++)
                            {
                                for (int j = 0; j < parameters.Count; j++)
                                {
                                    if (parameters[j].Order.Trim() == (i + 1).ToString())
                                    {
                                        parameters[j].Value = inArr[i];
                                        _fmt = _fmt.Replace("{" + parameters[j].Order + "}", inArr[i]);
                                    }
                                }
                            }
                            if (_fmt.Trim().Equals(inputString))
                            {
                                Error = false;
                            }
                            else
                            {
                                ErrorMessage = MCFormat_ERROR.FORMAT_MISMATCH;
                            }
                        }
                        else
                        {
                            ErrorMessage = MCFormat_ERROR.PARAMETER_COUNT;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
            }
            if (Error)
            {
                parameters = new List<Parameter>();
            }
        }

        public bool Error { get; set; }

        public List<Parameter> GetAllStatusParameters { get; set; }
        public String StatusFormat { get; set; }
        public MCFormat_ERROR ErrorMessage { get; set; }
        public String inputString { get; set; }
        public List<Parameter> parameters { get; set; }
    }
    public class MCSettings
    {
        public MCSettings(String _setting,List<Parameter> getAllSettings,String format)
        {
            inputString = _setting;
            SettingFormat = format;
            GetAllSettingsParameters = getAllSettings;
            try
            {
                Error = true;
                ErrorMessage = MCFormat_ERROR.NULL;
                String fmt = SettingFormat;
                String extraSymbol = "";
                parameters = GetAllSettingsParameters;
                String tmp = fmt;
                bool flag = false;
                if (fmt.StartsWith("{"))
                {
                    flag = true;
                }
                if (!flag)
                {
                    int firstIndex = fmt.IndexOf("{");
                    if (firstIndex > 0)
                    {
                        extraSymbol = fmt.Substring(0, firstIndex);
                        if (inputString.StartsWith(extraSymbol))
                        {
                            tmp = inputString.Replace(extraSymbol, "").Trim();
                        }
                        else
                        {
                            ErrorMessage = MCFormat_ERROR.FORMAT_NOT_ACCEPTED;
                        }
                    }
                    else
                    {
                        ErrorMessage = MCFormat_ERROR.INVALID_FORMAT;
                    }
                }
                if (ErrorMessage == MCFormat_ERROR.NULL)
                {
                    String divider = "";
                    try
                    {
                        int fi = fmt.IndexOf("}");
                        int ti = fmt.IndexOf("{", fi + 1);
                        divider = fmt.Substring(fi + 1, ti - fi - 1);
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = MCFormat_ERROR.SEPARATOR_NOT_FOUND;
                    }
                    if (ErrorMessage == MCFormat_ERROR.NULL)
                    {
                        String _fmt = fmt;
                        String[] inArr = tmp.Split(new String[] { divider }, StringSplitOptions.None);
                        if (inArr.Length == parameters.Count)
                        {
                            for (int i = 0; i < inArr.Length; i++)
                            {
                                for (int j = 0; j < parameters.Count; j++)
                                {
                                    if (parameters[j].Order.Trim() == (i + 1).ToString())
                                    {
                                        parameters[j].Value = inArr[i];
                                        _fmt = _fmt.Replace("{" + parameters[j].Order + "}", inArr[i]);
                                    }
                                }
                            }
                            if (_fmt.Trim().Equals(inputString))
                            {
                                Error = false;
                            }
                            else
                            {
                                ErrorMessage = MCFormat_ERROR.FORMAT_MISMATCH;
                            }
                        }
                        else
                        {
                            ErrorMessage = MCFormat_ERROR.PARAMETER_COUNT;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error = true;
            }
        }

        public bool Error { get; set; }

        public List<Parameter> GetAllSettingsParameters { get; set; }
        public String SettingFormat { get; set; }
        public MCFormat_ERROR ErrorMessage { get; set; }
        public List<Parameter> parameters { get; set; }
        public String inputString { get; set; }
    }

    public enum MCError { NULL = -1, PARSE_FAILED, SQL_ERROR, INVALID_IDENTIFIER, STATUS_ERROR, SETTING_ERROR }

    public class Machine
    {
        public static String ErrorMessage = "";
        public Machine()
        {
            Identifier = ClientIdentifier = SiteIdentifier = -1;
            MachineIdentifier = "";
            ModemSIMNumber = 0;
            MachineType = null;
            WantToDisplay = null;
            Status = null;
            Settings = null;
            LastUpdated = null;
            Ready = false;
            Error = MCError.NULL;
            HealthySetting = "";
        }
        public Machine(String MachineIdentifier,String ConnectionString,List<Parameter> getAllStatusParameters,List<Parameter> getAllSettingParameters,String statusFormat,String settingFormat)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select m.id,m.mctype,m.client_id,m.site_id,m.mc_id,m.modem_sim,m.want_to_display,m.last_updated,m.status,m.settings,m.healthy_setting,s.name as sitename from machines as m,(select id,name from sites) as s where m.site_id=s.id and m.mc_id LIKE '" + MachineIdentifier + "'", conn);
                cmd.Parameters.Add("@mid", System.Data.SqlDbType.Text).Value = MachineIdentifier;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    try
                    {
                        Identifier = Int32.Parse(dr[0].ToString());
                        MachineType = dr[1].ToString();
                        ClientIdentifier = Int32.Parse(dr[2].ToString());
                        SiteIdentifier = Int32.Parse(dr[3].ToString());
                        SiteName=dr["sitename"].ToString();
                        this.MachineIdentifier = dr[4].ToString();
                        ModemSIMNumber = Decimal.Parse(dr[5].ToString());
                        WantToDisplay = new List<string>();
                        String __temp = dr[6].ToString();
                        String[] __arr = __temp.Split(new String[] { "," }, StringSplitOptions.None);
                        foreach (String ___temp in __arr)
                            WantToDisplay.Add(___temp);
                        LastUpdated = dr[7].ToString();
                        Status = new MCStatus(dr[8].ToString(),getAllStatusParameters,statusFormat);
                        Settings = new MCSettings(dr[9].ToString(),getAllSettingParameters,settingFormat);
                        HealthySetting = dr[10].ToString();
                        if (Status.Error)
                        {
                            Ready = true;
                            Error = MCError.STATUS_ERROR;
                        }
                        else if (Settings.Error)
                        {
                            Ready = true;
                            Error = MCError.SETTING_ERROR;
                        }
                        else
                        {
                            Ready = true;
                            Error = MCError.NULL;
                        }
                    }
                    catch (Exception ex)
                    {
                        Error = MCError.PARSE_FAILED;
                        ErrorMessage = ex.Message;
                    }
                }
                else
                {
                    Ready = false;
                    Error = MCError.INVALID_IDENTIFIER;
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Ready = false;
                Error = MCError.SQL_ERROR;
                ErrorMessage = ex.Message;
            }
        }

        public int Identifier { get; set; }
        public String MachineType { get; set; }
        public int ClientIdentifier { get; set; }
        public int SiteIdentifier { get; set; }
        public String SiteName { get; set; }
        public String MachineIdentifier { get; set; }
        public Decimal ModemSIMNumber { get; set; }
        public List<String> WantToDisplay { get; set; }
        public String LastUpdated { get; set; }
        public MCStatus Status { get; set; }
        public MCSettings Settings { get; set; }
        public bool Ready { get; set; }
        public MCError Error { get; set; }
        public String HealthySetting { get; set; }
    }

    public class Parameter
    {

        public Parameter(String name, String format, String order, String category, bool checkForHealthy)
        {
            Name = name;
            Format = format;
            Order = order;
            Category = category;
            CheckForHealthy = checkForHealthy;
        }
        public Parameter(String name, String format, String order, String category, String value, bool checkForHealthy)
        {
            Name = name;
            Format = format;
            Order = order;
            Category = category;
            Value = value;
            CheckForHealthy = checkForHealthy;
        }

        public string Name { get; set; }
        public string Format { get; set; }
        public string Order { get; set; }
        public string Category { get; set; }
        public bool CheckForHealthy { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
