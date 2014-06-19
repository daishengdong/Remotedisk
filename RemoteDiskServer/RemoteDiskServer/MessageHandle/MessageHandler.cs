using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using RemoteDiskServer.SQLHandle;
using RemoteDiskServer.CommonStaticVar;
using RemoteDiskServer.FileSystemHandle;
using RemoteDiskServer.SocketHandle;

namespace RemoteDiskServer.MessageHandle
{
    class MessageHandler
    {
        // 登入消息处理
        public static bool login(string account, string password, string ip, string port)
        {
            try
            {
                string homeFolder = CommonStaticVariables.homePath + account;
                string selectCmdStr = "SELECT * FROM tb_accounts WHERE account = '" + account + "' And password = '" + password + "'";
                string updateCmdStr = "UPDATE tb_accounts SET ip = '" + ip + "', port = '" + port + "', status = '" + CommonStaticVariables.statusOnline + "', home = '" + homeFolder  + "', lastTime = '" + DateTime.Now + "' WHERE account = '" + account + "' AND password = '" + password + "'";
                if (SQLHandler.DrRead(selectCmdStr))
                {
                    SQLHandler.OperateRecord(updateCmdStr);
                    if (!Directory.Exists(homeFolder))
                    {
                        FileSystemHandler.CreateDirectory(homeFolder, DateTime.Now);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }

        // 注册消息处理
        public static bool registration(string account, string password, string ip, string port)
        {
            try
            {
                string homeFolder = CommonStaticVariables.homePath + account;
                string selectCmdStr = "SELECT account FROM tb_accounts WHERE account = '" + account + "'";
                if (SQLHandler.DrRead(selectCmdStr))
                {
                    return false;
                }
                else
                {
                    string insertCmdStr = "INSERT INTO tb_accounts (account, password, ip, port, status, home, registerTime) VALUES ( '" + account + "','" + password + "', '" + ip + "', '" + port + "', '" + CommonStaticVariables.statusOffline + "', '" + homeFolder + "', '" + DateTime.Now + "')";
                    SQLHandler.OperateRecord(insertCmdStr);
                    FileSystemHandler.CreateDirectory(homeFolder, DateTime.Now);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }

        // 登出消息处理
        public static bool logout(string account, string password)
        {
            try
            {
                string updateCmdStr = "UPDATE tb_accounts SET status = '" + CommonStaticVariables.statusOffline + "' WHERE account = '" + account + "' AND password = '" + password + "'";
                SQLHandler.OperateRecord(updateCmdStr);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }

        // 递归查找当前主目录
        public static bool findFileAndSync(string home, Socket curClientSocket, string constHome)
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
                    VerifyHandler.postMessage(curClientSocket, message);
                    VerifyHandler.verify(curClientSocket);
                    findFileAndSync(fullPath, curClientSocket, constHome);
                }
                // 查找文件
                foreach (FileInfo f in Dir.GetFiles())
                {
                    fullPath = Dir + "\\" + f.ToString();
                    paraPath = fullPath.Substring(constHome.Length + 1, fullPath.Length - constHome.Length - 1);
                    // flag = 1 时表示同步的是文件
                    string lastWriteTime = File.GetLastWriteTime(fullPath).ToString();
                    byte[] message = MessageAssembler.assembleFile(paraPath, lastWriteTime);
                    VerifyHandler.postMessage(curClientSocket, message);

                    if (!VerifyHandler.verify(curClientSocket))
                    {
                        FileTransferHandler.postFile(curClientSocket, fullPath);
                        VerifyHandler.verify(curClientSocket);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }

        //开机同步
        public static bool syncOnstart(string home, Socket curClientSocket)
        {
            string paraHome = home.Substring(0, home.Length - 1);
            VerifyHandler.verify(curClientSocket);
            if (!findFileAndSync(paraHome, curClientSocket, paraHome))
            {
                return false;
            }
            // flag = 2 表示已同步完成
            VerifyHandler.postMessage(curClientSocket, MessageAssembler.assembleDone());
            return true;
        }

        public static bool syncNow(string home, Socket curClientSocket)
        {
            bool loop = true;
            string fullPath = null;
            string para1 = null;
            string para2 = null;
            DateTime lastWriteTime = new DateTime();

            byte[] receiveBytes = new byte[CommonStaticVariables.constSize];

            while (loop)
            {
                if (curClientSocket.Poll(100, SelectMode.SelectRead))
                {
                    int successReceiveBytes = curClientSocket.Receive(receiveBytes);
                    string[] parMessage;
                    if ((parMessage = MessageParser.parseFileMessage(receiveBytes)) != null)
                    {
                        string flag = parMessage[0];
                        para1 = parMessage[1];
                        para2 = parMessage[2];
                        switch (flag)
                        {
                            case "0":
                                // 文件夹
                                fullPath = home + "\\" + para1;
                                lastWriteTime = DateTime.Parse(para2);
                                if (!Directory.Exists(fullPath))
                                {
                                    Directory.CreateDirectory(fullPath);
                                }
                                Directory.SetLastWriteTime(fullPath, lastWriteTime);
                                VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                break;
                            case "1":
                                // 文件
                                fullPath = home + "\\" + para1;
                                lastWriteTime = DateTime.Parse(para2);
                                if (!File.Exists(fullPath))
                                {
                                    VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    // 接收从客户器端传来的文件
                                    FileTransferHandler.receiveFile(fullPath, lastWriteTime, curClientSocket);
                                    VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    // 接收从客户器端传来的文件
                                }
                                else
                                {
                                    if (lastWriteTime == File.GetLastWriteTime(fullPath))
                                    {
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    }
                                    else
                                    {
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                        // 接收从客户器端传来的文件
                                        FileTransferHandler.receiveFile(fullPath, lastWriteTime, curClientSocket);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
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
            return true;
        }

        // 客户端新建了目录
        public static bool createDirectory(string directoryPath, DateTime lastWriteTime)
        {
            FileSystemHandler.CreateDirectory(directoryPath, lastWriteTime);
            return true;
        }

        // 客户端做了删除操作
        public static bool delete(string deletePath)
        {
            if (Directory.Exists(deletePath))
            {
                FileSystemHandler.DeleteDirectory(deletePath);
            }
            if (File.Exists(deletePath))
            {
                FileSystemHandler.DeleteFile(deletePath);
            }
            return true;
        }

        // 客户端重命名了目录
        public static bool renameDirectory(string sourceDirPath, string destDirPath, DateTime lastWriteTime)
        {
            FileSystemHandler.RenameDirectory(sourceDirPath, destDirPath, lastWriteTime);
            return true;
        }

        // 客户端修改了文件
        public static bool modifyFile(string sourceFilePath, DateTime lastWriteTime, Socket curClientSocket)
        {
            try
            {
                FileTransferHandler.receiveFile(sourceFilePath, lastWriteTime, curClientSocket);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }

        // 客户端新建了文件
        public static bool createFile(string filePath, DateTime lastWriteTime)
        {
            FileSystemHandler.CreateFile(filePath, lastWriteTime);
            return true;
        }

        // 客户端重命名了文件
        public static bool renameFile(string sourceFilePath, string destFilePath, DateTime lastWriteTime)
        {
            FileSystemHandler.RenameFile(sourceFilePath, destFilePath, lastWriteTime);
            return true;
        }
    }
}