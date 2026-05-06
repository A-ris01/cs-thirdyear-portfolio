namespace FoodOrdiring
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            cmbRole.Items.AddRange(new[] { "Admin", "Customer", "Driver", "Staff" });
            cmbRole.SelectedIndex = 1;

            txtPassword.PasswordChar = '*';

            var lnkRegister = new LinkLabel
            {
                Text = "New customer? Register here",
                Location = new Point(225, 375),
                AutoSize = true,
                ForeColor = Color.Gold,
                Font = new Font("Segoe UI", 9.5f),
                Visible = true
            };
            lnkRegister.LinkClicked += (s, e) => new RegisterForm().ShowDialog(this);
            Controls.Add(lnkRegister);
        }

        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtPassword_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e) { }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter your email and password.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string? role = DatabaseHelper.ValidateLogin(email, password);

                if (role == null)
                {
                    MessageBox.Show("Invalid email or password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int userId = DatabaseHelper.GetUserId(email);
                this.Hide();

                switch (role)
                {
                    case "Admin":
                    case "Staff":
                        new AdminDashboard(userId).ShowDialog();
                        break;
                    case "Customer":
                        new CustomerInterface(userId).ShowDialog();
                        break;
                    case "Driver":
                        new DriverDashboard(userId).ShowDialog();
                        break;
                    default:
                        MessageBox.Show($"Unknown role: {role}");
                        break;
                }

                this.Show();
                txtEmail.Clear();
                txtPassword.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
