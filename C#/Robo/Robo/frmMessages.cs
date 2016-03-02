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
    public partial class frmMessages : Form
    {
        public frmMessages()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu cm = new ContextMenu();
                cm.MenuItems.Add(new MenuItem("Show message", RightClick));
                cm.MenuItems.Add(new MenuItem("Delete message", RightClick));
                cm.Show(listView1,e.Location);
            }
        }

        void RightClick(object sender, EventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)sender;
                if (mi.Text.Equals("Show message") && listView1.SelectedItems.Count>0)
                {
                    MessageBox.Show(this,"Message: "+Environment.NewLine+Environment.NewLine+listView1.SelectedItems[0].SubItems[3].Text,"Sender: "+listView1.SelectedItems[0].SubItems[2].Text,MessageBoxButtons.OK,MessageBoxIcon.Information);
                } 
                else if (mi.Text.Equals("Delete message") && listView1.SelectedItems.Count>0) 
                {
                    if(listView1.SelectedItems.Count>0)
                    try
                    {
                        SqlConnection conn = new SqlConnection(Program.connString);
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("delete from messages where id=@id",conn);
                        cmd.Parameters.Add("@id",SqlDbType.Int).Value=listView1.SelectedItems[0].SubItems[0].Text;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        listView1.SelectedItems[0].Remove();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this,"Cannot delete this message from mind.","Mind error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void frmMessages_Load(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from messages",conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem li = new ListViewItem(new String[] {dr[0].ToString(),dr[5].ToString(),dr[3].ToString(),dr[6].ToString()});
                    listView1.Items.Add(li);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,"Cannot read messages from mind.","Mind Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                SqlConnection conn = new SqlConnection(Program.connString);
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from messages", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem li = new ListViewItem(new String[] { dr[0].ToString(), dr[5].ToString(), dr[3].ToString(), dr[6].ToString() });
                    listView1.Items.Add(li);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Cannot read messages from mind.", "Mind Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
