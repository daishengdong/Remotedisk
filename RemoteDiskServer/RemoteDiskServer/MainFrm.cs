using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using RemoteDiskServer.CommonStaticVar;
using RemoteDiskServer.SQLHandle;
using RemoteDiskServer.SocketHandle;

namespace RemoteDiskServer
{
    public partial class MainFrm : Form
    {
        private int oldFrmWidth = 0;
        private int oldFrmHeight = 0;
        private ClientHandler insClientHandler = null;
        private Thread mainThread = null;

        private delegate void insDelegate(object sender, EventArgs e);

        public void bindData()
        {
            this.dataGridView1.DataSource = SQLHandler.getData(CommonStaticVariables.synSelectString);
        }

        public MainFrm()
        {
            InitializeComponent();
            tsbRefresh.Enabled = false;
            tsbHalt.Enabled = false;
            tsbReboot.Enabled = false;
            // 初始化消息映射表
            CommonStaticVariables.init();
            if (!Directory.Exists(CommonStaticVariables.homePath))
            {
                Directory.CreateDirectory(CommonStaticVariables.homePath);
            }

            ResizeInit(this);
        }

        private void ResizeInit(Form frm)
        {
            oldFrmWidth = frm.Width;
            oldFrmHeight = frm.Height;

            foreach (Control control in frm.Controls)
            {
                control.Tag = control.Left.ToString() + "," + control.Top.ToString() + "," + control.Width.ToString() + "," + control.Height.ToString();
            }
        }

        private void MainFrm_Resize(object sender, EventArgs e)
        {
            float ScaleX;
            float ScaleY;

            Form f = (Form)sender;

            ScaleX = (float)f.Width / oldFrmWidth;
            ScaleY = (float)f.Height / oldFrmHeight;

            foreach (Control c in f.Controls)
            {
                string[] tmp = c.Tag.ToString().Split(',');
                c.Left = (int)(Convert.ToInt16(tmp[0]) * ScaleX);
                c.Top = (int)(Convert.ToInt16(tmp[1]) * ScaleY);
                c.Width = (int)(Convert.ToInt16(tmp[2]) * ScaleX);
                c.Height = (int)(Convert.ToInt16(tmp[3]) * ScaleY);
            }
        }

        // 开机要做的事：
        // 创建一个线程开始监听端口
        private void tsbStart_Click(object sender, EventArgs e)
        {
            bindData();
            // 创建一个线程开始监听端口
            insClientHandler = new ClientHandler(this);
            mainThread = new Thread(insClientHandler.listen);
            mainThread.IsBackground = true;
            mainThread.Start();

            tsbStart.Enabled = false;
            tsbRefresh.Enabled = true;
            tsbHalt.Enabled = true;
            tsbReboot.Enabled = true;
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            bindData();

            tsslableEntry.Text = "";
            tsslableAccount.Text = "";
            tsslableIP.Text = "";
            tsslablePort.Text = "";
        }

        private void tsbHalt_Click(object sender, EventArgs e)
        {
            if (mainThread.IsAlive)
            {
                mainThread.Abort();
                insClientHandler.shutdown();
            }

            tsbStart.Enabled = true;
            tsbRefresh.Enabled = false;
            tsbHalt.Enabled = false;
            tsbReboot.Enabled = false;
            dataGridView1.DataSource = null;

            tsslableEntry.Text = "";
            tsslableAccount.Text = "";
            tsslableIP.Text = "";
            tsslablePort.Text = "";
        }

        private void tsbConfigure_Click(object sender, EventArgs e)
        {
            ConfigureFrm insConfigureFrm = new ConfigureFrm();
            insConfigureFrm.Show();
        }

        private void tsbReboot_Click(object sender, EventArgs e)
        {
            insDelegate halt = tsbHalt_Click;
            halt.Invoke(sender, e);
            insDelegate start = tsbStart_Click;
            start.Invoke(sender, e);
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mainThread != null && mainThread.IsAlive)
            {
                mainThread.Abort();
            }
        }
    }
}
