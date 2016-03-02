namespace Robo
{
    partial class ManageParameters
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstStatus = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtParFormat = new System.Windows.Forms.ComboBox();
            this.txtOrder = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkHealth = new System.Windows.Forms.CheckBox();
            this.btnAddStatusParameter = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtParName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstSettings = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtParFormat1 = new System.Windows.Forms.ComboBox();
            this.txtOrder1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnAddSettingParameter = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtParName1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSaveClose = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCancelChanges = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstStatus);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 411);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status parameters";
            // 
            // lstStatus
            // 
            this.lstStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lstStatus.FullRowSelect = true;
            this.lstStatus.GridLines = true;
            this.lstStatus.Location = new System.Drawing.Point(7, 203);
            this.lstStatus.MultiSelect = false;
            this.lstStatus.Name = "lstStatus";
            this.lstStatus.Size = new System.Drawing.Size(354, 202);
            this.lstStatus.TabIndex = 1;
            this.lstStatus.UseCompatibleStateImageBehavior = false;
            this.lstStatus.View = System.Windows.Forms.View.Details;
            this.lstStatus.DoubleClick += new System.EventHandler(this.ListView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 30;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Parameter name";
            this.columnHeader2.Width = 109;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Format";
            this.columnHeader3.Width = 110;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Check for health";
            this.columnHeader4.Width = 100;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtParFormat);
            this.groupBox3.Controls.Add(this.txtOrder);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.chkHealth);
            this.groupBox3.Controls.Add(this.btnAddStatusParameter);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtParName);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(7, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(354, 163);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // txtParFormat
            // 
            this.txtParFormat.FormattingEnabled = true;
            this.txtParFormat.Location = new System.Drawing.Point(110, 49);
            this.txtParFormat.Name = "txtParFormat";
            this.txtParFormat.Size = new System.Drawing.Size(232, 23);
            this.txtParFormat.TabIndex = 2;
            // 
            // txtOrder
            // 
            this.txtOrder.Location = new System.Drawing.Point(78, 131);
            this.txtOrder.Name = "txtOrder";
            this.txtOrder.Size = new System.Drawing.Size(71, 23);
            this.txtOrder.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 134);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 15);
            this.label8.TabIndex = 6;
            this.label8.Text = "Order no :";
            // 
            // chkHealth
            // 
            this.chkHealth.AutoSize = true;
            this.chkHealth.Location = new System.Drawing.Point(155, 131);
            this.chkHealth.Name = "chkHealth";
            this.chkHealth.Size = new System.Drawing.Size(113, 19);
            this.chkHealth.TabIndex = 4;
            this.chkHealth.Text = "Check for health";
            this.chkHealth.UseVisualStyleBackColor = true;
            // 
            // btnAddStatusParameter
            // 
            this.btnAddStatusParameter.Location = new System.Drawing.Point(283, 130);
            this.btnAddStatusParameter.Name = "btnAddStatusParameter";
            this.btnAddStatusParameter.Size = new System.Drawing.Size(65, 23);
            this.btnAddStatusParameter.TabIndex = 5;
            this.btnAddStatusParameter.Text = "&ADD";
            this.btnAddStatusParameter.UseVisualStyleBackColor = true;
            this.btnAddStatusParameter.Click += new System.EventHandler(this.btnAddStatusParameter_Click);
            // 
            // label3
            // 
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Location = new System.Drawing.Point(14, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(328, 52);
            this.label3.TabIndex = 4;
            this.label3.Text = "eg. 00/00/0000 , 00:00:00 , 00:00 , 00.0 , 00.00\r\neg. {0:On,1:Off} , {123:FAIL,45" +
                "6:DONE}";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Display format :";
            // 
            // txtParName
            // 
            this.txtParName.Location = new System.Drawing.Point(110, 18);
            this.txtParName.Name = "txtParName";
            this.txtParName.Size = new System.Drawing.Size(232, 23);
            this.txtParName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Parameter name :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstSettings);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Location = new System.Drawing.Point(386, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 411);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings parameters";
            // 
            // lstSettings
            // 
            this.lstSettings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lstSettings.FullRowSelect = true;
            this.lstSettings.GridLines = true;
            this.lstSettings.Location = new System.Drawing.Point(14, 203);
            this.lstSettings.MultiSelect = false;
            this.lstSettings.Name = "lstSettings";
            this.lstSettings.Size = new System.Drawing.Size(354, 202);
            this.lstSettings.TabIndex = 3;
            this.lstSettings.UseCompatibleStateImageBehavior = false;
            this.lstSettings.View = System.Windows.Forms.View.Details;
            this.lstSettings.DoubleClick += new System.EventHandler(this.ListView_DoubleClick);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "ID";
            this.columnHeader5.Width = 30;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Parameter name";
            this.columnHeader6.Width = 154;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Format";
            this.columnHeader7.Width = 162;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtParFormat1);
            this.groupBox4.Controls.Add(this.txtOrder1);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.btnAddSettingParameter);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtParName1);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(14, 15);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(354, 165);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // txtParFormat1
            // 
            this.txtParFormat1.FormattingEnabled = true;
            this.txtParFormat1.Location = new System.Drawing.Point(116, 50);
            this.txtParFormat1.Name = "txtParFormat1";
            this.txtParFormat1.Size = new System.Drawing.Size(226, 23);
            this.txtParFormat1.TabIndex = 2;
            // 
            // txtOrder1
            // 
            this.txtOrder1.Location = new System.Drawing.Point(201, 129);
            this.txtOrder1.Name = "txtOrder1";
            this.txtOrder1.Size = new System.Drawing.Size(57, 23);
            this.txtOrder1.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(135, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Order no :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 15);
            this.label9.TabIndex = 8;
            this.label9.Text = "Display format :";
            // 
            // btnAddSettingParameter
            // 
            this.btnAddSettingParameter.Location = new System.Drawing.Point(277, 132);
            this.btnAddSettingParameter.Name = "btnAddSettingParameter";
            this.btnAddSettingParameter.Size = new System.Drawing.Size(65, 23);
            this.btnAddSettingParameter.TabIndex = 4;
            this.btnAddSettingParameter.Text = "&ADD";
            this.btnAddSettingParameter.UseVisualStyleBackColor = true;
            this.btnAddSettingParameter.Click += new System.EventHandler(this.btnAddSettingParameter_Click);
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Location = new System.Drawing.Point(6, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(336, 49);
            this.label4.TabIndex = 4;
            this.label4.Text = "eg. 00/00/0000 , 00:00:00 , 00:00 , 00.0 , 00.00\r\neg. {0:On,1:Off} , {123:FAIL,45" +
                "6:DONE}";
            // 
            // txtParName1
            // 
            this.txtParName1.Location = new System.Drawing.Point(116, 21);
            this.txtParName1.Name = "txtParName1";
            this.txtParName1.Size = new System.Drawing.Size(226, 23);
            this.txtParName1.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Parameter name :";
            // 
            // btnSaveClose
            // 
            this.btnSaveClose.Location = new System.Drawing.Point(663, 430);
            this.btnSaveClose.Name = "btnSaveClose";
            this.btnSaveClose.Size = new System.Drawing.Size(100, 23);
            this.btnSaveClose.TabIndex = 2;
            this.btnSaveClose.Text = "&Save && Close";
            this.btnSaveClose.UseVisualStyleBackColor = true;
            this.btnSaveClose.Click += new System.EventHandler(this.btnSaveClose_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(266, 434);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(250, 15);
            this.label7.TabIndex = 5;
            this.label7.Text = "Double click on list item to remove parameter.";
            // 
            // btnCancelChanges
            // 
            this.btnCancelChanges.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelChanges.Location = new System.Drawing.Point(9, 430);
            this.btnCancelChanges.Name = "btnCancelChanges";
            this.btnCancelChanges.Size = new System.Drawing.Size(109, 23);
            this.btnCancelChanges.TabIndex = 6;
            this.btnCancelChanges.Text = "&Cancel changes";
            this.btnCancelChanges.UseVisualStyleBackColor = true;
            this.btnCancelChanges.Click += new System.EventHandler(this.btnCancelChanges_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(130, 430);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "&Format helper";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ManageParameters
            // 
            this.AcceptButton = this.btnSaveClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelChanges;
            this.ClientSize = new System.Drawing.Size(780, 465);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCancelChanges);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnSaveClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ManageParameters";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage parameters";
            this.Load += new System.EventHandler(this.ManageParameters_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtParName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAddStatusParameter;
        private System.Windows.Forms.ListView lstStatus;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.CheckBox chkHealth;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ListView lstSettings;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnAddSettingParameter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtParName1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSaveClose;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCancelChanges;
        private System.Windows.Forms.TextBox txtOrder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtOrder1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox txtParFormat;
        private System.Windows.Forms.ComboBox txtParFormat1;
        private System.Windows.Forms.Button button1;
    }
}