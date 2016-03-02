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
    public partial class ParameterVariables : Form
    {
        private int previousSelected = -1;
        private List<MachineVariables> machines=new List<MachineVariables>();

        public ParameterVariables()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = -1;
            if (!groupBox1.Enabled)
            {
                Close();
            }
            foreach (MachineVariables parMac in machines)
            {
                String output = "";
                foreach (Variables param in parMac.variables)
                {
                    output += "[";
                    output += param.Name + "," + param.Health_From + "," + param.Health_To + "," + param.Unhealthy + "," + param.Stoped;
                    output += "]";
                }
                try
                {
                    SqlConnection conn = new SqlConnection(Program.connString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete from machine_settings where type LIKE '"+parMac.Type+"'",conn);
                    cmd.ExecuteNonQuery();
                    cmd=new SqlCommand("insert into machine_settings(type,parameter) values(@type,@parameter)",conn);
                    cmd.Parameters.Add("@type",SqlDbType.Text).Value=parMac.Type;
                    cmd.Parameters.Add("@parameter",SqlDbType.Text).Value=output;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,"Unable to write in mind.","Mind Error",MessageBoxButtons.OK,MessageBoxIcon.Hand);
                }
            }
        }

        public static List<MachineVariables> getMachineVariables()
        {
            List<MachineVariables> _machines = new List<MachineVariables>();
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from machine_settings", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    String type = dr["type"].ToString();
                    String param = dr["parameter"].ToString();
                    MachineVariables mv = new MachineVariables(type, new List<Variables>());
                    mv.inputString = param;
                    _machines.Add(mv);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                return null;
            }

            try
            {
                foreach (MachineVariables machine in _machines)
                {
                    String[] arrTemp = machine.inputString.Split(new String[] { "][" }, StringSplitOptions.None);
                    for (int i = 0; i < arrTemp.Length; i++)
                    {
                        arrTemp[i] = arrTemp[i].Replace("[", "").Replace("]", "");
                    }
                    List<Parameter> lstpars = ManageParameters.getAllParameters(PAR_CATEGORY.STATUS);
                    foreach (Parameter parm in lstpars)
                    {
                        if (parm.CheckForHealthy)
                        {
                            String hf="", ht="", uh="", st="";
                            foreach (String pfdb in arrTemp)
                            {
                                if (pfdb.ToLower().Contains(parm.Name.ToLower()))
                                {
                                    String[] arr = pfdb.Split(new String[] { "," }, StringSplitOptions.None);
                                    if (arr.Length == 5)
                                    {
                                        hf = arr[1];
                                        ht = arr[2];
                                        uh = arr[3];
                                        st = arr[4];
                                    }
                                    break;
                                }
                            }
                            machine.variables.Add(new Variables(parm.Name, hf, ht, uh, st));
                        }
                    }
                }
            }
            catch (Exception ex) { return null; }

            return _machines;
        }

        private void MachineSettings_Load(object sender, EventArgs e)
        {
            
            listBox1.Items.Clear();
            List<Parameter> _params = ManageParameters.getAllParameters(PAR_CATEGORY.STATUS);
            foreach (Parameter param in _params)
            {
                if(param.CheckForHealthy)
                listBox1.Items.Add(param);
            }
            machines = getMachineVariables();
            foreach (MachineVariables mv in machines)
            {
                comboBox1.Items.Add(mv.Type);
            }
            if (machines == null || machines.Count==0)
            {
                MessageBox.Show(this,"Robo cant read machines parameters from mind.","Mind error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            comboBox1.Focus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                previousSelected = -1;
                listBox1.SelectedIndex = -1;
                txt1.Text = txt2.Text = txt3.Text = txt4.Text = "";
                groupBox1.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String oldText="", newText="";
            MachineVariables mac = null;
            foreach (MachineVariables machine in machines)
            {
                String _T = comboBox1.Items[comboBox1.SelectedIndex].ToString();
                if (machine.Type.ToLower() == _T.ToLower())
                {
                    mac = machine;
                    break;
                }
            }
            if (mac == null) return;
            if (previousSelected > -1)
            {
                oldText = listBox1.Items[previousSelected].ToString();
                foreach (Variables param in mac.variables)
                {
                    if (param.Name.Equals(oldText))
                    {
                        if (txt1.Text.Trim().Length > 0 && txt2.Text.Trim().Length > 0 && txt3.Text.Trim().Length > 0 && txt4.Text.Trim().Length > 0)
                        {
                            param.Health_From = txt1.Text.Trim().Replace(".","").Replace("/","").Replace(":","");
                            param.Health_To = txt2.Text.Trim().Replace(".", "").Replace("/", "").Replace(":", "");
                            param.Unhealthy = txt3.Text.Trim().Replace(".", "").Replace("/", "").Replace(":", "");
                            param.Stoped = txt4.Text.Trim().Replace(".", "").Replace("/", "").Replace(":", "");
                        }
                    }
                }
            }
            if (listBox1.SelectedIndex > -1)
            {
                previousSelected = listBox1.SelectedIndex;
                Parameter selParam = (Parameter)listBox1.SelectedItem;
                newText = selParam.ToString();
                if (mac != null)
                {
                    foreach (Variables param in mac.variables)
                    {
                        if (param.Name.Equals(newText))
                        {
                            String fmt = selParam.Format;
                            if (selParam.Format.Contains("{"))
                            {
                                fmt = "";
                            }
                            txt1.Mask = txt2.Mask = txt3.Mask = txt4.Mask = fmt;
                            txt1.Text = param.Health_From;
                            txt2.Text = param.Health_To;
                            txt3.Text = param.Unhealthy;
                            txt4.Text = param.Stoped;
                            txt1.Focus();
                        }
                    }
                }
            }
        }
    }

    public class MachineVariables
    {
        public MachineVariables(String _type,List<Variables> _list)
        {
            Type = _type;
            variables = _list;
        }
        public string inputString { get; set; }
        public string Type { get; set; }
        public List<Variables> variables { get; set; }
    }
    public class Variables {
        public Variables()
        {
            Name = Health_From = Health_To = Unhealthy = Stoped = Order = "";
            StopedCondition = UnhealthyCondition = 1;
        }
        public Variables(String _name,String _hfrom,String _hto,String _unh,String _stopped)
        {
            Name = _name;
            Health_From = _hfrom;
            Health_To = _hto;
            Unhealthy = _unh;
            Stoped = _stopped;
            Order = "";
            StopedCondition = UnhealthyCondition = 1;
        }
        public string Name { get; set; }
        public string Health_From { get; set; }
        public string Health_To { get; set; }
        public string Unhealthy { get; set; }
        public string Stoped { get; set; }
        public string Order { get; set; }
        public int UnhealthyCondition { get; set; }
        public int StopedCondition { get; set; }
    }

    
}
