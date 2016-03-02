using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using USBModem;

namespace Robo
{
    public partial class frmModem : Form
    {
        public frmModem()
        {
            InitializeComponent();
        }

        private void frmModem_Load(object sender, EventArgs e)
        {
            Thread thr = new Thread(new ThreadStart(SearchModem));
            thr.Start();
        }

        private void SearchModem()
        {
            clearList();
            disableControls();
            try
            {
                String[] ports = Port.getAvailablePorts();
                foreach (String port in ports)
                {
                    try
                    {
                        Port p = new Port();
                        p.Notification = false;
                        p.disableAutoThreadStart = true;
                        p.setPort(port);
                        p.open();
                        if (p.isOpen())
                        {
                            p.getDirect().WriteLine("AT");
                            Thread.Sleep(300);
                            String data = p.getDirect().ReadExisting();
                            if (data.ToLower().Contains("ok"))
                            {
                                p.getDirect().WriteLine("ATI");//p.write("AT+CGMI");
                                Thread.Sleep(500);
                                String info = p.getDirect().ReadExisting();
                                String model = "", imei = "", manufacturer = "";
                                info = info.ToLower();
                                if (info.Contains("model:") && info.Contains("imei:") && info.Contains("manufacturer"))
                                {
                                    int start=-1, end=-1;
                                    start = info.IndexOf("manufacturer:");
                                    if (start > -1)
                                        start += "manufacturer:".Length;
                                    end = info.IndexOf((char)13, start);
                                    if (start > -1 && end > -1 && end > start)
                                        manufacturer = info.Substring(start, end - start);

                                    start = end = -1;
                                    start = info.IndexOf("model:");
                                    if (start > -1)
                                        start += "model:".Length;
                                    end = info.IndexOf((char)13, start);
                                    if (start > -1 && end > -1 && end > start)
                                        model = info.Substring(start, end - start);

                                    start = end = -1;
                                    start = info.IndexOf("imei:");
                                    if (start > -1)
                                        start += "imei:".Length;
                                    end = info.IndexOf((char)13, start);
                                    if (start > -1 && end > -1 && end > start)
                                        imei = info.Substring(start, end - start);

                                    Modem mod = new Modem();
                                    mod.Port = port;
                                    manufacturer = manufacturer.Trim();
                                    model = model.Trim();
                                    imei = imei.Trim();
                                    mod.Name = manufacturer.ToUpper() + " " + model.ToUpper() + " ("+imei+")";
                                    addToList(mod);
                                }
                            }
                        }
                        p.close();
                    }
                    catch (Exception ex) { 
                        //MessageBox.Show(""+ex); 
                    }
                }
            }
            catch (Exception ex)
            { 
                //MessageBox.Show(""+ex); 
            }
            enableControls();
        }

        void addToList(Modem modem)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action a = () => _addToList(modem);
                    Invoke(a);
                }
                else
                {
                    _addToList(modem);
                }
            }
            catch (Exception) { }
        }

        void _addToList(Modem modem)
        {
            bool flag = true;
            foreach (Object obj in listBox1.Items)
            {
                Modem mdm = (Modem)obj;
                if (mdm.Name == modem.Name)
                    flag = false;
            }
            if(flag)
                listBox1.Items.Add(modem);
        }

        void clearList()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action a = () => _clearList();
                    Invoke(a);
                }
                else
                {
                    _clearList();
                }
            }
            catch (Exception) { }
        }

        void _clearList()
        {
            listBox1.Items.Clear();
        }

        void disableControls()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action a = () => _disableControls();
                    Invoke(a);
                }
                else
                {
                    _disableControls();
                }
            }
            catch (Exception) { }
        }

        void enableControls()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action a = () => _enableControls();
                    Invoke(a);
                }
                else
                {
                    _enableControls();
                }
            }
            catch (Exception) { }
        }

        void _disableControls()
        {
            listBox1.Enabled = btnConnect.Enabled = btnRefresh.Enabled = false;
            btnCancel.Enabled = false;
            Cursor = Cursors.WaitCursor;
        }
        void _enableControls()
        {
            listBox1.Enabled = btnRefresh.Enabled = true;
            btnCancel.Enabled = true;
            Cursor = Cursors.Default;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Thread thr = new Thread(new ThreadStart(SearchModem));
            thr.Start();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                btnConnect.Enabled = true;
            }
            else
            {
                btnConnect.Enabled = false;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    Modem modem = (Modem)listBox1.SelectedItem;
                    Program.selectedModem = modem.Name;
                    Program.selectedPort = modem.Port;
                    Close();
                }
                catch (Exception) { }
            }
        }

        private void frmModem_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!btnCancel.Enabled)
            {
                e.Cancel = true;
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    Modem modem = (Modem)listBox1.SelectedItem;
                    Program.selectedModem = modem.Name;
                    Program.selectedPort = modem.Port;
                    Close();
                }
                catch (Exception) { }
            }
        }
    }

    class Modem
    {
        public string Port { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name + " (" + Port + ")";
        }
    }
}
