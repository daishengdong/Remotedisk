using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.InteropServices;
using RemoteDiskClient.CommonStaticVar;

namespace RemoteDiskClient.ConfigureHandle
{
    class ConfigureHandler
    {
        [DllImport("kernel32")] //返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")] //返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        // 读 ini 文件
        public static string ReadIniData(string Section, string Key)
        {
            if (ExistINIFile())
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, "", temp, 1024, CommonStaticVariables.iniFilePath);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        // 写 ini 文件
        public static void WriteIniData(string Section, string Key, string Value)
        {
            if (!ExistINIFile())
            {
                FileStream iniFile = new FileStream(CommonStaticVariables.iniFilePath, FileMode.Create, FileAccess.Write);
                iniFile.Close();
            }
            WritePrivateProfileString(Section, Key, Value, CommonStaticVariables.iniFilePath);
        }

        public static bool ExistINIFile()
        {
            return File.Exists(CommonStaticVariables.iniFilePath);
        }
    }
}