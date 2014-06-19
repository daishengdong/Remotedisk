using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

using RemoteDiskClient.CommonStaticVar;

namespace RemoteDiskClient.SocketHandle
{
    class VerifyHandler
    {
        public static bool verify(Socket clientSocket)
        {
            try
            {
                while (true)
                {
                    if (clientSocket.Poll(100, SelectMode.SelectRead))
                    {
                        byte[] messageReturn = new byte[CommonStaticVariables.constSize];
                        int messageCountReturn = clientSocket.Receive(messageReturn);
                        byte[] messageToCom = new byte[messageCountReturn];
                        Array.Copy(messageReturn, messageToCom, messageCountReturn);
                        if (byteEquals(messageToCom, CommonStaticVariables.messageDoneEncrypted))
                        {
                            return true;
                        }
                        else if (byteEquals(messageToCom, CommonStaticVariables.messageFailedEncrypted))
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool verify(byte[] sourceMessage, byte[] destMessage, int lengthOfSource)
        {
            byte[] messageToCom = new byte[lengthOfSource];
            Array.Copy(sourceMessage, messageToCom, lengthOfSource);
            if (byteEquals(messageToCom, destMessage))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool byteEquals(byte[] source, byte[] dest)
        {
            if (source == null || dest == null)
            {
                return false;
            }
            if (source.Length != dest.Length)
            {
                return false;
            }

            for (int i = 0; i < source.Length; ++i)
            {
                if (source[i] != dest[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static void postMessage(Socket curclientSocket, byte[] messageToPost)
        {
            curclientSocket.Send(messageToPost);
        }
    }
}