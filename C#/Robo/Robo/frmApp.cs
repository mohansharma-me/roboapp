using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using USBModem;
using MachineControl;
using System.Net.Mail;

namespace Robo
{
    public partial class frmApp : Form
    {
        /// <summary>
        /// This port is used as global connection with usb modem...
        /// </summary>
        private Port curPort;

        /// <summary>
        /// This is listview sorter object with this you need to add listener to ObjListView.ColumnClick+=sorterMethod
        /// </summary>
        private ListViewColumnSorter cSorter;

        /// <summary>
        /// this both are loader and inilizer of form
        /// </summary>
        public frmApp()
        {
            InitializeComponent();
        }
        private void frmApp_Load(object sender, EventArgs e)
        {
            new frmSplash().ShowDialog(this);
            if (Program.splashError)
            {
                MessageBox.Show(this, "Unable to load required library. Try to reinstall Robo with its library or upgrade its library.", "Library failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            UserLogin ul = new UserLogin();
            DialogResult dr = ul.ShowDialog(this);
            if (dr == DialogResult.Cancel || !ul.isLogged)
            {
                Close();
            }
            Program.curUser = ul.username;
            Program.curUserRights = ul.rights;
            lblWelcome.Text = "Welcome "+Program.curUser;
            setSecurity();
            hideAllPans();
            Text = Properties.Resources.appTitle +" - " + Program.actRetString;
            lblLogo.Text = Properties.Resources.appTitle;
            lblLabel.Text = Program.actRetString;
            statusNetwork.Text = "";
            statusLabel.Text = "Disconnected.";
            curPort = new Port();
            curPort.USBConnection += new Port.ConnectionHandler(curPort_USBConnection);
            curPort.MessageReceived += new Port.MessageHandler(curPort_MessageReceived);
            curPort.SignalChanged += new Port.NetworkHandler(curPort_SignalChanged);
            curPort.HandleError += new Port.ErrorHandler(curPort_HandleError);
            curPort.getSignal = true;
            
            
            initConnection();

            lstClients.ColumnClick += __SortOnClick_ListView_onColumnClick;
            lstSites.ColumnClick += __SortOnClick_ListView_onColumnClick;
            lstMachines.ColumnClick += __SortOnClick_ListView_onColumnClick;
            lstMCOMachines.ColumnClick += __SortOnClick_ListView_onColumnClick;
            lvMCSMachines.ColumnClick += __SortOnClick_ListView_onColumnClick;
            lvClientList.ColumnClick += __SortOnClick_ListView_onColumnClick;
        }

        private void setSecurity()
        {
            if (!Program.validateRights(UserRights.AddClients, Program.curUserRights))
                btnShowAddClient.Enabled = false;
            if (!Program.validateRights(UserRights.AddSites, Program.curUserRights))
                btnShowAddSite.Enabled = false;
            if (!Program.validateRights(UserRights.AddMachines, Program.curUserRights))
                btnShowAddMachine.Enabled = false;

            if (!Program.validateRights(UserRights.ViewClients, Program.curUserRights))
                btnShowViewClient.Enabled = false;
            if (!Program.validateRights(UserRights.ViewSites, Program.curUserRights))
                btnShowViewSites.Enabled = false;
            if (!Program.validateRights(UserRights.ViewMachines, Program.curUserRights))
                btnViewMachines.Enabled = false;

            if (!Program.validateRights(UserRights.MachineOperation, Program.curUserRights))
                btnMCOperation.Enabled = false;
            if (!Program.validateRights(UserRights.MachineSettings, Program.curUserRights))
                btnMCSettings.Enabled = false;
            if (!Program.validateRights(UserRights.EmailReports, Program.curUserRights))
                btnEmailReport.Enabled = false;
            if (!Program.validateRights(UserRights.Messages, Program.curUserRights))
                btnSettings.Enabled = false;
        }

        void curPort_USBConnection(Port port, Connection connEvent)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action a = () => showConnection(connEvent);
                    Invoke(a);
                }
                else
                {
                    showConnection(connEvent);
                }
            }
            catch (Exception) { }
        }

