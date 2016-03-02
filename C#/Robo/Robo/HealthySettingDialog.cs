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
    public partial class HealthySettingDialog : Form
    {
        public String itemDescription="",Order="";
        public String HF="", HT="", U="", S="";
        public int UC=1, SC=1;
        public HealthySettingDialog()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HealthySettingDialog_Load(object sender, EventArgs e)
        {
            List<MachineControl.Parameter> lists = ManageParameters.getAllParameters(PAR_CATEGORY.STATUS);
            MachineControl.Parameter par=lists.Find(x=>(x.Order==Order));
            if (par != null)
            {
                txtHF.Mask = txtHT.Mask = txtU.Mask = txtS.Mask = par.Format;
            }
            else
            {
                MessageBox.Show(this,"Unknown parameter selected.","Parameter not found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }
            lblDesc.Text = itemDescription;
            //MessageBox.Show(HF+Environment.NewLine+HT+Environment.NewLine+U+Environment.NewLine+S);
            txtHF.Text = HF.Trim();
            txtHT.Text = HT.Trim();
            txtS.Text = S.Trim();
            txtU.Text = U.Trim();
            if (UC > 0) radUNH1.Checked = true;
            if (UC < 0) radUNH2.Checked = true;
            if (UC == 0) radUNH3.Checked = true;
            if (SC > 0) radS1.Checked = true;
            if (SC < 0) radS2.Checked = true;
            if (SC == 0) radS3.Checked = true;


        }
        private String UT(String text)
        {
            return text.Replace(".","").Replace(":","").Replace("/","").Trim();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int _hf = Int32.Parse(UT(txtHF.Text));
                int _ht = Int32.Parse(UT(txtHT.Text));
                int _U = Int32.Parse(UT(txtU.Text));
                int _S = Int32.Parse(UT(txtS.Text));
                if(_ht<_hf) {
                    MessageBox.Show(this, "Healthy TO value cant be smaller than FROM value.", "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Input values are invalid number please try again.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            HF = UT(txtHF.Text);
            HT = UT(txtHT.Text);
            U = UT(txtU.Text);
            S = UT(txtS.Text);
            if (radS1.Checked)
                SC = 1;
            if (radS2.Checked)
                SC = -1;
            if (radS3.Checked)
                SC = 0;

            if (radUNH1.Checked)
                UC = 1;
            if (radUNH2.Checked)
                UC = -1;
            if (radUNH3.Checked)
                UC = 0;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
