namespace Robo
{
    partial class ShowDetails
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
            this.btnClose = new System.Windows.Forms.Button();
            this.gClients = new System.Windows.Forms.GroupBox();
            this.btnClientUpdate = new System.Windows.Forms.Button();
            this.txtClientNickname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtClientEmail = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtClientContactInfo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtClientContactPerson = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtClientAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtClientName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtClientID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gSites = new System.Windows.Forms.GroupBox();
            this.btnSiteUpdate = new System.Windows.Forms.Button();
            this.txtSiteDescription = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSiteName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSiteID = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.gMachines = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtProblems = new System.Windows.Forms.TextBox();
            this.lstMachines = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblStatus = new System.Windows.Forms.Label();
            this.gClients.SuspendLayout();
            this.gSites.SuspendLayout();
            this.gMachines.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(730, 471);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // gClients
            // 
            this.gClients.Controls.Add(this.btnClientUpdate);
            this.gClients.Controls.Add(this.txtClientNickname);
            this.gClients.Controls.Add(this.label6);
            this.gClients.Controls.Add(this.txtClientEmail);
            this.gClients.Controls.Add(this.label7);
            this.gClients.Controls.Add(this.txtClientContactInfo);
            this.gClients.Controls.Add(this.label5);
            this.gClients.Controls.Add(this.txtClientContactPerson);
            this.gClients.Controls.Add(this.label4);
            this.gClients.Controls.Add(this.txtClientAddress);
            this.gClients.Controls.Add(this.label3);
            this.gClients.Controls.Add(this.txtClientName);
            this.gClients.Controls.Add(this.label2);
            this.gClients.Controls.Add(this.txtClientID);
            this.gClients.Controls.Add(this.label1);
            this.gClients.Enabled = false;
            this.gClients.Location = new System.Drawing.Point(12, 12);
            this.gClients.Name = "gClients";
            this.gClients.Size = new System.Drawing.Size(320, 291);
            this.gClients.TabIndex = 1;
            this.gClients.TabStop = false;
            this.gClients.Text = "Client details";
            // 
            // btnClientUpdate
            // 
            this.btnClientUpdate.Location = new System.Drawing.Point(95, 247);
            this.btnClientUpdate.Name = "btnClientUpdate";
            this.btnClientUpdate.Size = new System.Drawing.Size(115, 23);
            this.btnClientUpdate.TabIndex = 14;
            this.btnClientUpdate.Text = "&Save client";
            this.btnClientUpdate.UseVisualStyleBackColor = true;
            this.btnClientUpdate.Click += new System.EventHandler(this.btnClientUpdate_Click);
            // 
            // txtClientNickname
            // 
            this.txtClientNickname.Location = new System.Drawing.Point(146, 194);
            this.txtClientNickname.Name = "txtClientNickname";
            this.txtClientNickname.Size = new System.Drawing.Size(155, 23);
            this.txtClientNickname.TabIndex = 13;
            this.txtClientNickname.TextChanged += new System.EventHandler(this.txtClientName_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(143, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Nickname :";
            // 
            // txtClientEmail
            // 
            this.txtClientEmail.Location = new System.Drawing.Point(24, 194);
            this.txtClientEmail.Name = "txtClientEmail";
            this.txtClientEmail.Size = new System.Drawing.Size(116, 23);
            this.txtClientEmail.TabIndex = 11;
            this.txtClientEmail.TextChanged += new System.EventHandler(this.txtClientName_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 15);
            this.label7.TabIndex = 10;
            this.label7.Text = "Email :";
            // 
            // txtClientContactInfo
            // 
            this.txtClientContactInfo.Location = new System.Drawing.Point(146, 147);
            this.txtClientContactInfo.Name = "txtClientContactInfo";
            this.txtClientContactInfo.Size = new System.Drawing.Size(155, 23);
            this.txtClientContactInfo.TabIndex = 9;
            this.txtClientContactInfo.TextChanged += new System.EventHandler(this.txtClientName_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(143, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Contact info :";
            // 
            // txtClientContactPerson
            // 
            this.txtClientContactPerson.Location = new System.Drawing.Point(24, 147);
            this.txtClientContactPerson.Name = "txtClientContactPerson";
            this.txtClientContactPerson.Size = new System.Drawing.Size(116, 23);
            this.txtClientContactPerson.TabIndex = 7;
            this.txtClientContactPerson.TextChanged += new System.EventHandler(this.txtClientName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Contact person :";
            // 
            // txtClientAddress
            // 
            this.txtClientAddress.Location = new System.Drawing.Point(24, 100);
            this.txtClientAddress.Name = "txtClientAddress";
            this.txtClientAddress.Size = new System.Drawing.Size(277, 23);
            this.txtClientAddress.TabIndex = 5;
            this.txtClientAddress.TextChanged += new System.EventHandler(this.txtClientName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Address :";
            // 
            // txtClientName
            // 
            this.txtClientName.Location = new System.Drawing.Point(98, 52);
            this.txtClientName.Name = "txtClientName";
            this.txtClientName.Size = new System.Drawing.Size(203, 23);
            this.txtClientName.TabIndex = 3;
            this.txtClientName.TextChanged += new System.EventHandler(this.txtClientName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name :";
            // 
            // txtClientID
            // 
            this.txtClientID.Location = new System.Drawing.Point(24, 52);
            this.txtClientID.Name = "txtClientID";
            this.txtClientID.ReadOnly = true;
            this.txtClientID.Size = new System.Drawing.Size(68, 23);
            this.txtClientID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID :";
            // 
            // gSites
            // 
            this.gSites.Controls.Add(this.btnSiteUpdate);
            this.gSites.Controls.Add(this.txtSiteDescription);
            this.gSites.Controls.Add(this.label8);
            this.gSites.Controls.Add(this.txtSiteName);
            this.gSites.Controls.Add(this.label9);
            this.gSites.Controls.Add(this.txtSiteID);
            this.gSites.Controls.Add(this.label10);
            this.gSites.Enabled = false;
            this.gSites.Location = new System.Drawing.Point(12, 309);
            this.gSites.Name = "gSites";
            this.gSites.Size = new System.Drawing.Size(320, 185);
            this.gSites.TabIndex = 2;
            this.gSites.TabStop = false;
            this.gSites.Text = "Site details";
            // 
            // btnSiteUpdate
            // 
            this.btnSiteUpdate.Location = new System.Drawing.Point(98, 141);
            this.btnSiteUpdate.Name = "btnSiteUpdate";
            this.btnSiteUpdate.Size = new System.Drawing.Size(115, 23);
            this.btnSiteUpdate.TabIndex = 15;
            this.btnSiteUpdate.Text = "&Save site";
            this.btnSiteUpdate.UseVisualStyleBackColor = true;
            this.btnSiteUpdate.Click += new System.EventHandler(this.btnSiteUpdate_Click);
            // 
            // txtSiteDescription
            // 
            this.txtSiteDescription.Location = new System.Drawing.Point(20, 99);
            this.txtSiteDescription.Name = "txtSiteDescription";
            this.txtSiteDescription.Size = new System.Drawing.Size(277, 23);
            this.txtSiteDescription.TabIndex = 11;
            this.txtSiteDescription.TextChanged += new System.EventHandler(this.txtSiteName_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 15);
            this.label8.TabIndex = 10;
            this.label8.Text = "Description :";
            // 
            // txtSiteName
            // 
            this.txtSiteName.Location = new System.Drawing.Point(94, 51);
            this.txtSiteName.Name = "txtSiteName";
            this.txtSiteName.Size = new System.Drawing.Size(203, 23);
            this.txtSiteName.TabIndex = 9;
            this.txtSiteName.TextChanged += new System.EventHandler(this.txtSiteName_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(91, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 15);
            this.label9.TabIndex = 8;
            this.label9.Text = "Name :";
            // 
            // txtSiteID
            // 
            this.txtSiteID.Location = new System.Drawing.Point(20, 51);
            this.txtSiteID.Name = "txtSiteID";
            this.txtSiteID.ReadOnly = true;
            this.txtSiteID.Size = new System.Drawing.Size(68, 23);
            this.txtSiteID.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 32);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 15);
            this.label10.TabIndex = 6;
            this.label10.Text = "ID :";
            // 
            // gMachines
            // 
            this.gMachines.Controls.Add(this.label12);
            this.gMachines.Controls.Add(this.label11);
            this.gMachines.Controls.Add(this.txtProblems);
            this.gMachines.Controls.Add(this.lstMachines);
            this.gMachines.Enabled = false;
            this.gMachines.Location = new System.Drawing.Point(339, 13);
            this.gMachines.Name = "gMachines";
            this.gMachines.Size = new System.Drawing.Size(461, 452);
            this.gMachines.TabIndex = 3;
            this.gMachines.TabStop = false;
            this.gMachines.Text = "Problem details";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 15);
            this.label12.TabIndex = 10;
            this.label12.Text = "Select machine :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 249);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(166, 15);
            this.label11.TabIndex = 9;
            this.label11.Text = "Problem in selected machine :";
            // 
            // txtProblems
            // 
            this.txtProblems.Location = new System.Drawing.Point(7, 267);
            this.txtProblems.Multiline = true;
            this.txtProblems.Name = "txtProblems";
            this.txtProblems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProblems.Size = new System.Drawing.Size(447, 166);
            this.txtProblems.TabIndex = 1;
            // 
            // lstMachines
            // 
            this.lstMachines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.lstMachines.FullRowSelect = true;
            this.lstMachines.GridLines = true;
            this.lstMachines.Location = new System.Drawing.Point(6, 42);
            this.lstMachines.MultiSelect = false;
            this.lstMachines.Name = "lstMachines";
            this.lstMachines.Size = new System.Drawing.Size(448, 193);
            this.lstMachines.TabIndex = 0;
            this.lstMachines.UseCompatibleStateImageBehavior = false;
            this.lstMachines.View = System.Windows.Forms.View.Details;
            this.lstMachines.SelectedIndexChanged += new System.EventHandler(this.lstMachines_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 29;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "M/C ID";
            this.columnHeader6.Width = 72;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Type";
            this.columnHeader7.Width = 50;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Site name";
            this.columnHeader8.Width = 99;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "SIM Number";
            this.columnHeader9.Width = 91;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Last updated";
            this.columnHeader10.Width = 97;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(366, 475);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(99, 15);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Loading details ...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ShowDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(812, 506);
            this.ControlBox = false;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.gMachines);
            this.Controls.Add(this.gSites);
            this.Controls.Add(this.gClients);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ShowDetails";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Details";
            this.Load += new System.EventHandler(this.ShowDetails_Load);
            this.gClients.ResumeLayout(false);
            this.gClients.PerformLayout();
            this.gSites.ResumeLayout(false);
            this.gSites.PerformLayout();
            this.gMachines.ResumeLayout(false);
            this.gMachines.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox gClients;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtClientID;
        private System.Windows.Forms.TextBox txtClientName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtClientAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtClientContactInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtClientContactPerson;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtClientNickname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtClientEmail;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox gSites;
        private System.Windows.Forms.TextBox txtSiteDescription;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSiteName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSiteID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnClientUpdate;
        private System.Windows.Forms.Button btnSiteUpdate;
        private System.Windows.Forms.GroupBox gMachines;
        private System.Windows.Forms.ListView lstMachines;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtProblems;
        private System.Windows.Forms.Label lblStatus;
    }
}