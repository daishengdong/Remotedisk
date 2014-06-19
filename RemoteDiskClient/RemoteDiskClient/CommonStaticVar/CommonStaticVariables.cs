using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using RemoteDiskClient.ConfigureHandle;
using RemoteDiskClient.SecurityHandle;

namespace RemoteDiskClient.CommonStaticVar
{
    class CommonStaticVariables
    {
        // Socket 部分
        public static string HostIp = "127.0.0.1";
        public static int Port = 82;
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

        // 客户端本地文件系统部分
        public static string homePath = @"D:\RemoteDiskClient\";

        // 配置文件地址
        public static string iniFilePath = Environment.CurrentDirectory + @"\configure.ini";

        public static void writeDefaultConfiguration()
        {
            ConfigureHandler.WriteIniData("Default", "IP", "127.0.0.1");
            ConfigureHandler.WriteIniData("Default", "Port", Convert.ToString(82));
            ConfigureHandler.WriteIniData("Default", "Home", @"D:\RemoteDiskClient\");
        }

        public static void loadDefaultConfiguration()
        {
            HostIp = ConfigureHandler.ReadIniData("Default", "IP");
            Port = Convert.ToInt32(ConfigureHandler.ReadIniData("Default", "Port"));
            homePath = ConfigureHandler.ReadIniData("Default", "Home");
        }

        public static void loadConfiguration()
        {
            string ip = ConfigureHandler.ReadIniData("Configuration", "IP");
            string port = ConfigureHandler.ReadIniData("Configuration", "Port");
            string home = ConfigureHandler.ReadIniData("Configuration", "Home");
            if (ip != string.Empty || port != string.Empty || home != string.Empty)
            {
                if (ip != string.Empty)
                {
                    HostIp = ip;
                }
                else
                {
                    HostIp = ConfigureHandler.ReadIniData("Default", "IP");
                }

                if (port != string.Empty)
                {
                    Port = Convert.ToInt32(port);
                }
                else
                {
                    Port = Convert.ToInt32(ConfigureHandler.ReadIniData("Default", "Port"));
                }

                if (home != string.Empty)
                {
                    homePath = home;
                }
                else
                {
                    homePath = ConfigureHandler.ReadIniData("Default", "Home");
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

            messageMap.Add("login", 0x00);
            messageMap.Add("regist", 0x01);
            messageMap.Add("logout", 0x02);
            messageMap.Add("syncOnstart", 0x10);
            messageMap.Add("syncNow", 0x11);

            messageMap.Add("createDirectory", 0x20);
            messageMap.Add("delete", 0x21);
            messageMap.Add("renameDirectory", 0x22);

            messageMap.Add("modifyFile", 0x23);
            messageMap.Add("createFile", 0x24);
            messageMap.Add("renameFile", 0x26);
        }
    }
}