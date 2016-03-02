using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MachineControl;

namespace Robo
{
    public partial class ShowDetails : Form
    {
        public ShowDetails()
        {
            InitializeComponent();
        }

        private void ShowDetails_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Loading details ...";
            String input = Program.showDetails;
            if (!(input.Contains("clients ") || input.Contains("sites ") || input.Contains("machines ")))
            {
                MessageBox.Show(this,"Less details are provided while opening this window, please try again.","No details found",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                Close();
            }
            String client_id = "", site_id = "", machine_id = "";
            if (input.Contains("clients "))
            {
                String[] arr = input.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 2)
                    client_id = arr[1].Trim();
            }
            if (input.Contains("sites "))
            {
                String[] arr = input.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 2)
                    site_id = arr[1].Trim();
            }
            if (input.Contains("machines "))
            {
                String[] arr = input.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 2)
                    machine_id = arr[1].Trim();
            }

            if (machine_id.Length > 0)
            {
                site_id = fetchSiteID(machine_id);
                client_id = fetchClientID(site_id, machine_id);
            }
            else if (site_id.Length > 0)
            {
                client_id = fetchClientID(site_id, "");
            }

            fillClientDetails(client_id);
            if (site_id.Length > 0)
                fillSiteDetails(site_id);
            fillMachinesDetails(client_id, site_id);
            lblStatus.Text = "Details loaded.";
        }

        private void fillMachinesDetails(String client_id,String site_id)
        {
            String q1 = "select * from machines as m,(select id as sid,name as sitename from sites) as s where sid=site_id and client_id=" + client_id;
            String q2 = q1 + " and site_id=" + site_id;
            String q="";
            if (client_id.Length > 0 && site_id.Length > 0)
                q = q2;
            else if (client_id.Length > 0)
                q = q1;
            else
            {
                MessageBox.Show(this,"Provided identification details about machines is invalid.","Identification details",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            try
            {
                SqlConnection conn=new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(q,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem li = new ListViewItem(new String[] {dr[0].ToString(),dr["mc_id"].ToString(),dr["mctype"].ToString(),dr["sitename"].ToString(),dr["modem_sim"].ToString(),dr["last_updated"].ToString() });
                    lstMachines.Items.Add(li);
                }
                dr.Close();
                conn.Close();
                if (lstMachines.Items.Count > 0)
                    gMachines.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,"Unable to read machine details from mind.","Mind error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        private void fillSiteDetails(String site_id)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from sites where id=" + site_id, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtSiteID.Text = site_id;
                    txtSiteName.Text = dr["name"].ToString();
                    txtSiteDescription.Text = dr["description"].ToString();
                    gSites.Enabled = true;
                }
                else
                {
                    MessageBox.Show(this, "No site having this identification found.", "Site not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to read site details from mind.", "Mind Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void fillClientDetails(String client_id)
        {
            try
            {
                SqlConnection conn=new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from clients where id="+client_id,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtClientID.Text = client_id;
                    txtClientName.Text = dr["name"].ToString();
                    txtClientAddress.Text = dr["address"].ToString();
                    txtClientContactPerson.Text = dr["contact_person"].ToString();
                    txtClientContactInfo.Text = dr["contact_no"].ToString();
                    txtClientEmail.Text = dr["email"].ToString();
                    txtClientNickname.Text = dr["nickname"].ToString();
                    gClients.Enabled = true;
                }
                else
                {
                    MessageBox.Show(this,"No client having this identification found.","Client not found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,"Unable to read client details from mind.","Mind Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private string fetchClientID(String site,String machine)
        {
            String cid = "-1";
            String q1 = "select client_id from sites where id=" + site;
            String q2 = "select client_id from machines where mc_id LIKE '" + machine + "'";
            String q = "";
            if (site.Length!=0)
                q = q1;
            if (machine.Length!=0)
                q = q2;
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand(q,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cid = dr[0].ToString();
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex) { }
            return cid;
        }
        private string fetchSiteID(String machine)
        {
            String sid = "-1";
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select site_id from machines where mc_id LIKE '"+machine+"'",conn);
                SqlDataReader dr=cmd.ExecuteReader();
                if (dr.Read())
                {
                    sid = dr[0].ToString();
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex) { }
            return sid;
        }

        private void btnClientUpdate_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Updating client ...";
            String id, name, address, cperson, cinfo, email, nickname;
            id = txtClientID.Text.Trim().ToLower();
            name = txtClientName.Text.Trim().ToLower();
            address = txtClientAddress.Text.Trim().ToLower();
            cperson = txtClientContactPerson.Text.Trim().ToLower();
            cinfo = txtClientContactInfo.Text.Trim().ToLower();
            email = txtClientEmail.Text.Trim().ToLower();
            nickname = txtClientNickname.Text.Trim().ToLower();

            bool flag = false;
            flag = validateClientDetails(id, txtClientID, "Client id") ? true : flag;
            flag = validateClientDetails(name, txtClientName, "Client name") ? true : flag;
            flag = validateClientDetails(address, txtClientAddress, "Client address") ? true : flag;
            flag = validateClientDetails(cperson, txtClientContactPerson, "Client contact person") ? true : flag;
            flag = validateClientDetails(cinfo, txtClientContactInfo, "Client contact info") ? true : flag;
            flag = validateClientDetails(email, txtClientEmail, "Client email") ? true : flag;
            flag = validateClientDetails(nickname, txtClientNickname, "Client nickname") ? true : flag;
            if (flag)
            {
                lblStatus.Text = "Updating client failed.";
                return;
            }

            try
            {
                SqlConnection conn=new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from clients where id!=@id and (name LIKE '%' + @name + '%' or email LIKE '%' + @email + '%' or nickname LIKE '%' + @nname + '%')", conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@nname", nickname);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtClientEmail.Text = email;
                    txtClientName.Text = name;
                    txtClientNickname.Text = nickname;
                    MessageBox.Show(this,"You have entered duplicate entries in name, email or nickname, you must provide unique name, email, nickname entries for better client management.","Duplicate entries",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                    lblStatus.Text = "Updating client failed.";
                    return;
                }
                dr.Close();
                cmd = new SqlCommand("update clients set name=@name,address=@address,contact_person=@cperson,contact_no=@cnumber,email=@email,nickname=@nname where id=@id",conn);
                cmd.Parameters.Add("@name",SqlDbType.Text).Value=name;
                cmd.Parameters.Add("@address",SqlDbType.Text).Value=address;
                cmd.Parameters.Add("@cperson",SqlDbType.Text).Value=cperson;
                cmd.Parameters.Add("@cnumber",SqlDbType.Text).Value=cinfo;
                cmd.Parameters.Add("@email",SqlDbType.Text).Value=email;
                cmd.Parameters.Add("@nname",SqlDbType.Text).Value=nickname;
                cmd.Parameters.Add("@id",SqlDbType.Int).Value=id;
                cmd.ExecuteNonQuery();
                conn.Close();
                btnClientUpdate.Enabled = false;
                lblStatus.Text = "Client profile updated.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to update client details to the mind." + ex, "Mind error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Updating client failed.";
            }
        }

        private bool validateClientDetails(String value,TextBox tb,String error)
        {
            bool flag = false;
            if (value.Length == 0)
            {
                flag = true;
                MessageBox.Show(this, "Invalid "+error+" value.", error+" invalid", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                tb.Focus();
            }
            return flag;
        }

        private void txtClientName_TextChanged(object sender, EventArgs e)
        {
            btnClientUpdate.Enabled = true;
        }

        private void txtSiteName_TextChanged(object sender, EventArgs e)
        {
            btnSiteUpdate.Enabled = true;
        }

        private void btnSiteUpdate_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Updating site ...";
            String id, name, description;
            id = txtSiteID.Text.Trim().ToLower();
            name = txtSiteName.Text.Trim().ToLower();
            description = txtSiteDescription.Text.Trim().ToLower();
            

            bool flag = false;
            flag = validateClientDetails(id, txtSiteID, "site id") ? true : flag;
            flag = validateClientDetails(name, txtSiteName, "site name") ? true : flag;
            flag = validateClientDetails(description, txtSiteDescription, "site description") ? true : flag;
            if (flag)
            {
                lblStatus.Text = "Updating site failed.";
                return;
            }
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from sites where id!=@id and (name LIKE '%' + @name + '%')", conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtSiteName.Text = name;
                    MessageBox.Show(this, "You have entered duplicate entries in site name, you must provide unique site name entries for better site management.", "Duplicate entries", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    lblStatus.Text = "Updating site failed.";
                    return;
                }
                dr.Close();
                cmd = new SqlCommand("update sites set name=@name,description=@description where id=@id", conn);
                cmd.Parameters.Add("@name", SqlDbType.Text).Value = name;
                cmd.Parameters.Add("@description", SqlDbType.Text).Value = description;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.ExecuteNonQuery();
                conn.Close();
                btnSiteUpdate.Enabled = false;
                lblStatus.Text = "Site profile updated.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to update site details to the mind." + ex, "Mind error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Updating site failed.";
            }
        }

        private void lstMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstMachines.Enabled = false;
            if (lstMachines.SelectedItems.Count > 0)
            {
                txtProblems.Text = "";
                lblStatus.Text = "Searching problem ...";
                ListViewItem li = lstMachines.SelectedItems[0];
                Machine machine = new Machine(li.SubItems[1].Text.Trim(),Program.connString,ManageParameters.getAllParameters(PAR_CATEGORY.STATUS),ManageParameters.getAllParameters(PAR_CATEGORY.SETTINGS),UserSetting.StatusMessageFormat,UserSetting.SettingMessageFormat);
                if (machine.Error != MCError.NULL)
                {
                    if (machine.Error == MCError.INVALID_IDENTIFIER)
                    {
                        updateProblem("Machine ID invalid.");
                    }
                    else if (machine.Error == MCError.SQL_ERROR)
                    {
                        updateProblem("Mind error.");
                    }
                    else if (machine.Error == MCError.PARSE_FAILED)
                    {
                        updateProblem("Machine's entries invalid.");
                    }
                    else if (machine.Error == MCError.SETTING_ERROR)
                    {
                        if (machine.Settings.ErrorMessage == MCFormat_ERROR.NULL)
                        {
                            updateProblem("Machine setting is not updated yet or invalid settings parameters.");
                        }
                        else if (machine.Settings.ErrorMessage == MCFormat_ERROR.FORMAT_MISMATCH)
                        {
                            updateProblem("Machine setting message isn't matched with given format.");
                        }
                        else if (machine.Settings.ErrorMessage == MCFormat_ERROR.FORMAT_NOT_ACCEPTED)
                        {
                            updateProblem("Machine setting message has errors.");
                        }
                        else if (machine.Settings.ErrorMessage == MCFormat_ERROR.INVALID_FORMAT)
                        {
                            updateProblem("Machine setting message has invalid format.");
                        }
                        else if (machine.Settings.ErrorMessage == MCFormat_ERROR.PARAMETER_COUNT)
                        {
                            updateProblem("Machine setting message's received parameters count isn't matched with format.");
                        }
                        else if (machine.Settings.ErrorMessage == MCFormat_ERROR.SEPARATOR_NOT_FOUND)
                        {
                            updateProblem("Machine setting message hasn't any separator.");
                        }
                        else
                        {
                            updateProblem("Machine setting message has error.");
                        }
                    }
                    else if (machine.Error == MCError.STATUS_ERROR)
                    {
                        if (machine.Status.ErrorMessage == MCFormat_ERROR.NULL)
                        {
                            updateProblem("Machine status is not updated yet or invalid status parameters.");
                        }
                        else if (machine.Status.ErrorMessage == MCFormat_ERROR.FORMAT_NOT_ACCEPTED)
                        {
                            updateProblem("Machine status message's has errors.");
                        }
                        else if (machine.Status.ErrorMessage == MCFormat_ERROR.FORMAT_MISMATCH)
                        {
                            updateProblem("Machine status message's format isn't matched with given format.");
                        }
                        else if (machine.Status.ErrorMessage == MCFormat_ERROR.INVALID_FORMAT)
                        {
                            updateProblem("Machine status message has invalid format.");
                        }
                        else if (machine.Status.ErrorMessage == MCFormat_ERROR.PARAMETER_COUNT)
                        {
                            updateProblem("Machine status message's received parameters count not macthed with given format.");
                        }
                        else if (machine.Status.ErrorMessage == MCFormat_ERROR.SEPARATOR_NOT_FOUND)
                        {
                            updateProblem("Machine status message's separator not found.");
                        }
                        else
                        {
                            updateProblem("Machine status message's has error.");
                        }
                        
                    }
                    else
                    {
                        updateProblem("Unknown problem id=1.");
                    }
                }

                if(true)
                {
                    if (machine.Status.Error)
                    {
                        updateProblem("Invalid received machine status parameters.");
                    }
                    else
                    {
                        List<MachineVariables> mparams = ParameterVariables.getMachineVariables();
                        if (mparams != null)
                        {
                            MachineVariables macParam=Program.ParseHealthyCommand(machine.HealthySetting);
                            List<Parameter> chkValues = machine.Status.parameters;

                            foreach (Variables var in macParam.variables)
                            {
                                Parameter par = chkValues.Find(x=>(x.Order==var.Order));
                                if (par != null)
                                {
                                    int result = checkParameter(par,var);
                                    //0 stoped, 1 unhealthy, 2 green, 3 unknown. -1 error
                                    if (result == 0)
                                    {
                                        updateProblem(par.Name + " indicates NOT_WORKING SIGNAL.");
                                    }
                                    else if (result == 1)
                                    {
                                        updateProblem(par.Name + " indicates UNHEALTHY SIGNAL.");
                                    }
                                    else if (result == 2)
                                    {
                                        updateProblem(par.Name + " indicates HEALTHY SIGNAL.");
                                    }
                                    else if (result == 3)
                                    {
                                        updateProblem(par.Name + " indicates NOT_MATCHED SIGNAL.");
                                    }
                                    else if (result == 1)
                                    {
                                        updateProblem(par.Name + " indicates ERROR SIGNAL.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            updateProblem("Error in reading parameters from mind.");
                        }
                    }
                }
            }
            lblStatus.Text = "Problem displayed.";
            lstMachines.Enabled = true;
        }

        private int checkParameter(Parameter par,Variables var)
        {
            try
            {
                Decimal actualValue = Decimal.Parse(par.Value.Trim());
                Decimal hf = Decimal.Parse(var.Health_From.Trim());
                Decimal ht = Decimal.Parse(var.Health_To.Trim());
                Decimal uh = Decimal.Parse(var.Unhealthy.Trim());
                Decimal st = Decimal.Parse(var.Stoped.Trim());

                if (var.StopedCondition == 1) if (actualValue >= st) return 0;
                if (var.StopedCondition == 0) if (actualValue == st) return 0;
                if (var.StopedCondition == -1) if (actualValue <= st) return 0;

                if (var.UnhealthyCondition == 1) if (actualValue >= uh) return 1;
                if (var.UnhealthyCondition == 0) if (actualValue == uh) return 1;
                if (var.UnhealthyCondition == -1) if (actualValue <= uh) return 1;

                if (actualValue >= hf && actualValue <= ht) return 2;

                return 3;
            }
            catch (Exception)
            {
                updateProblem("Error in parsing the machine parameter : " + par.Name);
            }
            return -1;
        }

        private void updateProblem(String msg)
        {
            txtProblems.Text += "" + msg + "" + Environment.NewLine+Environment.NewLine;
        }
    }
}
