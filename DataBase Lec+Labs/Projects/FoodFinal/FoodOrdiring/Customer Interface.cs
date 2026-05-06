using System.Data;

namespace FoodOrdiring
{
    public partial class CustomerInterface : Form
    {
        private readonly int _custId;
        private int _selectedRestId = -1;

        // Cart: key = ItemId, value = (Name, Price, Qty)
        private readonly Dictionary<int, (string Name, double Price, int Qty)> _cart = new();

        // Maps cart row index → ItemId for Remove button
        private readonly List<int> _cartRowItemIds = new();

        // Auto-refresh timer for My Orders tab (3 seconds)
        private System.Windows.Forms.Timer _ordersRefreshTimer = new();

        public CustomerInterface() : this(-1) { }

        public CustomerInterface(int custId)
        {
            InitializeComponent();
            _custId = custId;
            this.Text = "Customer Menu";
        }

        // ── Load ─────────────────────────────────────────────────────────────

        private void CustomerInterface_Load(object sender, EventArgs e)
        {
            SetupCartGrid();
            SetupRestaurantFilter();
            LoadFoodItems();
            UpdateTotal();

            btnPlaceOrder.Text = "Place Order";
            lblTotal.Text = "Total: 0 IQD";
        }

        private void SetupCartGrid()
        {
            dgvCart.Columns.Clear();
            dgvCart.Columns.Add("Name",  "Item");
            dgvCart.Columns.Add("Qty",   "Qty");
            dgvCart.Columns.Add("Price", "Subtotal (IQD)");

            // ── FIX 1: Add Remove button column ──────────────────────────────
            var removeBtn = new DataGridViewButtonColumn
            {
                Name         = "Remove",
                HeaderText   = "",
                Text         = "✕ Remove",
                UseColumnTextForButtonValue = true,
                FlatStyle    = FlatStyle.Flat,
                Width        = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dgvCart.Columns.Add(removeBtn);
            // ─────────────────────────────────────────────────────────────────

            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCart.AllowUserToAddRows  = false;

            // Wire up the cell click for the Remove button
            dgvCart.CellClick += DgvCart_CellClick;
        }

        // ── FIX 1: Handle Remove button click ────────────────────────────────
        private void DgvCart_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCart.Columns[e.ColumnIndex].Name != "Remove") return;

            if (e.RowIndex < _cartRowItemIds.Count)
            {
                int itemId = _cartRowItemIds[e.RowIndex];
                _cart.Remove(itemId);
                RefreshCartGrid();
                UpdateTotal();
            }
        }
        // ─────────────────────────────────────────────────────────────────────

        private void SetupRestaurantFilter()
        {
            if (cmbRestFilter == null) return;
            var dt = DatabaseHelper.GetRestaurantList();
            var all = dt.NewRow();
            all["Id"]   = -1;
            all["Name"] = "All Restaurants";
            dt.Rows.InsertAt(all, 0);

            cmbRestFilter.DataSource    = dt;
            cmbRestFilter.DisplayMember = "Name";
            cmbRestFilter.ValueMember   = "Id";
            cmbRestFilter.SelectedIndex = 0;
            cmbRestFilter.SelectedIndexChanged += (s, e) =>
            {
                _selectedRestId = (int)(cmbRestFilter.SelectedValue ?? -1);
                LoadFoodItems();
            };
        }

