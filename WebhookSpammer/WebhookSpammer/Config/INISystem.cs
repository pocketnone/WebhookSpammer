﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static WebhookSpammer.Config.NONELoggs;

namespace WebhookSpammer.Config
{
    public class INISystem
    {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public INISystem(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public int ReadInt(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            int Value = 0;
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            try
            {
                Value = int.Parse(RetVal.ToString());
            }
            catch
            {
                WriteState($"Error Read Int in Key: {Key}");
            }
           
            return Value;
        }
        
        public bool ReadBool(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            bool Value = false;
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            try
            {
                Value = bool.Parse(RetVal.ToString());
            }
            catch
            {
                WriteState($"Error Read Bool in Key: {Key}");
            }
            
            return Value;
        }
        
        public void Write(string Key, string Value, string Section = null) => WritePrivateProfileString(Section ?? EXE, Key, Value, Path);

        public void DeleteKey(string Key, string Section = null) =>  Write(Key, null, Section ?? EXE);

        public void DeleteSection(string Section = null) =>  Write(null, null, Section ?? EXE);

        public bool KeyExists(string Key, string Section = null) => Read(Key, Section).Length > 0;
        
    }
}