using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Collections;
using RemoteDiskServer.ConfigureHandle;
using RemoteDiskServer.SecurityHandle;

namespace RemoteDiskServer.CommonStaticVar
{
    class CommonStaticVariables
    {
        // 数据库部分
        public static string dataBase = "SKY-LAPTOP\\SQLEXPRESS";
        public static string connectString = "Data Source = " + dataBase + ";Initial Catalog = db_remoteDisk;Trusted_Connection = yes;Integrated Security = true";
        public static string synSelectString = "SELECT account AS '账户', ip AS 'IP', port AS '端口', status AS '状态', home AS '主目录', lastTime AS '上次登录', registerTime AS '注册时间' FROM tb_accounts";
        public static string statusOnline = "在线";
        public static string statusOffline = "离线";

        // Socket 部分
        public static string HostIp = "127.0.0.1";
        public static int Port = 82;
        public static int maxClientCount = 10;
        public static int constSize = 5 + 255 * 3;

        // 客户端消息处理部分
        public static Hashtable messageMap = new Hashtable();
        private static byte[] messageDone = Encoding.UTF8.GetBytes("DONE");
        private static byte[] messageFailed = Encoding.UTF8.GetBytes("FAIL");
        public static byte[] messageDoneEncrypted = EncryptionDecryptionHandler.messageEncrypt(messageDone);
        public static byte[] messageFailedEncrypted = EncryptionDecryptionHandler.messageEncrypt(messageFailed);
        private static byte[] messageBegin = Encoding.UTF8.GetBytes("BEGIN");
        private static byte[] messageOver = Encoding.UTF8.GetBytes("OVER");
        public static byte[] messageBeginEncrypted = EncryptionDecryptionHandler.messageEncrypt(messageBegin);
        public static byte[] messageOverEncrypted = EncryptionDecryptionHandler.messageEncrypt(messageOver);

        // 服务器本地文件系统部分
        public static string homePath = @"D:\RemoteDiskServer\";

        // 配置文件地址
        public static string iniFilePath = Environment.CurrentDirectory + @"\configure.ini";

        public static void writeDefaultConfiguration()
        {
            ConfigureHandler.WriteIniData("Default", "IP", "127.0.0.1");
            ConfigureHandler.WriteIniData("Default", "Port", Convert.ToString(82));
            ConfigureHandler.WriteIniData("Default", "MaxClientCount", Convert.ToString(10));
            ConfigureHandler.WriteIniData("Default", "DataBase", "SKY-LAPTOP\\SQLEXPRESS");
            ConfigureHandler.WriteIniData("Default", "Home", @"D:\RemoteDiskServer\");
        }

        public static void loadDefaultConfiguration()
        {
            HostIp = ConfigureHandler.ReadIniData("Default", "IP");
            Port = Convert.ToInt32(ConfigureHandler.ReadIniData("Default", "Port"));
            maxClientCount = Convert.ToInt32(ConfigureHandler.ReadIniData("Default", "MaxClientCount"));
            dataBase = ConfigureHandler.ReadIniData("Default", "DataBase");
            connectString = "Data Source = " + dataBase + ";Initial Catalog = db_remoteDisk;Trusted_Connection = yes;Integrated Security = true";
            homePath = ConfigureHandler.ReadIniData("Default", "Home");
        }

        public static void loadConfiguration()
        {
            string ip = ConfigureHandler.ReadIniData("Configuration", "IP");
            string port = ConfigureHandler.ReadIniData("Configuration", "Port");
            string p_maxClientCount = ConfigureHandler.ReadIniData("Configuration", "MaxClientCount");
            string p_dataBase = ConfigureHandler.ReadIniData("Configuration", "DataBase");
            string home = ConfigureHandler.ReadIniData("Configuration", "Home");
            if (ip != string.Empty || port != string.Empty || p_maxClientCount != string.Empty || p_dataBase != string.Empty || home != string.Empty)
            {
                if (ip != string.Empty)
                {
                    HostIp = ip;
                }
                if (port != string.Empty)
                {
                    Port = Convert.ToInt32(port);
                }
                if (p_maxClientCount != string.Empty)
                {
                    maxClientCount = Convert.ToInt32(p_maxClientCount);
                }
                if (p_dataBase != string.Empty)
                {
                    dataBase = p_dataBase;
                    connectString = "Data Source = " + dataBase + ";Initial Catalog = db_remoteDisk;Trusted_Connection = yes;Integrated Security = true";
                }
                if (home != string.Empty)
                {
                    homePath = home;
                }
            }
            else
            {
                loadDefaultConfiguration();
            }
        }

        public static void init()
        {
            writeDefaultConfiguration();
            loadConfiguration();

            messageMap.Add(0x00, "login");
            messageMap.Add(0x01, "regist");
            messageMap.Add(0x02, "logout");
            messageMap.Add(0x10, "syncOnstart");
            messageMap.Add(0x11, "syncNow");

            messageMap.Add(0x20, "createDirectory");
            messageMap.Add(0x21, "delete");
            messageMap.Add(0x22, "renameDirectory");

            messageMap.Add(0x23, "modifyFile");
            messageMap.Add(0x24, "createFile");
            messageMap.Add(0x26, "renameFile");
        }
    }
}