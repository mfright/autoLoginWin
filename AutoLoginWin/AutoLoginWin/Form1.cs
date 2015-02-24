using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoLoginWin
{
    public partial class Form1 : Form
    {
        String messageEnabled = "○　Windowsの自動ログインは有効です。";
        String messageDisabled = "×　Windowsの自動ログインは停止しています！";

        public Form1()
        {
            InitializeComponent();
        }


        //フォームがアクティブになった時
        private void Form1_Activated(object sender, EventArgs e)
        {
            if (System.Environment.Is64BitOperatingSystem)
            {
                readReg64();
            }
            else
            {
                readReg32();
            }
        }


        // 32bitのとき
        private void readReg32()
        {
            Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Winlogon", false);

            if (regkey == null)
            {
                MessageBox.Show("レジストリを読み込めません");
                return;
            }

            try
            {

                txtDomain.Text = (string)regkey.GetValue("DefaultDomainName");
                txtUser.Text = (string)regkey.GetValue("DefaultUserName");
                txtPassword.Text = (string)regkey.GetValue("DefaultPassword");

                string autoAdminLogon = (string)regkey.GetValue("AutoAdminLogon");

                if (autoAdminLogon != null && autoAdminLogon.IndexOf("1") != -1)
                {
                    lblStatus.Text = messageEnabled;
                    this.BackColor = Color.FromArgb(0xAA, 0xAA, 0xDD);
                }
                else
                {
                    lblStatus.Text = messageDisabled;
                    this.BackColor = Color.FromArgb(0xDD, 0xAA, 0xAA);
                }


            }
            catch (Exception ex)
            {

            }


        }


        // 64bitのとき
        private void readReg64()
        {
            string keyName = @"Software\Microsoft\Windows NT\CurrentVersion\Winlogon";

            using (var reg32 = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64))
            using (Microsoft.Win32.RegistryKey regkey = reg32.OpenSubKey(keyName))
            {
                try
                {
                    txtDomain.Text = (string)regkey.GetValue("DefaultDomainName");
                    txtUser.Text = (string)regkey.GetValue("DefaultUserName");
                    txtPassword.Text = (string)regkey.GetValue("DefaultPassword");

                    string autoAdminLogon = (string)regkey.GetValue("AutoAdminLogon");

                    if (autoAdminLogon != null && autoAdminLogon.IndexOf("1") != -1)
                    {
                        lblStatus.Text = messageEnabled;
                        this.BackColor = Color.FromArgb(0xAA, 0xAA, 0xDD);
                    }
                    else
                    {
                        lblStatus.Text = messageDisabled;
                        this.BackColor = Color.FromArgb(0xDD, 0xAA, 0xAA);
                    }


                }
                catch (NullReferenceException excep)
                {
                    
                }
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            settingsWindow mySettingsWindow = new settingsWindow(txtDomain.Text, txtUser.Text, txtPassword.Text);
            mySettingsWindow.ShowDialog(this);
        }

        
    }
}
