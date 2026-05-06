namespace FoodOrdiring
{
    partial class CustomerInterface
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            flowLayoutPanel1 = new FlowLayoutPanel();
            dgvCart          = new DataGridView();
            btnPlaceOrder    = new Button();
            lblTotal         = new Label();

            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            SuspendLayout();

            // flowLayoutPanel1
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Location   = new Point(12, 12);
            flowLayoutPanel1.Name       = "flowLayoutPanel1";
            flowLayoutPanel1.Size       = new Size(530, 410);
            flowLayoutPanel1.TabIndex   = 0;

            // dgvCart
            dgvCart.Location = new Point(555, 12);
            dgvCart.Name     = "dgvCart";
            dgvCart.Size     = new Size(230, 310);
            dgvCart.TabIndex = 1;

            // lblTotal
            lblTotal.Text      = "Total: 0 IQD";
            lblTotal.ForeColor = Color.Gold;
            lblTotal.Font      = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTotal.Location  = new Point(555, 328);
            lblTotal.Size      = new Size(230, 28);
            lblTotal.Name      = "lblTotal";

            // btnPlaceOrder
            btnPlaceOrder.BackColor = Color.Gold;
            btnPlaceOrder.Font      = new Font("Segoe UI", 12F);
            btnPlaceOrder.Location  = new Point(555, 360);
            btnPlaceOrder.Name      = "btnPlaceOrder";
            btnPlaceOrder.Size      = new Size(230, 50);
            btnPlaceOrder.TabIndex  = 2;
            btnPlaceOrder.Text      = "Place Order";
            btnPlaceOrder.Click    += btnPlaceOrder_Click;

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode       = AutoScaleMode.Font;
            BackColor           = Color.FromArgb(20, 20, 20);
            ClientSize          = new Size(800, 450);
            Controls.Add(btnPlaceOrder);
            Controls.Add(lblTotal);
            Controls.Add(dgvCart);
            Controls.Add(flowLayoutPanel1);
            Name = "CustomerInterface";
            Text = "Food Menu";

            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            ResumeLayout(false);
        }

        private FlowLayoutPanel flowLayoutPanel1;
        private DataGridView    dgvCart;
        private Button          btnPlaceOrder;
        private Label           lblTotal;
    }
}
