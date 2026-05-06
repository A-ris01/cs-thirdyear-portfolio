using MySql.Data.MySqlClient;
using System.Data;

namespace FoodOrdiring
{
    public static class DatabaseHelper
    {
        // ── MySQL Workbench connection string ────────────────────────────────
        // Uses the same format as the reference project (server/port/username/password/database)
        // Change username and password to match your MySQL Workbench credentials.
        private const string ConnString =
            "server=localhost;port=3306;username=root;password=12345;database=FoodOrdiring;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnString);
        }

        // ── Generic helpers ──────────────────────────────────────────────────

        public static DataTable ExecuteQuery(string sql, params MySqlParameter[] prms)
        {
            var dt = new DataTable();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddRange(prms);
            using var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

        public static int ExecuteNonQuery(string sql, params MySqlParameter[] prms)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddRange(prms);
            return cmd.ExecuteNonQuery();
        }

        public static object? ExecuteScalar(string sql, params MySqlParameter[] prms)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddRange(prms);
            return cmd.ExecuteScalar();
        }

        // ── Auth ─────────────────────────────────────────────────────────────

        /// <summary>Returns role string ("Admin","Customer","Staff","Driver") or null if not found.</summary>
        public static string? ValidateLogin(string email, string password)
        {
            const string sql = "SELECT Role FROM Users WHERE Email=@e AND Password=@p LIMIT 1";
            var result = ExecuteScalar(sql,
                new MySqlParameter("@e", email),
                new MySqlParameter("@p", password));
            return result?.ToString();
        }

        public static int GetUserId(string email)
        {
            var result = ExecuteScalar("SELECT Id FROM Users WHERE Email=@e",
                new MySqlParameter("@e", email));
            return result == null ? -1 : Convert.ToInt32(result);
        }

        // ── Restaurants ──────────────────────────────────────────────────────

        public static DataTable GetRestaurants()
        {
            return ExecuteQuery(
                "SELECT r.Id, r.Name, IFNULL(a.Name,'') AS AdminName " +
                "FROM Restaurant r LEFT JOIN Users a ON a.Id=r.AdminId " +
                "ORDER BY r.Id");
        }

        public static void AddRestaurant(string name, int adminId)
        {
            ExecuteNonQuery("INSERT INTO Restaurant(Name,AdminId) VALUES(@n,@a)",
                new MySqlParameter("@n", name),
                new MySqlParameter("@a", adminId));
        }

        public static void UpdateRestaurant(int id, string name, int adminId)
        {
            ExecuteNonQuery("UPDATE Restaurant SET Name=@n, AdminId=@a WHERE Id=@id",
                new MySqlParameter("@n", name),
                new MySqlParameter("@a", adminId),
                new MySqlParameter("@id", id));
        }

        public static void DeleteRestaurant(int id)
        {
            ExecuteNonQuery("DELETE FROM Restaurant WHERE Id=@id",
                new MySqlParameter("@id", id));
        }

        // ── Staff & Drivers ──────────────────────────────────────────────────

        public static DataTable GetStaffAndDrivers()
        {
            return ExecuteQuery(
                "SELECT u.Id, u.Name, u.Email, u.Role, " +
                "IFNULL(s.RestId, dp.Id) AS Extra, " +
                "IFNULL(dp.Vehicle,'') AS Vehicle " +
                "FROM Users u " +
                "LEFT JOIN RestaurantStaff s ON s.Id=u.Id " +
                "LEFT JOIN DeliveryPerson dp ON dp.Id=u.Id " +
                "WHERE u.Role IN ('Staff','Driver') ORDER BY u.Id");
        }

        public static void RegisterUser(string name, string email, string password, string role,
            string? vehicle = null, int restId = 0)
        {
            using var conn = GetConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                // Insert into Users
                var cmdUser = new MySqlCommand(
                    "INSERT INTO Users(Name,Email,Password,Role) VALUES(@n,@e,@p,@r); SELECT LAST_INSERT_ID();",
                    conn, tx);
                cmdUser.Parameters.AddWithValue("@n", name);
                cmdUser.Parameters.AddWithValue("@e", email);
                cmdUser.Parameters.AddWithValue("@p", password);
                cmdUser.Parameters.AddWithValue("@r", role);
                int newId = Convert.ToInt32(cmdUser.ExecuteScalar());

                if (role == "Staff")
                {
                    var cmdStaff = new MySqlCommand(
                        "INSERT INTO RestaurantStaff(Id,Name,Email,RestId) VALUES(@id,@n,@e,@rid)",
                        conn, tx);
                    cmdStaff.Parameters.AddWithValue("@id", newId);
                    cmdStaff.Parameters.AddWithValue("@n", name);
                    cmdStaff.Parameters.AddWithValue("@e", email);
                    cmdStaff.Parameters.AddWithValue("@rid", restId);
                    cmdStaff.ExecuteNonQuery();
                }
                else if (role == "Driver")
                {
                    var cmdDriver = new MySqlCommand(
                        "INSERT INTO DeliveryPerson(Id,Name,Email,Vehicle) VALUES(@id,@n,@e,@v)",
                        conn, tx);
                    cmdDriver.Parameters.AddWithValue("@id", newId);
                    cmdDriver.Parameters.AddWithValue("@n", name);
                    cmdDriver.Parameters.AddWithValue("@e", email);
                    cmdDriver.Parameters.AddWithValue("@v", vehicle ?? "");
                    cmdDriver.ExecuteNonQuery();
                }

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public static void RemoveUser(int id)
        {
            // Cascade: remove from sub-tables first
            ExecuteNonQuery("DELETE FROM RestaurantStaff WHERE Id=@id", new MySqlParameter("@id", id));
            ExecuteNonQuery("DELETE FROM DeliveryPerson WHERE Id=@id", new MySqlParameter("@id", id));
            ExecuteNonQuery("DELETE FROM Users WHERE Id=@id", new MySqlParameter("@id", id));
        }

        // ── Menu Items ───────────────────────────────────────────────────────

        public static DataTable GetMenuItems(int? restId = null)
        {
            if (restId.HasValue)
                return ExecuteQuery(
                    "SELECT m.Id, m.Name, m.Price, r.Name AS Restaurant " +
                    "FROM MenuItem m JOIN Restaurant r ON r.Id=m.RestId " +
                    "WHERE m.RestId=@rid ORDER BY m.Id",
                    new MySqlParameter("@rid", restId.Value));

            return ExecuteQuery(
                "SELECT m.Id, m.Name, m.Price, r.Name AS Restaurant " +
                "FROM MenuItem m JOIN Restaurant r ON r.Id=m.RestId ORDER BY m.Id");
        }

        public static void AddMenuItem(string name, double price, int restId)
        {
            ExecuteNonQuery("INSERT INTO MenuItem(Name,Price,RestId) VALUES(@n,@p,@r)",
                new MySqlParameter("@n", name),
                new MySqlParameter("@p", price),
                new MySqlParameter("@r", restId));
        }

        public static void UpdateMenuItem(int id, string name, double price)
        {
            ExecuteNonQuery("UPDATE MenuItem SET Name=@n, Price=@p WHERE Id=@id",
                new MySqlParameter("@n", name),
                new MySqlParameter("@p", price),
                new MySqlParameter("@id", id));
        }

        public static void DeleteMenuItem(int id)
        {
            ExecuteNonQuery("DELETE FROM MenuItem WHERE Id=@id",
                new MySqlParameter("@id", id));
        }

        // ── Orders ───────────────────────────────────────────────────────────

        public static DataTable GetOrders(int? custId = null)
        {
            if (custId.HasValue)
                return ExecuteQuery(
                    "SELECT o.Id, o.Status, r.Name AS Restaurant, c.Name AS Customer " +
                    "FROM Orders o " +
                    "JOIN Restaurant r ON r.Id=o.RestId " +
                    "JOIN Users c ON c.Id=o.CustId " +
                    "WHERE o.CustId=@cid ORDER BY o.Id DESC",
                    new MySqlParameter("@cid", custId.Value));

            return ExecuteQuery(
                "SELECT o.Id, o.Status, r.Name AS Restaurant, c.Name AS Customer " +
                "FROM Orders o " +
                "JOIN Restaurant r ON r.Id=o.RestId " +
                "JOIN Users c ON c.Id=o.CustId " +
                "ORDER BY o.Id DESC");
        }

        public static int PlaceOrder(int custId, int restId, List<(int ItemId, int Qty)> items)
        {
            using var conn = GetConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                var cmdOrder = new MySqlCommand(
                    "INSERT INTO Orders(Status,CustId,RestId) VALUES('Pending',@cid,@rid); SELECT LAST_INSERT_ID();",
                    conn, tx);
                cmdOrder.Parameters.AddWithValue("@cid", custId);
                cmdOrder.Parameters.AddWithValue("@rid", restId);
                int orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

                foreach (var (itemId, qty) in items)
                {
                    var cmdDetail = new MySqlCommand(
                        "INSERT INTO OrderDetails(OrderId,ItemId,Qty) VALUES(@oid,@iid,@qty)",
                        conn, tx);
                    cmdDetail.Parameters.AddWithValue("@oid", orderId);
                    cmdDetail.Parameters.AddWithValue("@iid", itemId);
                    cmdDetail.Parameters.AddWithValue("@qty", qty);
                    cmdDetail.ExecuteNonQuery();
                }

                tx.Commit();
                return orderId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public static DataTable GetOrderDetails(int orderId)
        {
            return ExecuteQuery(
                "SELECT m.Name, od.Qty, m.Price, (od.Qty * m.Price) AS Subtotal " +
                "FROM OrderDetails od JOIN MenuItem m ON m.Id=od.ItemId " +
                "WHERE od.OrderId=@oid",
                new MySqlParameter("@oid", orderId));
        }

        public static void UpdateOrderStatus(int orderId, string status)
        {
            ExecuteNonQuery("UPDATE Orders SET Status=@s WHERE Id=@id",
                new MySqlParameter("@s", status),
                new MySqlParameter("@id", orderId));
        }

        // ── Delivery ─────────────────────────────────────────────────────────

        public static DataTable GetDeliveries()
        {
            return ExecuteQuery(
                "SELECT d.Id, o.Id AS OrderId, o.Status, dp.Name AS Driver " +
                "FROM Delivery d " +
                "JOIN Orders o ON o.Id=d.OrderId " +
                "JOIN DeliveryPerson dp ON dp.Id=d.DriverId " +
                "ORDER BY d.Id DESC");
        }

        public static void AssignDelivery(int orderId, int driverId)
        {
            ExecuteNonQuery(
                "INSERT INTO Delivery(OrderId,DriverId) VALUES(@oid,@did) " +
                "ON DUPLICATE KEY UPDATE DriverId=@did",
                new MySqlParameter("@oid", orderId),
                new MySqlParameter("@did", driverId));
            UpdateOrderStatus(orderId, "Out for Delivery");
        }

        // ── Lookup helpers ───────────────────────────────────────────────────

        public static DataTable GetAdmins()
        {
            return ExecuteQuery("SELECT Id, Name FROM Users WHERE Role='Admin' ORDER BY Name");
        }

        public static DataTable GetRestaurantList()
        {
            return ExecuteQuery("SELECT Id, Name FROM Restaurant ORDER BY Name");
        }

        public static DataTable GetDrivers()
        {
            return ExecuteQuery(
                "SELECT dp.Id, u.Name FROM DeliveryPerson dp JOIN Users u ON u.Id=dp.Id ORDER BY u.Name");
        }

        // ── Customer registration ─────────────────────────────────────────────

        public static void RegisterCustomer(string name, string email, string password, string address)
        {
            using var conn = GetConnection();
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                var cmd = new MySqlCommand(
                    "INSERT INTO Users(Name,Email,Password,Role) VALUES(@n,@e,@p,'Customer'); SELECT LAST_INSERT_ID();",
                    conn, tx);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@e", email);
                cmd.Parameters.AddWithValue("@p", password);
                int newId = Convert.ToInt32(cmd.ExecuteScalar());

                var cmdCust = new MySqlCommand(
                    "INSERT INTO Customer(Id,Name,Email,Address) VALUES(@id,@n,@e,@a)",
                    conn, tx);
                cmdCust.Parameters.AddWithValue("@id", newId);
                cmdCust.Parameters.AddWithValue("@n", name);
                cmdCust.Parameters.AddWithValue("@e", email);
                cmdCust.Parameters.AddWithValue("@a", address);
                cmdCust.ExecuteNonQuery();

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}
