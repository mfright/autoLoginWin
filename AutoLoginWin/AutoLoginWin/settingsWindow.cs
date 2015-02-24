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
    public partial class settingsWindow : Form
    {
        public settingsWindow(String domain, String userName, String password)
        {
            InitializeComponent();

            txtDomain.Text = domain;
            txtUserName.Text = userName;
            txtPassword.Text = password;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (System.Environment.Is64BitOperatingSystem)
            {
                writeReg64();
            }
            else
            {
                writeReg32();
            }

            this.Close();
        }

        // 32bitでレジストリに書き込む
        private void writeReg32()
        {
            Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Winlogon", true);


            if (regkey == null)
            {
                MessageBox.Show("レジストリを処理できません");
                return;
            }

            try
            {

                regkey.SetValue("DefaultDomainName", txtDomain.Text);
                regkey.SetValue("DefaultUserName", txtUserName.Text);
                regkey.SetValue("DefaultPassword", txtPassword.Text);
                if (radioEnable.Checked)
                {
                    regkey.SetValue("AutoAdminLogon", "1");
                }
                else
                {
                    regkey.SetValue("AutoAdminLogon", "0");
                }
            }
            catch (Exception ex)
            {

            }
        }


        // 64bitで情報を描きこむ
        private void writeReg64()
        {
            string keyName = @"Software\Microsoft\Windows NT\CurrentVersion\Winlogon";

            using (var reg32 = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64))
            using (Microsoft.Win32.RegistryKey regkey = reg32.OpenSubKey(keyName, true))
            {
                try
                {

                    regkey.SetValue("DefaultDomainName", txtDomain.Text);
                    regkey.SetValue("DefaultUserName", txtUserName.Text);
                    regkey.SetValue("DefaultPassword", txtPassword.Text);
                    if (radioEnable.Checked)
                    {
                        regkey.SetValue("AutoAdminLogon", "1");
                    }
                    else
                    {
                        regkey.SetValue("AutoAdminLogon", "0");
                    }

                }
                catch (NullReferenceException ex)
                {
                }
            }
        }
    }
}