        void curPort_MessageReceived(Port port, List<USBModem.Message> receivedMessages)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action a = () => newMessages(receivedMessages);
                    Invoke(a);
                }
                else
                {
                    newMessages(receivedMessages);
                }
            }
            catch (Exception) { }
        }

        void curPort_SignalChanged(Port port, CSIGNAL signal,CREG registration)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Action a = () => ShowSignal(signal, registration);
                    Invoke(a);
                }
                else
                {
                    ShowSignal(signal, registration);
                }
            }
            catch (Exception) { }
        }

        void newMessages(List<USBModem.Message> messages)
        {
            if (messages.Count == 0)
                return;
            List<string> sims = new List<string>();
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into messages values(@index,@status,@sender,@alphabet,@datetime,@message)",conn);
                cmd.Parameters.Add("@index", SqlDbType.Text);
                cmd.Parameters.Add("@status",SqlDbType.Text);
                cmd.Parameters.Add("@sender",SqlDbType.Text);
                cmd.Parameters.Add("@alphabet",SqlDbType.Text);
                cmd.Parameters.Add("@datetime",SqlDbType.Text);
                cmd.Parameters.Add("@message",SqlDbType.Text);
                foreach (USBModem.Message msg in messages)
                {
                    cmd.Parameters["@index"].Value = msg.Index;
                    cmd.Parameters["@status"].Value = msg.Status;
                    cmd.Parameters["@sender"].Value = msg.Sender;
                    cmd.Parameters["@alphabet"].Value = msg.Alphabet;
                    cmd.Parameters["@datetime"].Value = msg.Time;
                    cmd.Parameters["@message"].Value = msg.MessageText;
                    cmd.ExecuteNonQuery();
                }                
                conn.Close();
            }
            catch (Exception) { }

            List<string> updated = new List<string>();    
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select modem_sim from machines",conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    sims.Add(dr[0].ToString());
                }
                dr.Close();
                conn.Close();
                foreach (USBModem.Message msg in messages)
                {
                    String sim=sims.Find(x=>(msg.Sender.Contains(x)));
                    if (sim != null && sim.Length > 0)
                    {
                        msg.MessageText = msg.MessageText.Trim();
                        MCStatus mstatus = new MCStatus(msg.MessageText.Trim(), ManageParameters.getAllParameters(PAR_CATEGORY.STATUS), UserSetting.StatusMessageFormat);
                        if (!mstatus.Error)
                        {
                            try
                            {
                                conn = new SqlConnection(Program.connString);
                                conn.Open();
                                cmd = new SqlCommand("update machines set status=@status,last_updated=@lu where modem_sim LIKE '%"+sim.Trim()+"%'",conn);
                                cmd.Parameters.Add("@status", SqlDbType.Text).Value = msg.MessageText.Trim();
                                DateTime dt=DateTime.Now;
                                cmd.Parameters.Add("@lu", SqlDbType.Text).Value = dt.Day + "/" + dt.Month + "/" + dt.Year + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second + " " + dt.ToString("tt");
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                updated.Add(sim+" :: status updated");
                            }
                            catch (Exception) { }
                        }

                        MCSettings msetting = new MCSettings(msg.MessageText.Trim(), ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS), UserSetting.SettingMessageFormat);
                        if (!msetting.Error)
                        {
                            try
                            {
                                conn = new SqlConnection(Program.connString);
                                conn.Open();
                                cmd = new SqlCommand("update machines set settings=@setting,last_updated=@lu where modem_sim LIKE '%" + sim.Trim() + "%'", conn);
                                cmd.Parameters.Add("@setting", SqlDbType.Text).Value = msg.MessageText.Trim();
                                DateTime dt = DateTime.Now;
                                cmd.Parameters.Add("@lu", SqlDbType.Text).Value = dt.Day + "/" + dt.Month + "/" + dt.Year + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second + " " + dt.ToString("tt");
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                updated.Add(sim + " :: settings updated");
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
            catch (Exception)
            {}
            String output="";
            Decimal counter = 1;
            foreach (String sim in updated)
            {
                output += (counter++).ToString() + ". " + sim + Environment.NewLine;
            }
            output += Environment.NewLine + Environment.NewLine + "Kindly visit machine operation page for more details..";
            if(updated.Count>0)
            MessageBox.Show("Following machines has received their update message:"+Environment.NewLine+output,"Machine updates",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        void showConnection(Connection conn)
        {
            try
            {
                if (conn == Connection.Connected)
                {
                    statusLabel.Text = "Connected to: " + Program.selectedPort;
                }
                else if (conn == Connection.Disconnected)
                {
                    Program.selectedPort = Program.selectedModem;
                    statusNetwork.Text = "";
                    statusLabel.Text = "Disconnected";
                    curPort.close();
                    curPort = new Port();
                    curPort.Notification = false;
                    curPort.USBConnection += new Port.ConnectionHandler(curPort_USBConnection);
                    curPort.MessageReceived += new Port.MessageHandler(curPort_MessageReceived);
                    curPort.SignalChanged += new Port.NetworkHandler(curPort_SignalChanged);
                    curPort.HandleError += new Port.ErrorHandler(curPort_HandleError);
                    curPort.getSignal = true;
                }
            }
            catch (Exception) { }
        }
        void ShowSignal(CSIGNAL signal,CREG reg)
        {
            try
            {
                if (reg == CREG.REGISTERED_HOMENETWORK || reg == CREG.REGISTERED_ROAMING)
                {
                    statusNetwork.Text = "[ " + signal.ToString() + " signal ]";
                }
                else if (reg == CREG.NOTREGISTERED_NOTSEARCHING)
                {
                    statusNetwork.Text = "[ Not registered. ]";
                }
                else if (reg == CREG.NOTREGISTERED_SEARCHING)
                {
                    statusNetwork.Text = "[ Not registered, Searching... ]";
                }
                else if (reg == CREG.REGISTRATION_DENIED)
                {
                    statusNetwork.Text = "[ Registration denied. ]";
                }
            }
            catch (Exception) { }
        }

        void curPort_HandleError(Port port, string errTitle, string errMsg, Exception exception)
        {
            try
            {
                if (port.Notification)
                    if (this.InvokeRequired)
                    {
                        Action a = () => curPort_HandleError(port, errTitle, errMsg, exception);
                        Invoke(a);
                    }
                    else
                    {
                        MessageBox.Show(this, errMsg + Environment.NewLine + Environment.NewLine + "Error message:" + Environment.NewLine + exception.Message, errTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
            }
            catch (Exception) { }
            //}
        }

        /// <summary>
        /// this initConnection method will open dialog for finding usb modem.
        /// and returns the Program[selectedModem, selectedPort] with specific details,
        /// if its contain nothing that mean there is no modem to connect.
        /// </summary>
        private void initConnection()
        {
            try
            {
                if (curPort.ConnectionState == Connection.Connected)
                {
                    MessageBox.Show(this, "Please disconnect connected modem first, then reconnect another modem.", "Modem already connected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                new frmModem().ShowDialog(this);

                if (Program.selectedPort.Length > 0 && Program.selectedPort.ToLower().StartsWith("com"))
                {
                    statusLabel.Text = "Validating modem on port:" + Program.selectedPort;
                    try
                    {
                        curPort.close();
                        curPort.setPort(Program.selectedPort);
                        curPort.Notification = false;
                        curPort.open();
                        if (curPort.isOpen())
                        {
                            if (!curPort._cmgf())
                            {
                                MessageBox.Show(this, "Sorry we are unable to initilize SIM card for using it for message transfering." + Environment.NewLine + "Try to re-connect your modem or restart your system if you are using this modem first time on your system or check whether your this modem is support message transfer services or not.", "SIM initilization failed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                try
                                {
                                    curPort.close();
                                    curPort.stopThreads();
                                }
                                catch (Exception) { }
                                curPort.ConnectionState = Connection.Disconnected;
                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(this, "Connecting modem for communication failed." + Environment.NewLine + ex.Message, "Modem communication failed.", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    if (curPort.isOpen() && curPort.ConnectionState == Connection.Connected)
                    {
                        statusNetwork.Text = "[ Searching... ]";
                        statusLabel.Text = "Connected to " + Program.selectedPort;
                    }
                    else
                    {
                        Program.selectedModem = Program.selectedPort = "";
                        statusNetwork.Text = "";
                        statusLabel.Text = "Disconnected.";
                        curPort = new Port();
                        curPort.Notification = false;
                        curPort.USBConnection += new Port.ConnectionHandler(curPort_USBConnection);
                        curPort.MessageReceived += new Port.MessageHandler(curPort_MessageReceived);
                        curPort.SignalChanged += new Port.NetworkHandler(curPort_SignalChanged);
                        curPort.HandleError += new Port.ErrorHandler(curPort_HandleError);
                        curPort.getSignal = true;
                    }
                }
                else
                {
                    statusLabel.Text = "Disconnected";
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// form closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                curPort.stopThreads();
                curPort.close();
            }
            catch (Exception ex) { }
            Application.Exit();
        }

        /// <summary>
        /// Add client button in Manage Clients panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddClient_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = true;
                foreach (Control ctrl in panAddClient.Controls)
                {
                    if (ctrl.Tag != null && ctrl.Tag.ToString().Equals("input"))
                    {
                        TextBox tb = (TextBox)ctrl;
                        if (tb.Text.Trim().Length == 0)
                        {
                            flag = false;
                        }
                    }
                }
                if (!flag)
                {
                    MessageBox.Show(this, "Please enter valid client details, one of textbox is empty or none filled.", "Client details", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                try
                {
                    SqlConnection conn = new SqlConnection(Program.connString);
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand("select * from clients where name LIKE '%' + @name + '%' or email LIKE '%' + @email + '%' or nickname LIKE '%' + @nname + '%'", conn);
                    cmd1.Parameters.AddWithValue("@name", txtName.Text.ToLower().Trim());
                    cmd1.Parameters.AddWithValue("@email", txtEmail.Text.ToLower().Trim());
                    cmd1.Parameters.AddWithValue("@nname", txtNName.Text.ToLower().Trim());
                    SqlDataReader dr1 = cmd1.ExecuteReader();
                    if (dr1.Read())
                    {
                        MessageBox.Show(this, "You have entered duplicate entries in name, email or nickname, you must provide unique name, email, nickname entries for better client management.", "Duplicate entries", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dr1.Close();
                        conn.Close();
                        return;
                    }
                    dr1.Close();
                    SqlCommand cmd = new SqlCommand("INSERT INTO clients(name,address,contact_person,contact_no,email,nickname) values(@name,@address,@cperson,@cno,@email,@nname)", conn);
                    cmd.Parameters.Add("@name", SqlDbType.Text, txtName.Text.Length).Value = txtName.Text.ToLower().Trim();
                    cmd.Parameters.Add("@address", SqlDbType.Text, txtAddress.Text.Length).Value = txtAddress.Text.ToLower().Trim();
                    cmd.Parameters.Add("@cperson", SqlDbType.Text, txtCPerson.Text.Length).Value = txtCPerson.Text.ToLower().Trim();
                    cmd.Parameters.Add("@cno", SqlDbType.Text, txtCInfo.Text.Length).Value = txtCInfo.Text.ToLower().Trim();
                    cmd.Parameters.Add("@email", SqlDbType.Text, txtEmail.Text.Length).Value = txtEmail.Text.ToLower().Trim();
                    cmd.Parameters.Add("@nname", SqlDbType.Text, txtNName.Text.Length).Value = txtNName.Text.ToLower().Trim();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show(this, "Added successfuly.", "Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    __list_fill_ClientList();
                    //refreshList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Unable to add new record to mind.", "Mind error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// By clicking on refresh button in Manage Clients page will refresh the list of clients.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                refreshList();
            }
            catch (Exception) { }
        }
        /// <summary>
        /// By triggering btnRefresh_Click, this method will be called for refreshing the client list.
        /// </summary>
        private void refreshList()
        {
            try
            {
                lstClients.Items.Clear();
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT c.id as cid,c.name as clientname FROM clients as c", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    String[] data = new String[13];
                    data[0] = dr["cid"].ToString();
                    data[1] = dr["clientname"].ToString();
                    data[2] = "-";
                    SqlConnection cn = new SqlConnection(Program.connString);
                    cn.Open();
                    /* Site count */
                    SqlCommand cm = new SqlCommand("select count(*) from sites where client_id=" + data[0], cn);
                    SqlDataReader _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        data[2] = _dr[0].ToString();
                    }
                    _dr.Close();
                    /* RO Count */
                    cm = new SqlCommand("select count(*) from machines where client_id=" + data[0] + " and mctype LIKE 'RO'", cn);
                    _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        data[3] = _dr[0].ToString();
                    }
                    _dr.Close();
                    /* Softner Count */
                    cm = new SqlCommand("select count(*) from machines where client_id=" + data[0] + " and mctype LIKE 'Softner'", cn);
                    _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        data[4] = _dr[0].ToString();
                    }
                    _dr.Close();
                    /* ATM Count */
                    cm = new SqlCommand("select count(*) from machines where client_id=" + data[0] + " and mctype LIKE 'ATM'", cn);
                    _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        data[5] = _dr[0].ToString();
                    }
                    _dr.Close();
                    /* DM Count */
                    cm = new SqlCommand("select count(*) from machines where client_id=" + data[0] + " and mctype LIKE 'DM'", cn);
                    _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        data[6] = _dr[0].ToString();
                    }
                    _dr.Close();
                    /* MB Count */
                    cm = new SqlCommand("select count(*) from machines where client_id=" + data[0] + " and mctype LIKE 'MB'", cn);
                    _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        data[7] = _dr[0].ToString();
                    }
                    _dr.Close();
                    /* Total machines Count */
                    cm = new SqlCommand("select count(*) from machines where client_id=" + data[0], cn);
                    _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        data[8] = _dr[0].ToString();
                    }
                    _dr.Close();

                    //check health limitations
                    ListViewItem li = new ListViewItem(data);


                    List<String> findlist = new List<String>();
                    List<int> indexlist = new List<int>();

                    findlist.Add("RO"); indexlist.Add(3);
                    findlist.Add("SOFTNER"); indexlist.Add(4);
                    findlist.Add("ATM"); indexlist.Add(5);
                    findlist.Add("DM"); indexlist.Add(6);
                    findlist.Add("MB"); indexlist.Add(7);

                    __machineValidateStatus(data[0], null, 9,findlist,indexlist, ref li);

                    lstClients.Items.Add(li);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to read client details from mind.", "Client detail error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Show validation colors in listviewitem
        /// </summary>
        /// <param name="clientID">Client id from database</param>
        /// <param name="siteID">Site id from database may be null</param>
        /// <param name="statusIndex">Final status index number on listviewitem</param>
        /// <param name="findlist">Find list of string RO,ATM etc..</param>
        /// <param name="indexlist">Index list of int 3,4 etc...</param>
        /// <param name="li">Refference: listviewitem thats accepts changes</param>
        private void __machineValidateStatus(String clientID,String siteID,int statusIndex,List<String> findlist,List<int> indexlist,ref ListViewItem li)
        {
            try
            {
                li.UseItemStyleForSubItems = false;

                List<Machine> finalList = new List<Machine>();
                SqlConnection cn = new SqlConnection(Program.connString);
                cn.Open();
                String query = "select * from machines where client_id=" + clientID;
                if (siteID != null)
                {
                    query += " and site_id=" + siteID;
                }
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataReader _dr = cm.ExecuteReader();

                List<Machine> healthyMachines = new List<Machine>();
                List<Machine> unhealthyMachines = new List<Machine>();
                List<Machine> stopedMachines = new List<Machine>();
                List<Machine> unknownMachines = new List<Machine>();

                while (_dr.Read())
                {
                    String status = _dr["status"].ToString().Trim();
                    Machine tmpMac = new Machine();
                    tmpMac.ClientIdentifier = Int32.Parse(clientID);
                    tmpMac.Identifier = Int32.Parse(_dr[0].ToString());
                    tmpMac.LastUpdated = _dr["last_updated"].ToString();
                    tmpMac.MachineIdentifier = _dr["mc_id"].ToString();
                    tmpMac.MachineType = _dr["mctype"].ToString();
                    tmpMac.ModemSIMNumber = Decimal.Parse(_dr["modem_sim"].ToString());
                    tmpMac.SiteIdentifier = Int32.Parse(_dr["site_id"].ToString());
                    tmpMac.WantToDisplay = new List<string>();
                    tmpMac.Status = new MCStatus(status, ManageParameters.getAllParameters(PAR_CATEGORY.STATUS), UserSetting.StatusMessageFormat);
                    tmpMac.HealthySetting = _dr["healthy_setting"].ToString();
                    if (status.Length > 0)
                    {
                        if (!tmpMac.Status.Error)
                        {
                            finalList.Add(tmpMac);
                        }
                        else
                        {
                            unknownMachines.Add(tmpMac);
                        }
                    }
                    else
                    {
                        unknownMachines.Add(tmpMac);
                    }
                }
                _dr.Close();

                String _out = __calcMatchMCStatus(finalList, ref healthyMachines, ref unhealthyMachines, ref stopedMachines, ref unknownMachines);
                //String _out = __countMachinesStatus(finalList, ref healthyMachines, ref unhealthyMachines, ref stopedMachines, ref unknownMachines);
                cn.Close();
                //li.SubItems[statusIndex].Text = "H(" + healthyMachines.Count + ") U(" + unhealthyMachines.Count + ") N(" + stopedMachines.Count + ") X(" + unknownMachines.Count + ")";
                li.SubItems[statusIndex].Text = healthyMachines.Count.ToString();
                li.SubItems[statusIndex + 1].Text = unhealthyMachines.Count.ToString();
                li.SubItems[statusIndex + 2].Text = stopedMachines.Count.ToString();
                li.SubItems[statusIndex + 3].Text = unknownMachines.Count.ToString();

                Color h = Color.LightGreen, uh = Color.LightSlateGray, sto = Color.Red, un = Color.IndianRed;
                Color fc = Color.White;

                __colorAssignment(Color.IndianRed, Color.White, unknownMachines, findlist, indexlist, ref li);
                __colorAssignment(Color.LightGreen, Color.Black, healthyMachines, findlist, indexlist, ref li);
                __colorAssignment(Color.LightSlateGray, Color.White, unhealthyMachines, findlist, indexlist, ref li);
                __colorAssignment(Color.Red, Color.White, stopedMachines, findlist, indexlist, ref li);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(this,"Unable to fetch data from mind. "+Environment.NewLine+"Error message:"+Environment.NewLine+"Machine status can't be validated.","Machine validation failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private String __calcMatchMCStatus(List<Machine> finalList,ref List<Machine> hlist,ref List<Machine> uhlist,ref List<Machine> slist,ref List<Machine> ulist)
        {
            foreach (Machine mac in finalList)
            {
                List<Variables> variables = getVariablesWithHSetting(mac.HealthySetting);
                foreach (Variables var in variables)
                {
                    Parameter par = mac.Status.parameters.Find(x => (x.Order == var.Order));
                    if (par != null)
                    {
                        try
                        {
                            Decimal value = Decimal.Parse(par.Value);
                            Decimal hf = Decimal.Parse(var.Health_From);
                            Decimal ht = Decimal.Parse(var.Health_To);
                            Decimal u = Decimal.Parse(var.Unhealthy);
                            Decimal s = Decimal.Parse(var.Stoped);
                            int UC = var.UnhealthyCondition;
                            int SC = var.StopedCondition;
                            int flag = 0;

                            if (SC > 0 && value > s) { slist.Add(mac); flag = 1; }
                            if (SC < 0 && value < s) { slist.Add(mac); flag = 1; }
                            if (SC == 0 && value == s) { slist.Add(mac); flag = 1; }

                            if (flag == 0)
                            {
                                flag = 0;
                                if (UC > 0 && value > u) { uhlist.Add(mac); flag = 1; }
                                if (UC < 0 && value < u) { uhlist.Add(mac); flag = 1; }
                                if (UC == 0 && value == u) { uhlist.Add(mac); flag = 1; }
                            }

                            if (flag == 0)
                            {
                                flag = 0;
                                if (value >= hf && value <= ht) { hlist.Add(mac); flag = 1; }
                            }

                            if (flag == 0)
                                ulist.Add(mac);
                        }
                        catch (Exception ex)
                        {}
                    }
                }
            }
            //remove duplicates from all which matches in stopedlist
            foreach (Machine mac in slist)
            {
                hlist.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                uhlist.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                ulist.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
            }
            //remove duplicates from all which matches in unhealthylist except stoped
            foreach (Machine mac in uhlist)
            {
                hlist.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                ulist.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
            }
            //remove as above from unknown
            foreach (Machine mac in hlist)
            {
                ulist.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
            }

            removeDuplicates(slist);
            removeDuplicates(uhlist);
            removeDuplicates(hlist);
            removeDuplicates(ulist);
            return "";
        }

        private List<Variables> getVariablesWithHSetting(String hSetting)
        {
            List<Variables> list = new List<Variables>();
            try
            {
                String temp = hSetting.Replace("{[","");
                temp = temp.Replace("]}","");
                String[] array = temp.Split(new String[] {"]["},StringSplitOptions.None);
                foreach (String param in array)
                {
                    //o hf ht uc u sc s
                    String[] subarray = param.Split(new String[] { " " }, StringSplitOptions.None);
                    if (subarray.Length == 7)
                    {
                        Variables var = new Variables("", subarray[1], subarray[2], subarray[4], subarray[6]);
                        var.Order = subarray[0];
                        int intTemp=1;
                        bool flag = Int32.TryParse(subarray[3], out intTemp);
                        if (flag)
                            var.UnhealthyCondition = intTemp;
                        intTemp = 1;
                        flag = Int32.TryParse(subarray[5], out intTemp);
                        if (flag)
                            var.StopedCondition = intTemp;
                        list.Add(var);
                    }
                }
            }
            catch (Exception ex)
            {}
            return list;
        }

        /// <summary>
        /// Assign color automatically according to the findlist name and machine type from given list.
        /// </summary>
        /// <param name="bgColor"></param>
        /// <param name="fgColor"></param>
        /// <param name="machines"></param>
        /// <param name="findList"></param>
        /// <param name="indexList"></param>
        /// <param name="li"></param>
        private void __colorAssignment(Color bgColor, Color fgColor, List<Machine> machines,List<String> findList,List<int> indexList, ref ListViewItem li)
        {
            try
            {
                foreach (Machine mac in machines)
                {
                    for (int i = 0; i < findList.Count && findList.Count == indexList.Count; i++)
                    {
                        if (findList[i].ToLower().Trim().Equals(mac.MachineType.ToLower().Trim()))
                        {
                            li.SubItems[indexList[i]].BackColor = bgColor;
                            li.SubItems[indexList[i]].ForeColor = fgColor;
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private String __countMachinesStatus(List<Machine> machineList,ref List<Machine> healthyMachines,ref List<Machine> unhealthyMachines,ref List<Machine> stopedMachines,ref List<Machine> unknownMachines)
        {
            String retString = "";
            try
            {
                List<MachineVariables> mparams = ParameterVariables.getMachineVariables();
                if (mparams != null)
                {
                    foreach (Machine tmpMac in machineList)
                    {
                        MachineVariables param = new MachineVariables(tmpMac.MachineType, new List<Variables>());
                        foreach (MachineVariables tmpParams in mparams)
                        {
                            if (tmpParams.Type.ToLower().Trim().Equals(param.Type.ToLower().Trim()))
                            {
                                param = tmpParams;
                                break;
                            }
                        }
                        List<Parameter> chkValues = new List<Parameter>();
                        List<Parameter> chkParams = tmpMac.Status.parameters;//ManageParameters.getAllParameters(PAR_CATEGORY.STATUS);
                        foreach (Parameter par in chkParams)
                        {
                            if (par.CheckForHealthy)
                            {
                                chkValues.Add(par);
                            }
                        }
                        foreach (Variables tmpPar in param.variables)
                        {
                            foreach (Parameter chkvalue in chkValues)
                            {
                                retString = __validateMachinesParametersWithUserParameters(chkvalue.Value, tmpMac, tmpPar, chkvalue.Name, ref healthyMachines, ref unhealthyMachines, ref stopedMachines, ref unknownMachines);
                            }
                        }
                    }
                }
                //remove duplicates from all which matches in stopedlist
                foreach (Machine mac in stopedMachines)
                {
                    healthyMachines.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                    unhealthyMachines.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                    unknownMachines.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                }
                //remove duplicates from all which matches in unhealthylist except stoped
                foreach (Machine mac in unhealthyMachines)
                {
                    healthyMachines.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                    unknownMachines.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                }
                //remove as above from unknown
                foreach (Machine mac in healthyMachines)
                {
                    unknownMachines.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                }

                removeDuplicates(stopedMachines);
                removeDuplicates(unhealthyMachines);
                removeDuplicates(healthyMachines);
                removeDuplicates(unknownMachines);
            }
            catch (Exception) { }
            return retString;
        }

        private void removeDuplicates(List<Machine> from)
        {
            try
            {
                List<Machine> templist = new List<Machine>(from);
                foreach (Machine mac in templist)
                {
                    List<Machine> _tl = from.FindAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                    if (_tl.Count > 0)
                    {
                        from.RemoveAll(x => (x.MachineIdentifier == mac.MachineIdentifier && x.ModemSIMNumber == mac.ModemSIMNumber));
                        from.Add(_tl[0]);
                    }
                }
            }
            catch (Exception) { }
        }

        private String __validateMachinesParametersWithUserParameters(String parmActualValue,Machine tmpMac,Variables tmpPar,String chkValue, ref List<Machine> healthyMachines, ref List<Machine> unhealthyMachines, ref List<Machine> stopedMachines, ref List<Machine> unknownMachines)
        {
            String retString = "";
            try
            {
                if (tmpPar.Name.ToLower().Contains(chkValue.ToLower().Trim()))
                {
                    try
                    {
                        Decimal actualValue = Decimal.Parse(parmActualValue);
                        Decimal hStart = Decimal.Parse(tmpPar.Health_From.Trim());
                        Decimal hTo = Decimal.Parse(tmpPar.Health_To.Trim());
                        Decimal unh = Decimal.Parse(tmpPar.Unhealthy.Trim());
                        Decimal stoped = Decimal.Parse(tmpPar.Stoped.Trim());
                        if (actualValue >= stoped)
                        {
                            //if (!FindDuplicate(unhealthyMachines, unknownMachines, stopedMachines, tmpMac))
                            stopedMachines.Add(tmpMac);
                            retString = "s";
                        }
                        else if (actualValue >= unh)
                        {
                            //if (!FindDuplicate(unhealthyMachines, unknownMachines, stopedMachines, tmpMac))
                            unhealthyMachines.Add(tmpMac);
                            retString = "u";
                        }
                        else if (actualValue >= hStart && actualValue <= hTo)
                        {
                            //if (!FindDuplicate(unhealthyMachines, unknownMachines, stopedMachines, tmpMac))
                            healthyMachines.Add(tmpMac);
                            retString = "h";
                        }
                        else
                        {
                            //if (!FindDuplicate(unhealthyMachines, unknownMachines, stopedMachines, tmpMac))
                            unknownMachines.Add(tmpMac);
                            retString = "x";
                        }
                    }
                    catch (Exception ex)
                    {
                        retString = "PARSE_ERROR";
                    }
                }
            }
            catch (Exception) { }
            return retString;
        }

        private bool FindDuplicate(List<Machine> s1,List<Machine> s2,List<Machine> s3,Machine machine)
        {
            bool flag = false;
            try
            {
                Machine mac = s1.Find(x => (x.MachineIdentifier == machine.MachineIdentifier && x.ModemSIMNumber == machine.ModemSIMNumber));
                flag = mac != null ? true : flag;
                mac = s2.Find(x => (x.MachineIdentifier == machine.MachineIdentifier && x.ModemSIMNumber == machine.ModemSIMNumber));
                flag = mac != null ? true : flag;
                mac = s3.Find(x => (x.MachineIdentifier == machine.MachineIdentifier && x.ModemSIMNumber == machine.ModemSIMNumber));
                flag = mac != null ? true : flag;
            }
            catch (Exception) { }
            return flag;
        }

        /// <summary>
        /// This will hide all other Panels and shows only Manage Client panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnManageClient_Click(object sender, EventArgs e)
        {
            /*try
            {
                hideAllPans();
                cSorter = new ListViewColumnSorter();
                lstClients.ListViewItemSorter = cSorter;
                txtName.Text = txtAddress.Text = txtCInfo.Text = txtCPerson.Text = txtEmail.Text = txtNName.Text = "";
                txtName.Focus();
                refreshList();
                showPan(panViewClient);
                txtName.Focus();
            }
            catch (Exception) { }*/
        }

        /// <summary>
        /// This method actually hides all the panels.
        /// </summary>
        private void hideAllPans()
        {
            try
            {
                panAddClient.Visible = false;
                panViewClient.Visible = false;
                panAddSites.Visible = false;
                panViewSites.Visible = false;
                panViewMachines.Visible = false;
                panAddMachine.Visible = false;
                panEmailReport.Visible = false;

                panMCOperation.Visible = false;
                panMachineSettings.Visible = false;
                label9.Visible = true;
            }
            catch (Exception) { }
        }
        /// <summary>
        /// this method actually show the 'pan'.
        /// </summary>
        /// <param name="pan"></param>
        private void showPan(Panel pan)
        {
            try
            {
                pan.Top = 130;
                pan.Left = 172;
                pan.Width = 846;
                pan.Height = 520;
                pan.Visible = true;
                label9.Visible = false;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// This method will display Manage Sites and hide all other panels.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnManageSite_Click(object sender, EventArgs e)
        {
            /*try
            {
                hideAllPans();
                txtSiteName.Text = txtSiteDescription.Text = "";
                bindCmbClients(cmbClients);
                cSorter = new ListViewColumnSorter();
                lstSites.ListViewItemSorter = cSorter;
                bindSiteLV();
                showPan(panManageSite);
                txtSiteName.Focus();
            }
            catch (Exception) { }*/
        }

        /// <summary>
        /// This method will add the all clients to the passed ComboBox
        /// </summary>
        /// <param name="cmb">Where this method fill all the client with ComboBoxItem(name,id)</param>
        private void bindCmbClients(ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select id,name from clients", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ComboBoxItem ci = new ComboBoxItem();
                    ci.Text = dr[1].ToString();
                    ci.Value = dr[0].ToString();
                    cmb.Items.Add(ci);
                    cmb.SelectedIndex = 0;
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to find all client's name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// this method will fill the ListView of Manage Sites panel.
        /// </summary>
        private void bindSiteLV()
        {
            try
            {
                lstSites.Items.Clear();
                ListViewItem headLi = new ListViewItem(new String[] { "", "", "", "", "LAST UPDATED", "STATUS", "LAST UPDATED", "STATUS", "LAST UPDATED", "STATUS", "LAST UPDATED", "STATUS" });
                headLi.UseItemStyleForSubItems = true;
                headLi.BackColor = Color.PaleTurquoise;
                lstSites.Items.Add(headLi);

                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT s.id as sid,s.name as sitename,c.id as clientid,c.name as clientname FROM sites as s, clients as c WHERE s.client_id=c.id", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    String[] data = new String[13];
                    data[0] = dr["sid"].ToString();
                    data[1] = dr["sitename"].ToString();
                    data[2] = dr["clientid"].ToString();
                    data[3] = dr["clientname"].ToString();

                    SqlConnection cn = new SqlConnection(Program.connString);
                    cn.Open();

                    /* RO Count 4*/
                    SqlCommand cm = new SqlCommand("select * from machines where site_id=" + data[0] + " and client_id=" + data[2] + " and mctype LIKE 'RO'", cn);
                    SqlDataReader _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        int fc=4;
                        data[fc] = _dr["last_updated"].ToString();
                        if (data[fc].Trim().Length == 0)
                            data[fc] = "Not updated";
                        Machine tmpMachine = new Machine(_dr["mc_id"].ToString(), Program.connString, ManageParameters.getAllParameters(PAR_CATEGORY.STATUS), ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS), UserSetting.StatusMessageFormat, UserSetting.SettingMessageFormat);
                        if (!tmpMachine.Status.Error)
                        {
                            Parameter par = tmpMachine.Status.parameters.Find(x => (x.Order == "1"));
                            if (par != null)
                            {
                                MaskedMan.Mask = ""; MaskedMan.Text = "";
                                MaskedMan.Text = __getFormattedValue(par, MaskedMan);
                                data[fc+1] = MaskedMan.Text;
                            }
                        }
                    }
                    _dr.Close();
                    /* Softner Count */
                    cm = new SqlCommand("select * from machines where site_id=" + data[0] + " and client_id=" + data[2] + " and mctype LIKE 'Softner'", cn);
                    _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        int fc = 6;
                        data[fc] = _dr["last_updated"].ToString();
                        if (data[fc].Trim().Length == 0)
                            data[fc] = "Not updated";
                        Machine tmpMachine = new Machine(_dr["mc_id"].ToString(), Program.connString, ManageParameters.getAllParameters(PAR_CATEGORY.STATUS), ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS), UserSetting.StatusMessageFormat, UserSetting.SettingMessageFormat);
                        if (!tmpMachine.Status.Error)
                        {
                            Parameter par = tmpMachine.Status.parameters.Find(x => (x.Order == "1"));
                            if (par != null)
                            {
                                MaskedMan.Mask = ""; MaskedMan.Text = "";
                                MaskedMan.Text = __getFormattedValue(par, MaskedMan);
                                data[fc + 1] = MaskedMan.Text;
                            }
                        }
                    }
                    _dr.Close();
                    /* DM Count */
                    cm = new SqlCommand("select * from machines where site_id=" + data[0] + " and client_id=" + data[2] + " and mctype LIKE 'DM'", cn);
                    _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        int fc = 8;
                        data[fc] = _dr["last_updated"].ToString();
                        if (data[fc].Trim().Length == 0)
                            data[fc] = "Not updated";
                        Machine tmpMachine = new Machine(_dr["mc_id"].ToString(), Program.connString, ManageParameters.getAllParameters(PAR_CATEGORY.STATUS), ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS), UserSetting.StatusMessageFormat, UserSetting.SettingMessageFormat);
                        if (!tmpMachine.Status.Error)
                        {
                            Parameter par = tmpMachine.Status.parameters.Find(x => (x.Order == "1"));
                            if (par != null)
                            {
                                MaskedMan.Mask = ""; MaskedMan.Text = "";
                                MaskedMan.Text = __getFormattedValue(par, MaskedMan);
                                data[fc + 1] = MaskedMan.Text;
                            }
                        }
                    }
                    _dr.Close();
                    /* MB Count */
                    cm = new SqlCommand("select * from machines where site_id=" + data[0] + " and client_id=" + data[2] + " and mctype LIKE 'MB'", cn);
                    _dr = cm.ExecuteReader();
                    if (_dr.Read())
                    {
                        int fc = 10;
                        data[fc] = _dr["last_updated"].ToString();
                        if (data[fc].Trim().Length == 0)
                            data[fc] = "Not updated";
                        Machine tmpMachine = new Machine(_dr["mc_id"].ToString(), Program.connString, ManageParameters.getAllParameters(PAR_CATEGORY.STATUS), ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS), UserSetting.StatusMessageFormat, UserSetting.SettingMessageFormat);
                        if (!tmpMachine.Status.Error)
                        {
                            Parameter par = tmpMachine.Status.parameters.Find(x => (x.Order == "1"));
                            if (par != null)
                            {
                                MaskedMan.Mask = ""; MaskedMan.Text = "";
                                MaskedMan.Text = __getFormattedValue(par, MaskedMan);
                                data[fc + 1] = MaskedMan.Text;
                            }
                        }
                    }
                    _dr.Close();
                    //data[9] = "-";
                    cn.Close();
                    ListViewItem li = new ListViewItem(data);
                    /*
                    List<String> findlist = new List<String>();
                    List<int> indexlist = new List<int>();

                    findlist.Add("RO"); indexlist.Add(4);
                    findlist.Add("SOFTNER"); indexlist.Add(5);
                    findlist.Add("ATM"); indexlist.Add(6);
                    findlist.Add("DM"); indexlist.Add(7);
                    findlist.Add("MB"); indexlist.Add(8);

                    __machineValidateStatus(data[2],data[0], 9, findlist, indexlist, ref li);
                    */
                    lstSites.Items.Add(li);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to fetch full details about all sites.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// this method will add the site from Manage Sites panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSite_Click(object sender, EventArgs e)
        {
            if (cmbClients.SelectedIndex == -1)
            {
                MessageBox.Show(this,"Please select client to add new site under that name.","Error",MessageBoxButtons.OK,MessageBoxIcon.Hand);
                cmbClients.Focus();
                return;
            }
            if (txtSiteName.Text.Trim().Length == 0)
            {
                MessageBox.Show(this, "Please enter site name to add new site.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                txtSiteName.Focus();
                return;
            }
            if (txtSiteDescription.Text.Trim().Length == 0)
            {
                DialogResult diag = MessageBox.Show(this,"Are you sure to you didn't need site description ?","Site description",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (diag == DialogResult.No)
                    return;
            }

            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("select * from sites where client_id=@cid and name LIKE '%' + @sitename + '%'",conn);
                cmd1.Parameters.Add("@cid", SqlDbType.Int).Value = (cmbClients.SelectedItem as ComboBoxItem).Value.ToString();
                cmd1.Parameters.AddWithValue("@sitename", txtSiteName.Text.ToLower().Trim());
                SqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.Read())
                {
                    MessageBox.Show(this, "You have entered duplicate entries in site name, you must provide unique site name entry for better site management.", "Duplicate entries", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dr1.Close();
                    conn.Close();
                    return;
                }
                dr1.Close();
                SqlCommand cmd = new SqlCommand("INSERT INTO sites(client_id,name,description) values(@cid,@name,@desc)", conn);
                cmd.Parameters.Add("@cid", SqlDbType.Int).Value = (cmbClients.SelectedItem as ComboBoxItem).Value.ToString();
                cmd.Parameters.Add("@name", SqlDbType.Text, txtSiteName.Text.Length).Value = txtSiteName.Text.ToLower().Trim();
                cmd.Parameters.Add("@desc", SqlDbType.Text, txtSiteDescription.Text.Length).Value = txtSiteDescription.Text.ToLower().Trim();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show(this, "Successfuly added.", "Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //bindSiteLV();
                __list_fill_SitesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to add new records to the mind.", "Mind error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// this will refresh the sites listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshSites_Click(object sender, EventArgs e)
        {
            bindSiteLV();
        }

        /// <summary>
        /// this called on column click event for sorting user based column in desc or asce.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void __SortOnClick_ListView_onColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                ListView lv = sender as ListView;
                if (e.Column == cSorter.SortColumn)
                {
                    // Reverse the current sort direction for this column.
                    if (cSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                    {
                        cSorter.Order = System.Windows.Forms.SortOrder.Descending;
                    }
                    else
                    {
                        cSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                    }
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    cSorter.SortColumn = e.Column;
                    cSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                }

                // Perform the sort with these new sort options.
                lv.Sort();
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// this will display the Manage Machine panel and hides all other panels.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnManageMC_Click(object sender, EventArgs e)
        {
            /*try
            {
                hideAllPans();
                cSorter = new ListViewColumnSorter();
                lstMachines.ListViewItemSorter = cSorter;
                if (tabControl1.SelectedTab == tabPage1)
                {
                    cmbMCClient.SelectedIndexChanged -= cmbMCClient_SelectedIndexChanged;
                    bindCmbClients(cmbMCClient);
                    cmbMCClient.Text = "Select client";
                    cmbMCSites.Items.Clear();
                    cmbMCSites.Enabled = false;
                    txtLMSIM.Text = txtMCID.Text = "";
                    cmbMachineType.SelectedIndex = -1;
                    cmbMCClient.SelectedIndex = -1;
                    cmbMCClient.SelectedIndexChanged += cmbMCClient_SelectedIndexChanged;
                    cmbMachineType.Text = "Select machine type";
                    cmbMCClient.Text = "Select client";
                }
                try
                {
                    cmbMachineType.Items.Clear();
                    cmbViewMachineType.Items.Clear();
                    SqlConnection conn = new SqlConnection(Program.connString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select type from machine_settings", conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ComboBoxItem cmi = new ComboBoxItem(dr[0].ToString(), dr[0].ToString());
                        cmbMachineType.Items.Add(cmi);
                        cmbViewMachineType.Items.Add(cmi);
                    }
                    dr.Close();
                    conn.Close();
                }
                catch (Exception ex)
                { }

                if (tabControl1.SelectedTab == tabPage2)
                {
                    if (cmbViewMachineType.Items.Count > 0)
                        cmbViewMachineType.SelectedIndex = 0;
                }
                List<Parameter> listParams = ManageParameters.getAllParameters(PAR_CATEGORY.STATUS);
                chkLstWTD.Items.Clear();
                foreach (Parameter par in listParams)
                {
                    chkLstWTD.Items.Add(par);
                }
                showPan(panManageMC);
            }
            catch (Exception) { }*/
        }

        /// <summary>
        /// this will simply add new machine from Add M/C(tabPage2) belongs to ManageMachine panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMC_Click(object sender, EventArgs e)
        {
            if (cmbMachineType.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please select appropriate machine type from list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmbMachineType.Focus();
                return;
            }
            if (cmbMCClient.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please select appropriate client from list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmbMCClient.Focus();
                return;
            }
            if (cmbMCSites.SelectedIndex == -1)
            {
                MessageBox.Show(this, "Please select appropriate site from list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmbMCSites.Focus();
                return;
            }
            if (txtLMSIM.Text.Trim().Length == 0 || txtLMSIM.Text.Trim().Length > 10)
            {
                MessageBox.Show(this, "Please enter valid lcu modem sim number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtLMSIM.Focus();
                return;
            }
            else
            {
                try
                {
                    Decimal dd = Decimal.Parse(txtLMSIM.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,"Invalid sim number.","Sim number",MessageBoxButtons.OK,MessageBoxIcon.Hand);
                    return;
                }
            }
            if (txtMCID.Text.Trim().Length == 0)
            {
                MessageBox.Show(this, "Please enter valid machine id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMCID.Focus();
                return;
            }
            if (chkLstWTD.CheckedItems.Count == 0)
            {
                DialogResult dr = MessageBox.Show(this, "You didn't selected any Want To Display option.", "Are you sure ?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr.ToString().Equals("No"))
                {
                    chkLstWTD.Focus();
                    return;
                }
            }
            if (lvMCHealthySettings.CheckedItems.Count == 0)
            {
                MessageBox.Show(this,"You didn't selected healthy setting parameter to validate machine status. Please select appropriate parameter and enter validation value to it (try again).","Machine validation not selected",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            String wtd = "";
            for (int i = 0; i < chkLstWTD.CheckedItems.Count; i++)
            {
                wtd = wtd + "," + (chkLstWTD.CheckedItems[i] as Parameter).Order;
                if (i == chkLstWTD.CheckedItems.Count - 1)
                {
                    wtd = wtd.Substring(1, wtd.Length - 1);
                }
            }

            foreach (ListViewItem li in lvMCHealthySettings.CheckedItems)
            {
                if (li.SubItems[2].Text.Contains("-") || li.SubItems[3].Text.Contains("-") || li.SubItems[4].Text.Contains("-"))
                {
                    MessageBox.Show(this,"Checked item in healthy setting list has invalid entries, please uncheck that item or enter proper inputs to it.","Invalid validation input",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
            }

            String hss = "{";
            foreach (ListViewItem li in lvMCHealthySettings.CheckedItems)
            {
                String tmp = "[";
                String[] arr = li.SubItems[2].Text.Split(new String[] { " to " }, StringSplitOptions.None);
                int SC=1, UC=1;
                String unh = li.SubItems[3].Text.Replace("Greater than", "");
                unh = unh.Replace("Less than", "");
                unh=unh.Replace("Equal to", "");
                unh=unh.Replace(" ", "");
                if (li.SubItems[3].Text.Contains("Greater"))
                    UC = 1;
                if (li.SubItems[3].Text.Contains("Less"))
                    UC = -1;
                if (li.SubItems[3].Text.Contains("Equal"))
                    UC = 0;

                if (li.SubItems[4].Text.Contains("Greater"))
                    SC = 1;
                if (li.SubItems[4].Text.Contains("Less"))
                    SC = -1;
                if (li.SubItems[4].Text.Contains("Equal"))
                    SC = 0;

                String sto = li.SubItems[4].Text.Replace("Greater than", "");
                sto = sto.Replace("Less than", "");
                sto=sto.Replace("Equal to", "");
                sto=sto.Replace(" ", "");

                tmp += li.SubItems[5].Text + " " + arr[0] + " " + arr[1] + " " + UC + " " + unh + " " + SC + " " + sto;
                tmp += "]";
                hss += tmp;
            }
            hss += "}";

            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("select * from machines where mc_id LIKE '%' + @mid + '%' or modem_sim LIKE '%' + @sim + '%'", conn);
                cmd1.Parameters.AddWithValue("@mid", txtMCID.Text.ToLower().Trim());
                cmd1.Parameters.AddWithValue("@sim", txtLMSIM.Text.ToLower().Trim());
                SqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.Read())
                {
                    MessageBox.Show(this, "You have entered duplicate entries in machine id, sim number, you must provide unique machine id, sim number entries for better machine management.", "Duplicate entries", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dr1.Close();
                    conn.Close();
                    return;
                }
                dr1.Close();
                SqlCommand cmd = new SqlCommand("INSERT INTO machines(mctype,client_id,site_id,mc_id,modem_sim,want_to_display,last_updated,status,settings,healthy_setting) values(@mtype,@cid,@sid,@mid,@msim,@wtd,null,'','',@hsetting)", conn);
                cmd.Parameters.Add("@mtype", SqlDbType.Text, cmbMachineType.SelectedText.Trim().Length).Value = cmbMachineType.SelectedItem.ToString();
                cmd.Parameters.Add("@cid", SqlDbType.Int).Value = (cmbMCClient.SelectedItem as ComboBoxItem).Value.ToString();
                cmd.Parameters.Add("@sid", SqlDbType.Int).Value = (cmbMCSites.SelectedItem as ComboBoxItem).Value.ToString();
                cmd.Parameters.Add("@mid", SqlDbType.Text, txtMCID.Text.Trim().Length).Value = txtMCID.Text.Trim().ToLower();
                cmd.Parameters.Add("@msim", SqlDbType.Text, txtLMSIM.Text.Trim().Length).Value = txtLMSIM.Text.Trim().ToLower();
                cmd.Parameters.Add("@wtd", SqlDbType.Text, wtd.Length).Value = wtd.ToLower();
                cmd.Parameters.Add("@hsetting", SqlDbType.Text).Value = hss;
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show(this, "Successfuly added.", "Machine Add", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to add new records to mind.", "Mind error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// this will simply reset the Add MC panel or View MC panel to its default position and values or say state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_Click(object sender, EventArgs e)
        {
            /*try
            {
                cmbMCClient.SelectedIndexChanged -= cmbMCClient_SelectedIndexChanged;
                bindCmbClients(cmbMCClient);
                cmbMCClient.Text = "Select client";
                cmbMCSites.Items.Clear();
                cmbMCSites.Enabled = false;
                txtLMSIM.Text = txtMCID.Text = "";
                cmbMachineType.SelectedIndex = -1;
                cmbMCClient.SelectedIndex = -1;
                cmbMCClient.SelectedIndexChanged += cmbMCClient_SelectedIndexChanged;
                cmbMCClient.Text = "Select client";

                if (tabControl1.SelectedTab == tabPage2)
                {
                    cmbViewMachineType.SelectedIndex = 0;
                }
            }
            catch (Exception) { }*/
        }

        /// <summary>
        /// This will simply fill simply fill the cmbSites in ManageMachines panel according to the selected client from dropdown menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMCClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbMCClient.SelectedIndex>-1)
            __fillSitesOnClients(cmbMCClient, cmbMCSites);
        }

        /// <summary>
        /// This will actually fill the sites's dropdown menu according to client's dropdown's selected item.
        /// </summary>
        private void __fillSitesOnClients(ComboBox _client,ComboBox _sites)
        {
            try
            {
                if (_client.SelectedIndex > -1)
                {
                    _sites.SelectedIndex = -1;
                    _sites.Text = "Select site";
                    _sites.Items.Clear();
                    _sites.Enabled = false;
                    SqlConnection conn = new SqlConnection(Program.connString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT id,name from sites where client_id=@cid", conn);
                    cmd.Parameters.Add("@cid", SqlDbType.Int).Value = (_client.SelectedItem as ComboBoxItem).Value;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        _sites.Enabled = true;
                        ComboBoxItem ci = new ComboBoxItem();
                        ci.Text = dr["name"].ToString();
                        ci.Value = dr["id"].ToString();
                        _sites.Items.Add(ci);
                    }
                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to fetch sites according to selected client.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// this will simply refresh ViewMC's listview with appropriate machines details selected by RadioButtons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewMachine_TypeSelected(object sender, EventArgs e)
        {
            try
            {
                if (cmbViewMachineType.SelectedIndex>-1)
                {
                    String type = (cmbViewMachineType.SelectedItem as ComboBoxItem).Value;
                    lstMachines.Items.Clear();
                    SqlConnection conn = new SqlConnection(Program.connString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select m.mc_id,m.modem_sim,m.client_id,s.name,m.last_updated from machines as m,sites as s where m.site_id=s.id and mctype LIKE '" + type + "'", conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        String[] data = new String[6];
                        data[0] = dr["mc_id"].ToString();
                        data[1] = dr["modem_sim"].ToString();
                        data[2] = dr["client_id"].ToString();
                        data[3] = dr["name"].ToString();
                        data[4] = dr["last_updated"].ToString();
                        if (data[4].Trim().Length == 0)
                            data[4] = "--";
                        data[5] = "--";
                        Machine machine = new Machine(data[0], Program.connString, ManageParameters.getAllParameters(PAR_CATEGORY.STATUS), ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS), UserSetting.StatusMessageFormat, UserSetting.SettingMessageFormat);
                        if (machine.Status.Error)
                        {
                            data[5] = "Not updated!";
                        }
                        else
                        {
                            MaskedMan.Mask = "";
                            MaskedMan.Text = "";

                            data[5] = "";
                            foreach (Parameter par in machine.Status.parameters)
                            {
                                bool flag = false;
                                foreach (string str in machine.WantToDisplay)
                                {
                                    if (par.Order.Equals(str.Trim()))
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (flag)
                                {
                                    MaskedMan.Text = __getFormattedValue(par, MaskedMan);
                                    data[5] += par.Name.ToUpper() + " = " + MaskedMan.Text + " ," + Environment.NewLine;
                                }
                            }

                            MaskedMan.Mask = "";
                            MaskedMan.Text = "";
                        }
                        ListViewItem li = new ListViewItem(data);
                        li.ToolTipText = data[5];
                        lstMachines.Items.Add(li);
                    }
                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to read machine(s) details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Whenever users click on statusNetwork or statusLabel will start initilization of connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusNetwork_Click(object sender, EventArgs e)
        {
            initConnection();
        }
        /// <summary>
        /// Whenever users click on statusNetwork or statusLabel will start initilization of connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusLabel_Click(object sender, EventArgs e)
        {
            initConnection();
        }

        /// <summary>
        /// will shows the M/C Operation panel and hides all other panels.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMCOperation_Click(object sender, EventArgs e)
        {
            try
            {
                hideAllPans();
                cSorter = new ListViewColumnSorter();
                lstMCOMachines.ListViewItemSorter = cSorter;
                bindCmbClients(cmbMCOclients);
                cmbMCOSites.Items.Clear();
                cmbMCOSites.Enabled = false;
                lstMCOMachines.Items.Clear();
                cmbMCOclients.SelectedIndex = -1;
                groupBox3.Enabled = false;
                groupBox4.Visible = true;
                showPan(panMCOperation);
                cmbMCOclients.Focus();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Show appropriate form when user click on Machine Setting option.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMCSettings_Click(object sender, EventArgs e)
        {
            try
            {
                hideAllPans();
                cSorter = new ListViewColumnSorter();
                lvMCSMachines.ListViewItemSorter = cSorter;
                bindCmbClients(cmbMCSClients);
                cmbMCSSites.Items.Clear();
                cmbMCSSites.Enabled = false;
                lvMCSMachines.Items.Clear();
                cmbMCSClients.SelectedIndex = -1;
                groupBox6.Enabled = false;
                showPan(panMachineSettings);
                cmbMCSClients.Focus();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Its enable and fill the sites's dropdown list of selected client's sites.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMCOclients_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox3.Enabled = false;
                cmbMCOSites.Items.Clear();
                lstMCOMachines.Items.Clear();
                __fillSitesOnClients(cmbMCOclients, cmbMCOSites);
            }
            catch (Exception) { }
        }

        private void __fillMachinesLVOnSites(ListView _lv,ComboBox _sites)
        {
            try
            {
                if (_sites.SelectedIndex > -1)
                {
                    _lv.Items.Clear();
                    SqlConnection conn = new SqlConnection(Program.connString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select id,mc_id,modem_sim,last_updated,mctype from machines where site_id=@sid", conn);
                    cmd.Parameters.Add("@sid", SqlDbType.Int).Value = (_sites.SelectedItem as ComboBoxItem).Value;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ListViewItem li = new ListViewItem(new String[] { dr[1].ToString(), dr[2].ToString(), dr[4].ToString() });
                        _lv.Items.Add(li);
                    }
                    dr.Close();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to fetch machines according to selected site, please try again or restart the robo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// This will fill the list of machines by identifying selected site in sites's dropdown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMCOSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            __fillMachinesLVOnSites(lstMCOMachines, cmbMCOSites);
        }

        /// <summary>
        /// this will provide the machine details of selected machine from this in right panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstMCOMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstMCOMachines.SelectedIndices.Count > 0)
                {
                    lblMCO_MCID.Text = lblMCO_SimNumber.Text = lblMCO_Status.Text = lblMCO_Updated.Text = "--";
                    lvSitesList.Items.Clear();
                    ListViewItem li = lstMCOMachines.SelectedItems[0];
                    Machine mac = new Machine(li.SubItems[0].Text.Trim(), Program.connString, ManageParameters.getAllParameters(PAR_CATEGORY.STATUS), ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS), UserSetting.StatusMessageFormat, UserSetting.SettingMessageFormat);
                    if (mac.Ready)
                    {
                        lblMCO_MCID.Text = li.SubItems[0].Text;
                        lblMCO_SimNumber.Text = li.SubItems[1].Text;

                        if (mac.LastUpdated.Trim().Length != 0)
                        {
                            lblMCO_Updated.Text =mac.LastUpdated;     
                        } else {
                            lblMCO_Updated.Text = "Not yet!"; 
                        }

                        Parameter par=mac.Status.parameters.Find(x=>(x.Order=="1"));
                        if(par!=null) {
                            MaskedMan.Mask="";MaskedMan.Text="";
                            lblMCO_Status.Text=__getFormattedValue(par,MaskedMan);
                        } else {
                            lblMCO_Status.Text="-";
                        }

                        if (!mac.Status.Error)
                        {
                            lvMCStatus.Items.Clear();
                            foreach(Parameter p in mac.Status.parameters) 
                            {
                                MaskedMan.Mask="";MaskedMan.Text="";
                                String fmtVal = __getFormattedValue(p,MaskedMan);
                                ListViewItem _li=new ListViewItem(new String[] {p.Name,fmtVal});
                                lvMCStatus.Items.Add(_li);
                            }
                        }

                        groupBox3.Enabled = true;
                    }
                    else if (mac.Error == MCError.INVALID_IDENTIFIER)
                    {
                        MessageBox.Show(this, "Machine not found in mind, you have removed it.", "Machine not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if(!mac.Ready)
                    {
                        MessageBox.Show(this, "There is something gone wrong, please try again with some time or restart robo." + Environment.NewLine + "Error msg:" + Environment.NewLine + Machine.ErrorMessage, mac.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,"Unable to find machine details.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// this provides general listview right click integration to delete that items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void __lstView_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    try
                    {
                        ListView lv = (ListView)sender;
                        ContextMenu cm = new ContextMenu();
                        String delete = "Delete";
                        if (lv.Name == lstClients.Name)
                            delete += " client";
                        if (lv.Name == lstSites.Name)
                            delete += " site";
                        if (lv.Name == lstMachines.Name)
                            delete += " machine";

                        if (lv.Name == lstMachines.Name)
                        {
                            cm.MenuItems.Add(new MenuItem("Update status ("+lv.SelectedItems[0].SubItems[1].Text+")", __mouseOnRightClickDelete));
                            cm.MenuItems.Add(new MenuItem("Update setting (" + lv.SelectedItems[0].SubItems[1].Text + ")", __mouseOnRightClickDelete));
                            cm.MenuItems.Add(new MenuItem("-", __mouseOnRightClickDelete));
                        }
                        cm.MenuItems.Add(new MenuItem(delete, __mouseOnRightClickDelete));
                        cm.MenuItems.Add(new MenuItem("-", __mouseOnRightClickDelete));
                        cm.MenuItems.Add(new MenuItem("Show details", __mouseOnRightClickDelete));
                        if (lv.Name == lstSites.Name && lv.SelectedIndices[0] == 0)
                        {
                        }
                        else
                        {
                            cm.Show((Control)sender, e.Location);
                        }
                    }
                    catch (Exception ex) { }
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// this will simply process the delete operation triggered by the right click menu of appropriate listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void __mouseOnRightClickDelete(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                MenuItem mi = (MenuItem)sender;
                ListView lv = (ListView)mi.GetContextMenu().SourceControl;

                if(mi.Text.Equals("Show details"))
                    if (lv.Name == lstClients.Name)
                    {
                        Program.showDetails = "clients "+lv.SelectedItems[0].SubItems[0].Text;
                        new ShowDetails().ShowDialog(this);
                    }
                    else if (lv.Name == lstSites.Name)
                    {
                        Program.showDetails = "sites " + lv.SelectedItems[0].SubItems[0].Text;
                        new ShowDetails().ShowDialog(this);
                    }
                    else if (lv.Name == lstMachines.Name)
                    {
                        Program.showDetails = "machines " + lv.SelectedItems[0].SubItems[0].Text;
                        new ShowDetails().ShowDialog(this);
                    }

                if (mi.Text.StartsWith("Update") && lv.Name==lstMachines.Name)
                {
                    if (mi.Text.Contains("status"))
                    {
                        MCO_SendMessage(lv.SelectedItems[0].SubItems[1].Text.Trim(),"Update status".ToUpper(), UserSetting.ReadStatusMessage);
                    }
                    if (mi.Text.Contains("setting"))
                    {
                        MCO_SendMessage(lv.SelectedItems[0].SubItems[1].Text.Trim(), "Update setting".ToUpper(), UserSetting.ReadSettingMessage);
                    }
                }

                if(mi.Text.StartsWith("Delete"))
                    if (lv.Name == lstClients.Name)
                    {
                        DialogResult diag = MessageBox.Show(this,"Are you sure delete this client with its all sites and machines from mind ?","Delete client",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                        if(diag==DialogResult.Yes)
                        foreach (ListViewItem li in lv.SelectedItems)
                        {
                            SqlConnection conn = new SqlConnection(Program.connString);
                            conn.Open();
                            SqlCommand cmd = new SqlCommand("delete from machines where client_id=@cid", conn);
                            cmd.Parameters.Add("@cid", SqlDbType.Int).Value = li.SubItems[0].Text.Trim();
                            cmd.ExecuteNonQuery();

                            cmd = new SqlCommand("delete from sites where client_id=@cid", conn);
                            cmd.Parameters.Add("@cid", SqlDbType.Int).Value = li.SubItems[0].Text.Trim();
                            cmd.ExecuteNonQuery();

                            cmd = new SqlCommand("delete from clients where id=@cid", conn);
                            cmd.Parameters.Add("@cid", SqlDbType.Int).Value = li.SubItems[0].Text.Trim();
                            cmd.ExecuteNonQuery();

                            lv.Items.Remove(li);

                            conn.Close();
                        }
                    }
                    else if (lv.Name == lstSites.Name)
                    {
                        DialogResult diag = MessageBox.Show(this, "Are you sure delete this site with its all machines from mind ?", "Delete site", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (diag == DialogResult.Yes)
                            foreach (ListViewItem li in lv.SelectedItems)
                            {
                                SqlConnection conn = new SqlConnection(Program.connString);
                                conn.Open();
                                SqlCommand cmd = new SqlCommand("delete from machines where site_id=@cid", conn);
                                cmd.Parameters.Add("@cid", SqlDbType.Int).Value = li.SubItems[0].Text.Trim();
                                cmd.ExecuteNonQuery();

                                cmd = new SqlCommand("delete from sites where id=@cid", conn);
                                cmd.Parameters.Add("@cid", SqlDbType.Int).Value = li.SubItems[0].Text.Trim();
                                cmd.ExecuteNonQuery();

                                lv.Items.Remove(li);

                                conn.Close();
                            }
                    }
                    else if (lv.Name == lstMachines.Name)
                    {
                        DialogResult diag = MessageBox.Show(this, "Are you sure delete this machine from mind ?", "Delete machine", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (diag == DialogResult.Yes)
                            foreach (ListViewItem li in lv.SelectedItems)
                            {
                                SqlConnection conn = new SqlConnection(Program.connString);
                                conn.Open();
                                SqlCommand cmd = new SqlCommand("delete from machines where mc_id LIKE '" + li.SubItems[0].Text.Trim() + "'", conn);
                                cmd.ExecuteNonQuery();

                                lv.Items.Remove(li);

                                conn.Close();
                            }
                    }
                Cursor = Cursors.Default;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(this,"Unable to delete selected record, please try again or restart robo.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Send message for status update.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMCO_StatusUpdate_Click(object sender, EventArgs e)
        {
            MCO_SendMessage("Update status".ToUpper(), UserSetting.ReadStatusMessage);
        }

        /// <summary>
        /// Provide boolean return by indicating that Modem is connected or not.
        /// </summary>
        private bool isConnected
        {
            get {
                bool ret = false;
                if (Program.selectedModem.Length > 0 && Program.selectedPort.Length > 0 && curPort.isOpen() && curPort.ConnectionState==Connection.Connected)
                {
                    ret = true;
                }
                return ret;
            }
        }

        /// <summary>
        /// Temporary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblWelcome_Click_1(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Send message for setting update.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMCO_SettingUpdate_Click(object sender, EventArgs e)
        {
            MCO_SendMessage("Update settings".ToUpper(), UserSetting.ReadSettingMessage);
        }

        /// <summary>
        /// Show reading's value when appropriate reading choosen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMCO_Readings_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (cmbMCO_Readings.SelectedIndex > -1)
            {
                Parameter par = (Parameter)cmbMCO_Readings.SelectedItem;
                txtMCO_valReading.Text = __getFormattedValue(par,txtMCO_valReading);
            }*/
        }

        private String __getFormattedValue(Parameter par,MaskedTextBox textBox)
        {
            try
            {
                String text = par.Name;
                String value = par.Value.Trim();
                String format = par.Format.Trim();
                String final = par.Value.Trim();
                if (format.Contains("{") && format.Contains("}"))
                {
                    String temp = format.Replace("{", "").Replace("}", "").Trim();
                    String[] arr = temp.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    List<string[]> values = new List<string[]>();

                    foreach (String _arr in arr)
                    {
                        String[] newarr = _arr.Split(new String[] { ":" }, StringSplitOptions.None);
                        if (newarr.Length == 2)
                        {
                            values.Add(newarr);
                        }
                    }
                    foreach (string[] trp in values)
                    {
                        if (value.Equals(trp[0]))
                        {
                            final = trp[1];
                            break;
                        }
                    }
                    textBox.Mask = "";
                }
                else
                {
                    textBox.Mask = format;
                    textBox.Text = value;
                    final = textBox.Text;
                }
                return final;
            }
            catch (Exception) { MessageBox.Show(this,"Unable to parse parameter value. Please re-upadte status or setting because parameters are may cracked.","Parameter currpted",MessageBoxButtons.OK,MessageBoxIcon.Error); }
            return "PAR-ERR";
        }

        /// <summary>
        /// Show setting's value when appropriate reading choosen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMCO_Settings_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (cmbMCO_Settings.SelectedIndex > -1)
            {
                Parameter cmi = (Parameter)cmbMCO_Settings.SelectedItem;
                txtMCO_valSetting.Text = __getFormattedValue(cmi,txtMCO_valSetting);
            }*/
        }

        /// <summary>
        /// Send message to start machine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMCO_MC_Start_Click(object sender, EventArgs e)
        {
            MCO_SendMessage("start machine".ToUpper(), UserSetting.MachineStartMessage);
        }

        /// <summary>
        /// Thread to send message
        /// </summary>
        private static void __thrSendMessage(object pObj)
        {
            try
            {
                object[] oArr = (object[])pObj;
                Port p = (Port)oArr[0];
                String number = (String)oArr[1];
                String cmd = (String)oArr[2];
                String work = (String)oArr[3];
                if (p.ConnectionState == Connection.Connected && p.isOpen())
                {
                    MessageSent output = p.sendMessage(number.Trim(), cmd);
                    if (output == MessageSent.ERROR)
                    {
                        MessageBox.Show("Can't send message unknown error:[1||2] is occured, please check that sim has valid activation to send message. If it's occured again please try re-connect modem and sim card and then try again.", "Message not send - SIM Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (output == MessageSent.CANT_SEND)
                    {
                        MessageBox.Show("Can't send message known error:[Modem not initilized] is occured, please check that modem is connected or reconnect it and try again.", "Message not send", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (output == MessageSent.SENT)
                    {
                        MessageBox.Show("Message successively sent to " + number + " !", work, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Message sent to " + number + " !", work, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please connect modem with robo to send message.", "No modem found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// To send message to the given number with appropriate machine command.
        /// </summary>
        /// <param name="work"></param>
        /// <param name="cmd"></param>
        private void MCO_SendMessage(String work,String cmd)
        {
            DialogResult diag = MessageBox.Show(this,"Are you sure to send message to "+lblMCO_SimNumber.Text+" for "+work+" ?",work,MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (diag == DialogResult.Yes)
            {
                ParameterizedThreadStart thStart = new ParameterizedThreadStart(__thrSendMessage);
                Thread thread = new Thread(thStart);
                object[] objs = new object[4];
                objs[0] = curPort;
                objs[1] = lblMCO_SimNumber.Text;
                objs[2] = cmd;
                objs[3] = work;
                thread.Start(objs);
            }
        }
        private void MCO_SendMessage(String number, String work, String cmd)
        {
            lblMCO_SimNumber.Text = number;
            MCO_SendMessage(work, cmd);
            lblMCO_SimNumber.Text = "";
        }
        /// <summary>
        /// Send message to stop machine.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMCO_MC_Stop_Click(object sender, EventArgs e)
        {
            MCO_SendMessage("STOP MACHINE".ToUpper(), UserSetting.MachineStopMessage);
        }

        /// <summary>
        /// Send message to start sand filter backwash
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMCO_MC_SFBW_Click(object sender, EventArgs e)
        {
            MCO_SendMessage("START SAND FILTER BACKWASH".ToUpper(), UserSetting.SandFilterBackWashMessage);
        }

        /// <summary>
        /// Send message to start carbon filter backwash
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMCO_MC_CFBW_Click(object sender, EventArgs e)
        {
            MCO_SendMessage("START CARBON FILTER BACKWASH".ToUpper(), UserSetting.CarbonFilterBackWashMessage);
        }

        /// <summary>
        /// Send message to start softner filter backwash
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMCO_MC_SR_Click(object sender, EventArgs e)
        {
            MCO_SendMessage("START SOFTNER FILTER BACKWASH".ToUpper(),UserSetting.SoftnerFilterBackWashMessage);
        }

        /// <summary>
        /// Send message to flushing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMCO_MC_Flushing_Click(object sender, EventArgs e)
        {
            MCO_SendMessage("START FLUSHING".ToUpper(),UserSetting.StartFlusingMessage);
        }

        /// <summary>
        /// Show menu when user click global setting button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettings_Click(object sender, EventArgs e)
        {
            ContextMenu cm = new ContextMenu();
            //cm.MenuItems.Add(new MenuItem("Manage &parameters", __btnSettingRightClick_Click));
            ///**/cm.MenuItems.Add(new MenuItem("Parameter &variables", __btnSettingRightClick_Click));
            cm.MenuItems.Add(new MenuItem("&Messages", __btnSettingRightClick_Click));
            //cm.MenuItems.Add(new MenuItem("-"));
            cm.MenuItems.Add(new MenuItem("&Fake Update", __btnSettingRightClick_Click));
            //cm.MenuItems.Add(new MenuItem("-"));
            //cm.MenuItems.Add(new MenuItem("Product &activation", __btnSettingRightClick_Click));
            cm.Show(btnSettings, new Point(1, 1));
        }

        /// <summary>
        /// Provide process to the user's choosen global setting options.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void __btnSettingRightClick_Click(object sender, EventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)sender;
                if (mi.Text.Equals("Manage &parameters"))
                {
                    new ManageParameters().ShowDialog(this);
                }
                else if (mi.Text.Equals("&Messages"))
                {
                    new frmMessages().ShowDialog(this);
                }
                else if (mi.Text.Equals("Parameter &variables"))
                {
                    new ParameterVariables().ShowDialog(this);
                }
                else if (mi.Text.Equals("&Fake Update"))
                {
                    USBModem.Message msg1=new USBModem.Message();
                    msg1.Sender=txtName.Text;
                    msg1.MessageText=txtAddress.Text;

                    USBModem.Message msg2=new USBModem.Message();
                    msg2.Sender=txtName.Text;
                    msg2.MessageText=txtNName.Text;

                    List<USBModem.Message> lstMSGS=new List<USBModem.Message>();
                    lstMSGS.Add(msg1);
                    lstMSGS.Add(msg2);
                    curPort_MessageReceived(curPort, lstMSGS);
                }
            }
            catch (Exception ex)
            {}
        }

        private void statusStrip1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextMenu cm = new ContextMenu();
                    cm.MenuItems.Add(new MenuItem("&Connect modem", StatusBar_RightClick));
                    cm.MenuItems.Add(new MenuItem("&Disconnect modem", StatusBar_RightClick));
                    cm.MenuItems.Add(new MenuItem("-", StatusBar_RightClick));
                    cm.MenuItems.Add(new MenuItem("&Modem info", StatusBar_RightClick));
                    cm.Show(statusStrip1, e.Location);
                }
            }
            catch (Exception ex) { }
        }

        void StatusBar_RightClick(object sender, EventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)sender;
                if (mi.Text.Equals("&Connect modem"))
                {
                    if (isConnected)
                    {
                        MessageBox.Show(this,"Modem already connected, please disconnect first for reconnecting the modem.","Modem present",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    } else {
                        initConnection();
                    }
                }
                else if (mi.Text.Equals("&Disconnect modem"))
                {
                    if (isConnected)
                    {
                        try
                        {
                            Program.selectedPort = Program.selectedModem = "";
                            curPort.ConnectionState = Connection.Disconnected;
                            curPort.stopThreads();
                            curPort.close();
                            statusNetwork.Text = "";
                            statusLabel.Text = "Disconnected";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this,"Unable to disconnect modem right now, try again later.","Can't disconnect",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(this,"Modem not found to disconnect it.","No modem found",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    }
                }
                else if (mi.Text.Equals("&Modem info"))
                {
                    MessageBox.Show(this,"Modem connected => "+(isConnected?"Yes":"No")+Environment.NewLine+"Modem name: "+Program.selectedModem+Environment.NewLine+"Modem on port: "+Program.selectedPort,"Modem info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {}
        }

        private void _listView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ListView lv = (ListView)sender;
                if (lv.SelectedItems.Count > 0)
                {
                    if (lv.Name == lstClients.Name)
                    {
                        Program.showDetails = "clients "+lv.SelectedItems[0].SubItems[0].Text;
                        new ShowDetails().ShowDialog(this);
                    }
                    else if (lv.Name == lstSites.Name && lv.SelectedIndices[0]!=0)
                    {
                        Program.showDetails = "sites " + lv.SelectedItems[0].SubItems[0].Text;
                        new ShowDetails().ShowDialog(this);
                    }
                    else if (lv.Name == lstMachines.Name)
                    {
                        Program.showDetails = "machines " + lv.SelectedItems[0].SubItems[0].Text;
                        new ShowDetails().ShowDialog(this);
                    }
                }
            }
            catch (Exception ex)
            {}
        }

        private void btnUserSettings_Click(object sender, EventArgs e)
        {
            new UserManagement().ShowDialog(this);
            //new UserSettings().ShowDialog(this);
        }

        private void btnEmailReport_Click(object sender, EventArgs e)
        {
            hideAllPans();
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select name,contact_person,contact_no,email,nickname,address from clients",conn);
                SqlDataReader dr = cmd.ExecuteReader();
                lvEmailClients.Items.Clear();
                while (dr.Read())
                {
                    ListViewItem li = new ListViewItem(new String[] { "",dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString() });
                    lvEmailClients.Items.Add(li);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to read client details from robo's mind, please try again.", "MIND Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            showPan(panEmailReport);
            txtYourName.Focus();
        }

        private void lblMCS_Format_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            hideAllPans();
        }

        private void btnShowAddClient_Click(object sender, EventArgs e)
        {
            hideAllPans();
            __list_fill_ClientList();
            showPan(panAddClient);
            txtName.Text = txtAddress.Text = txtCInfo.Text = txtCPerson.Text = txtEmail.Text = txtNName.Text = "";
            txtName.Focus();
        }

        private void __list_fill_ClientList()
        {
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from clients", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                lvClientList.Items.Clear();
                while (dr.Read())
                {
                    ListViewItem li = new ListViewItem(new String[] { dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString() });
                    lvClientList.Items.Add(li);
                }
                dr.Close();
                conn.Close();
                cSorter = new ListViewColumnSorter();
                lvClientList.ListViewItemSorter = cSorter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to fetch client list from MIND Library, please try again after restarting robo.", "MIND Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowViewClient_Click(object sender, EventArgs e)
        {
            hideAllPans();
            refreshList();
            cSorter = new ListViewColumnSorter();
            lstClients.ListViewItemSorter = cSorter;
            showPan(panViewClient);
        }

        private void btnShowAddSite_Click(object sender, EventArgs e)
        {
            hideAllPans();
            txtSiteName.Text = txtSiteDescription.Text = "";
            bindCmbClients(cmbClients);
            cSorter = new ListViewColumnSorter();
            lvSitesList.ListViewItemSorter = cSorter;
            __list_fill_SitesList();
            showPan(panAddSites);
        }

        private void __list_fill_SitesList()
        {
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select s.id,s.name,c.name as client_name,s.description from sites as s, clients as c where s.client_id=c.id",conn);
                SqlDataReader dr = cmd.ExecuteReader();
                lvSitesList.Items.Clear();
                while (dr.Read())
                {
                    ListViewItem li = new ListViewItem(new String[] { dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString() });
                    lvSitesList.Items.Add(li);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to fetch sites details from MIND Library.", "MIND Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowViewSites_Click(object sender, EventArgs e)
        {
            hideAllPans();
            bindSiteLV();
            cSorter = new ListViewColumnSorter();
            lstSites.ListViewItemSorter = cSorter;
            showPan(panViewSites);
        }

        private void btnViewMachines_Click(object sender, EventArgs e)
        {
            hideAllPans();
            bindCmbClients(cmbMCClients);
            cSorter = new ListViewColumnSorter();
            lstMachines.ListViewItemSorter = cSorter;
            cmbMCClients.SelectedIndex = -1;
            try
            {
                //cmbMachineType.Items.Clear();
                cmbViewMachineType.Items.Clear();
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select type from machine_settings", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ComboBoxItem cmi = new ComboBoxItem(dr[0].ToString(), dr[0].ToString());
                    //cmbMachineType.Items.Add(cmi);
                    cmbViewMachineType.Items.Add(cmi);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            { }
            showPan(panViewMachines);
        }

        private void cmbMCClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMCClients.SelectedIndex > -1)
            {
                __fillSitesOnClients(cmbMCClients, cmbMCVSites);
            }
        }

        private void btnMCShow_Click(object sender, EventArgs e)
        {
            String queryType = "";
            if (cmbMCClients.SelectedIndex > -1)
            {
                queryType = "and m.client_id=" + (cmbMCClients.SelectedItem as ComboBoxItem).Value+" and";
            }
            if (cmbMCVSites.SelectedIndex > -1)
            {
                queryType = "and s.id=" + (cmbMCVSites.SelectedItem as ComboBoxItem).Value + " and";
            }
            if (cmbViewMachineType.SelectedIndex <= -1)
            {
                MessageBox.Show(this, "Please select appropriate machine type from machine type list.", "No machine type selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                String type = (cmbViewMachineType.SelectedItem as ComboBoxItem).Value;
                lstMachines.Items.Clear();
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select m.mc_id,m.modem_sim,m.client_id,s.name,m.last_updated from machines as m,sites as s where m.site_id=s.id " + queryType + " mctype LIKE '" + type + "'", conn);
                
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    String[] data = new String[11];
                    data[0] = dr["mc_id"].ToString();
                    data[1] = dr["modem_sim"].ToString();
                    data[2] = dr["client_id"].ToString();
                    data[3] = dr["name"].ToString();
                    data[4] = dr["last_updated"].ToString();
                    if (data[4].Trim().Length == 0)
                        data[4] = "--";
                    data[5] = "--";
                    Machine machine = new Machine(data[0], Program.connString, ManageParameters.getAllParameters(PAR_CATEGORY.STATUS), ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS), UserSetting.StatusMessageFormat, UserSetting.SettingMessageFormat);
                    if (machine.Status.Error)
                    {
                        data[5] = "Not updated!";
                    }
                    else
                    {
                        

                        MaskedMan.Mask = "";MaskedMan.Text = "";

                        Parameter _param = machine.Status.parameters.Find(x => (x.Order == "1"));
                        if (_param != null)
                        {
                            MaskedMan.Text = __getFormattedValue(_param, MaskedMan);
                            data[5] = MaskedMan.Text;
                        }
                        _param = machine.Status.parameters.Find(x => (x.Order == "10"));
                        if (_param != null)
                        {
                            MaskedMan.Mask = ""; MaskedMan.Text = "";
                            MaskedMan.Text = __getFormattedValue(_param, MaskedMan);
                            data[6] = MaskedMan.Text;
                        }
                        _param = machine.Status.parameters.Find(x => (x.Order == "11"));
                        if (_param != null)
                        {
                            MaskedMan.Mask = ""; MaskedMan.Text = "";
                            MaskedMan.Text = __getFormattedValue(_param, MaskedMan);
                            data[7] = MaskedMan.Text;
                        }
                        _param = machine.Status.parameters.Find(x => (x.Order == "8"));
                        if (_param != null)
                        {
                            MaskedMan.Mask = ""; MaskedMan.Text = "";
                            MaskedMan.Text = __getFormattedValue(_param, MaskedMan);
                            data[8] = MaskedMan.Text;
                        }
                        _param = machine.Status.parameters.Find(x => (x.Order == "9"));
                        if (_param != null)
                        {
                            MaskedMan.Mask = ""; MaskedMan.Text = "";
                            MaskedMan.Text = __getFormattedValue(_param, MaskedMan);
                            data[9] = MaskedMan.Text;
                        }
                        MaskedMan.Mask = "";
                        MaskedMan.Text = "";
                        data[10] = "";
                        foreach (Parameter par in machine.Status.parameters)
                        {
                            bool flag = false;
                            foreach (string str in machine.WantToDisplay)
                            {
                                if (par.Order.Equals(str.Trim()))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                MaskedMan.Text = __getFormattedValue(par, MaskedMan);
                                data[10] += par.Name.ToUpper() + " = " + MaskedMan.Text + " ," + Environment.NewLine;
                            }
                        }

                        MaskedMan.Mask = "";
                        MaskedMan.Text = "";
                    }
                    ListViewItem li = new ListViewItem(data);
                    li.ToolTipText = data[5];
                    lstMachines.Items.Add(li);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception) { }
        }

        private void btnShowAddMachine_Click(object sender, EventArgs e)
        {
            hideAllPans();
            bindCmbClients(cmbMCClient);

            cmbMCClient.SelectedIndexChanged -= cmbMCClient_SelectedIndexChanged;
            cmbMCClient.Text = "Select client";
            cmbMCSites.Items.Clear();
            cmbMCSites.Enabled = false;
            txtLMSIM.Text = txtMCID.Text = "";
            cmbMachineType.SelectedIndex = -1;
            cmbMCClient.SelectedIndex = -1;
            cmbMachineType.Text = "Select machine type";
            cmbMCClient.Text = "Select client";
            cmbMCClient.SelectedIndexChanged += cmbMCClient_SelectedIndexChanged;

            try
            {
                cmbMachineType.Items.Clear();
                //cmbViewMachineType.Items.Clear();
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select type from machine_settings", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ComboBoxItem cmi = new ComboBoxItem(dr[0].ToString(), dr[0].ToString());
                    cmbMachineType.Items.Add(cmi);
                    //cmbViewMachineType.Items.Add(cmi);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            { }

            List<Parameter> listParams = ManageParameters.getAllParameters(PAR_CATEGORY.STATUS);
            chkLstWTD.Items.Clear();
            lvMCHealthySettings.Items.Clear();
            foreach (Parameter par in listParams)
            {
                chkLstWTD.Items.Add(par);
                if (par.CheckForHealthy)
                {
                    ListViewItem li = new ListViewItem(new String[] { "", par.Name, "-", "-", "-", par.Order.ToString() });
                    lvMCHealthySettings.Items.Add(li);
                }
            }

            showPan(panAddMachine);
        }

        private void lvMCHealthySettings_DoubleClick(object sender, EventArgs e)
        {
            if (lvMCHealthySettings.SelectedItems.Count > 0)
            {
                ListViewItem li=lvMCHealthySettings.SelectedItems[0];
                HealthySettingDialog hsd = new HealthySettingDialog();
                hsd.itemDescription = li.SubItems[1].Text;
                hsd.Order = li.SubItems[5].Text;
                if (!li.SubItems[2].Text.Equals("-"))
                {
                    String[] temp = li.SubItems[2].Text.Split(new String[] { " to " }, StringSplitOptions.None);
                    hsd.HF = temp[0];
                    hsd.HT = temp[1];
                }
                if (!li.SubItems[3].Text.Equals("-"))
                {
                    String temp = li.SubItems[3].Text.Substring(li.SubItems[3].Text.LastIndexOf(' '), li.SubItems[3].Text.Length - li.SubItems[3].Text.LastIndexOf(' '));
                    hsd.U = temp;
                    if (li.SubItems[3].Text.Contains("Greater"))
                        hsd.UC = 1;
                    if (li.SubItems[3].Text.Contains("Less"))
                        hsd.UC = -1;
                    if (li.SubItems[3].Text.Contains("Equal"))
                        hsd.UC = 0;
                }
                if (!li.SubItems[4].Text.Equals("-"))
                {
                    String temp = li.SubItems[4].Text.Substring(li.SubItems[4].Text.LastIndexOf(' '), li.SubItems[4].Text.Length - li.SubItems[4].Text.LastIndexOf(' '));
                    hsd.S = temp;

                    if (li.SubItems[4].Text.Contains("Greater"))
                        hsd.SC = 1;
                    if (li.SubItems[4].Text.Contains("Less"))
                        hsd.SC = -1;
                    if (li.SubItems[4].Text.Contains("Equal"))
                        hsd.SC = 0;
                }
                DialogResult dr = hsd.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    lvMCHealthySettings.SelectedItems[0].SubItems[2].Text = hsd.HF + " to " + hsd.HT;
                    String conc = "";
                    if (hsd.UC > 0)
                        conc = "Greater than";
                    if (hsd.UC < 0)
                        conc = "Less than";
                    if (hsd.UC == 0)
                        conc = "Equal to";
                    lvMCHealthySettings.SelectedItems[0].SubItems[3].Text = conc + " " + hsd.U;
                    conc = "";
                    if (hsd.SC > 0)
                        conc = "Greater than";
                    if (hsd.SC < 0)
                        conc = "Less than";
                    if (hsd.SC == 0)
                        conc = "Equal to";
                    lvMCHealthySettings.SelectedItems[0].SubItems[4].Text = conc + " " + hsd.S;
                }
                if(dr==DialogResult.Cancel)
                {
                    lvMCHealthySettings.SelectedItems[0].Checked = false;
                }
            }
        }

        private void cmbMCSClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox6.Enabled = false;
            cmbMCSSites.Items.Clear();
            lvMCSMachines.Items.Clear();
            __fillSitesOnClients(cmbMCSClients, cmbMCSSites);
        }

        private void cmbMCSSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            __fillMachinesLVOnSites(lvMCSMachines, cmbMCSSites);
        }

        private void lvMCSMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lvMCSMachines.SelectedIndices.Count > 0)
                {
                    lblMCS_Status.Text = lblMCSMCID.Text = lblMCSSimNumber.Text = lblMCSUpdatedOn.Text = "--";
                    lvMCSSetting.Items.Clear();
                    ListViewItem li = lvMCSMachines.SelectedItems[0];
                    Machine mac = new Machine(li.SubItems[0].Text.Trim(), Program.connString, ManageParameters.getAllParameters(PAR_CATEGORY.STATUS), ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS), UserSetting.StatusMessageFormat, UserSetting.SettingMessageFormat);
                    if (mac.Ready)
                    {
                        
                        lblMCSMCID.Text = li.SubItems[0].Text;
                        lblMCSSimNumber.Text = li.SubItems[1].Text;

                        if (mac.LastUpdated.Trim().Length != 0)
                        {
                            lblMCSUpdatedOn.Text = mac.LastUpdated;
                        }
                        else
                        {
                            lblMCSUpdatedOn.Text = "Not yet!";
                        }

                        Parameter par = mac.Status.parameters.Find(x => (x.Order == "1"));
                        if (par != null)
                        {
                            MaskedMan.Mask = ""; MaskedMan.Text = "";
                            lblMCS_Status.Text = __getFormattedValue(par, MaskedMan);
                        }
                        else
                        {
                            lblMCO_Status.Text = "-";
                        }

                        if (!mac.Settings.Error)
                        {
                            lvMCSSetting.Items.Clear();
                            foreach (Parameter p in mac.Settings.parameters)
                            {
                                MaskedMan.Mask = ""; MaskedMan.Text = "";
                                ListViewItem _li = new ListViewItem(new String[] { p.Name, __getFormattedValue(p, MaskedMan), p.Format,p.Order });
                                lvMCSSetting.Items.Add(_li);
                            }
                        }

                        groupBox6.Enabled = true;
                    }
                    else if (mac.Error == MCError.INVALID_IDENTIFIER)
                    {
                        MessageBox.Show(this, "Machine not found in mind, you have removed it.", "Machine not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!mac.Ready)
                    {
                        MessageBox.Show(this, "There is something gone wrong, please try again with some time or restart robo." + Environment.NewLine + "Error msg:" + Environment.NewLine + Machine.ErrorMessage, mac.Error.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to find machine details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lvMCSSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void lvMCSSetting_DoubleClick(object sender, EventArgs e)
        {
            if (lvMCSSetting.SelectedItems.Count > 0)
            {
                GetSettingValue form = new GetSettingValue();
                ListViewItem li = lvMCSSetting.SelectedItems[0];
                form.itemDescription = li.SubItems[0].Text;
                form.format = li.SubItems[2].Text.Trim();
                form.value = li.SubItems[1].Text.Replace(".", "").Replace(":", "").Replace("/", "");
                DialogResult dr=form.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    MaskedMan.Mask = ""; MaskedMan.Text = "";
                    Parameter par = new Parameter(form.itemDescription, form.format, "-1", "setting", form.value, false);
                    li.SubItems[1].Text = __getFormattedValue(par, MaskedMan);
                }
            }
            
        }

        private void btnApplySetting_Click(object sender, EventArgs e)
        {
            String output = UserSetting.WriteSettingMessage;
            String sep = UserSetting.WriteSettingSeparator;
            if (lvMCSSetting.Items.Count > 0)
            {
                for (int i = 1; i <= lvMCSSetting.Items.Count; i++)
                {
                    if(i!=lvMCSSetting.Items.Count)
                        output += "{" + i + "}" + sep;
                    if(i==lvMCSSetting.Items.Count)
                        output += "{" + i + "}";
                }
                foreach (ListViewItem li in lvMCSSetting.Items)
                {
                    String value = li.SubItems[1].Text.Replace(".", "").Replace(":", "").Replace("/", "");
                    if (li.SubItems[2].Text.Contains("{"))
                    {
                        String temp = li.SubItems[2].Text.Replace("{", "").Replace("}","");
                        String[] array = temp.Split(new String[] {","},StringSplitOptions.None);
                        foreach (String arr1 in array)
                        {
                            String[] subarray = arr1.Split(new String[] {":"},StringSplitOptions.None);
                            if (subarray.Length == 2)
                            {
                                if (subarray[1].Equals(li.SubItems[1].Text))
                                {
                                    value = subarray[0];
                                    break;
                                }
                            }
                        }
                    }
                    output = output.Replace("{" + li.SubItems[3].Text.Trim() + "}", value);
                }
                if (output.Contains("{") || output.Contains("}"))
                {
                    MessageBox.Show(this, "Number of setting parameter has invalid count, please try again by removing machine and re-adding this machine from MIND.", "Invalid parameter count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //MessageBox.Show(output);
                    MCO_SendMessage(lblMCSSimNumber.Text.Trim(), "APPLY SETTING", output);
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmApp_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void btnSendMail_Click(object sender, EventArgs e)
        {
            if (txtYourName.Text.Trim().Length == 0)
            {
                MessageBox.Show(this, "You must enter your name to send report to selected emails.", "Your name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (lbEmails.Items.Count == 0)
            {
                MessageBox.Show(this,"You must add atleast one email address to send report to it.","No emails",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if (lvEmailClients.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "You must select atleast one client to send report.", "No clients", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<String> fileDatas = new List<String>();
            List<String> clientNames = new List<String>();
            foreach (ListViewItem li in lvEmailClients.CheckedItems)
            {
                try
                {
                    SqlConnection conn = new SqlConnection(Program.connString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select id,name,address,contact_person,contact_no,email,nickname from clients where name LIKE '%' + @cname + '%'",conn);
                    cmd.Parameters.AddWithValue("@cname",li.SubItems[1].Text);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        String cid = dr["id"].ToString();
                        String temp = Program.templateClient;
                        temp = temp.Replace("%title%","Client: "+dr["name"].ToString());
                        temp = temp.Replace("%clientname%", dr["name"].ToString());
                        temp = temp.Replace("%cperson%", dr["contact_person"].ToString());
                        temp = temp.Replace("%cno%", dr["contact_no"].ToString());
                        temp = temp.Replace("%email%", dr["email"].ToString());
                        temp = temp.Replace("%nickname%", dr["nickname"].ToString());
                        temp = temp.Replace("%address%", dr["address"].ToString());

                        SqlConnection cn = new SqlConnection(Program.connString);
                        cn.Open();
                        SqlCommand cm = new SqlCommand("select * from sites where client_id="+cid,cn);
                        SqlDataReader d = cm.ExecuteReader();
                        while (d.Read())
                        {
                            String tmp = "<tr><td>" + d["id"].ToString() + "</td><td>" + d["name"].ToString() + "</td><td>" + d["description"].ToString() + "</td></tr>%sitetr%";
                            temp = temp.Replace("%sitetr%",tmp);
                        }
                        d.Close();
                        temp = temp.Replace("%sitetr%","");

                        cm = new SqlCommand("select mc_id from machines where client_id="+cid,cn);
                        d=cm.ExecuteReader();
                        List<Parameter> lpStatus = ManageParameters.getAllParameters(PAR_CATEGORY.STATUS);
                        List<Parameter> lpSetting = ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS);
                        String fmtStatus = UserSetting.StatusMessageFormat;
                        String fmtSetting = UserSetting.SettingMessageFormat;
                        while (d.Read())
                        {
                            Machine mac = new Machine(d[0].ToString(),Program.connString,lpStatus,lpSetting,fmtStatus,fmtSetting);

                            String tmp = "<tr><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>";
                            tmp = tmp.Replace("{1}", mac.MachineIdentifier);
                            tmp = tmp.Replace("{2}", mac.SiteName);
                            tmp = tmp.Replace("{3}", mac.ModemSIMNumber.ToString());
                            tmp = tmp.Replace("{4}", mac.MachineType);
                            Parameter par = mac.Status.parameters.Find(x=>(x.Order=="1"));
                            if (par != null)
                            {
                                MaskedMan.Mask = ""; MaskedMan.Text = "";
                                tmp = tmp.Replace("{5}", __getFormattedValue(par, MaskedMan));
                            }
                            else
                            {
                                tmp = tmp.Replace("{5}", "Not updated");
                            }
                            String tmp1 = "";
                            if(!mac.Status.Error)
                            foreach (Parameter pp in mac.Status.parameters)
                            {
                                MaskedMan.Mask = ""; MaskedMan.Text = "";
                                tmp1 += pp.Name.ToUpper() + " = " + __getFormattedValue(pp,MaskedMan)+"<br/>";
                            }
                            tmp = tmp.Replace("{6}",tmp1)+"%machinetr%";
                            temp = temp.Replace("%machinetr%",tmp);
                        }
                        d.Close();
                        temp = temp.Replace("%machinetr%","");
                        cn.Close();

                        fileDatas.Add(temp);
                        clientNames.Add(li.SubItems[1].Text);
                    }
                    dr.Close();
                    conn.Close();
                }
                catch (Exception ex) {
                    MessageBox.Show(this,"Error in sending mail to defined address."+Environment.NewLine+"Error message:"+ex.Message,"Can't send mail",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            String emails = "";
            foreach (String eaddress in lbEmails.Items)
            {
                emails += eaddress + ",";
            }
            emails = emails.Substring(0, emails.Length - 1);
            object[] objs=new object[4];
            objs[0] = fileDatas;
            objs[1] = clientNames;
            objs[2] = emails;
            objs[3] = txtYourName.Text;
            Thread thrSender = new Thread(new ParameterizedThreadStart(__thread_send_mail));
            thrSender.Start(objs);
        }

        private void __thread_send_mail(object obj)
        {
            MessageBox.Show("In very short-time robo will give you confirmation about email sent or not. Please wait for next notification. Robo will give you notification for each client report delivery to your mailbox.","Sending mail",MessageBoxButtons.OK,MessageBoxIcon.Information);
            object[] objs=(object[])obj;
            List<String> fileDatas = (List<String>)objs[0];
            List<String> clientNames = (List<String>)objs[1];
            String emails = (String)objs[2];
            String name = (String)objs[3];

            for (int i = 0; i < fileDatas.Count; i++)
            {
                bool flag=SendMail("no-replay@gmail.com", emails, "Robo Report: " + clientNames[i] + " details", fileDatas[i]+"</br></br>- "+name, "robo.ver1.orgengitech", "?roborobo");
                if (flag)
                    MessageBox.Show("Client "+clientNames[i]+"'s details has been sent to given address","Sent",MessageBoxButtons.OK,MessageBoxIcon.Information);
                else
                    MessageBox.Show("Client " + clientNames[i] + "'s details was not sent to given address", "Sent", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddEmail_Click(object sender, EventArgs e)
        {
            if (isEmail(txtEmailAddress.Text.Trim()))
            {
                if (lbEmails.Items.Contains(txtEmailAddress.Text.ToLower().Trim()))
                {
                    txtEmailAddress.Text = "";
                    txtEmailAddress.Focus();
                }
                else
                {
                    lbEmails.Items.Add(txtEmailAddress.Text.ToLower().Trim());
                    txtEmailAddress.Text = "";
                    txtEmailAddress.Focus();
                }
            }
            else
            {
                MessageBox.Show(this, "Invalid email address, try again.", "Email address", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool isEmail(String emailAddress)
        {
            bool flag = false;
            String[] arr1 = emailAddress.Split(new String[] {"@"},StringSplitOptions.None);
            if (arr1.Length == 2)
            {
                String[] arr2 = arr1[1].Split(new String[] { "." }, StringSplitOptions.None);
                if (arr2.Length == 2)
                    flag = true;
            }
            return flag;
        }

        private void lbEmails_DoubleClick(object sender, EventArgs e)
        {
            if (lbEmails.SelectedIndex > -1)
            {
                lbEmails.Items.RemoveAt(lbEmails.SelectedIndex);
            }
        }

        private bool SendMail(String from,String to,String subject,String html,String username,String password)
        {
            SmtpClient sc = new SmtpClient("smtp.gmail.com");
            MailMessage msg = null;

            try
            {
                msg = new MailMessage(from,to, subject,html);
                msg.IsBodyHtml = true;
                sc.EnableSsl = true;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.UseDefaultCredentials = false;
                sc.Credentials = new System.Net.NetworkCredential(username, password);
                sc.Send(msg);
                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(this, "Error in sending mail to defined address." + Environment.NewLine + "Error message:" + ex.Message, "Can't send mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            finally
            {
                if (msg != null)
                {
                    msg.Dispose();
                }
            }
        }

        private void btnMCUpdate_Click(object sender, EventArgs e)
        {
            if (machineSIMNumber == "")
                machineSIMNumber = lblMCO_SimNumber.Text;
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add(new MenuItem("&Update Status", __rightClick_updateButton));
            cm.MenuItems.Add(new MenuItem("&Update Settings", __rightClick_updateButton));
            cm.Show((Control)sender, new Point(1,1));
        }
        private String machineSIMNumber = "";
        private void __rightClick_updateButton(object sender,EventArgs e)
        {
            try
            {
                MenuItem mi=(MenuItem)sender;
                if (mi.Text.Equals("&Update Status"))
                {
                    MCO_SendMessage(machineSIMNumber, "UPDATE STATUS", UserSetting.ReadStatusMessage);
                }
                if (mi.Text.Equals("&Update Settings"))
                {
                    MCO_SendMessage(machineSIMNumber, "UPDATE SETTINGS", UserSetting.ReadSettingMessage);
                }
            } catch(Exception) {}
        }

        private void btnMCUpdate1_Click(object sender, EventArgs e)
        {
            machineSIMNumber = lblMCSSimNumber.Text;
            btnMCUpdate_Click(sender, e);
        }
    }

}
