using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RemoteDiskServer.SecurityHandle;

namespace RemoteDiskServer.MessageHandle
{
    class MessageAssembler
    {
        public static byte[] assembleDir(string path, string lastWriteTime)
        {
            byte flag = 0;
            return commonFunction(flag, path, lastWriteTime);
        }

        public static byte[] assembleFile(string path, string lastWriteTime)
        {
            byte flag = 1;
            return commonFunction(flag, path, lastWriteTime);
        }

        public static byte[] assembleDone()
        {
            byte flag = 2;
            byte[] temp = new byte[4];
            temp[0] = flag;
            temp[1] = 2;
            temp[2] = 0;
            temp[3] = 0;
            return EncryptionDecryptionHandler.messageEncrypt(temp);
        }

        public static byte[] commonFunction(byte flag, string path, string lastWriteTime)
        {
            byte segmentCount = 2;
            byte[] para1Byte = Encoding.UTF8.GetBytes(path);
            byte[] para2Byte = Encoding.UTF8.GetBytes(lastWriteTime);
            byte lengthOfPara1 = Convert.ToByte(para1Byte.Length);
            byte lengthOfPara2 = Convert.ToByte(para2Byte.Length);
            int lengthOfMsg = 4 + lengthOfPara1 + lengthOfPara2;
            byte[] message = new byte[lengthOfMsg];
            message[0] = flag;
            message[1] = segmentCount;
            message[2] = lengthOfPara1;
            message[3] = lengthOfPara2;
            Array.Copy(para1Byte, 0, message, 4, lengthOfPara1);
            Array.Copy(para2Byte, 0, message, 4 + lengthOfPara1, lengthOfPara2);
            return EncryptionDecryptionHandler.messageEncrypt(message);
        }
    }
}