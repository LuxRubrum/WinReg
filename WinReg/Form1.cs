using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace WinReg
{
    public partial class Form1 : Form
    {
        [DllImport("advapi32.dll", EntryPoint = "RegOpenKeyW", CharSet = CharSet.Unicode)]
        static extern int RegOpenKey(UIntPtr hKey, string lpSubKey, out UIntPtr phkResult);
        [DllImport("advapi32.dll")]
        static extern int RegCloseKey(UIntPtr hKey);
        [DllImport("advapi32.dll", EntryPoint = "RegQueryValueExW", CharSet = CharSet.Unicode)]
        static extern int RegQueryValueEx(UIntPtr hKey, string lpValueName, int lpReserved, uint lpType, StringBuilder lpData, ref int lpcbData);


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder outInfo;
            int len;
            UIntPtr hregkey;
            string subkey;
            int val;
            int res;
            UIntPtr hKey = (UIntPtr)0x80000002;
            subkey = "HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0";
            val = RegOpenKey(hKey, subkey, out hregkey);
            if (val != 0)
            {
                MessageBox.Show("Ошибка при открытии: " + val.ToString());
            }
            else
            {
                len = 1024;
                outInfo = new StringBuilder(len);
                res = RegQueryValueEx(hregkey, "ProcessorNameString", 0, 0, outInfo, ref len);
                if (res != 0)
                {
                    MessageBox.Show("Ошибка чтения параметра: " + res.ToString());
                }
                val = RegCloseKey(hregkey);
                textBox1.Text = outInfo.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegistryKey myKey = Registry.LocalMachine;
            if (myKey != null)
            {
                RegistryKey wKey = myKey.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
                if (wKey != null)
                {
                    string outInfo = wKey.GetValue("ProcessorNameString") as string;
                    if (outInfo == null)
                    {
                        MessageBox.Show("Ошибка считывания параметра");
                    }
                    else
                    {
                        textBox2.Text = outInfo;
                    }
                    wKey.Close();
                }
                else
                {
                    MessageBox.Show("Ошибка открытия раздела");
                }
            }
            else
            {
                MessageBox.Show("Ошибка чтения регистра");
            }
        }
    }
}
