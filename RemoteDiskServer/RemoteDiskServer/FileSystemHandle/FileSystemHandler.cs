using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;

namespace RemoteDiskServer.FileSystemHandle
{
    class FileSystemHandler
    {
        public static void modifyDirectory(string dirPath, DateTime lastWriteTime)
        {
            try
            {
                Directory.SetLastWriteTime(dirPath, lastWriteTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public static void CreateDirectory(string dirPath, DateTime lastWriteTime)
        {
            try
            {
                Directory.CreateDirectory(dirPath);
                Directory.SetLastWriteTime(dirPath, lastWriteTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public static void DeleteDirectory(string dirPath)
        {
            try
            {
                Directory.Delete(dirPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public static void RenameDirectory(string sourceDirPath, string destDirPath, DateTime lastWriteTime)
        {
            try
            {
                Directory.Move(sourceDirPath, destDirPath);
                Directory.SetLastWriteTime(destDirPath, lastWriteTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public static void CreateFile(string filePath, DateTime lastWriteTime)
        {
            try
            {
                FileStream fileToWrite = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fileToWrite.Close();
                File.SetLastWriteTime(filePath, lastWriteTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public static void DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public static void RenameFile(string sourceFilePath, string destFilePath, DateTime lastWriteTime)
        {
            try
            {
                File.Move(sourceFilePath, destFilePath);
                File.SetLastWriteTime(destFilePath, lastWriteTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}