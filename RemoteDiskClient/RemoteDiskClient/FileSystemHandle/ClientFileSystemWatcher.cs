using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using RemoteDiskClient.CommonStaticVar;
using RemoteDiskClient.SocketHandle;

namespace RemoteDiskClient.FileSystemHandle
{
    class ClientFileSystemWatcher
    {
        MainFrm insMainFrm = null;

        public ClientFileSystemWatcher(MainFrm insMainFrm)
        {
            this.insMainFrm = insMainFrm;

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = CommonStaticVariables.homePath;
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // 添加事件句柄
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // 开始监视
            watcher.EnableRaisingEvents = true;
        }

        // 定义事件句柄
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            string path = e.FullPath;
            string paraPath = path.Substring(CommonStaticVariables.homePath.Length, path.Length - CommonStaticVariables.homePath.Length);
            string lastWriteTime = null;
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    {
                        // 如果是文件夹 change，直接无视
                        // 否则会出现 bug

                        // 如果是文件 change，要同步
                        if (File.Exists(path))
                        {
                            lastWriteTime = File.GetLastWriteTime(path).ToString();
                            if (!ServerHandler.modifyFile(paraPath, path, lastWriteTime))
                            {
                                insMainFrm.notifyIcon1.BalloonTipText = "上传失败";
                                insMainFrm.notifyIcon1.ShowBalloonTip(200);
                            }
                            else
                            {
                                insMainFrm.notifyIcon1.BalloonTipText = "上传完毕";
                                insMainFrm.notifyIcon1.ShowBalloonTip(200);
                            }
                        }
                        break;
                    }
                case WatcherChangeTypes.Created:
                    {
                        if (Directory.Exists(path))
                        {
                            lastWriteTime = Directory.GetLastWriteTime(path).ToString();
                            if (!ServerHandler.createDirectory(paraPath, lastWriteTime))
                            {
                                insMainFrm.notifyIcon1.BalloonTipText = "同步失败";
                                insMainFrm.notifyIcon1.ShowBalloonTip(200);
                            }
                        }
                        if (File.Exists(path))
                        {
                            lastWriteTime = File.GetLastWriteTime(path).ToString();
                            if (!ServerHandler.createFile(paraPath, lastWriteTime))
                            {
                                insMainFrm.notifyIcon1.BalloonTipText = "同步失败";
                                insMainFrm.notifyIcon1.ShowBalloonTip(200);
                            }
                        }
                        break;
                    }
                case WatcherChangeTypes.Deleted:
                    {
                        // 无论是文件夹还是文件删除，操作一样
                        // 只是在服务器端要加以区别
                        if (!ServerHandler.delete(paraPath))
                        {
                            insMainFrm.notifyIcon1.BalloonTipText = "同步失败";
                            insMainFrm.notifyIcon1.ShowBalloonTip(200);
                        }
                        break;
                    }
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            string oldPath = e.OldFullPath;
            string newPath = e.FullPath;
            string paraOldPath = oldPath.Substring(CommonStaticVariables.homePath.Length, oldPath.Length - CommonStaticVariables.homePath.Length);
            string paraNewPath = newPath.Substring(CommonStaticVariables.homePath.Length, newPath.Length - CommonStaticVariables.homePath.Length);

            if (Directory.Exists(newPath))
            {
                string lastWriteTime = Directory.GetLastWriteTime(newPath).ToString();
                if (!ServerHandler.renameDirectory(paraOldPath, paraNewPath, lastWriteTime))
                {
                    insMainFrm.notifyIcon1.BalloonTipText = "同步失败";
                    insMainFrm.notifyIcon1.ShowBalloonTip(200);
                }
            }
            if (File.Exists(newPath))
            {
                string lastWriteTime = File.GetLastWriteTime(newPath).ToString();
                if (!ServerHandler.renameFile(paraOldPath, paraNewPath, lastWriteTime))
                {
                    insMainFrm.notifyIcon1.BalloonTipText = "同步失败";
                    insMainFrm.notifyIcon1.ShowBalloonTip(200);
                }
            }
        }
    }
}