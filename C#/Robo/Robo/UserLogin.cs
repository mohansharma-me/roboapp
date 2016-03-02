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
    public partial class UserLogin : Form
    {
        public Boolean isLogged = false;
        public String username = "", rights = "";
        public UserLogin()
        {
            InitializeComponent();
        }

        private void UserLogin_Load(object sender, EventArgs e)
        {
            isLogged = false;
            txtUsername.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select username,password,rights from users where username LIKE '%' + @user + '%' and password LIKE '%' + @pass + '%'",conn);
                cmd.Parameters.AddWithValue("@user",txtUsername.Text);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr[0].ToString().Equals(txtUsername.Text) && dr[1].ToString().Equals(txtPassword.Text))
                    {
                        username = txtUsername.Text;
                        rights = dr["rights"].ToString();
                        DialogResult = DialogResult.OK;
                        isLogged = true;
                    }
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something gone wrong with MIND Library, please try again after restarting robo.", "MIND Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (isLogged)
                Close();
            else
                MessageBox.Show(this,"Invalid login details, username or password mismatched.","Login failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }
}
