namespace RemoteDiskClient
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtBoxPassword = new System.Windows.Forms.TextBox();
            this.txtBoxAccount = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelAccount = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnRegist = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开快盘ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.立即进行一次同步ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.配置CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.同步完成后关机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出快盘EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtBoxPassword);
            this.panel1.Controls.Add(this.txtBoxAccount);
            this.panel1.Controls.Add(this.labelPassword);
            this.panel1.Controls.Add(this.labelAccount);
            this.panel1.Location = new System.Drawing.Point(129, 87);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 206);
            this.panel1.TabIndex = 0;
            // 
            // txtBoxPassword
            // 
            this.txtBoxPassword.Location = new System.Drawing.Point(120, 134);
            this.txtBoxPassword.Name = "txtBoxPassword";
            this.txtBoxPassword.PasswordChar = '*';
            this.txtBoxPassword.Size = new System.Drawing.Size(165, 20);
            this.txtBoxPassword.TabIndex = 3;
            // 
            // txtBoxAccount
            // 
            this.txtBoxAccount.Location = new System.Drawing.Point(120, 58);
            this.txtBoxAccount.Name = "txtBoxAccount";
            this.txtBoxAccount.Size = new System.Drawing.Size(165, 20);
            this.txtBoxAccount.TabIndex = 2;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(65, 137);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(31, 13);
            this.labelPassword.TabIndex = 1;
            this.labelPassword.Text = "密码";
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(65, 61);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(31, 13);
            this.labelAccount.TabIndex = 0;
            this.labelAccount.Text = "账户";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(339, 367);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnRegist
            // 
            this.btnRegist.Location = new System.Drawing.Point(420, 367);
            this.btnRegist.Name = "btnRegist";
            this.btnRegist.Size = new System.Drawing.Size(75, 23);
            this.btnRegist.TabIndex = 2;
            this.btnRegist.Text = "注册";
            this.btnRegist.UseVisualStyleBackColor = true;
            this.btnRegist.Click += new System.EventHandler(this.btnRegist_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(501, 367);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(75, 23);
            this.btnSetting.TabIndex = 3;
            this.btnSetting.Text = "配置";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开快盘ToolStripMenuItem,
            this.toolStripSeparator1,
            this.立即进行一次同步ToolStripMenuItem,
            this.toolStripSeparator2,
            this.配置CToolStripMenuItem,
            this.toolStripSeparator3,
            this.同步完成后关机ToolStripMenuItem,
            this.退出快盘EToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 132);
            // 
            // 打开快盘ToolStripMenuItem
            // 
            this.打开快盘ToolStripMenuItem.Name = "打开快盘ToolStripMenuItem";
            this.打开快盘ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.打开快盘ToolStripMenuItem.Text = "打开快盘(&K)";
            this.打开快盘ToolStripMenuItem.Click += new System.EventHandler(this.打开快盘ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(181, 6);
            // 
            // 立即进行一次同步ToolStripMenuItem
            // 
            this.立即进行一次同步ToolStripMenuItem.Name = "立即进行一次同步ToolStripMenuItem";
            this.立即进行一次同步ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.立即进行一次同步ToolStripMenuItem.Text = "立即进行一次同步(&S)";
            this.立即进行一次同步ToolStripMenuItem.Click += new System.EventHandler(this.立即进行一次同步ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // 配置CToolStripMenuItem
            // 
            this.配置CToolStripMenuItem.Name = "配置CToolStripMenuItem";
            this.配置CToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.配置CToolStripMenuItem.Text = "配置(&C)";
            this.配置CToolStripMenuItem.Click += new System.EventHandler(this.配置CToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(181, 6);
            // 
            // 同步完成后关机ToolStripMenuItem
            // 
            this.同步完成后关机ToolStripMenuItem.CheckOnClick = true;
            this.同步完成后关机ToolStripMenuItem.Name = "同步完成后关机ToolStripMenuItem";
            this.同步完成后关机ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.同步完成后关机ToolStripMenuItem.Text = "同步完成后关机";
            this.同步完成后关机ToolStripMenuItem.Click += new System.EventHandler(this.同步完成后关机ToolStripMenuItem_Click);
            // 
            // 退出快盘EToolStripMenuItem
            // 
            this.退出快盘EToolStripMenuItem.Name = "退出快盘EToolStripMenuItem";
            this.退出快盘EToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.退出快盘EToolStripMenuItem.Text = "退出快盘(&E)";
            this.退出快盘EToolStripMenuItem.Click += new System.EventHandler(this.退出快盘EToolStripMenuItem_Click);
            // 
            // MainFrm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(598, 402);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnRegist);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.Text = "杭电快盘";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtBoxPassword;
        private System.Windows.Forms.TextBox txtBoxAccount;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegist;
        private System.Windows.Forms.Button btnSetting;
        public System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 打开快盘ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 立即进行一次同步ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 配置CToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 同步完成后关机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出快盘EToolStripMenuItem;
    }
}

