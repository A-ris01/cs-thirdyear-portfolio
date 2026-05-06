using System.Data;

namespace FoodOrdiring
{
    public partial class AdminDashboard : Form
    {
        private readonly int _adminId;

        // Runtime controls added to Orders / Deliveries tabs
        private DataGridView? dgvOrders;
        private DataGridView? dgvDeliveries;
        private ComboBox?     cmbDriver;
        private ComboBox?     cmbRestMenu;

        // 3-second auto-refresh timer
        private readonly System.Windows.Forms.Timer _refreshTimer = new() { Interval = 3000 };

        public AdminDashboard() : this(-1) { }

        public AdminDashboard(int adminId)
        {
            InitializeComponent();
            _adminId  = adminId;
            this.Text = "Admin Dashboard";
        }

        // ─── Load ─────────────────────────────────────────────────────────────

        private void Form1_Load(object sender, EventArgs e)
        {
            // Tab 2 – role combo
            if (!cmbUserType.Items.Contains("Staff"))
                cmbUserType.Items.AddRange(new[] { "Staff", "Driver" });
            cmbUserType.SelectedIndex = 0;
            tabPage2.Controls.Add(cmbUserType);

            // Tab 3 – restaurant combo (sits above the menu grid)
            cmbRestMenu = new ComboBox
            {
                Location      = new Point(35, 52),
                Size          = new Size(165, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            tabPage3.Controls.Add(cmbRestMenu);

            // Build runtime tabs
            AddOrdersTab();
            AddDeliveriesTab();

            // Initial data load
            LoadAdminCombo();
            LoadRestaurants();
            LoadStaffAndDrivers();
            LoadMenuItems();
            LoadRestaurantCombo();
            LoadOrders();
            LoadDeliveries();
            LoadDriverCombo();

            // Start 3-second auto-refresh
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();

            this.FormClosed += (s, ev) => _refreshTimer.Stop();
        }

        // ─── Timer tick: refresh the currently visible tab ────────────────────

        private void RefreshTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                int selected = tabControl1.SelectedIndex;
                switch (selected)
                {
                    case 0: LoadRestaurants();    break;
                    case 1: LoadStaffAndDrivers(); break;
                    case 2: LoadMenuItems();       break;
                    case 3: LoadOrders();          break;
                    case 4: LoadDeliveries();      break;
                }
            }
            catch { /* silently skip on DB error during background refresh */ }
        }

        // ─── Tab 1 : Restaurant Management ───────────────────────────────────

        private void LoadAdminCombo()
        {
            var dt = DatabaseHelper.GetAdmins();
            cmbAdminID.DataSource    = dt;
            cmbAdminID.DisplayMember = "Name";
            cmbAdminID.ValueMember   = "Id";
        }

        private void LoadRestaurants() =>
            dgvRestaurants.DataSource = DatabaseHelper.GetRestaurants();

        private void btnAddRest_Click(object sender, EventArgs e)
        {
            string name = txtRestName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name) || name == "Enter Restaurant Name")
            { MessageBox.Show("Enter a restaurant name."); return; }

