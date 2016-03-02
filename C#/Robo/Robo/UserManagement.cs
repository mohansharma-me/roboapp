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
    public partial class UserManagement : Form
    {
        private String[] rightArray = new String[] {"Add Clients","View Clients","Add Sites","View Sites","Add Machines","View Machines","Machine Operation","Machine Settings","Manage Users","Email Reports","View Messages"};
        public UserManagement()
        {
            InitializeComponent();
        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            txtUsername.Text = Program.curUser;
            if (!Program.validateRights(UserRights.ManageUsers, Program.curUserRights))
            {
                groupBox1.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
                __fillUsers();
            }
        }

        private String getRight(String rightString)
        {
            String output = "";
            if (!rightString.Contains("0"))
                return "All Rights";
            if (!rightString.Contains("1"))
                return "No Rights";

            for (int i = 0; i < rightString.Length; i++)
            {
                if (Program.validateRights((UserRights)i, rightString))
                    output += ((UserRights)i).ToString() + ", ";
            }
            return output;
        }

        private void __fillUsers()
        {
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select username,rights from users", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                lvUsers.Items.Clear();
                while (dr.Read())
                {
                    ListViewItem li = new ListViewItem(new String[] { dr[0].ToString(), getRight(dr[1].ToString()) });
                    lvUsers.Items.Add(li);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error in loading user details from robo mind. please try again.", "MIND Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            try
            {
                int flag = 0;
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from users where username LIKE '%' + @user + '%' and password LIKE '%' + @pass + '%'",conn);
                cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                cmd.Parameters.AddWithValue("@pass", txtOP.Text);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    flag = 1;
                }
                dr.Close();
                if (flag == 1)
                {
                    cmd = new SqlCommand("update users set password=@pass where username LIKE '%' + @user + '%'",conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@pass", txtNP.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(this,"Password successfully updated.","Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    txtOP.Text = "";
                    txtNP.Text = "";
                }
                else
                {
                    MessageBox.Show(this,"Old password doesn't match current logged username. try again!","Login failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(""+ex);
            }
        }

        private void btnNewUser_Click(object sender, EventArgs e)
        {
            NewUser nu = new NewUser();
            nu.rights = rightArray;
            nu.ShowDialog(this);
            __fillUsers();
        }

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count > 0)
                btnDeleteUser.Enabled = true;
            else
                btnDeleteUser.Enabled = false;
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            ListViewItem li = lvUsers.SelectedItems[0];
            if (li.SubItems[1].Text.Equals("All Rights"))
            {
                int count = 0;
                foreach (ListViewItem ll in lvUsers.Items)
                {
                    if (ll.SubItems[1].Text.Equals("All Rights"))
                        count++;
                }
                if (count <= 1)
                {
                    MessageBox.Show(this,"You can't delete this user because this user only has All Rights to manage robo but if you want to delete this user then you need to first add one with all rights and then you can delete this user.","All rights not found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
            }
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from users where username LIKE '%' + @user + '%'",conn);
                cmd.Parameters.AddWithValue("@user",li.SubItems[0].Text.Trim());
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show(this,"Selected user removed from robo's mind.","Deleted",MessageBoxButtons.OK,MessageBoxIcon.Information);
                __fillUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Robo cant delete this user from MIND, please try again.", "MIND Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
