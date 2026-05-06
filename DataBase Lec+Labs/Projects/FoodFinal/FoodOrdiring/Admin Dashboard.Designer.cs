namespace FoodOrdiring
{
    partial class AdminDashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            btnAddRest = new Button();
            btnUpdateRest = new Button();
            btnDeleteRest = new Button();
            txtRestName = new TextBox();
            dgvRestaurants = new DataGridView();
            tabPage2 = new TabPage();
            txtVehicle = new TextBox();
            dataGridView1 = new DataGridView();
            btnHire = new Button();
            btnFire = new Button();
            txtUserName = new TextBox();
            txtUserEmail = new TextBox();
            tabPage3 = new TabPage();
            label2 = new Label();
            numericUpDown1 = new NumericUpDown();
            dataGridView2 = new DataGridView();
            btnAddItem = new Button();
            btnEditItem = new Button();
            txtItemName = new TextBox();
            label1 = new Label();
            cmbUserType = new ComboBox();
            cmbAdminID = new ComboBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRestaurants).BeginInit();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(776, 426);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.Black;
            tabPage1.Controls.Add(cmbAdminID);
            tabPage1.Controls.Add(btnAddRest);
            tabPage1.Controls.Add(btnUpdateRest);
            tabPage1.Controls.Add(btnDeleteRest);
            tabPage1.Controls.Add(txtRestName);
            tabPage1.Controls.Add(dgvRestaurants);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(768, 398);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Restaurant Management";
            tabPage1.Click += tabPage1_Click;
            // 
            // btnAddRest
            // 
            btnAddRest.BackColor = Color.Gold;
            btnAddRest.Location = new Point(20, 150);
            btnAddRest.Name = "btnAddRest";
            btnAddRest.Size = new Size(75, 23);
            btnAddRest.TabIndex = 0;
            btnAddRest.Text = "ADD";
            btnAddRest.UseVisualStyleBackColor = false;
            btnAddRest.Click += btnAddRest_Click;
            // 
            // btnUpdateRest
            // 
            btnUpdateRest.BackColor = Color.Gold;
            btnUpdateRest.Location = new Point(101, 150);
            btnUpdateRest.Name = "btnUpdateRest";
            btnUpdateRest.Size = new Size(75, 23);
            btnUpdateRest.TabIndex = 1;
            btnUpdateRest.Text = "UPDATE";
            btnUpdateRest.UseVisualStyleBackColor = false;
            btnUpdateRest.Click += btnUpdateRest_Click;
            // 
            // btnDeleteRest
            // 
            btnDeleteRest.BackColor = Color.Gold;
            btnDeleteRest.Location = new Point(182, 150);
            btnDeleteRest.Name = "btnDeleteRest";
            btnDeleteRest.Size = new Size(75, 23);
            btnDeleteRest.TabIndex = 2;
            btnDeleteRest.Text = "DELETE";
            btnDeleteRest.UseVisualStyleBackColor = false;
            btnDeleteRest.Click += btnDeleteRest_Click;
            // 
            // txtRestName
            // 
            txtRestName.Location = new Point(20, 88);
            txtRestName.Name = "txtRestName";
            txtRestName.Size = new Size(237, 23);
            txtRestName.TabIndex = 3;
            txtRestName.Text = "Enter Restaurant Name";
            txtRestName.TextChanged += txtRestName_TextChanged;
            // 
            // dgvRestaurants
            // 
            dgvRestaurants.Location = new Point(295, 26);
            dgvRestaurants.Name = "dgvRestaurants";
            dgvRestaurants.Size = new Size(450, 350);
            dgvRestaurants.TabIndex = 5;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.Black;
            tabPage2.Controls.Add(txtVehicle);
            tabPage2.Controls.Add(dataGridView1);
            tabPage2.Controls.Add(btnHire);
            tabPage2.Controls.Add(btnFire);
            tabPage2.Controls.Add(txtUserName);
            tabPage2.Controls.Add(txtUserEmail);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(768, 398);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Staff & Delivery";
            // 
            // txtVehicle
            // 
            txtVehicle.Location = new Point(47, 146);
            txtVehicle.Name = "txtVehicle";
            txtVehicle.Size = new Size(165, 23);
            txtVehicle.TabIndex = 7;
            txtVehicle.TextChanged += txtVehicle_TextChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.Location = new Point(296, 25);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(450, 350);
            dataGridView1.TabIndex = 6;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // btnHire
            // 
            btnHire.BackColor = Color.Gold;
            btnHire.Location = new Point(47, 180);
            btnHire.Name = "btnHire";
            btnHire.Size = new Size(75, 23);
            btnHire.TabIndex = 0;
            btnHire.Text = "REGISTER";
            btnHire.UseVisualStyleBackColor = false;
            btnHire.Click += btnHire_Click;
            // 
            // btnFire
            // 
            btnFire.BackColor = Color.Gold;
            btnFire.Location = new Point(137, 180);
            btnFire.Name = "btnFire";
            btnFire.Size = new Size(75, 23);
            btnFire.TabIndex = 1;
            btnFire.Text = "REMOVE";
            btnFire.UseVisualStyleBackColor = false;
            btnFire.Click += btnFire_Click;
            // 
            // txtUserName
            // 
            txtUserName.Location = new Point(47, 117);
            txtUserName.Name = "txtUserName";
            txtUserName.Size = new Size(165, 23);
            txtUserName.TabIndex = 2;
            txtUserName.TextChanged += txtUserName_TextChanged;
            // 
            // txtUserEmail
            // 
            txtUserEmail.Location = new Point(47, 88);
            txtUserEmail.Name = "txtUserEmail";
            txtUserEmail.Size = new Size(165, 23);
            txtUserEmail.TabIndex = 3;
            txtUserEmail.TextChanged += txtUserEmail_TextChanged;
            // 
            // tabPage3
            // 
            tabPage3.BackColor = Color.Black;
            tabPage3.Controls.Add(label2);
            tabPage3.Controls.Add(numericUpDown1);
            tabPage3.Controls.Add(dataGridView2);
            tabPage3.Controls.Add(btnAddItem);
            tabPage3.Controls.Add(btnEditItem);
            tabPage3.Controls.Add(txtItemName);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(768, 398);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Menu Management";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(35, 123);
            label2.Name = "label2";
            label2.Size = new Size(44, 21);
            label2.TabIndex = 14;
            label2.Text = "Price";
            label2.Click += label2_Click;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(35, 147);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(165, 23);
            numericUpDown1.TabIndex = 13;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // dataGridView2
            // 
            dataGridView2.Location = new Point(284, 24);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(450, 350);
            dataGridView2.TabIndex = 12;
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            // 
            // btnAddItem
            // 
            btnAddItem.BackColor = Color.Gold;
            btnAddItem.Location = new Point(35, 179);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(75, 23);
            btnAddItem.TabIndex = 8;
            btnAddItem.Text = "Add";
            btnAddItem.UseVisualStyleBackColor = false;
            btnAddItem.Click += btnAddItem_Click;
            // 
            // btnEditItem
            // 
            btnEditItem.BackColor = Color.Gold;
            btnEditItem.Location = new Point(125, 179);
            btnEditItem.Name = "btnEditItem";
            btnEditItem.Size = new Size(75, 23);
            btnEditItem.TabIndex = 9;
            btnEditItem.Text = "Edit";
            btnEditItem.UseVisualStyleBackColor = false;
            btnEditItem.Click += btnEditItem_Click;
            // 
            // txtItemName
            // 
            txtItemName.Location = new Point(35, 89);
            txtItemName.Name = "txtItemName";
            txtItemName.Size = new Size(165, 23);
            txtItemName.TabIndex = 10;
            txtItemName.Text = "tem Name";
            txtItemName.TextChanged += textBox2_TextChanged;
            // 
            // label1
            // 
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 0;
            // 
            // cmbUserType
            // 
            cmbUserType.Location = new Point(0, 0);
            cmbUserType.Name = "cmbUserType";
            cmbUserType.Size = new Size(121, 23);
            cmbUserType.TabIndex = 0;
            // 
            // cmbAdminID
            // 
            cmbAdminID.FormattingEnabled = true;
            cmbAdminID.Location = new Point(20, 119);
            cmbAdminID.Name = "cmbAdminID";
            cmbAdminID.Size = new Size(237, 23);
            cmbAdminID.TabIndex = 6;
            cmbAdminID.Text = "Admin";
            cmbAdminID.SelectedIndexChanged += cmbAdminID_SelectedIndexChanged;
            // 
            // Form1
            // 
            BackColor = Color.Black;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Admin Dashboard";
            this.Load += new System.EventHandler(this.Form1_Load);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRestaurants).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvRestaurants;
        private System.Windows.Forms.TextBox txtRestName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddRest;
        private System.Windows.Forms.Button btnUpdateRest;
        private System.Windows.Forms.Button btnDeleteRest;
        private System.Windows.Forms.ComboBox cmbUserType;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtUserEmail;
        private System.Windows.Forms.TextBox txtVehicle;
        private System.Windows.Forms.Button btnHire;
        private System.Windows.Forms.Button btnFire;
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private Button btnAddItem;
        private Button btnEditItem;
        private TextBox txtItemName;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private ComboBox cmbAdminID;
    }
}