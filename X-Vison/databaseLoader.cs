using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;

namespace Center_Maneger
{
    public static class databaseLoader
    {
        private static string _connectionString = "Data Source=database.db;Version=3;";

         public static DataTable LoadData(string tableName) // used to fill tabs in settings
        {
            string query = String.Format("SELECT * FROM {0}",tableName);
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


        public static void InsertRecord(string tableName, Dictionary<string, object> columns) // insert new record
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

                    command.ExecuteNonQuery();///////////////    /erorr/     ///////// 
                }
            }
        }

        public static void DeleteRecord(string tableName, string keyColumn, string keyValue) // delete record
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


        public static List<object> SelectData(string tableName, string column, string whereClause = "")
        {
           List <object> data = new List<object>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string columnNames = string.Join(", ", column);
                string query = String.Format("SELECT {0} FROM {1}",columnNames, tableName);

                if (!string.IsNullOrEmpty(whereClause))
                {
                    query += String.Format(" WHERE {0}", whereClause);
                }

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

        public static DataTable GetUserData() // get user data in ألاعضاء
        {
            DataTable dataTable = new DataTable();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = @"SELECT u.name, u.phone, f.faculty_name, j.job_name, level
                             FROM users u
                             JOIN faculties f ON u.faculty_id = f.id
                             JOIN jobs j ON u.job_id = j.id";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                adapter.Fill(dataTable);
            }

            return dataTable;
        }




        public static Dictionary<int, Tuple<string, string, int>> GetActiveUsers() // get active users data to fill grid of chairs
        {
            var activeUsers = new Dictionary<int, Tuple<string, string, int>>();

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



        public static Tuple<string, string, bool> GetUserDataByChairNum(int chairNum) // gets data about user to fill logout window
         {
            string userName = string.Empty;
            string enterDate = string.Empty;
            bool hasActiveOffer = false;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();// افتكر عدد الساعات الباقية من الباقة
                string query = @"
                    SELECT 
                        u.name, 
                        a.enter_date,
                        CASE WHEN uo.user_id IS NOT NULL THEN 1 ELSE 0 END AS HasActiveOffer
                    FROM 
                        active_users a
                    JOIN 
                        users u ON a.user_id = u.id
                    LEFT JOIN 
                        user_offer uo ON u.id = uo.user_id AND uo.start_date <= CURRENT_DATE AND uo.end_date >= CURRENT_DATE
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
                            hasActiveOffer = reader.GetInt32(2) == 1;
                        }
                    }
                }
            }
            Tuple<string, string, bool> data = new Tuple<string, string, bool>(userName, enterDate, hasActiveOffer);

            return data;
        }




        public static int GetPriceByDuration(int hours) // gets the price based on the time the uer spent
        {
            int price = 0;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                SELECT 
                    cost 
                FROM 
                    prices 
                WHERE 
                    @Duration BETWEEN from_date AND to_date
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



        public static List<string> GetUserNames(string filter, string col) // used to filter combo boxes when searching for existing users
    {
        List<string> userNames = new List<string>();
        
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = String.Format("SELECT {0} FROM users WHERE {0} LIKE @filter LIMIT 5",col);
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

    }
}
