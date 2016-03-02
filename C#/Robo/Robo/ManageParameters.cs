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
    public partial class ManageParameters : Form
    {
        public ManageParameters()
        {
            InitializeComponent();
        }

        private void btnAddStatusParameter_Click(object sender, EventArgs e)
        {
            String parname = txtParName.Text.Trim();
            String format = txtParFormat.Text.Trim();
            String orderno = txtOrder.Text.Trim();
            if (parname.Length == 0)
            {
                MessageBox.Show(this, "Invalid parameter name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtParName.Focus();
                return;
            }
            if (format.Length == 0)
            {
                MessageBox.Show(this, "Invalid parameter format.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtParFormat.Focus();
                return;
            }
            if (orderno.Length == 0)
            {
                MessageBox.Show(this, "Invalid order no.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOrder.Focus();
                return;
            }
            else
            {
                try
                {
                    Decimal tmp = Decimal.Parse(orderno, System.Globalization.NumberStyles.Integer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Invalid order no.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtOrder.Focus();
                    return;
                }
            }
            if (txtParFormat.SelectedIndex > -1)
            {
                format = (txtParFormat.SelectedItem as ComboBoxItem).Value;
            }
            bool newFlag = true;
            Decimal max = 0,order=-1;
            foreach (ListViewItem lia in lstStatus.Items)
            {
                try
                {
                    order = Decimal.Parse(orderno.ToString(),System.Globalization.NumberStyles.Integer);
                    Decimal tempMax = Decimal.Parse(lia.SubItems[0].Text.Trim(),System.Globalization.NumberStyles.Integer);
                    if (tempMax > max)
                        max = tempMax;
                    if (lia.SubItems[1].Text.ToLower().Trim().Equals(parname.ToLower().Trim()))
                        newFlag = false;
                    if (lia.SubItems[0].Text.ToLower().Trim().Equals(orderno.ToLower().Trim()))
                        newFlag = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,"Invalid order number. order number should be integer or some digit value not a string or floating value.","Order number",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
            }

            if (!newFlag)
            {
                MessageBox.Show(this, "This parameter name or order name is already in list, if you want to update that then remove it from list and add again same one.", "Duplicate exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (order <= max + 1)
                {
                    Decimal od=0;
                    try {od=Decimal.Parse(orderno,System.Globalization.NumberStyles.Integer);} catch(Exception) {}
                    if (order == -1 && od!=1 && od==0)
                    {
                        MessageBox.Show(this,"Order number should be starts with 1.","Order number",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    else
                    {
                        ListViewItem li = new ListViewItem(new String[] { orderno, parname, format, chkHealth.Checked ? "Yes" : "No" });
                        lstStatus.Items.Add(li);
                        txtParName.Text = txtParFormat.Text = txtOrder.Text = "";
                        chkHealth.Checked = false;
                        txtParName.Focus();
                    }
                }
                else
                {
                    MessageBox.Show(this,"You should enter order numbers in sequance, but here you missed one order no.","Order number sequance",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    txtOrder.Focus();
                }
            }
        }

        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ListView lv = (ListView)sender;
                if (lv.SelectedItems.Count > 0)
                {
                    lv.SelectedItems[0].Remove();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,"Unable to delete item from list, try again.","Delete item",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnCancelChanges_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddSettingParameter_Click(object sender, EventArgs e)
        {
            String parname = txtParName1.Text.Trim();
            String format = txtParFormat1.Text.Trim();
            String orderno = txtOrder1.Text.Trim();
            if (parname.Length == 0)
            {
                MessageBox.Show(this, "Invalid parameter name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtParName1.Focus();
                return;
            }
            if (format.Length == 0)
            {
                MessageBox.Show(this, "Invalid parameter format.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtParFormat1.Focus();
                return;
            }
            if (orderno.Length == 0)
            {
                MessageBox.Show(this, "Invalid order no.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOrder1.Focus();
                return;
            }
            else
            {
                try
                {
                    Decimal tmp = Decimal.Parse(orderno, System.Globalization.NumberStyles.Integer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Invalid order no.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtOrder.Focus();
                    return;
                }
            }
            if (txtParFormat1.SelectedIndex > -1)
            {
                format = (txtParFormat1.SelectedItem as ComboBoxItem).Value;
            }
            bool newFlag = true;
            Decimal max = 0, order = -1;
            foreach (ListViewItem lia in lstSettings.Items)
            {
                try
                {
                    order = Decimal.Parse(orderno.ToString(), System.Globalization.NumberStyles.Integer);
                    Decimal tempMax = Decimal.Parse(lia.SubItems[0].Text.Trim(), System.Globalization.NumberStyles.Integer);
                    if (tempMax > max)
                        max = tempMax;
                    if (lia.SubItems[1].Text.ToLower().Trim().Equals(parname.ToLower().Trim()))
                        newFlag = false;
                    if (lia.SubItems[0].Text.ToLower().Trim().Equals(orderno.ToLower().Trim()))
                        newFlag = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Invalid order number. order number should be integer or some digit value not a string or floating value.", "Order number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!newFlag)
            {
                MessageBox.Show(this, "This parameter name or order name is already in list, if you want to update that then remove it from list and add again same one.", "Duplicate exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtParName1.Focus();
            }
            else
            {
                if (order <= max + 1)
                {
                    Decimal od=0;
                    try {od=Decimal.Parse(orderno,System.Globalization.NumberStyles.Integer);} catch(Exception) {}
                    if (order == -1 && od != 1)
                    {
                        MessageBox.Show(this, "Order number should be starts with 1.", "Order number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        ListViewItem li = new ListViewItem(new String[] { orderno, parname, format });
                        lstSettings.Items.Add(li);
                        txtParName1.Text = txtParFormat1.Text = txtOrder1.Text = "";
                        txtParName1.Focus();
                    }
                }
                else
                {
                    MessageBox.Show(this, "You should enter order numbers in sequance, but here you missed one order no.", "Order number sequance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtOrder1.Focus();
                }
            }
        }

        private void ManageParameters_Load(object sender, EventArgs e)
        {
            try
            {
                ComboBoxItem[] citems=new ComboBoxItem[9];
                citems[0] = new ComboBoxItem("On(1)/Off(0) Format", "{1:On,0:Off}");
                citems[1] = new ComboBoxItem("On(0)/Off(1) Format", "{0:On,1:Off}");
                citems[2] = new ComboBoxItem("Date Format (00/00/00)", "00/00/00");
                citems[3] = new ComboBoxItem("Date Format (00/00/0000)", "00/00/0000");
                citems[4] = new ComboBoxItem("Full time (00:00:00)", "00:00:00");
                citems[5] = new ComboBoxItem("Hour+Minute Format (00:00)", "00:00");
                citems[6] = new ComboBoxItem("Hour Format (HH)", "00");
                citems[7] = new ComboBoxItem("Number Format 1 (0.00)", "0.00");
                citems[8] = new ComboBoxItem("Number Format 2 (00.0)", "00.0");
                txtParFormat.Items.AddRange(citems);
                txtParFormat1.Items.AddRange(citems);

                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd=new SqlCommand("select * from parameters",conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    String name, orderno, cfh, format, category;
                    orderno = dr[1].ToString();
                    name = dr[2].ToString();
                    format = dr[3].ToString();
                    cfh = dr[4].ToString();
                    category = dr[5].ToString();
                    if (category.Equals("status"))
                    {
                        lstStatus.Items.Add(new ListViewItem(new String[] { orderno, name, format, cfh }));
                    }
                    else
                    {
                        lstSettings.Items.Add(new ListViewItem(new String[] { orderno, name, format }));
                    }
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,"Unable to read from mind.","Mind error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnSaveClose_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd;
                bool flag = true;
                try
                {
                    cmd = new SqlCommand("delete from parameters", conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,"Unable to refresh mind. Try again.","Mind error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    flag = false;
                }
                if (flag)
                {
                    cmd = new SqlCommand("insert into parameters(orderno,name,format,checkforhealthy,category) values(@order,@name,@format,@cfh,@category)", conn);
                    cmd.Parameters.Add("@order", SqlDbType.Text);
                    cmd.Parameters.Add("@name", SqlDbType.Text);
                    cmd.Parameters.Add("@format", SqlDbType.Text);
                    cmd.Parameters.Add("@cfh", SqlDbType.Text);
                    cmd.Parameters.Add("@category", SqlDbType.Text);
                    foreach (ListViewItem li in lstStatus.Items)
                    {
                        cmd.Parameters["@order"].Value = li.SubItems[0].Text;
                        cmd.Parameters["@name"].Value = li.SubItems[1].Text;
                        cmd.Parameters["@format"].Value = li.SubItems[2].Text;
                        cmd.Parameters["@cfh"].Value = li.SubItems[3].Text;
                        cmd.Parameters["@category"].Value = "status";
                        cmd.ExecuteNonQuery();
                    }
                    foreach (ListViewItem li in lstSettings.Items)
                    {
                        cmd.Parameters["@order"].Value = li.SubItems[0].Text;
                        cmd.Parameters["@name"].Value = li.SubItems[1].Text;
                        cmd.Parameters["@format"].Value = li.SubItems[2].Text;
                        cmd.Parameters["@cfh"].Value = "";
                        cmd.Parameters["@category"].Value = "settings";
                        cmd.ExecuteNonQuery();
                    }
                }
                conn.Close();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,"Unable to write into mind. Try again.","Mind error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public static List<Parameter> getAllParameters(PAR_CATEGORY parc)
        {
            List<Parameter> final = new List<Parameter>();
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from parameters where category LIKE '"+parc.ToString().ToLower()+"'",conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Parameter parm = new Parameter(dr["name"].ToString(), dr["format"].ToString(), dr["orderno"].ToString(), dr["category"].ToString(), dr["checkforhealthy"].ToString().Equals("Yes") ? true : false);
                    final.Add(parm);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {}
            return final;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String message = "Different types of display format : "+Environment.NewLine+Environment.NewLine;
            message += "(1.) If you write {1:ABC,2:DEF} in display format, then at display time if that parameter's value is 1 then it will show ABC, and same as if its 2 then shows DEF." + Environment.NewLine+Environment.NewLine;
            message += "(2.) If you write 00.0 or 00:00 or 0/0/0 and suppose parameter's value is 1234 comes then it will shows respectively 12.3, 12:34, 1/2/3." + Environment.NewLine + Environment.NewLine;
            message += "For more info contact my(Robo) developer."+Environment.NewLine+Environment.NewLine;
            message += "Thank you.";
            MessageBox.Show(this,message,"Display format helper",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }

    public enum PAR_CATEGORY { STATUS, SETTINGS }
}
