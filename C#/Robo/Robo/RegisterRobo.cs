using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Threading;

namespace Robo
{
    public partial class RegisterRobo : Form
    {
        private String receivedData = "";
        public RegisterRobo()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RegisterRobo_Load(object sender, EventArgs e)
        {
            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0)
            {
                textBox1.Focus();
                return;
            }
            if (textBox2.Text.Trim().Length == 0)
            {
                textBox2.Focus();
                return;
            }
            try
            {
                WebClient wc = new WebClient();
                wc.UploadProgressChanged += new UploadProgressChangedEventHandler(wc_UploadProgressChanged);
                wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted);
                btnRegister.Visible = false;
                pb.Visible = true;
                btnRegister.Visible = false;
                btnCancel.Visible = false;
                String postData = "software=robo&name="+textBox1.Text+"&key="+textBox2.Text;
                Uri url = new Uri("http://orgengitech.wcodez.com/client_activation.php");
                //Uri url = new Uri("http://iammegamohan.hosta.biz/client_activation.php");
                //wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0(Windows; U; Windows NT 5.2; rv:1.9.2) Gecko/20100101 Firefox/3.6");
                //wc.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                wc.UseDefaultCredentials = true;
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0(Windows; U; Windows NT 5.2; rv:1.9.2) Gecko/20100101 Firefox/3.6";
                //wc.Headers[HttpRequestHeader.ContentType] = "text/plain";
                //MessageBox.Show(wc.DownloadString(url));
                wc.UploadStringAsync(url, postData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,"Unable to conenct with internet world, try again."+ex,"Internet Connectivity");
            }
        }

        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                receivedData = e.Result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,"Robo unable to connect with its activation server, please check your internet connectivity or contact activation server provider. "+Environment.NewLine+"Error message:"+ex,"Server Protocol",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            downloaded();
        }

        void wc_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            pb.Value = e.ProgressPercentage;
        }


        void downloaded()
        {
            if (receivedData.Contains(textBox2.Text + " "))
            {
                UserSetting.Activation = textBox1.Text;
                Program.actRetString = textBox1.Text;
                Close();
            }
            else
            {
                MessageBox.Show(this,"Company name and serial number not matched, try again."+Environment.NewLine+"Make sure, you are writing in capital or in small letter because its case-sensitive.","Activation failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            pb.Value = 0;
            pb.Visible = false;
            btnCancel.Visible = true;
            btnRegister.Visible = true;
        }
    }
}
