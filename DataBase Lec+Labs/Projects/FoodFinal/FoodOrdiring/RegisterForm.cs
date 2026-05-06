namespace FoodOrdiring
{
    public class RegisterForm : Form
    {
        public RegisterForm()
        {
            Text      = "Customer Registration";
            Size      = new Size(420, 380);
            BackColor = Color.FromArgb(20, 20, 20);
            StartPosition = FormStartPosition.CenterParent;

            var make = (string lbl, int y) =>
            {
                var l = new Label { Text = lbl, ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10), Location = new Point(30, y), AutoSize = true };
                Controls.Add(l);
                var t = new TextBox { Location = new Point(30, y + 22), Size = new Size(340, 26) };
                Controls.Add(t);
                return t;
            };

            var txtName    = make("Full Name",    30);
            var txtEmail   = make("Email",        90);
            var txtPass    = make("Password",    150);
            txtPass.PasswordChar = '*';
            var txtAddress = make("Address",     210);

            var btnReg = new Button
            {
                Text      = "Register",
                BackColor = Color.Gold,
                Font      = new Font("Segoe UI", 11, FontStyle.Bold),
                Location  = new Point(30, 280),
                Size      = new Size(160, 38)
            };

            btnReg.Click += (s, e) =>
            {
                string name = txtName.Text.Trim(), email = txtEmail.Text.Trim(),
                       pass = txtPass.Text.Trim(), addr  = txtAddress.Text.Trim();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(pass))
                {
                    MessageBox.Show("Name, Email and Password are required.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    DatabaseHelper.RegisterCustomer(name, email, pass, addr);
                    MessageBox.Show("Account created! You can now log in.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            };

            Controls.Add(btnReg);
        }
    }
}
