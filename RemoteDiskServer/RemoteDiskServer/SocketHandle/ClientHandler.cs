using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using RemoteDiskServer.CommonStaticVar;
using RemoteDiskServer.MessageHandle;
using RemoteDiskServer.SQLHandle;

using System.Data;

namespace RemoteDiskServer.SocketHandle
{
    class ClientHandler
    {
        // ClientSocket 用于处理客户端连接请求的 Socket  
        private Socket clientSocket = null;
        private Socket serverSocket = null;
        // 响应的客户端 Socket 套接字队列
        private static List<Socket> clientSocketList = new List<Socket>(CommonStaticVariables.maxClientCount);

        private static MainFrm insMainFrm = null;
        private delegate void promoteDelegate(string entry, string account, string ip, string port);
        private static promoteDelegate insPromoteDelegate = new promoteDelegate(promoteUI);
        private delegate void bindDelegate();

        public ClientHandler(MainFrm mFrm)
        {
            insMainFrm = mFrm;
        }

        public void shutdown()
        {
            this.serverSocket.Close();
        }

        public void listen()
        {
            IPAddress HostIp = IPAddress.Parse(CommonStaticVariables.HostIp);

            //创建一个网络端点  
            IPEndPoint IPE = new IPEndPoint(HostIp, CommonStaticVariables.Port);

            //创建服务端服务端套接字
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //将套接字与网络端点绑定  
            serverSocket.Bind(IPE);

            //将套接字置为侦听状态，并设置最大队列数为 CommonStaticVariables.maxClientCount  
            serverSocket.Listen(CommonStaticVariables.maxClientCount);

            while (true)
            {
                //以同步方式从侦听套接字的连接请求队列中提取第一个挂起的连接请求，然后创建并返回新的 Socket  
                //新的套接字：包含对方计算机的IP和端口号，可使用这个套接字与本机进行通信    
                clientSocket = serverSocket.Accept();

                if (clientSocket != null)
                {
                    if (clientSocketList.Count <= CommonStaticVariables.maxClientCount)
                    {
                        // 进队
                        clientSocketList.Add(clientSocket);
                        Thread handleTrd = new Thread(ClientHandler.handleFunction);
                        handleTrd.IsBackground = true;
                        handleTrd.Start(clientSocket);
                    }
                }
            }
        }

        public static void promoteUI(string entry, string account, string ip, string port)
        {
            insMainFrm.tsslableEntry.Text = entry;
            insMainFrm.tsslableAccount.Text = account;
            insMainFrm.tsslableIP.Text = ip;
            insMainFrm.tsslablePort.Text = port;
            bindDelegate insBindDelegate = new bindDelegate(insMainFrm.bindData);
            insMainFrm.Invoke(insBindDelegate);
        }

