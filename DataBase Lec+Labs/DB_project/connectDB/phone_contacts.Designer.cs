namespace LabCode
{
    partial class phone_contacts
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
            this.LstContacts = new System.Windows.Forms.ListBox();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.TxtFullName = new System.Windows.Forms.TextBox();
            this.TxtPhone = new System.Windows.Forms.TextBox();
            this.BtnDel = new System.Windows.Forms.Button();
            this.TxtSearch = new System.Windows.Forms.TextBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.CboCateg = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnUpdate = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.GrdContacts = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.DtpBirthday = new System.Windows.Forms.DateTimePicker();
            this.BtnDelGrid = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GrdContacts)).BeginInit();
            this.SuspendLayout();
            // 
            // LstContacts
            // 
            this.LstContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LstContacts.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LstContacts.FormattingEnabled = true;
            this.LstContacts.ItemHeight = 20;
            this.LstContacts.Location = new System.Drawing.Point(639, 440);
            this.LstContacts.Name = "LstContacts";
            this.LstContacts.Size = new System.Drawing.Size(578, 224);
            this.LstContacts.TabIndex = 0;
            this.LstContacts.SelectedIndexChanged += new System.EventHandler(this.LstContacts_SelectedIndexChanged);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAdd.Location = new System.Drawing.Point(47, 345);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(160, 35);
            this.BtnAdd.TabIndex = 1;
            this.BtnAdd.Text = "Add New Contact";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // TxtFullName
            // 
            this.TxtFullName.Location = new System.Drawing.Point(239, 96);
            this.TxtFullName.Name = "TxtFullName";
            this.TxtFullName.Size = new System.Drawing.Size(185, 20);
            this.TxtFullName.TabIndex = 2;
            // 
            // TxtPhone
            // 
            this.TxtPhone.Location = new System.Drawing.Point(239, 153);
            this.TxtPhone.Name = "TxtPhone";
            this.TxtPhone.Size = new System.Drawing.Size(185, 20);
            this.TxtPhone.TabIndex = 3;
            // 
            // BtnDel
            // 
            this.BtnDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDel.Location = new System.Drawing.Point(389, 480);
            this.BtnDel.Name = "BtnDel";
            this.BtnDel.Size = new System.Drawing.Size(160, 35);
            this.BtnDel.TabIndex = 5;
            this.BtnDel.Text = "Delete from ListBox";
            this.BtnDel.UseVisualStyleBackColor = true;
            this.BtnDel.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // TxtSearch
            // 
            this.TxtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtSearch.Location = new System.Drawing.Point(738, 39);
            this.TxtSearch.Name = "TxtSearch";
            this.TxtSearch.Size = new System.Drawing.Size(244, 26);
            this.TxtSearch.TabIndex = 7;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSearch.Location = new System.Drawing.Point(1025, 37);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(91, 28);
            this.BtnSearch.TabIndex = 8;
            this.BtnSearch.Text = "Search";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // CboCateg
            // 
            this.CboCateg.FormattingEnabled = true;
            this.CboCateg.Location = new System.Drawing.Point(239, 217);
            this.CboCateg.Name = "CboCateg";
            this.CboCateg.Size = new System.Drawing.Size(185, 21);
            this.CboCateg.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Contact Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Contact Tel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(25, 215);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Contact Category";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(569, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "Search for Contact";
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUpdate.Location = new System.Drawing.Point(213, 345);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(160, 35);
            this.BtnUpdate.TabIndex = 15;
            this.BtnUpdate.Text = "Save(Update)";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(611, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 24);
            this.label5.TabIndex = 16;
            this.label5.Text = "My Contact List:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(25, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 24);
            this.label6.TabIndex = 17;
            this.label6.Text = "Contact Details";
            // 
            // GrdContacts
            // 
            this.GrdContacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GrdContacts.Location = new System.Drawing.Point(639, 153);
            this.GrdContacts.Name = "GrdContacts";
            this.GrdContacts.Size = new System.Drawing.Size(578, 270);
            this.GrdContacts.TabIndex = 18;
            this.GrdContacts.SelectionChanged += new System.EventHandler(this.GrdContacts_SelectionChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(25, 284);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(159, 20);
            this.label7.TabIndex = 19;
            this.label7.Text = "Contact Date of Birth";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // DtpBirthday
            // 
            this.DtpBirthday.Location = new System.Drawing.Point(239, 283);
            this.DtpBirthday.Name = "DtpBirthday";
            this.DtpBirthday.ShowCheckBox = true;
            this.DtpBirthday.Size = new System.Drawing.Size(200, 20);
            this.DtpBirthday.TabIndex = 20;
            this.DtpBirthday.ValueChanged += new System.EventHandler(this.DtpBirthday_ValueChanged);
            // 
            // BtnDelGrid
            // 
            this.BtnDelGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDelGrid.Location = new System.Drawing.Point(379, 345);
            this.BtnDelGrid.Name = "BtnDelGrid";
            this.BtnDelGrid.Size = new System.Drawing.Size(160, 35);
            this.BtnDelGrid.TabIndex = 22;
            this.BtnDelGrid.Text = "Delete from Grid";
            this.BtnDelGrid.UseVisualStyleBackColor = true;
            this.BtnDelGrid.Click += new System.EventHandler(this.BtnDelGrid_Click);
            // 
            // phone_contacts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1302, 669);
            this.Controls.Add(this.BtnDelGrid);
            this.Controls.Add(this.DtpBirthday);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.GrdContacts);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BtnUpdate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CboCateg);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.TxtSearch);
            this.Controls.Add(this.BtnDel);
            this.Controls.Add(this.TxtPhone);
            this.Controls.Add(this.TxtFullName);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.LstContacts);
            this.Name = "phone_contacts";
            this.Text = "phone_contacts";
            this.Load += new System.EventHandler(this.phone_contacts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GrdContacts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LstContacts;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.TextBox TxtFullName;
        private System.Windows.Forms.TextBox TxtPhone;
        private System.Windows.Forms.Button BtnDel;
        private System.Windows.Forms.TextBox TxtSearch;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.ComboBox CboCateg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView GrdContacts;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker DtpBirthday;
        private System.Windows.Forms.Button BtnDelGrid;
    }
}