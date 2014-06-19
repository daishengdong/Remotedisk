using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RemoteDiskServer.SecurityHandle;
using RemoteDiskServer.CommonStaticVar;

namespace RemoteDiskServer.MessageHandle
{
    class MessageParser
    {
        // 对消息的解析
        public static string[] parse(byte[] messageByteEncrypted)
        {
            try
            {
                int i;

                byte[] messageByte = EncryptionDecryptionHandler.messageDecrypt(messageByteEncrypted);

                // 消息类型
                int flagIndex = messageByte[0];
                // 消息段数
                int segmentCount = messageByte[1];
                string flag = CommonStaticVariables.messageMap[(object)flagIndex].ToString();

                // 每个段的长度
                int[] eachSize = new int[segmentCount];
                // 每个段的内容
                string[] parameter = new string[segmentCount];
                // 当前段的起始位置
                int curStart = 2 + segmentCount;
                for (i = 0; i < segmentCount; ++i)
                {
                    if (i == 0)
                    {
                        eachSize[i] = messageByte[2 + i];
                        byte[] temp = new byte[eachSize[i]];
                        Array.Copy(messageByte, curStart, temp, 0, eachSize[i]);
                        parameter[i] = Encoding.UTF8.GetString(temp);
                    }
                    else
                    {
                        eachSize[i] = messageByte[2 + i];
                        curStart += eachSize[i - 1];
                        byte[] temp = new byte[eachSize[i]];
                        Array.Copy(messageByte, curStart, temp, 0, eachSize[i]);
                        parameter[i] = Encoding.UTF8.GetString(temp);
                    }
                }

                string[] returnString = new string[1 + segmentCount];
                returnString[0] = flag;
                for (i = 0; i < segmentCount; ++i)
                {
                    returnString[i + 1] = parameter[i];
                }
                return returnString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // 对消息的解析
        public static string[] parseFileMessage(byte[] messageByteEncrypted)
        {
            try
            {
                int i;

                byte[] messageByte = EncryptionDecryptionHandler.messageDecrypt(messageByteEncrypted);

                // 消息类型
                string flag = messageByte[0].ToString();
                // 消息段数
                int segmentCount = messageByte[1];

                // 每个段的长度
                int[] eachSize = new int[segmentCount];
                // 每个段的内容
                string[] parameter = new string[segmentCount];
                // 当前段的起始位置
                int curStart = 2 + segmentCount;
                for (i = 0; i < segmentCount; ++i)
                {
                    if (i == 0)
                    {
                        eachSize[i] = messageByte[2 + i];
                        byte[] temp = new byte[eachSize[i]];
                        Array.Copy(messageByte, curStart, temp, 0, eachSize[i]);
                        parameter[i] = Encoding.UTF8.GetString(temp);
                    }
                    else
                    {
                        eachSize[i] = messageByte[2 + i];
                        curStart += eachSize[i - 1];
                        byte[] temp = new byte[eachSize[i]];
                        Array.Copy(messageByte, curStart, temp, 0, eachSize[i]);
                        parameter[i] = Encoding.UTF8.GetString(temp);
                    }
                }

                string[] returnString = new string[1 + segmentCount];
                returnString[0] = flag;
                for (i = 0; i < segmentCount; ++i)
                {
                    returnString[i + 1] = parameter[i];
                }
                return returnString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}