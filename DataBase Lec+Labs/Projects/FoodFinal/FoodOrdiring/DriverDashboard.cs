using System.Data;

namespace FoodOrdiring
{
    public class DriverDashboard : Form
    {
        private readonly int _driverId;
        private DataGridView dgvDeliveries;
        private Label lblStatus;

        // ── FIX 2: Auto-refresh timer ─────────────────────────────────────────
        private System.Windows.Forms.Timer _refreshTimer = new();
        // ─────────────────────────────────────────────────────────────────────

        public DriverDashboard(int driverId)
        {
            _driverId = driverId;
            Text      = "Driver Dashboard";
            Size      = new Size(700, 460);
            BackColor = Color.FromArgb(20, 20, 20);
            StartPosition = FormStartPosition.CenterScreen;

            lblStatus = new Label
            {
                Text      = "Your Assigned Deliveries",
                ForeColor = Color.Gold,
                Font      = new Font("Segoe UI", 13, FontStyle.Bold),
                Location  = new Point(12, 12),
                AutoSize  = true
            };

            // ── FIX 2: "Auto-refreshing" indicator label ──────────────────────
            var lblAutoRefresh = new Label
            {
                Text      = "🔄 Auto-refresh: every 3s",
                ForeColor = Color.Gray,
                Font      = new Font("Segoe UI", 8),
                Location  = new Point(270, 18),
                AutoSize  = true
            };
            // ─────────────────────────────────────────────────────────────────

            dgvDeliveries = new DataGridView
            {
                Location = new Point(12, 48),
                Size     = new Size(660, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                GridColor = Color.DimGray,
                ColumnHeadersDefaultCellStyle = { BackColor = Color.DimGray, ForeColor = Color.White }
            };

            var btnRefresh = new Button
            {
                Text = "Refresh Now", BackColor = Color.Gold,
                Location = new Point(12, 358), Size = new Size(110, 32)
            };
            btnRefresh.Click += (s, e) => LoadDeliveries();

            var btnMarkDelivered = new Button
            {
                Text = "Mark Delivered", BackColor = Color.Gold,
                Location = new Point(132, 358), Size = new Size(140, 32)
            };
            btnMarkDelivered.Click += (s, e) =>
            {
                if (dgvDeliveries.CurrentRow == null) { MessageBox.Show("Select a row."); return; }
                int orderId = Convert.ToInt32(dgvDeliveries.CurrentRow.Cells["OrderId"].Value);
                DatabaseHelper.UpdateOrderStatus(orderId, "Delivered");
                LoadDeliveries();
                MessageBox.Show("Order marked as Delivered!", "Done",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            Controls.AddRange(new Control[] { lblStatus, lblAutoRefresh, dgvDeliveries, btnRefresh, btnMarkDelivered });
            LoadDeliveries();

            // ── FIX 2: Start 3-second auto-refresh timer ─────────────────────
            _refreshTimer.Interval = 3000;
            _refreshTimer.Tick    += (s, e) => LoadDeliveries();
            _refreshTimer.Start();

            this.FormClosed += (s, e) => _refreshTimer.Stop();
            // ─────────────────────────────────────────────────────────────────
        }

        private void LoadDeliveries()
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(
                    "SELECT d.Id, d.OrderId, o.Status, c.Name AS Customer, r.Name AS Restaurant " +
                    "FROM Delivery d " +
                    "JOIN Orders o ON o.Id=d.OrderId " +
                    "JOIN Users c ON c.Id=o.CustId " +
                    "JOIN Restaurant r ON r.Id=o.RestId " +
                    "WHERE d.DriverId=@did ORDER BY d.Id DESC",
                    new MySql.Data.MySqlClient.MySqlParameter("@did", _driverId));
                dgvDeliveries.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}
