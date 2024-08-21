using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace Center_Maneger
{
    public static class databaseLoader
    {
        private static string _connectionString = "Data Source=Center_Manager.sf;Version=3;";

         public static DataTable LoadData(string tableName, string additionalInfo = "") // used to fill tabs in settings
        {
            try
            {
                string query = String.Format("SELECT * FROM {0} ",tableName) + additionalInfo ;
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Remove the 'id' column if it exists
                    if (dataTable.Columns.Contains("id"))
                    {
                        dataTable.Columns.Remove("id");
                    }

                    return dataTable;
                }

            }
            catch (Exception)
            {

                return null;
            }
        }


        public static void InsertRecord(string tableName, Dictionary<string, object> columns) // insert new record
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        string columnNames = string.Join(", ", columns.Keys);
                        string parameterNames = string.Join(", ", columns.Keys.Select(k => "@" + k).ToArray());

                        command.CommandText = String.Format("INSERT INTO {0} ({1}) VALUES ({2})",tableName, columnNames, parameterNames);
                                                            // INSERT INTO classes (class_name, cost) VALUES (@class_name, @cost )
                        foreach (var column in columns)
                        {
                            command.Parameters.AddWithValue("@" + column.Key, column.Value);
                        }

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception )
            {
                MessageBox.Show("خطأ في عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }

        public static void DeleteRecord(string tableName, string keyColumn, string keyValue) // delete record
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = String.Format("DELETE FROM {0} WHERE {1} = @keyValue", tableName, keyColumn);
                        command.Parameters.AddWithValue("@keyValue", keyValue);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception )
            {
                MessageBox.Show("خطأ في عملية الحذف", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);

                
            }
        }
        public static void UpdateData(string tableName, Dictionary<string, object> columnsValues, string whereClause)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    // Construct the SET part of the query
                    var setParts = columnsValues.Select(kv => String.Format("{0} = @{0}",kv.Key));
                    string setClause = string.Join(", ", setParts);

                    // Construct the full query
                    string query = String.Format("UPDATE {0} SET {1} WHERE {2}",tableName, setClause, whereClause);

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        // Add parameters to the command
                        foreach (var kv in columnsValues)
                        {
                            command.Parameters.AddWithValue(String.Format("@{0}",kv.Key), kv.Value);
                        }

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();
                        //return rowsAffected > 0; // Return true if at least one row was updated
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("خطأ في عملية الادخال", " خطأ ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public static List<object> SelectData(string tableName, string column, string whereClause = "", string additionalInfo = "")
        {
           List <object> data = new List<object>();
            try
            {

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string columnNames = string.Join(", ", column);
                    string query = String.Format("SELECT {0} FROM {1}",columnNames, tableName);

                    if (!string.IsNullOrEmpty(whereClause))
                    {
                        query += String.Format(" WHERE {0}", whereClause);
                    }
                    query += additionalInfo;
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                            
                                data.Add(reader.GetValue(0));
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception )
            {
               

                return data;
            }
        }

        public static DataTable GetUserData() // get user data in ألاعضاء
        {
            DataTable dataTable = new DataTable();
            try
            {

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"SELECT u.name, u.phone, f.faculty_name, j.job_name, u.level
                                 FROM users u
                                 LEFT JOIN faculties f ON u.faculty_id = f.id
                                 LEFT JOIN jobs j ON u.job_id = j.id";
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                    adapter.Fill(dataTable);
                }

                return dataTable;
            }
            catch (Exception )
            {
                return dataTable;
            }
        }

        public static DataTable GetOffersData(bool isExpired) // get user data in ألاعضاء
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"SELECT uo.user_id, u.name, o.offer_name, uo.start_date, uo.end_date, o.hours, uo.left_hours, o.cost, uo.is_expired
                                 FROM users u
                                 JOIN user_offer uo ON u.id= uo.user_id
                                 JOIN offers o ON o.id = uo.offer_id WHERE uo.is_expired = 0 ";
                    if (isExpired)
                    {
                        query += " OR uo.is_expired = 1";
                    }
               
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                    adapter.Fill(dataTable);
                }

                return dataTable;
            }
            catch (Exception  )
            {

                return dataTable;
            }
           
        }



        public static Dictionary<int, Tuple<string, string, int>> GetActiveUsers() // get active users data to fill grid of chairs
        {
            var activeUsers = new Dictionary<int, Tuple<string, string, int>>();
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT a.chair_num, u.name, a.enter_date, a.user_id FROM active_users a JOIN users u ON a.user_id = u.id";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int chairNum = reader.GetInt32(0);
                        string userName = reader.GetString(1);
                        string enterDate = reader.GetString(2);
                        int user_id = reader.GetInt32(3);
                        activeUsers.Add(chairNum, Tuple.Create(userName, enterDate, user_id));
                    }
                    reader.Close();
                }
                return activeUsers;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Tuple<string, string, int, string>> GetActiveClasses() // get active users data to fill grid of chairs
        {
            var activeClasses = new List<Tuple<string, string, int, string>>();
            try
            {

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"SELECT u.name, uc.enter_date, uc.user_id, c.class_name 
                                    FROM user_class uc
                                    JOIN users u ON uc.user_id = u.id
                                    JOIN classes c ON c.id = uc.class_id;";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                    
                        string userName = reader.GetString(0);
                        string enterDate = reader.GetString(1);
                        int user_id = reader.GetInt32(2);
                        string className = reader.GetString(3);
                        activeClasses.Add(Tuple.Create(userName, enterDate, user_id, className));
                    }
                    reader.Close();
                }

                return activeClasses;
            }
            catch (Exception )
            {
                return null;

            }
        }




        public static Tuple<string, string> GetUserDataByChairNum(int chairNum ) // gets data about user to fill logout window
         {
            string userName = string.Empty;
            string enterDate = string.Empty;

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();// افتكر عدد الساعات الباقية من الباقة
                    string query = @" SELECT 
                                    u.name, 
                                    a.enter_date
                                FROM 
                                    active_users a
                                JOIN 
                                    users u ON a.user_id = u.id    
                                WHERE 
                                    a.chair_num = @ChairNum";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ChairNum", chairNum);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userName = reader.GetString(0);
                                enterDate = reader.GetString(1);
                            
                            }
                        }
                    }
                }
            
                Tuple<string, string> data = new Tuple<string, string>(userName, enterDate);

                return data;

            }
            catch (Exception )
            {
                return null;

            }
        }




        public static int GetPriceByDuration(int hours) // gets the price based on the time the uer spent
        {
            int price = 0;
            try
            {

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT 
                        cost 
                    FROM 
                        prices 
                    WHERE 
                        @Duration >= from_date AND  @Duration < to_date
                    LIMIT 1";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Duration", hours);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                price = reader.GetInt32(0);
                            }
                        }
                    }
                }

                return price;
            }
            catch (Exception)
            {
                return 0;

            }
        }



        public static List<string> GetUserNames(string filter,string table, string col,string join="") // used to filter combo boxes when searching for existing users
        {

            List<string> userNames = new List<string>();
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = String.Format("SELECT {0} FROM {1} {2} WHERE {0} LIKE @filter LIMIT 5",col,table,join);
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@filter", String.Format("{0}%",filter));
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userNames.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                return userNames;

            }
            catch (Exception )
            {
                return null;
            }
        }


        public static DataTable GetUserRecords(DateTime fromDate, DateTime toDate) // get user data in ألاعضاء
        {
            DataTable dataTable = new DataTable();
            try
            {

               string query = @"SELECT u.name, u.phone, ur.type, ur.enter_date, ur.leave_date, ur.reservation_cost, ur.kitchen, ur.total, ur.paid
                                FROM users u
                                JOIN user_records ur ON u.id= ur.user_id
                                WHERE ur.leave_date >= @fromDate AND ur.leave_date<= @toDate
                               
                                UNION ALL

                                SELECT du.name, du.phone, ur.type, ur.enter_date, ur.leave_date, ur.reservation_cost, ur.kitchen, ur.total, ur.paid
                                FROM deleted_users du
                                JOIN user_records ur ON du.id = ur.user_id
                                WHERE ur.leave_date >= @fromDate AND ur.leave_date <= @toDate
                                ORDER BY ur.leave_date
                                ";

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fromDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));

                        using (var adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            
                return dataTable;
            }
            catch (Exception )
            {
                MessageBox.Show("خطأ في تحميل بيانات الاعضاء");
                return dataTable;
            }
        }

        public static DataTable GetOffersData(DateTime fromDate, DateTime toDate) 
        {
            DataTable dataTable = new DataTable();
            try
            {


                string query = @"SELECT u.name, o.offer_name, uo.start_date, o.cost
                                FROM users u
                                JOIN user_offer uo ON u.id= uo.user_id
                                JOIN offers o ON o.id = uo.offer_id 
                                WHERE uo.start_date >= @fromDate AND uo.start_date <= @toDate ";
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fromDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));

                        using (var adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception )
            {
                return dataTable;
            }
          
        }


        public static DataTable GetKitchenData(DateTime fromDate, DateTime toDate)
        {
            DataTable dataTable = new DataTable();
            try
            {

                string query = @" SELECT combined.product_name, SUM(combined.amount) as total_amount, SUM(combined.cost) as total_cost
                                FROM (
                                    SELECT k.product_name, uk.amount, uk.cost
                                    FROM kitchen k
                                    JOIN user_kitchen uk ON k.id = uk.product_id
                                    WHERE uk.leave_date >= @fromDate AND uk.leave_date <= @toDate
    
                                    UNION ALL
    
                                    SELECT dk.product_name, uk.amount, uk.cost
                                    FROM deleted_kitchen dk
                                    JOIN user_kitchen uk ON dk.id = uk.product_id
                                    WHERE uk.leave_date >= @fromDate AND uk.leave_date <= @toDate
                                ) combined
                                GROUP BY combined.product_name
                                ORDER BY combined.product_name";

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fromDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));

                        using (var adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }

                return dataTable;
            }
            catch (Exception)
            {
                MessageBox.Show("خطأ في تحميل بيانات البوفيه");
                return dataTable;
                
            }
        }

        public static List<Tuple<int, string, int, int, int, string, int>> GetUserIdWithOffers()
        {
            List<Tuple<int, string, int, int, int, string, int>> data = new List<Tuple<int, string, int, int, int, string, int>>();
            try
            {

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string query = String.Format(@"SELECT uo.user_id, au.enter_date, au.last_hour, uo.left_hours, uo.spent_hours, uo.end_date , o.hours FROM user_offer uo
                                                  LEFT JOIN active_users au on uo.user_id = au.user_id 
                                                  JOIN offers o ON uo.offer_id = o.id
                                                  WHERE uo.is_expired = 0");

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int user_id = reader.GetInt32(0);
                                string enter_date = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                int last_hour = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                                int left_hours = reader.GetInt32(3);
                                int spent_hours = reader.GetInt32(4);
                                string end_date = reader.GetString(5);
                                int offerHours = reader.GetInt32(6);
                                data.Add(Tuple.Create(user_id, enter_date, last_hour, left_hours, spent_hours, end_date, offerHours));
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
            
        }

        public static List<Tuple<int, string, int, double, string, double>> GetProductsData(string product_type = "")
        {
            List<Tuple<int, string, int, double, string, double>> data = new List<Tuple<int, string, int, double, string, double>>();
            try
            {

                int productId = new int();
                string productName= string.Empty;
                int amount = new int();
                double cost = new double();
                string product_t;

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();// افتكر عدد الساعات الباقية من الباقة
                    string query = @" SELECT 
                                        id,
                                        product_name, 
                                        amount,
                                        sell_cost,
                                        product_type,
                                        purchase_cost
                                    FROM 
                                        kitchen ";
                    if (!string.IsNullOrEmpty(product_type))
                    {
                        query += " WHERE product_type = @product_type ";
                    }
                   
                    query += " ORDER BY id";

                    
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(product_type))
                        {
                            command.Parameters.AddWithValue("@product_type", product_type);

                        }
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productId = reader.GetInt32(0);
                                productName = reader.GetString(1);
                                amount = reader.GetInt32(2);
                                cost = reader.GetDouble(3);
                                product_t = reader.GetString(4);
                                double purchaseCost = reader.GetDouble(5);
                                data.Add(Tuple.Create(productId, productName, amount, cost,product_t, purchaseCost));
                            }
                        }
                    }
                }


                return data;
            }
            catch (Exception )
            {
                return null;
            }
            
        }


        public static Tuple<string,int, double, double,string> GetProductData(int id) // gets data about user to fill logout window
        {
            try
            {

                Tuple<string, int, double, double,string> data = null;
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();// افتكر عدد الساعات الباقية من الباقة
                    string query = @" SELECT 
                                    product_name, 
                                    amount,
                                    purchase_cost,
                                    sell_cost,
                                    product_type
                                FROM 
                                    kitchen
                                WHERE 
                                    id = @id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string productName = reader.GetString(0);
                                int amount = reader.GetInt32(1);
                                double purchaseCost = reader.GetDouble(2);
                                double sellCost = reader.GetDouble(3);
                                string product_t = reader.GetString(4);

                                data = new Tuple<string, int, double, double,string>(productName, amount, purchaseCost, sellCost, product_t);

                            }
                        }
                    }
                }


                return data;
            }
            catch (Exception )
            {
                return null;
            }
           
        }


        public static Dictionary<int, int> GetProductsDictionary(int userId)
        {
            Dictionary<int, int> data = new Dictionary<int, int>();

            try
            {

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();// افتكر عدد الساعات الباقية من الباقة
                    string query = @" SELECT 
                                        product_id,                                     
                                        amount
                                    FROM 
                                        user_kitchen
                                    WHERE user_id = @user_id AND is_logged_out = 0";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int productId = reader.GetInt32(0);
                                int amount = reader.GetInt32(1);
                                data[productId] = amount;
                            }
                        }
                    }
                }
                return data;
            }
            catch (Exception )
            {
                return null;
            }
            
        }


        public static Dictionary<string, object> AddDeletedRecord(int tableId, string table)
        {
            Dictionary<string, object> data = null;

            try
            {

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();// افتكر عدد الساعات الباقية من الباقة
                    string query = String.Format(" SELECT  * FROM {0} WHERE id = @id", table);

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", tableId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (table == "users")
                                {
                                    int id = reader.GetInt32(0);
                                    string name = reader.GetString(1);
                                    string phone = reader.GetString(2);
                                    int fId = reader.GetInt32(3);
                                    int jId = reader.GetInt32(4);
                                    string level = reader.GetString(5);

                                    data = new Dictionary<string, object> {
                                        {"id", id},
                                        {"name", name},
                                        {"phone", phone},
                                        {"faculty_id", fId},
                                        {"job_id", jId},
                                        {"level", level},
                                   
                                    };
                                }
                                else
                                {
                                    int id = reader.GetInt32(0);
                                    string name = reader.GetString(1);
                                    int amount= reader.GetInt32(2);
                                    double purchase = reader.GetDouble(3);
                                    double sell = reader.GetDouble(4);
                                    string type = reader.GetString(5);
                                    data = new Dictionary<string, object> {
                                        {"id", id},
                                        {"product_name", name},
                                        {"amount", amount},
                                        {"purchase_cost", purchase},
                                        {"sell_cost", sell},
                                        {"product_type", type},
                                   
                                    };
                                }

                               
                            }
                        }
                    }
                }
                return data;
              
            }
            catch (Exception)
            {
                return data;
            }

        }

        public static Dictionary<string,Tuple<double,double>> getprofit(List<string> names)
        {
            Dictionary<string, Tuple<double, double>> data = new Dictionary<string, Tuple<double, double>>();
            double purchase = 0 , sell = 0  ;
            string name;
            
            try
            {

                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string productNamesForQuery = string.Join("','", names);

                    string query = String.Format("Select purchase_cost , sell_cost ,product_name FROM kitchen WHERE product_name IN ('{0}')", productNamesForQuery);
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                purchase=reader.GetDouble(0);
                                sell=reader.GetDouble(1);
                                name = reader.GetString(2);
                                data[name] = (Tuple.Create(purchase,sell));
                            }
                        }
                    }
                }

                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