        private void LoadFoodItems()
        {
            flowLayoutPanel1.Controls.Clear();
            try
            {
                var dt = DatabaseHelper.GetMenuItems(_selectedRestId > 0 ? _selectedRestId : null);
                foreach (DataRow row in dt.Rows)
                {
                    int    id    = Convert.ToInt32(row["Id"]);
                    string name  = row["Name"].ToString() ?? "";
                    double price = Convert.ToDouble(row["Price"]);
                    string rest  = row["Restaurant"].ToString() ?? "";

                    var card = BuildFoodCard(id, name, price, rest);
                    flowLayoutPanel1.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load menu: " + ex.Message);
            }
        }

        private Panel BuildFoodCard(int itemId, string name, double price, string restName)
        {
            var panel = new Panel
            {
                Width     = 180,
                Height    = 140,
                BackColor = Color.FromArgb(30, 30, 30),
                Margin    = new Padding(8),
                Cursor    = Cursors.Hand
            };

            var lblName = new Label
            {
                Text      = name,
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 10, FontStyle.Bold),
                Location  = new Point(8, 8),
                Size      = new Size(164, 40),
                AutoEllipsis = true
            };

            var lblRest = new Label
            {
                Text      = restName,
                ForeColor = Color.Gray,
                Font      = new Font("Segoe UI", 7),
                Location  = new Point(8, 50),
                Size      = new Size(164, 18)
            };

            var lblPrice = new Label
            {
                Text      = price.ToString("N0") + " IQD",
                ForeColor = Color.Gold,
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                Location  = new Point(8, 68),
                AutoSize  = true
            };

            var numQty = new NumericUpDown
            {
                Minimum  = 1,
                Maximum  = 20,
                Value    = 1,
                Location = new Point(8, 92),
                Size     = new Size(60, 24)
            };

            var btnAdd = new Button
            {
                Text      = "Add",
                BackColor = Color.Gold,
                ForeColor = Color.Black,
                Font      = new Font("Segoe UI", 8, FontStyle.Bold),
                Location  = new Point(76, 92),
                Size      = new Size(60, 26),
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.FlatAppearance.BorderSize = 0;

            btnAdd.Click += (s, e) =>
            {
                int qty = (int)numQty.Value;
                if (_cart.ContainsKey(itemId))
                    _cart[itemId] = (name, price, _cart[itemId].Qty + qty);
                else
                    _cart[itemId] = (name, price, qty);
                RefreshCartGrid();
                UpdateTotal();
            };

            panel.Controls.AddRange(new Control[] { lblName, lblRest, lblPrice, numQty, btnAdd });
            return panel;
        }

        // ── Cart ──────────────────────────────────────────────────────────────

        private void RefreshCartGrid()
        {
            dgvCart.Rows.Clear();
            _cartRowItemIds.Clear();   // FIX 1: keep row→itemId mapping in sync

            foreach (var kv in _cart)
            {
                var (name, price, qty) = kv.Value;
                dgvCart.Rows.Add(name, qty, (price * qty).ToString("N0"));
                _cartRowItemIds.Add(kv.Key);   // FIX 1: track itemId per row
            }
        }

        private void UpdateTotal()
        {
            double total = _cart.Values.Sum(v => v.Price * v.Qty);
            lblTotal.Text = "Total: " + total.ToString("N0") + " IQD";
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (_cart.Count == 0)
            {
                MessageBox.Show("Your cart is empty.", "Empty Cart",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_custId <= 0)
            {
                MessageBox.Show("Please log in first."); return;
            }

            int firstItemId = _cart.Keys.First();
            var restDt = DatabaseHelper.ExecuteQuery(
                "SELECT RestId FROM MenuItem WHERE Id=@id",
                new MySql.Data.MySqlClient.MySqlParameter("@id", firstItemId));

            if (restDt.Rows.Count == 0) { MessageBox.Show("Cannot determine restaurant."); return; }
            int restId = Convert.ToInt32(restDt.Rows[0]["RestId"]);

            var items = _cart.Select(kv => (kv.Key, kv.Value.Qty)).ToList();

            try
            {
                int orderId = DatabaseHelper.PlaceOrder(_custId, restId, items);
                _cart.Clear();
                _cartRowItemIds.Clear();
                RefreshCartGrid();
                UpdateTotal();
                MessageBox.Show($"Order #{orderId} placed successfully!\nStatus: Pending",
                    "Order Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error placing order: " + ex.Message);
            }
        }

        // ── My Orders tab ─────────────────────────────────────────────────────

        private DataGridView? dgvMyOrders;
        private ComboBox? cmbRestFilter;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            cmbRestFilter = new ComboBox
            {
                Location = new Point(12, 432),
                Size     = new Size(200, 24),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            Controls.Add(cmbRestFilter);

            var tabCtrl = new TabControl { Location = new Point(0, 0), Size = ClientSize };

            var tabMenu = new TabPage("Menu") { BackColor = Color.FromArgb(20, 20, 20) };
            tabMenu.Controls.Add(flowLayoutPanel1);
            tabMenu.Controls.Add(dgvCart);
            tabMenu.Controls.Add(btnPlaceOrder);
            tabMenu.Controls.Add(lblTotal);

            if (cmbRestFilter != null)
            {
                cmbRestFilter.Location = new Point(10, 432);
                tabMenu.Controls.Add(cmbRestFilter);
            }

            var tabOrders = new TabPage("My Orders") { BackColor = Color.FromArgb(20, 20, 20) };
            dgvMyOrders = new DataGridView
            {
                Location = new Point(10, 40),
                Size     = new Size(760, 340),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // ── FIX 2: Auto-refresh label showing countdown ───────────────────
            var lblRefreshStatus = new Label
            {
                Text      = "Auto-refreshing every 3s",
                ForeColor = Color.Gray,
                Font      = new Font("Segoe UI", 8),
                Location  = new Point(110, 15),
                AutoSize  = true
            };
            // ─────────────────────────────────────────────────────────────────

            var btnRefreshOrders = new Button
            {
                Text      = "Refresh Now",
                BackColor = Color.Gold,
                Location  = new Point(10, 10),
                Size      = new Size(95, 28)
            };
            btnRefreshOrders.Click += (s, ev) => LoadMyOrders();

            tabOrders.Controls.Add(dgvMyOrders);
            tabOrders.Controls.Add(btnRefreshOrders);
            tabOrders.Controls.Add(lblRefreshStatus);

            Controls.Remove(flowLayoutPanel1);
            Controls.Remove(dgvCart);
            Controls.Remove(btnPlaceOrder);

            tabCtrl.TabPages.Add(tabMenu);
            tabCtrl.TabPages.Add(tabOrders);
            Controls.Add(tabCtrl);
            tabCtrl.BringToFront();

            CustomerInterface_Load(this, EventArgs.Empty);
            SetupRestaurantFilter();
            LoadMyOrders();

            // ── FIX 2: Wire up 3-second auto-refresh timer ───────────────────
            _ordersRefreshTimer.Interval = 3000;   // 3 000 ms = 3 seconds
            _ordersRefreshTimer.Tick += (s, ev) =>
            {
                // Only reload when the My Orders tab is visible
                if (tabCtrl.SelectedTab == tabOrders)
                    LoadMyOrders();
            };
            _ordersRefreshTimer.Start();

            // Stop timer when form closes to avoid cross-thread issues
            this.FormClosed += (s, ev) => _ordersRefreshTimer.Stop();
            // ─────────────────────────────────────────────────────────────────
        }

        private void LoadMyOrders()
        {
            if (dgvMyOrders == null) return;
            try { dgvMyOrders.DataSource = DatabaseHelper.GetOrders(_custId > 0 ? _custId : null); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dgvCart_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
    }
}
