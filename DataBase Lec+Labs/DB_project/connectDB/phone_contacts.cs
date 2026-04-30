using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LabCode
{
    public partial class phone_contacts : Form
    {
        string connectionString = "server=;port=3306;username=;password=;database=phone_book;";

        public phone_contacts()
        {
            InitializeComponent();
        }

        private void LoadContacts(string filter = null)
        {
            LstContacts.Items.Clear();
            LstContacts.DisplayMember = "Text";
            LstContacts.ValueMember = "Value";

            // 1. Define the base query (always use the LEFT JOIN to get category names)
            string query = @"SELECT ContactId, ContactName, ContactTel, cc.CategoryName 
                     FROM contacts c 
                     LEFT JOIN contact_category cc ON c.ContactCategoryID = cc.ContactCategoryID";

            // 2. Append the filter if it exists
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query += " WHERE c.ContactName LIKE @name";
            }

            query += " ORDER BY c.ContactId";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // 3. Add parameter only if filtering
                        if (!string.IsNullOrWhiteSpace(filter))
                        {
                            cmd.Parameters.AddWithValue("@name", "%" + filter + "%");
                        }

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 4. Handle Nulls safely using the 
                                // Using GetString safely because we've already checked for DBNull
                                string categ = reader["CategoryName"] == DBNull.Value
                                           ? "No Category"
                                         : reader.GetString("CategoryName");

                                //string categ = reader.GetString("CategoryName");this will throw error

                                int id = reader.GetInt32("ContactId");
                                string name = reader.GetString("ContactName");

                                LstContacts.Items.Add(new
                                {
                                    Text = $"ID: {id}, Name: {name}, Category: {categ}",
                                    Value = id
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }
        private void LoadCategories()
        {
            CboCateg.DisplayMember = "Text";
            CboCateg.ValueMember = "Value";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {

                    conn.Open();
                    string query = "SELECT ContactCategoryID, CategoryName FROM contact_category";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CboCateg.Items.Add(new
                                {
                                    Text = reader.GetString("CategoryName"),
                                    Value = reader.GetInt32("ContactCategoryID")
                                });
                                CboCateg.Sorted = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void phone_contacts_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadContacts();
            loadadapter();
        }

        private void loadadapter()
        {

            // LINE 1: Create an empty in-memory table 
            DataTable table = new DataTable();
            // LINE 2: Create connection object (not yet open)

            using (var conn = new MySqlConnection(connectionString))
            {
                // LINE 3: Open the physical connection to MySQL
                conn.Open();
                // LINE 4: Write the SQL query as a string                         
                string sql = "SELECT ContactId, ContactName, ContactTel,ContactBirthday, CategoryName,c.ContactCategoryID FROM contacts c LEFT JOIN contact_category cc ON c.ContactCategoryID = cc.ContactCategoryID";

               
                // LINE 5: Attach the SQL query to the connection
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // LINE 6: Create adapter - moves INSIDE the braces
                    var adapter = new MySqlDataAdapter(cmd);

                    // LINE 7: Execute / Fill (Now this will work!)

                    adapter.Fill(table);
                    
                } // Variables 'cmd' and 'adapter' are cleaned up here
            }
            GrdContacts.DataSource = table; //Bind DataTable to DataGridView
            //GrdContacts.Columns["ContactCategoryID"].Visible = false;

        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            int id;

            if (CboCateg.SelectedItem != null)
            {
                var selectedItem = (dynamic)CboCateg.SelectedItem;
                id = selectedItem.Value;
            }
            else
            {
                id = -1;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                // Parameterized query
                string query = "INSERT INTO contacts (ContactName, ContactTel,ContactCategoryId) VALUES (@name, @phone,@ContactCategoryID)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", TxtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", TxtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@ContactCategoryID", id == -1 ? (object)DBNull.Value : id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record saved successfully!");
                        // Clear textboxes
                        foreach (Control c in this.Controls)
                        {
                            if (c is TextBox)
                            {
                                ((TextBox)c).Clear();
                            }
                        }
                        CboCateg.SelectedIndex = -1;

                    }
                }
            }
            loadadapter(); // Refresh list
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            // Check if item is selected
            if (LstContacts.SelectedItem == null)
            {
                MessageBox.Show("Please select a contact to delete");
                return;
            }

            // Retrieve selected ID using dynamic
            var selectedItem = (dynamic)LstContacts.SelectedItem;
            int id = selectedItem.Value;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM contacts WHERE ContactId = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Deleted");
                }
            }
            LoadContacts();

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = TxtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Search box is empty");
                LoadContacts();
                return;
            }
            else
            {
                LoadContacts(searchTerm);

            }

        }


        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // STEP 1: Ensure a row is selected
                if (GrdContacts.SelectedRows.Count == 0)   // nothing selected?
                {
                    MessageBox.Show("Please select a contact to update.");
                    return;   // stop here
                }

                // STEP 2: Grab the selected row and read the ContactId
                DataGridViewRow row = GrdContacts.SelectedRows[0];   // first selected row
                var contactId = row.Cells["ContactId"].Value;   // this goes into WHERE clause

                // STEP 3: Read updated values from the form controls
                string name = TxtFullName.Text.Trim();   // updated name
                string phone = TxtPhone.Text.Trim();   // updated phone

                // STEP 4: Handle Birthday (nullable)
                // DtpBirthday.Checked == false means user chose 'no birthday'
                var dob = DtpBirthday.Checked
                             ? (object)DtpBirthday.Value.Date   // checked → use date (strip time)
                             : DBNull.Value;   // unchecked → store NULL in database

                // STEP 5: Handle Category (nullable FK)
                object categId;
                if (CboCateg.SelectedItem != null)   // something selected in ComboBox?
                {
                    var sel = (dynamic)CboCateg.SelectedItem;   // cast to dynamic to read .Value
                    categId = sel.Value;   // extract the integer ID
                }
                else
                {
                    categId = DBNull.Value;   // no category → store NULL
                }

                // STEP 6: Execute the UPDATE query
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    // write the update command here 







                }   // connection closed automatically

                // STEP 7: Refresh the grid
                loadadapter();   // reload from DB — shows the new values
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }

        }

        private void LstContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cast the anonymous object to dynamic so we can read .Value
            var selected = (dynamic)LstContacts.SelectedItem;
            int contactId = selected.Value;   // The hidden ContactId we stored earlier

            string query = "SELECT ContactName, ContactTel,c.ContactCategoryID,categoryname FROM contacts c left join contact_category on c.ContactCategoryID=contact_category.ContactCategoryID " +
                           "WHERE c.ContactId = @id";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", contactId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())   // Read() returns true if a row was found
                            {
                                TxtFullName.Text = reader.GetString("ContactName");
                                TxtPhone.Text = reader.GetString("ContactTel");

                                // ── Match the ComboBox to the stored CategoryID ──
                                // Use SelectedValue because it matches the 'Value' property we defined earlier
                                //int targetId = reader["ContactCategoryID"] as int? ?? -1;
                                // 1. Handle Null reading from Database using Ternary Operator
                                int targetId = reader["ContactCategoryID"] == DBNull.Value
                                               ? -1
                                               : Convert.ToInt32(reader["ContactCategoryID"]);

                                // 2. Optimized Selection Logic
                                if (targetId == -1)
                                {
                                    // Case: Value is Null - Deselect the ComboBox immediately
                                    CboCateg.SelectedIndex = -1;
                                }
                                else
                                {
                                    // Case: Value exists - Search through existing items
                                    foreach (var item in CboCateg.Items)
                                    {
                                        if (((dynamic)item).Value == targetId)
                                        {
                                            CboCateg.SelectedItem = item;
                                            break;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading contact: " + ex.Message);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void DtpBirthday_ValueChanged(object sender, EventArgs e)
        {

        }

        private void BtnDelGrid_Click(object sender, EventArgs e)
        {

            try
            {
                // Step 1: Check that the user has actually selected a row
                if (GrdContacts.SelectedRows.Count > 0)
                {

                    // Step 2: Access the first selected row    
                    DataGridViewRow row = GrdContacts.SelectedRows[0]; // or var

                    // Step 3: Read the ContactId from the selected row
                    var contactId = row.Cells["ContactId"].Value;

                    // Step 4: Execute the DELETE query and load the grid data
                    // write your code here 
                    







                }
                else
                {
                    MessageBox.Show("Please select a contact to delete.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void BtnUpdateGrid_Click(object sender, EventArgs e)
        {
            try
            {
                // Step 1: Ensure a row is selected
                if (GrdContacts.SelectedRows.Count > 0)
                {

                    // Step 2: Grab the selected row   
                    DataGridViewRow row = GrdContacts.SelectedRows[0]; // or var

                    // Step 3: Extract field values from the selected row's cells
                    var contactId = row.Cells["ContactId"].Value;

                    string firstName = row.Cells["ContactName"].Value.ToString();
                    string tel = row.Cells["ContactTel"].Value.ToString();

                    // Step 4: Handle NULL birthday safely using DBNull.Value check
                    // DBNull.Value represents a database NULL in .NET
                    // DateTime? is a NULLABLE type — it can hold a DateTime OR null


                    DateTime? dob = row.Cells["ContactBirthday"].Value == DBNull.Value
                            ? (DateTime?)null
                            : (DateTime)row.Cells["ContactBirthday"].Value;

                    // Step 5: After editing, refresh the grid to show updated data
                    loadadapter();
                }
                else
                {
                    MessageBox.Show("Please select a contact to delete.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }

        private void GrdContacts_SelectionChanged(object sender, EventArgs e)
        {
            // Check if there is actually a row selected to avoid "Null Reference" errors
            if (GrdContacts.CurrentRow.Index >= 0)
            {
                // Get the currently highlighted row
                DataGridViewRow row = GrdContacts.CurrentRow;

                // Fill TextBoxes - use ?.ToString() to safely handle empty cells
                TxtFullName.Text = row.Cells["ContactName"].Value?.ToString();
                TxtPhone.Text = row.Cells["ContactTel"].Value?.ToString();

                // Handle the ID (you'll need this for your Update button later!)
                // If you don't have a textbox for ID, store it in a private variable
                string contactId = row.Cells["ContactId"].Value.ToString();

                // Handle Date safely
                if (row.Cells["ContactBirthday"].Value != DBNull.Value)
                {
                    DtpBirthday.Value = (DateTime)row.Cells["ContactBirthday"].Value;
                    DtpBirthday.Checked = true;
                }
                else
                {
                    // If there IS NO date, reset it to today and uncheck the box
                    DtpBirthday.Value = DateTime.Now;
                    DtpBirthday.Checked = false; // This requires ShowCheckBox = true in properties
                }


                var categoryIdValue = row.Cells["ContactCategoryID"].Value;

                // 2. Check for NULL or DBNull
                if (categoryIdValue == DBNull.Value)
                {
                    // If it's NULL in the database, clear the ComboBox selection
                    CboCateg.SelectedIndex = -1;
                }
                else
                {
                    int targetId = Convert.ToInt32(categoryIdValue);
                    // If it's NOT null, try to set the SelectedValue
                    // This will work as long as CboCateg.ValueMember is set to "ContactCategoryID"
                    // Case: Value exists - Search through existing items
                    foreach (var item in CboCateg.Items)
                    {
                        if (((dynamic)item).Value == targetId)
                        {
                            CboCateg.SelectedItem = item;
                            break;
                        }
                    }
                }


            }

        }
    }


}
