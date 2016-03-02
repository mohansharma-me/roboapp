using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using USBModem;
using MachineControl;

namespace Robo
{
    public partial class frmSplash : Form
    {
        private static __temporary temp=new __temporary();
        public frmSplash()
        {
            InitializeComponent();
        }

        private class __temporary
        {
            public bool value { get; set; }
            public string strValue { get; set; }
            public bool exit { get; set; }
            public bool lblAct { get; set; }
            public string actString { get; set; }
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            temp.value = false;
            temp.strValue = "Loading ...";
            this.Text = Properties.Resources.appTitle;
            lblTitle.Text = Properties.Resources.appTitle;
            lblVersion.Text = "Version: " + Properties.Resources.appVersion;
            temp.actString = "Product not activated";
            Thread thr = new Thread(new ParameterizedThreadStart(thread));
            thr.Start((object)temp);
        }

        void showActivation()
        {
            if (this.InvokeRequired)
            {
                Action a = () => triggerDialog();
                Invoke(a);
            }
            else
            {
                triggerDialog();
            }
        }

        void triggerDialog()
        {
            new RegisterRobo().ShowDialog(this);
        }

        private  void thread(object arg)
        {
            try
            {
                __temporary bl = (__temporary)arg;
                bl.strValue = "Loading MIND library ...";
                try
                {
                    SqlConnection conn = new SqlConnection(Program.connString);
                    conn.Open();
                    conn.Close();
                }
                catch (Exception)
                {
                    bl.strValue = "MIND Library failed.";
                    Thread.Sleep(2000);
                }
                if (bl.strValue == "MIND Library failed.")
                {
                    bl.exit = true;
                    return;
                }
                bl.strValue = "Checking activation ...";
                Program.actRetString = UserSetting.Activation;
                if (Program.actRetString.Length<=0)
                {
                    //new RegisterRobo().ShowDialog();
                    showActivation();
                    if (Program.actRetString.Length<=0)
                    {
                        bl.strValue = "Activation failed.";
                        bl.lblAct = true;
                        bl.actString = "Product not activated";
                        Thread.Sleep(2000);
                        bl.exit = true;
                        return;
                    }
                }
                else if(Program.actRetString.Length<=0)
                {
                    bl.strValue = "Activation failed.";
                    bl.lblAct = true;
                    bl.actString = "Product not activated";
                    Thread.Sleep(2000);
                    bl.exit = true;
                    return;
                }
                bl.lblAct = true;
                bl.actString = Program.actRetString;
                bl.strValue = "Loading USBModem library ...";
                Thread.Sleep(500);
                try
                {
                    USBModem.Port p1 = new Port();
                }
                catch (Exception)
                {
                    bl.strValue = "USBModem Library failed.";
                    Thread.Sleep(2000);
                    bl.exit = true;
                    return;
                }
                bl.strValue = "Loading MachineControl library ...";
                Thread.Sleep(500);
                try
                {
                    Machine mac = new Machine();
                }
                catch (Exception)
                {
                    bl.strValue = "MachineControl Library failed.";
                    Thread.Sleep(2000);
                    bl.exit = true;
                    return;
                }
                bl.value = true;
            }
            catch (StackOverflowException ex) 
            {
                MessageBox.Show("Robo unable to connect with its mind, please try restart robo or reinstall robo program."+Environment.NewLine+"Error message: "+ex.Message,"Mind connectivity",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Program.splashError = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblActivation.Visible = temp.lblAct;
            lblActivation.Text = temp.actString;
            label1.Text = temp.strValue;
            if (temp.exit)
                Application.Exit();
            else if (temp.value)
                Close();
        }
    }
}
