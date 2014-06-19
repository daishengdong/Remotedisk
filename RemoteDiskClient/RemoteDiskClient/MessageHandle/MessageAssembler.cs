using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using RemoteDiskClient.CommonStaticVar;
using RemoteDiskClient.SecurityHandle;

namespace RemoteDiskClient.MessageHandle
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

        public static byte[] loginMsgAssemble(string account, string password)
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["login"]);
            return commonDoubleFunction(flag, account, password);
        }

        public static byte[] logoutMsgAssemble(string account, string password)
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["logout"]);
            return commonDoubleFunction(flag, account, password);
        }

        public static byte[] registMsgAssemble(string account, string password)
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["regist"]);
            return commonDoubleFunction(flag, account, password);
        }

        public static byte[] createDirectory(string path, string lastWriteTime)
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["createDirectory"]);
            return commonDoubleFunction(flag, path, lastWriteTime);
        }

        public static byte[] createFile(string path, string lastWriteTime)
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["createFile"]);
            return commonDoubleFunction(flag, path, lastWriteTime);
        }

        public static byte[] delete(string path)
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["delete"]);
            return commonSingleFunction(flag, path);
        }

        public static byte[] renameDirectory(string sourceDirPath, string destDirPath, string lastWriteTime)
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["renameDirectory"]);
            return commonTripleFunction(flag, sourceDirPath, destDirPath, lastWriteTime);
        }

        public static byte[] renameFile(string sourceDirPath, string destDirPath, string lastWriteTime)
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["renameFile"]);
            return commonTripleFunction(flag, sourceDirPath, destDirPath, lastWriteTime);
        }

        public static byte[] modifyFile(string sourceFilePath, string lastWriteTime)
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["modifyFile"]);
            return commonDoubleFunction(flag, sourceFilePath, lastWriteTime);
        }

        public static byte[] syncOnstart()
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["syncOnstart"]);
            byte[] temp = new byte[3];
            temp[0] = flag;
            temp[1] = 1;
            temp[2] = 0;
            return EncryptionDecryptionHandler.messageEncrypt(temp);
        }

        public static byte[] syncNow()
        {
            byte flag = Convert.ToByte(CommonStaticVariables.messageMap["syncNow"]);
            byte[] temp = new byte[3];
            temp[0] = flag;
            temp[1] = 1;
            temp[2] = 0;
            return EncryptionDecryptionHandler.messageEncrypt(temp);
        }

        public static byte[] commonSingleFunction(byte flag, string para)
        {
            byte segmentCount = 1;
            byte[] paraByte = Encoding.UTF8.GetBytes(para);
            byte lengthOfPara = Convert.ToByte(paraByte.Length);
            int lengthOfMsg = 3 + lengthOfPara;
            byte[] message = new byte[lengthOfMsg];
            message[0] = flag;
            message[1] = segmentCount;
            message[2] = lengthOfPara;
            Array.Copy(paraByte, 0, message, 3, lengthOfPara);
            return EncryptionDecryptionHandler.messageEncrypt(message);
        }

        public static byte[] commonDoubleFunction(byte flag, string para1, string para2)
        {
            byte segmentCount = 2;
            byte[] para1Byte = Encoding.UTF8.GetBytes(para1);
            byte[] para2Byte = Encoding.UTF8.GetBytes(para2);
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

        public static byte[] commonTripleFunction(byte flag, string para1, string para2, string para3)
        {
            byte segmentCount = 3;
            byte[] para1Byte = Encoding.UTF8.GetBytes(para1);
            byte[] para2Byte = Encoding.UTF8.GetBytes(para2);
            byte[] para3Byte = Encoding.UTF8.GetBytes(para3);
            byte lengthOfPara1 = Convert.ToByte(para1Byte.Length);
            byte lengthOfPara2 = Convert.ToByte(para2Byte.Length);
            byte lengthOfPara3 = Convert.ToByte(para3Byte.Length);
            int lengthOfMsg = 5 + lengthOfPara1 + lengthOfPara2 + lengthOfPara3;
            byte[] message = new byte[lengthOfMsg];
            message[0] = flag;
            message[1] = segmentCount;
            message[2] = lengthOfPara1;
            message[3] = lengthOfPara2;
            message[4] = lengthOfPara3;
            Array.Copy(para1Byte, 0, message, 5, lengthOfPara1);
            Array.Copy(para2Byte, 0, message, 5 + lengthOfPara1, lengthOfPara2);
            Array.Copy(para3Byte, 0, message, 5 + lengthOfPara1 + lengthOfPara2, lengthOfPara3);
            return EncryptionDecryptionHandler.messageEncrypt(message);
        }
    }
}