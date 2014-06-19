using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using RemoteDiskClient.MessageHandle;
using RemoteDiskClient.CommonStaticVar;
using RemoteDiskClient.SocketHandle;

namespace RemoteDiskClient.SocketHandle
{
    class ServerHandler
    {
        private static IPAddress HostIP = IPAddress.Parse(CommonStaticVariables.HostIp);
        private static IPEndPoint IPE = new IPEndPoint(HostIP, CommonStaticVariables.Port);
        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static void connectServer()
        {
            try
            {
                if (!clientSocket.Connected)
                {
                    clientSocket.Connect(IPE);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public static bool login(string account, string password)
        {
            connectServer();
            byte[] message = MessageAssembler.loginMsgAssemble(account, password);
            VerifyHandler.postMessage(clientSocket, message);

            if (!VerifyHandler.verify(clientSocket))
            {
                return false;
            }
            return true;
        }

        public static bool logout(string account, string password)
        {
            if (!clientSocket.Connected)
            {
                return true;
            }

            byte[] message = MessageAssembler.logoutMsgAssemble(account, password);
            VerifyHandler.postMessage(clientSocket, message);

            if (!VerifyHandler.verify(clientSocket))
            {
                return false;
            }
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            return true;
        }

        public static bool regist(string account, string password)
        {
            connectServer();
            byte[] message = MessageAssembler.registMsgAssemble(account, password);
            VerifyHandler.postMessage(clientSocket, message);

            if (!VerifyHandler.verify(clientSocket))
            {
                return false;
            }
            return true;
        }

        public static bool createDirectory(string path, string lastWriteTime)
        {
            connectServer();
            byte[] message = MessageAssembler.createDirectory(path, lastWriteTime);
            VerifyHandler.postMessage(clientSocket, message);

            if (!VerifyHandler.verify(clientSocket))
            {
                return false;
            }
            return true;
        }

        public static bool createFile(string path, string lastWriteTime)
        {
            connectServer();
            byte[] message = MessageAssembler.createFile(path, lastWriteTime);
            VerifyHandler.postMessage(clientSocket, message);

            if (!VerifyHandler.verify(clientSocket))
            {
                return false;
            }
            return true;
        }

        public static bool delete(string path)
        {
            connectServer();
            byte[] message = MessageAssembler.delete(path);
            VerifyHandler.postMessage(clientSocket, message);

            if (!VerifyHandler.verify(clientSocket))
            {
                return false;
            }
            return true;
        }

        public static bool renameDirectory(string sourceDirPath, string destDirPath, string lastWriteTime)
        {
            connectServer();
            byte[] message = MessageAssembler.renameDirectory(sourceDirPath, destDirPath, lastWriteTime);
            VerifyHandler.postMessage(clientSocket, message);

            if (!VerifyHandler.verify(clientSocket))
            {
                return false;
            }
            return true;
        }

        public static bool renameFile(string sourceFilePath, string destFilePath, string lastWriteTime)
        {
            connectServer();
            byte[] message = MessageAssembler.renameFile(sourceFilePath, destFilePath, lastWriteTime);
            VerifyHandler.postMessage(clientSocket, message);

            if (!VerifyHandler.verify(clientSocket))
            {
                return false;
            }
            return true;
        }

        public static bool modifyFile(string sourceFilePath, string fullPath, string lastWriteTime)
        {
            connectServer();
            byte[] message = MessageAssembler.modifyFile(sourceFilePath, lastWriteTime);
            VerifyHandler.postMessage(clientSocket, message);

            VerifyHandler.verify(clientSocket);
            FileTransferHandler.postFile(clientSocket, fullPath);

            if (!VerifyHandler.verify(clientSocket))
            {
                return false;
            }
            return true;
        }

        public static void syncOnstart()
        {
            bool loop = true;
            string fullPath = null;
            string para1 = null;
            string para2 = null;
            DateTime lastWriteTime = new DateTime();
            byte[] message = MessageAssembler.syncOnstart();

            connectServer();
            VerifyHandler.postMessage(clientSocket, message);
            VerifyHandler.verify(clientSocket);
            VerifyHandler.postMessage(clientSocket, CommonStaticVariables.messageDoneEncrypted);

            byte[] receiveBytes = new byte[CommonStaticVariables.constSize];

            while (loop)
            {
                if (clientSocket.Poll(100, SelectMode.SelectRead))
                {
                    int successReceiveBytes = clientSocket.Receive(receiveBytes);
                    string[] parMessage;
                    if ((parMessage = MessageParser.parse(receiveBytes)) != null)
                    {
                        string flag = parMessage[0];
                        para1 = parMessage[1];
                        para2 = parMessage[2];
                        switch (flag)
                        {
                            case "0":
                                // 文件夹
                                fullPath = CommonStaticVariables.homePath + para1;
                                lastWriteTime = DateTime.Parse(para2);
                                if (!Directory.Exists(fullPath))
                                {
                                    Directory.CreateDirectory(fullPath);
                                }
                                Directory.SetLastWriteTime(fullPath, lastWriteTime);
                                VerifyHandler.postMessage(clientSocket, CommonStaticVariables.messageDoneEncrypted);
                                break;
                            case "1":
                                // 文件
                                fullPath = CommonStaticVariables.homePath + para1;
                                lastWriteTime = DateTime.Parse(para2);
                                if (!File.Exists(fullPath))
                                {
                                    VerifyHandler.postMessage(clientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    // 接收从服务器端传来的文件
                                    FileTransferHandler.receiveFile(fullPath, lastWriteTime, clientSocket);
                                    VerifyHandler.postMessage(clientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    // 接收从服务器端传来的文件
                                }
                                else
                                {
                                    if (lastWriteTime == File.GetLastWriteTime(fullPath))
                                    {
                                        VerifyHandler.postMessage(clientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    }
                                    else
                                    {
                                        VerifyHandler.postMessage(clientSocket, CommonStaticVariables.messageFailedEncrypted);
                                        // 接收从服务器端传来的文件
                                        FileTransferHandler.receiveFile(fullPath, lastWriteTime, clientSocket);
                                        VerifyHandler.postMessage(clientSocket, CommonStaticVariables.messageDoneEncrypted);
                                        // 接收从服务器端传来的文件
                                    }
                                }
                                break;
                            case "2":
                                // 同步完成
                                loop = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        // 递归查找当前主目录
        public static void findFileAndSync(string home, string constHome)
        {
            string fullPath = null;
            string paraPath = null;
            DirectoryInfo Dir = new DirectoryInfo(home);
            try
            {
                // 查找子目录
                foreach (DirectoryInfo d in Dir.GetDirectories())
                {
                    fullPath = Dir + "\\" + d.ToString();
                    paraPath = fullPath.Substring(constHome.Length + 1, fullPath.Length - constHome.Length - 1);
                    // flag = 0 时表示同步的是文件夹
                    string lastWriteTime = Directory.GetLastWriteTime(fullPath).ToString();
                    byte[] message = MessageAssembler.assembleDir(paraPath, lastWriteTime);
                    VerifyHandler.postMessage(clientSocket, message);
                    VerifyHandler.verify(clientSocket);
                    findFileAndSync(fullPath, constHome);
                }
                // 查找文件
                foreach (FileInfo f in Dir.GetFiles())
                {
                    fullPath = Dir + "\\" + f.ToString();
                    paraPath = fullPath.Substring(constHome.Length + 1, fullPath.Length - constHome.Length - 1);
                    // flag = 1 时表示同步的是文件
                    string lastWriteTime = File.GetLastWriteTime(fullPath).ToString();
                    byte[] message = MessageAssembler.assembleFile(paraPath, lastWriteTime);
                    VerifyHandler.postMessage(clientSocket, message);

                    if (!VerifyHandler.verify(clientSocket))
                    {
                        FileTransferHandler.postFile(clientSocket, fullPath);
                        VerifyHandler.verify(clientSocket);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public static void syncNow()
        {
            connectServer();
            string paraHome = CommonStaticVariables.homePath.Substring(0, CommonStaticVariables.homePath.Length - 1);
            byte[] message = MessageAssembler.syncNow();

            VerifyHandler.postMessage(clientSocket, message);
            VerifyHandler.verify(clientSocket);

            findFileAndSync(paraHome, paraHome);
            // flag = 2 表示已同步完成
            VerifyHandler.postMessage(clientSocket, MessageAssembler.assembleDone());
        }      
    }
}