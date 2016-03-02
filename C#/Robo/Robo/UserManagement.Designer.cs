namespace Robo
{
    partial class UserManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabChangePassword = new System.Windows.Forms.TabPage();
            this.btnChangePass = new System.Windows.Forms.Button();
            this.txtNP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabAdmin = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.lvUsers = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnNewUser = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabChangePassword.SuspendLayout();
            this.tabAdmin.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabChangePassword);
            this.tabControl1.Controls.Add(this.tabAdmin);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(353, 296);
            this.tabControl1.TabIndex = 0;
            // 
            // tabChangePassword
            // 
            this.tabChangePassword.Controls.Add(this.btnChangePass);
            this.tabChangePassword.Controls.Add(this.txtNP);
            this.tabChangePassword.Controls.Add(this.label3);
            this.tabChangePassword.Controls.Add(this.txtOP);
            this.tabChangePassword.Controls.Add(this.label2);
            this.tabChangePassword.Controls.Add(this.txtUsername);
            this.tabChangePassword.Controls.Add(this.label1);
            this.tabChangePassword.Location = new System.Drawing.Point(4, 24);
            this.tabChangePassword.Name = "tabChangePassword";
            this.tabChangePassword.Padding = new System.Windows.Forms.Padding(3);
            this.tabChangePassword.Size = new System.Drawing.Size(345, 268);
            this.tabChangePassword.TabIndex = 0;
            this.tabChangePassword.Text = "Change Password";
            this.tabChangePassword.UseVisualStyleBackColor = true;
            // 
            // btnChangePass
            // 
            this.btnChangePass.Location = new System.Drawing.Point(121, 143);
            this.btnChangePass.Name = "btnChangePass";
            this.btnChangePass.Size = new System.Drawing.Size(119, 23);
            this.btnChangePass.TabIndex = 6;
            this.btnChangePass.Text = "&Change now";
            this.btnChangePass.UseVisualStyleBackColor = true;
            this.btnChangePass.Click += new System.EventHandler(this.btnChangePass_Click);
            // 
            // txtNP
            // 
            this.txtNP.Location = new System.Drawing.Point(101, 92);
            this.txtNP.Name = "txtNP";
            this.txtNP.PasswordChar = '*';
            this.txtNP.Size = new System.Drawing.Size(212, 23);
            this.txtNP.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "New Pass :";
            // 
            // txtOP
            // 
            this.txtOP.Location = new System.Drawing.Point(101, 63);
            this.txtOP.Name = "txtOP";
            this.txtOP.PasswordChar = '*';
            this.txtOP.Size = new System.Drawing.Size(212, 23);
            this.txtOP.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Old Pass :";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(101, 34);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.ReadOnly = true;
            this.txtUsername.Size = new System.Drawing.Size(212, 23);
            this.txtUsername.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username :";
            // 
            // tabAdmin
            // 
            this.tabAdmin.Controls.Add(this.groupBox1);
            this.tabAdmin.Location = new System.Drawing.Point(4, 24);
            this.tabAdmin.Name = "tabAdmin";
            this.tabAdmin.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdmin.Size = new System.Drawing.Size(345, 268);
            this.tabAdmin.TabIndex = 1;
            this.tabAdmin.Text = "Manage users";
            this.tabAdmin.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDeleteUser);
            this.groupBox1.Controls.Add(this.lvUsers);
            this.groupBox1.Controls.Add(this.btnNewUser);
            this.groupBox1.Location = new System.Drawing.Point(-4, -4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 276);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.Enabled = false;
            this.btnDeleteUser.Location = new System.Drawing.Point(251, 20);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(91, 23);
            this.btnDeleteUser.TabIndex = 10;
            this.btnDeleteUser.Text = "&Delete user";
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            // 
            // lvUsers
            // 
            this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvUsers.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.GridLines = true;
            this.lvUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvUsers.Location = new System.Drawing.Point(9, 49);
            this.lvUsers.MultiSelect = false;
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.ShowGroups = false;
            this.lvUsers.Size = new System.Drawing.Size(333, 217);
            this.lvUsers.TabIndex = 8;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.Details;
            this.lvUsers.SelectedIndexChanged += new System.EventHandler(this.lvUsers_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Username";
            this.columnHeader1.Width = 102;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Rights";
            this.columnHeader2.Width = 900;
            // 
            // btnNewUser
            // 
            this.btnNewUser.Location = new System.Drawing.Point(10, 20);
            this.btnNewUser.Name = "btnNewUser";
            this.btnNewUser.Size = new System.Drawing.Size(81, 23);
            this.btnNewUser.TabIndex = 9;
            this.btnNewUser.Text = "&New user";
            this.btnNewUser.UseVisualStyleBackColor = true;
            this.btnNewUser.Click += new System.EventHandler(this.btnNewUser_Click);
            // 
            // UserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 321);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserManagement";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Settings";
            this.Load += new System.EventHandler(this.UserManagement_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabChangePassword.ResumeLayout(false);
            this.tabChangePassword.PerformLayout();
            this.tabAdmin.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabChangePassword;
        private System.Windows.Forms.TabPage tabAdmin;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChangePass;
        private System.Windows.Forms.ListView lvUsers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnNewUser;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}