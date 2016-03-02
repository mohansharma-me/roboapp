using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Robo
{
    public partial class NewUser : Form
    {
        public String[] rights=null;
        public NewUser()
        {
            InitializeComponent();
        }

        private void NewUser_Load(object sender, EventArgs e)
        {
            if (rights == null)
            {
                MessageBox.Show(this,"Rights list isn't initilized.","Rights Missing",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Close();
            }
            lvRights.Items.Clear();
            for (int i = 0; i < rights.Length; i++)
            {
                ListViewItem li = new ListViewItem(new String[] { "", rights[i] });
                lvRights.Items.Add(li);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim().Length == 0)
            {
                MessageBox.Show(this,"Please enter username to create new user.","No username",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtUsername.Focus();
                return;
            }
            if (txtPass1.Text.Trim().Length == 0 || txtPass2.Text.Trim().Length == 0)
            {
                MessageBox.Show(this, "Please enter password to create new user.", "No password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass1.Focus();
                return;
            }
            if (!txtPass1.Text.Equals(txtPass2.Text))
            {
                MessageBox.Show(this, "Your entered password is mismatched, you must enter both password to confirm you are writing perfect password.", "Re-password not matched", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass1.Focus();
                return;
            }
            if (lvRights.CheckedItems.Count == 0)
            {
                MessageBox.Show(this, "You didn't selected any right to this user, you must atleast select one right for this user.", "No rights", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lvRights.Focus();
                return;
            }
            String right="";
            for (int i = 0; i < lvRights.Items.Count; i++)
            {
                String app = lvRights.Items[i].Checked ? "1" : "0";
                right += app;
            }
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into users(username,password,rights) values(@u,@p,@r)",conn);
                cmd.Parameters.AddWithValue("@u",txtUsername.Text);
                cmd.Parameters.AddWithValue("@p",txtPass1.Text);
                cmd.Parameters.AddWithValue("@r",right);
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show(this,"New user added successfully.","Added",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to add new user into robo's mind, please try again.", "MIND Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtUsername.Text = txtPass1.Text = txtPass2.Text = "";
            foreach (ListViewItem li in lvRights.Items)
                li.Checked = false;
            txtUsername.Focus();
        }
    }
}