        public static void handleFunction(object paraClientSocket)
        {
            Socket curClientSocket = (Socket)paraClientSocket;
            // 当前用户
            byte[] receiveBytes = new byte[CommonStaticVariables.constSize];
            IPEndPoint tempIPE = (IPEndPoint)curClientSocket.RemoteEndPoint;
            string ip = tempIPE.Address.ToString();
            string port = tempIPE.Port.ToString();
            string account = null;
            string password = null;
            // 当前用户主目录
            string home = CommonStaticVariables.homePath;
            // 当前要操作的对象路径
            string objPath = null;

            // 当前操作对象最近修改时间
            DateTime lastWriteTime = new DateTime();

            // 针对两个参数的函数设计的变量
            string para1 = null;
            string para2 = null;
            string para3 = null;
            string sourcePath = null;
            string destPath = null;

            bool loop = true;

            try
            {
                while (loop)
                {
                    if (curClientSocket.Poll(100, SelectMode.SelectRead))
                    {
                        Array.Clear(receiveBytes, 0, CommonStaticVariables.constSize);
                        int successReceiveBytes = curClientSocket.Receive(receiveBytes);
                        string[] parMessage;
                        if ((parMessage = MessageParser.parse(receiveBytes)) != null)
                        {
                            string flag = parMessage[0];
                            switch (flag)
                            {
                                case "login":
                                    // 登录验证
                                    account = parMessage[1];
                                    password = parMessage[2];
                                    if (MessageHandler.login(account, password, ip, port))
                                    {
                                        home += account + "\\";
                                        insPromoteDelegate.Invoke("用户接入", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("用户接入失败", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    }
                                    break;
                                case "regist":
                                    account = parMessage[1];
                                    password = parMessage[2];
                                    if (MessageHandler.registration(account, password, ip, port))
                                    {
                                        insPromoteDelegate.Invoke("用户注册", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("用户注册失败", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    }
                                    break;
                                case "logout":
                                    if (MessageHandler.logout(account, password))
                                    {
                                        insPromoteDelegate.Invoke("用户下线", account, ip, port);

                                        loop = false;
                                        clientSocketList.Remove(curClientSocket);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                        curClientSocket.Shutdown(SocketShutdown.Both);
                                        curClientSocket.Close();
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("用户下线失败", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    }
                                    break;
                                case "syncOnstart":
                                    VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    if (MessageHandler.syncOnstart(home, curClientSocket))
                                    {
                                        insPromoteDelegate.Invoke("开机同步完成", account, ip, port);
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("开机同步失败", account, ip, port);
                                    }
                                    break;
                                case "syncNow":
                                    VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    if (MessageHandler.syncNow(home, curClientSocket))
                                    {
                                        insPromoteDelegate.Invoke("同步完成", account, ip, port);
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("同步失败", account, ip, port);
                                    }
                                    break;
                                case "createDirectory":
                                    para1 = parMessage[1];
                                    para2 = parMessage[2];
                                    objPath = home + para1;
                                    lastWriteTime = DateTime.Parse(para2);
                                    if (MessageHandler.createDirectory(objPath, lastWriteTime))
                                    {
                                        insPromoteDelegate.Invoke("创建目录", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("创建目录失败", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    }
                                    break;
                                case "delete":
                                    para1 = parMessage[1];
                                    objPath = home + para1;
                                    if (MessageHandler.delete(objPath))
                                    {
                                        insPromoteDelegate.Invoke("删除项目", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("删除项目失败", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    }
                                    break;
                                case "renameDirectory":
                                    para1 = parMessage[1];
                                    para2 = parMessage[2];
                                    para3 = parMessage[3];
                                    sourcePath = home + para1;
                                    destPath = home + para2;
                                    lastWriteTime = DateTime.Parse(para3);
                                    if (MessageHandler.renameDirectory(sourcePath, destPath, lastWriteTime))
                                    {
                                        insPromoteDelegate.Invoke("重命名目录", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("重命名目录失败", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    }
                                    break;
                                case "modifyFile":
                                    para1 = parMessage[1];
                                    para2 = parMessage[2];
                                    objPath = home + para1;
                                    lastWriteTime = DateTime.Parse(para2);
                                    VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    if (MessageHandler.modifyFile(objPath, lastWriteTime, curClientSocket))
                                    {
                                        insPromoteDelegate.Invoke("修改文件", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("修改文件失败", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    }
                                    break;
                                case "createFile":
                                    para1 = parMessage[1];
                                    para2 = parMessage[2];
                                    objPath = home + para1;
                                    lastWriteTime = DateTime.Parse(para2);
                                    if (MessageHandler.createFile(objPath, lastWriteTime))
                                    {
                                        insPromoteDelegate.Invoke("创建文件", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("创建文件失败", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    }
                                    break;
                                case "renameFile":
                                    para1 = parMessage[1];
                                    para2 = parMessage[2];
                                    para3 = parMessage[3];
                                    sourcePath = home + para1;
                                    destPath = home + para2;
                                    lastWriteTime = DateTime.Parse(para3);
                                    if (MessageHandler.renameFile(sourcePath, destPath, lastWriteTime))
                                    {
                                        insPromoteDelegate.Invoke("重命名文件", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageDoneEncrypted);
                                    }
                                    else
                                    {
                                        insPromoteDelegate.Invoke("重命名文件失败", account, ip, port);
                                        VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    }
                                    break;
                                default:
                                    VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                                    break;
                            }
                        }
                        else
                        {
                            // 客户端传递非法格式的信息
                            VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                        }
                    }
                    else
                    {
                        if (curClientSocket.Poll(100, SelectMode.SelectError))
                        {
                            loop = false;
                            clientSocketList.Remove(curClientSocket);
                            curClientSocket.Shutdown(SocketShutdown.Both);
                            curClientSocket.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is SocketException)
                {
                    MessageHandler.logout(account, password);
                    insPromoteDelegate.Invoke("非正常断开链接", account, ip, port);
                    clientSocketList.Remove(curClientSocket);
                    curClientSocket.Shutdown(SocketShutdown.Both);
                    curClientSocket.Close();
                }
                else
                {
                    VerifyHandler.postMessage(curClientSocket, CommonStaticVariables.messageFailedEncrypted);
                }
            }
        }
    }
}