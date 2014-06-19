namespace RemoteDiskServer
{
    partial class MainFrm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbStart = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbHalt = new System.Windows.Forms.ToolStripButton();
            this.tsbReboot = new System.Windows.Forms.ToolStripButton();
            this.tsbConfigure = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslableEntryPre = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslableEntry = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.tsslableAccountPre = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslableAccount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslableIPPre = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslableIP = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslPortPre = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslablePort = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(4, 35);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(758, 433);
            this.dataGridView1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbStart,
            this.tsbRefresh,
            this.tsbHalt,
            this.tsbReboot,
            this.tsbConfigure});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(767, 33);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbStart
            // 
            this.tsbStart.AutoSize = false;
            this.tsbStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbStart.Image = ((System.Drawing.Image)(resources.GetObject("tsbStart.Image")));
            this.tsbStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStart.Name = "tsbStart";
            this.tsbStart.Size = new System.Drawing.Size(45, 30);
            this.tsbStart.Text = "开机";
            this.tsbStart.Click += new System.EventHandler(this.tsbStart_Click);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.AutoSize = false;
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsbRefresh.Image")));
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(45, 30);
            this.tsbRefresh.Text = "刷新";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbHalt
            // 
            this.tsbHalt.AutoSize = false;
            this.tsbHalt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbHalt.Image = ((System.Drawing.Image)(resources.GetObject("tsbHalt.Image")));
            this.tsbHalt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHalt.Name = "tsbHalt";
            this.tsbHalt.Size = new System.Drawing.Size(45, 30);
            this.tsbHalt.Text = "停机";
            this.tsbHalt.Click += new System.EventHandler(this.tsbHalt_Click);
            // 
            // tsbReboot
            // 
            this.tsbReboot.AutoSize = false;
            this.tsbReboot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbReboot.Image = ((System.Drawing.Image)(resources.GetObject("tsbReboot.Image")));
            this.tsbReboot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReboot.Name = "tsbReboot";
            this.tsbReboot.Size = new System.Drawing.Size(45, 30);
            this.tsbReboot.Text = "重启";
            this.tsbReboot.Click += new System.EventHandler(this.tsbReboot_Click);
            // 
            // tsbConfigure
            // 
            this.tsbConfigure.AutoSize = false;
            this.tsbConfigure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbConfigure.Image = ((System.Drawing.Image)(resources.GetObject("tsbConfigure.Image")));
            this.tsbConfigure.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConfigure.Name = "tsbConfigure";
            this.tsbConfigure.Size = new System.Drawing.Size(45, 30);
            this.tsbConfigure.Text = "配置";
            this.tsbConfigure.Click += new System.EventHandler(this.tsbConfigure_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslableEntryPre,
            this.tsslableEntry,
            this.toolStripSplitButton1,
            this.tsslableAccountPre,
            this.tsslableAccount,
            this.tsslableIPPre,
            this.tsslableIP,
            this.tsslPortPre,
            this.tsslablePort});
            this.statusStrip1.Location = new System.Drawing.Point(0, 471);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(767, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslableEntryPre
            // 
            this.tsslableEntryPre.AutoSize = false;
            this.tsslableEntryPre.Name = "tsslableEntryPre";
            this.tsslableEntryPre.Size = new System.Drawing.Size(35, 17);
            this.tsslableEntryPre.Text = "事务：";
            this.tsslableEntryPre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslableEntry
            // 
            this.tsslableEntry.AutoSize = false;
            this.tsslableEntry.Name = "tsslableEntry";
            this.tsslableEntry.Size = new System.Drawing.Size(100, 17);
            this.tsslableEntry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(16, 20);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // tsslableAccountPre
            // 
            this.tsslableAccountPre.AutoSize = false;
            this.tsslableAccountPre.Name = "tsslableAccountPre";
            this.tsslableAccountPre.Size = new System.Drawing.Size(35, 17);
            this.tsslableAccountPre.Text = "账户:";
            // 
            // tsslableAccount
            // 
            this.tsslableAccount.AutoSize = false;
            this.tsslableAccount.Name = "tsslableAccount";
            this.tsslableAccount.Size = new System.Drawing.Size(80, 17);
            this.tsslableAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslableIPPre
            // 
            this.tsslableIPPre.AutoSize = false;
            this.tsslableIPPre.Name = "tsslableIPPre";
            this.tsslableIPPre.Size = new System.Drawing.Size(20, 17);
            this.tsslableIPPre.Text = "IP：";
            this.tsslableIPPre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslableIP
            // 
            this.tsslableIP.AutoSize = false;
            this.tsslableIP.Name = "tsslableIP";
            this.tsslableIP.Size = new System.Drawing.Size(80, 17);
            this.tsslableIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslPortPre
            // 
            this.tsslPortPre.AutoSize = false;
            this.tsslPortPre.Name = "tsslPortPre";
            this.tsslPortPre.Size = new System.Drawing.Size(35, 17);
            this.tsslPortPre.Text = "端口：";
            this.tsslPortPre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslablePort
            // 
            this.tsslablePort.AutoSize = false;
            this.tsslablePort.Name = "tsslablePort";
            this.tsslablePort.Size = new System.Drawing.Size(50, 17);
            this.tsslablePort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 493);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainFrm";
            this.Text = "服务器端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.Resize += new System.EventHandler(this.MainFrm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbStart;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbHalt;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslableEntryPre;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripStatusLabel tsslableIPPre;
        private System.Windows.Forms.ToolStripStatusLabel tsslPortPre;
        public System.Windows.Forms.ToolStripStatusLabel tsslableEntry;
        public System.Windows.Forms.ToolStripStatusLabel tsslableIP;
        public System.Windows.Forms.ToolStripStatusLabel tsslablePort;
        private System.Windows.Forms.ToolStripStatusLabel tsslableAccountPre;
        public System.Windows.Forms.ToolStripStatusLabel tsslableAccount;
        private System.Windows.Forms.ToolStripButton tsbConfigure;
        private System.Windows.Forms.ToolStripButton tsbReboot;
    }
}

