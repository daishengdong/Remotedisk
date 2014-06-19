using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using RemoteDiskClient.MessageHandle;
using RemoteDiskClient.CommonStaticVar;
using RemoteDiskClient.SocketHandle;
using RemoteDiskClient.FileSystemHandle;
using RemoteDiskClient.ShutdownHandle;

namespace RemoteDiskClient
{
    public partial class MainFrm : Form
    {
        private static string account = null;
        private static string password = null;

        public MainFrm()
        {
            InitializeComponent();
            for (int i = 0; i <= 7; ++i)
            {
                this.contextMenuStrip1.Items[i].Enabled = false;
            }
            notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            notifyIcon1.Text = "杭电快盘";
            notifyIcon1.Visible = true;
            CommonStaticVariables.init();
            if (!Directory.Exists(CommonStaticVariables.homePath))
            {
                Directory.CreateDirectory(CommonStaticVariables.homePath);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                account = txtBoxAccount.Text.Trim();
                password = txtBoxPassword.Text.Trim();

                if (ServerHandler.login(account, password))
                {
                    this.Visible = false;

                    // 登录成功后首先开机同步
                    Thread syncThread = new Thread(ServerHandler.syncOnstart);
                    syncThread.IsBackground = true;
                    syncThread.Start();
                    syncThread.Join();

                    notifyIcon1.BalloonTipText = "开机同步完成";
                    notifyIcon1.ShowBalloonTip(200);

                    for (int i = 0; i <= 7; ++i)
                    {
                        this.contextMenuStrip1.Items[i].Enabled = true;
                    }

                    // 创建本地文件系统监视器
                    ClientFileSystemWatcher insClientFileSystemWatcher = new ClientFileSystemWatcher(this);
                }
                else
                {
                    MessageBox.Show("登录失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!ServerHandler.logout(account, password))
                {
                    MessageBox.Show("登出失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            try
            {
                account = txtBoxAccount.Text.Trim();
                password = txtBoxPassword.Text.Trim();

                if (!ServerHandler.regist(account, password))
                {
                    MessageBox.Show("注册失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            ConfigureFrm insConfigureFrm = new ConfigureFrm();
            insConfigureFrm.Show();
        }

        private void 退出快盘EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 配置CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigureFrm insConfigureFrm = new ConfigureFrm();
            insConfigureFrm.Show();
        }

        private void 打开快盘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", CommonStaticVariables.homePath);
        }

        private void 立即进行一次同步ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread syncNowThread = new Thread(ServerHandler.syncNow);
            syncNowThread.IsBackground = true;
            syncNowThread.Start();
            syncNowThread.Join();
            if (同步完成后关机ToolStripMenuItem.Checked)
            {
                ShutdownHandler.DoExitWin(ShutdownHandler.EWX_SHUTDOWN);
            }

            notifyIcon1.BalloonTipText = "同步完成";
            notifyIcon1.ShowBalloonTip(200);
        }

        private void 同步完成后关机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (同步完成后关机ToolStripMenuItem.Checked)
            {
                同步完成后关机ToolStripMenuItem.CheckState = CheckState.Checked;
            }
            else
            {
                同步完成后关机ToolStripMenuItem.CheckState = CheckState.Unchecked;
            }
        }
    }
}
