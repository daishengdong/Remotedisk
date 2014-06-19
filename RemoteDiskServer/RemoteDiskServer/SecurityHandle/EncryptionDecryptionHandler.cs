using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteDiskServer.SecurityHandle
{
    class EncryptionDecryptionHandler
    {
        /*
         private static void reverse(byte[] R, int from, int to)
         {
             byte i, temp;
             for (i = 0; i < (to - from + 1) / 2; ++i)
             {
                 temp = R[from + i];
                 R[from + i] = R[to - i];
                 R[to - i] = temp;
             }
         }

         private static void converse(byte[] R, int n, int p)
         {
             reverse(R, 0, p - 1);
             reverse(R, p, n - 1);
             reverse(R, 0, n - 1);
         }

         public static byte[] messageEncrypt(byte[] sourceMessage)
         {
             int i, j;
             int count = 0;
             byte[] bitToInt = new byte[8];
             byte[] messageEncrypted = new byte[sourceMessage.Length];
             Array.Copy(sourceMessage, messageEncrypted, sourceMessage.Length);
             for (i = 0; i < messageEncrypted.Length; ++i)
             {
                 count = 0;
                 for (j = 0; j < 8; ++j)
                 {
                     if ((messageEncrypted[i] & (1 << j)) != 0)
                     {
                         bitToInt[j] = 1;
                         ++count;
                     }
                     else
                     {
                         bitToInt[j] = 0;
                     }
                 }

                 converse(bitToInt, 8, count);

                 byte singleByte = 0;
                 for (j = 0; j < 8; ++j)
                 {
                     if (bitToInt[j] == 1)
                     {
                         singleByte |= (byte)(1 << j);
                     }
                 }
                 messageEncrypted[i] = singleByte;
             }
             return messageEncrypted;
         }

         public static byte[] messageDecrypt(byte[] sourceMessage)
         {
             int i, j;
             int count = 0;
             byte[] bitToInt = new byte[8];
             byte[] messageDecrypted = new byte[sourceMessage.Length];
             Array.Copy(sourceMessage, messageDecrypted, sourceMessage.Length);
             for (i = 0; i < messageDecrypted.Length; ++i)
             {
                 count = 0;
                 for (j = 0; j < 8; ++j)
                 {
                     if ((messageDecrypted[i] & (1 << j)) != 0)
                     {
                         bitToInt[j] = 1;
                         ++count;
                     }
                     else
                     {
                         bitToInt[j] = 0;
                     }
                 }

                 converse(bitToInt, 8, 8 - count);

                 byte singleByte = 0;
                 for (j = 0; j < 8; ++j)
                 {
                     if (bitToInt[j] == 1)
                     {
                         singleByte |= (byte)(1 << j);
                     }
                 }
                 messageDecrypted[i] = singleByte;
             }
             return messageDecrypted;
         }
         */

        private static void ror(byte k, byte b)
        {
            byte size = sizeof(byte) * 8;
            b %= size;

            byte low = (byte)(k << (size - b));
            byte high = (byte)(k >> b);

            k = (byte)(low | high);
        }

        private static void rol(byte k, byte b)
        {
            byte size = sizeof(byte) * 8;
            b %= size;

            byte low = (byte)(k << b);
            byte high = (byte)(k >> (size - b));

            k = (byte)(low | high);
        }

        private static byte getCount(byte messageByte)
        {
            byte num = 0;
            while (messageByte != 0)
            {
                messageByte &= (byte)(messageByte - 1);
                ++num;
            }
            return num;
        }

        public static byte[] messageEncrypt(byte[] sourceMessage)
        {
            int i, j;
            byte count = 0;
            byte[] bitToInt = new byte[8];
            byte[] messageEncrypted = new byte[sourceMessage.Length];
            Array.Copy(sourceMessage, messageEncrypted, sourceMessage.Length);
            for (i = 0; i < messageEncrypted.Length; ++i)
            {
                count = getCount(messageEncrypted[i]);
                ror(messageEncrypted[i], count);
            }
            return messageEncrypted;
        }

        public static byte[] messageDecrypt(byte[] sourceMessage)
        {
            int i, j;
            byte count = 0;
            byte[] messageDecrypted = new byte[sourceMessage.Length];
            Array.Copy(sourceMessage, messageDecrypted, sourceMessage.Length);
            for (i = 0; i < messageDecrypted.Length; ++i)
            {
                count = getCount(messageDecrypted[i]);
                rol(messageDecrypted[i], count);
            }
            return messageDecrypted;
        }
    }
}