            int adminId = cmbAdminID.SelectedValue != null
                ? (int)cmbAdminID.SelectedValue : _adminId;
            try
            {
                DatabaseHelper.AddRestaurant(name, adminId);
                txtRestName.Clear();
                LoadRestaurants();
                LoadRestaurantCombo();
                MessageBox.Show("Restaurant added.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btnUpdateRest_Click(object sender, EventArgs e)
        {
            if (dgvRestaurants.CurrentRow == null) { MessageBox.Show("Select a row."); return; }
            int    id   = Convert.ToInt32(dgvRestaurants.CurrentRow.Cells["Id"].Value);
            string name = txtRestName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("Enter new name."); return; }
            int adminId = cmbAdminID.SelectedValue != null
                ? (int)cmbAdminID.SelectedValue : _adminId;
            try
            {
                DatabaseHelper.UpdateRestaurant(id, name, adminId);
                LoadRestaurants();
                MessageBox.Show("Restaurant updated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btnDeleteRest_Click(object sender, EventArgs e)
        {
            if (dgvRestaurants.CurrentRow == null) { MessageBox.Show("Select a row."); return; }
            int id = Convert.ToInt32(dgvRestaurants.CurrentRow.Cells["Id"].Value);
            if (MessageBox.Show("Delete this restaurant and all its data?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try { DatabaseHelper.DeleteRestaurant(id); LoadRestaurants(); }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void cmbAdminID_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtRestName_TextChanged(object sender, EventArgs e) { }
        private void tabPage1_Click(object sender, EventArgs e) { }

        // ─── Tab 2 : Staff & Drivers ──────────────────────────────────────────

        private void LoadStaffAndDrivers() =>
            dataGridView1.DataSource = DatabaseHelper.GetStaffAndDrivers();

        private void btnHire_Click(object sender, EventArgs e)
        {
            string name  = txtUserName.Text.Trim();
            string email = txtUserEmail.Text.Trim();
            string role  = cmbUserType.Text.Trim();
            string veh   = txtVehicle.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(role))
            { MessageBox.Show("Fill Name, Email and Role."); return; }

            try
            {
                DatabaseHelper.RegisterUser(name, email, "password123", role,
                    role == "Driver" ? veh : null);
                LoadStaffAndDrivers();
                txtUserName.Clear(); txtUserEmail.Clear(); txtVehicle.Clear();
                MessageBox.Show($"{role} registered. Default password: password123",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btnFire_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) { MessageBox.Show("Select a row."); return; }
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
            if (MessageBox.Show("Remove this person?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try { DatabaseHelper.RemoveUser(id); LoadStaffAndDrivers(); }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvStaffDetails_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void txtUserName_TextChanged(object sender, EventArgs e) { }
        private void txtUserEmail_TextChanged(object sender, EventArgs e) { }
        private void txtVehicle_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }

        // ─── Tab 3 : Menu Management ──────────────────────────────────────────

        private void LoadRestaurantCombo()
        {
            if (cmbRestMenu == null) return;
            var dt = DatabaseHelper.GetRestaurantList();
            cmbRestMenu.DataSource    = dt;
            cmbRestMenu.DisplayMember = "Name";
            cmbRestMenu.ValueMember   = "Id";
        }

        private void LoadMenuItems() =>
            dataGridView2.DataSource = DatabaseHelper.GetMenuItems();

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            string name  = txtItemName.Text.Trim();
            double price = (double)numericUpDown1.Value;
            if (string.IsNullOrWhiteSpace(name) || name == "tem Name")
            { MessageBox.Show("Enter item name."); return; }
            if (price <= 0) { MessageBox.Show("Enter a valid price."); return; }
            if (cmbRestMenu?.SelectedValue == null)
            { MessageBox.Show("Select a restaurant."); return; }
            int restId = (int)cmbRestMenu.SelectedValue;
            try
            {
                DatabaseHelper.AddMenuItem(name, price, restId);
                LoadMenuItems();
                txtItemName.Clear(); numericUpDown1.Value = 0;
                MessageBox.Show("Item added.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null) { MessageBox.Show("Select a row."); return; }
            int    id    = Convert.ToInt32(dataGridView2.CurrentRow.Cells["Id"].Value);
            string name  = txtItemName.Text.Trim();
            double price = (double)numericUpDown1.Value;
            if (string.IsNullOrWhiteSpace(name) || price <= 0)
            { MessageBox.Show("Enter valid name and price."); return; }
            try
            {
                DatabaseHelper.UpdateMenuItem(id, name, price);
                LoadMenuItems();
                MessageBox.Show("Item updated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dataGridView2.Rows[e.RowIndex];
            txtItemName.Text     = row.Cells["Name"].Value?.ToString() ?? "";
            numericUpDown1.Value = Convert.ToDecimal(row.Cells["Price"].Value);
        }

        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { }

        // ─── Tab 4 : Orders ───────────────────────────────────────────────────

        private void AddOrdersTab()
        {
            var tab = new TabPage("Orders") { BackColor = Color.Black };

            // Auto-refresh indicator
            var lblTimer = new Label
            {
                Text      = "⟳ Auto-refreshing every 3s",
                ForeColor = Color.LimeGreen,
                Font      = new Font("Segoe UI", 8),
                Location  = new Point(540, 295),
                AutoSize  = true,
                Visible = true
                
            };

            dgvOrders = new DataGridView
            {
                Location  = new Point(12, 12), Size = new Size(740, 270),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White, GridColor = Color.DimGray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray;
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            var btnRefresh = new Button { Text = "↺ Refresh", BackColor = Color.Gold,
                Location = new Point(12, 290), Size = new Size(90, 28) };
            btnRefresh.Click += (s, ev) => LoadOrders();

            var btnReady = new Button { Text = "Mark Ready", BackColor = Color.Gold,
                Location = new Point(112, 290), Size = new Size(110, 28) };
            btnReady.Click += (s, ev) =>
            {
                if (dgvOrders?.CurrentRow == null) { MessageBox.Show("Select a row."); return; }
                try
                {
                    DatabaseHelper.UpdateOrderStatus(
                        Convert.ToInt32(dgvOrders.CurrentRow.Cells["Id"].Value), "Ready");
                    LoadOrders();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            };

            cmbDriver = new ComboBox
            {
                Location = new Point(232, 290), Size = new Size(160, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            var btnAssign = new Button { Text = "Assign Driver", BackColor = Color.Gold,
                Location = new Point(402, 290), Size = new Size(120, 28) };
            btnAssign.Click += (s, ev) =>
            {
                if (dgvOrders?.CurrentRow == null || cmbDriver?.SelectedValue == null)
                { MessageBox.Show("Select an order and a driver."); return; }
                int ordId = Convert.ToInt32(dgvOrders.CurrentRow.Cells["Id"].Value);
                int drvId = (int)cmbDriver.SelectedValue;
                try
                {
                    DatabaseHelper.AssignDelivery(ordId, drvId);
                    LoadOrders(); LoadDeliveries();
                    MessageBox.Show("Driver assigned.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            };

            tab.Controls.AddRange(new Control[]
                { dgvOrders, btnRefresh, btnReady, cmbDriver, btnAssign, lblTimer });
            tabControl1.TabPages.Add(tab);
        }

        private void LoadOrders()
        {
            if (dgvOrders != null)
                dgvOrders.DataSource = DatabaseHelper.GetOrders();
        }

        private void LoadDriverCombo()
        {
            if (cmbDriver == null) return;
            var dt = DatabaseHelper.GetDrivers();
            cmbDriver.DataSource    = dt;
            cmbDriver.DisplayMember = "Name";
            cmbDriver.ValueMember   = "Id";
        }

        // ─── Tab 5 : Deliveries ───────────────────────────────────────────────

        private void AddDeliveriesTab()
        {
            var tab = new TabPage("Deliveries") { BackColor = Color.Black };

            var lblTimer = new Label
            {
                Text      = "⟳ Auto-refreshing every 3s",
                ForeColor = Color.LimeGreen,
                Font      = new Font("Segoe UI", 8),
                Location  = new Point(540, 375),
                AutoSize  = true,
                Visible = true
            };

            dgvDeliveries = new DataGridView
            {
                Location  = new Point(12, 12), Size = new Size(740, 355),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White, GridColor = Color.DimGray,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true
            };
            dgvDeliveries.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray;
            dgvDeliveries.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            tab.Controls.Add(dgvDeliveries);
            tab.Controls.Add(lblTimer);
            tabControl1.TabPages.Add(tab);
        }

        private void LoadDeliveries()
        {
            if (dgvDeliveries != null)
                dgvDeliveries.DataSource = DatabaseHelper.GetDeliveries();
        }
    }
}
