using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Robo
{
    public partial class GetSettingValue : Form
    {
        public String itemDescription = "";
        public Boolean bFlag = false;
        public String format = "";
        public String value = "";

        public GetSettingValue()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void GetSettingValue_Load(object sender, EventArgs e)
        {
            itemDesc.Text = itemDescription;
            if (format.Contains("{"))
            {
                txtVal.Visible = false;
                cmb.Visible = true;
                String temp = format.Replace("{","").Replace("}","");
                String[] array = temp.Split(new String[] {","},StringSplitOptions.None);
                foreach (String arr1 in array)
                {
                    String[] subarray = arr1.Split(new String[] {":"},StringSplitOptions.None);
                    if (subarray.Length == 2)
                    {
                        ComboBoxItem cmi = new ComboBoxItem(subarray[1], subarray[0]);                        
                        cmb.Items.Add(cmi);
                    }
                }
            }
            else
            {
                cmb.Visible = false;
                txtVal.Visible = true;
                txtVal.Mask = format;
                txtVal.Text = value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtVal.Visible)
                value = txtVal.Text.Replace(":", "").Replace(".", "").Replace("/", "");
            if (cmb.Visible)
            {
                if (cmb.SelectedIndex > -1)
                {
                    value = (cmb.SelectedItem as ComboBoxItem).Value;
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
