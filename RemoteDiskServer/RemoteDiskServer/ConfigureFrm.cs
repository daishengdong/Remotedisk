using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using RemoteDiskServer.ConfigureHandle;
using RemoteDiskServer.CommonStaticVar;

namespace RemoteDiskServer
{
    public partial class ConfigureFrm : Form
    {
        private string IP = null;
        private string Port = null;
        private string MaxClientCount = null;
        private string DataBase = null;
        private string HomePath = null;

        private int lastPoint = 0;
        private int lastlastPoint = 0;
        private int pointCount = 0;

        private void loadItem()
        {
            string ip = ConfigureHandler.ReadIniData("Configuration", "IP");
            string port = ConfigureHandler.ReadIniData("Configuration", "Port");
            string maxClientCount = ConfigureHandler.ReadIniData("Configuration", "MaxClientCount");
            string dataBase = ConfigureHandler.ReadIniData("Configuration", "DataBase");
            string home = ConfigureHandler.ReadIniData("Configuration", "Home");
            if (ip != string.Empty || port != string.Empty || maxClientCount != string.Empty || dataBase != string.Empty || home != string.Empty)
            {
                if (ip != string.Empty)
                {
                    txtBoxIP.Text = ip;
                }
                else
                {
                    txtBoxIP.Text = ConfigureHandler.ReadIniData("Default", "IP");
                }

                if (port != string.Empty)
                {
                    txtBoxPort.Text = port;
                }
                else
                {
                    txtBoxPort.Text = ConfigureHandler.ReadIniData("Default", "Port");
                }

                if (maxClientCount != string.Empty)
                {
                    txtBoxMaxClientCount.Text = maxClientCount;
                }
                {
                    txtBoxMaxClientCount.Text = ConfigureHandler.ReadIniData("Default", "MaxClientCount");
                }

                if (dataBase != string.Empty)
                {
                    txtBoxDataBase.Text = dataBase;
                }
                {
                    txtBoxDataBase.Text = ConfigureHandler.ReadIniData("Default", "DataBase");
                }

                if (home != string.Empty)
                {
                    txtBoxHome.Text = home;
                }
                else
                {
                    txtBoxHome.Text = ConfigureHandler.ReadIniData("Default", "Home");
                }
            }
            else
            {
                txtBoxIP.Text = ConfigureHandler.ReadIniData("Default", "IP");
                txtBoxPort.Text = ConfigureHandler.ReadIniData("Default", "Port");
                txtBoxMaxClientCount.Text = ConfigureHandler.ReadIniData("Default", "MaxClientCount");
                txtBoxDataBase.Text = ConfigureHandler.ReadIniData("Default", "DataBase");
                txtBoxHome.Text = ConfigureHandler.ReadIniData("Default", "Home");
            }
        }

        public ConfigureFrm()
        {
            InitializeComponent();
            loadItem();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            IP = txtBoxIP.Text.Trim();
            Port = txtBoxPort.Text.Trim();
            MaxClientCount = txtBoxMaxClientCount.Text.Trim();
            DataBase = txtBoxDataBase.Text.Trim();
            HomePath = txtBoxHome.Text.Trim();
            ConfigureHandler.WriteIniData("Configuration", "IP", IP);
            ConfigureHandler.WriteIniData("Configuration", "Port", Port);
            ConfigureHandler.WriteIniData("Configuration", "MaxClientCount", MaxClientCount);
            ConfigureHandler.WriteIniData("Configuration", "DataBase", DataBase);
            ConfigureHandler.WriteIniData("Configuration", "Home", HomePath);
            CommonStaticVariables.loadConfiguration();
            MessageBox.Show("配置成功！\n下次启动机器时生效！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = @"D:\RemoteDiskServer\";
            fbd.ShowNewFolderButton = true;
            fbd.ShowDialog();
            txtBoxHome.Text = fbd.SelectedPath + "\\";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (File.Exists(CommonStaticVariables.iniFilePath))
            {
                File.Delete(CommonStaticVariables.iniFilePath);
            }
            CommonStaticVariables.writeDefaultConfiguration();
            CommonStaticVariables.loadDefaultConfiguration();
            loadItem();
        }

        private void txtBoxIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                if (pointCount <= 3 && lastPoint < 3)
                {
                    e.Handled = true;
                    if (txtBoxIP.SelectionLength == txtBoxIP.Text.Length)
                    {
                        txtBoxIP.Text = "";
                        lastPoint = lastlastPoint = pointCount = 0;
                    }
                    txtBoxIP.Text += e.KeyChar;
                    txtBoxIP.SelectionStart = txtBoxIP.Text.Length;
                    ++lastPoint;
                    if (lastPoint == 3 && pointCount != 3)
                    {
                        txtBoxIP.Text += ".";
                        ++pointCount;
                        txtBoxIP.SelectionStart = txtBoxIP.Text.Length;
                        lastlastPoint = lastPoint;
                        lastPoint = 0;
                    }
                }
                else if (pointCount < 3 && lastPoint == 3)
                {
                    e.Handled = true;
                    txtBoxIP.Text += "." + e.KeyChar;
                    ++pointCount;
                    txtBoxIP.SelectionStart = txtBoxIP.Text.Length;
                    lastlastPoint = lastPoint;
                    lastPoint = 1;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else if (e.KeyChar == '.')
            {
                if (pointCount < 3 && lastPoint > 0)
                {
                    e.Handled = true;
                    if (txtBoxIP.SelectionLength == txtBoxIP.Text.Length)
                    {
                        txtBoxIP.Text = "";
                        lastPoint = lastlastPoint = pointCount = 0;
                    }
                    txtBoxIP.Text += e.KeyChar;
                    txtBoxIP.SelectionStart = txtBoxIP.Text.Length;
                    ++pointCount;
                    lastPoint = 0;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else if (e.KeyChar == '\b')
            {
                if (txtBoxIP.Text != "")
                {
                    string tempText = txtBoxIP.Text;
                    if (Char.IsNumber(tempText[tempText.Length - 1]))
                    {
                        --lastPoint;
                    }
                    else if (tempText[tempText.Length - 1] == '.')
                    {
                        lastPoint = lastlastPoint;
                        --pointCount;
                    }
                }
                else
                {
                    lastPoint = lastlastPoint = pointCount = 0;
                }
            }
            else if (txtBoxIP.Text.Length == 0 && e.KeyChar == '0')
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtBoxPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || e.KeyChar == '\b'))
            {
                e.Handled = true;
            }
            else if (txtBoxPort.Text.Length == 0 && e.KeyChar == '0')
            {
                e.Handled = true;
            }
        }

        private void txtBoxMaxClientCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar) || e.KeyChar == '\b'))
            {
                e.Handled = true;
            }
            else if (txtBoxMaxClientCount.Text.Length == 0 && e.KeyChar == '0')
            {
                e.Handled = true;
            }
        }
    }
}
