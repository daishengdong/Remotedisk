using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Net.Sockets;
using RemoteDiskServer.SecurityHandle;
using RemoteDiskServer.CommonStaticVar;

namespace RemoteDiskServer.SocketHandle
{
    class FileTransferHandler
    {
        public static void receiveFile(string fullPath, DateTime lastWriteTime, Socket clientSocket)
        {
            try
            {
                byte[] receiveBytes = new byte[CommonStaticVariables.constSize];
                // 接收从客户端传来的文件
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                FileStream fileToReceive = new FileStream(fullPath, FileMode.Append, FileAccess.Write);
                BinaryWriter binarywrite = new BinaryWriter(fileToReceive);

                while (true)
                {
                    if (clientSocket.Poll(100, SelectMode.SelectRead))
                    {
                        Array.Clear(receiveBytes, 0, CommonStaticVariables.constSize);
                        int count = clientSocket.Receive(receiveBytes);

                        if (VerifyHandler.verify(receiveBytes, CommonStaticVariables.messageBeginEncrypted, count))
                        {
                            VerifyHandler.postMessage(clientSocket, CommonStaticVariables.messageDoneEncrypted);
                            continue;
                        }
                        else if (VerifyHandler.verify(receiveBytes, CommonStaticVariables.messageOverEncrypted, count))
                        {
                            binarywrite.Flush();
                            binarywrite.Close();
                            fileToReceive.Close();
                            break;
                        }
                        else
                        {
                            //将接收的流用写成文件
                            byte[] fileFragmentToWrite = EncryptionDecryptionHandler.messageDecrypt(receiveBytes);
                            binarywrite.Write(fileFragmentToWrite, 0, count);
                        }
                    }
                    else
                    {
                        if (clientSocket.Poll(100, SelectMode.SelectError))
                        {
                            break;
                        }
                    }
                }
                File.SetLastWriteTime(fullPath, lastWriteTime);
            }
            catch (Exception ex)
            {
            }
        }

        public static void postFile(Socket clientSocket, string sourceFilePath)
        {
            try
            {
                VerifyHandler.postMessage(clientSocket, CommonStaticVariables.messageBeginEncrypted);
                VerifyHandler.verify(clientSocket);

                FileStream fileToSend = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read);  //注意与receive的filestream的区别

                BinaryReader binaryreader = new BinaryReader(fileToSend);
                byte[] fileBytes = new byte[CommonStaticVariables.constSize];
                int count;
                while ((count = binaryreader.Read(fileBytes, 0, CommonStaticVariables.constSize)) != 0)                 //这个注意是将文件写成流的形式
                {
                    byte[] fileFragmentToSend = EncryptionDecryptionHandler.messageEncrypt(fileBytes);
                    clientSocket.Send(fileFragmentToSend, count, SocketFlags.None);           //发送文件流到目标机器
                }

                binaryreader.Close();
                fileToSend.Close();
                System.Threading.Thread.Sleep(200);
                VerifyHandler.postMessage(clientSocket, CommonStaticVariables.messageOverEncrypted);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
