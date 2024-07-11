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

         public static DataTable LoadData(string tableName)
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


        public static void InsertRecord(string tableName, Dictionary<string, object> columns)
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

        public static void DeleteRecord(string tableName, string keyColumn, string keyValue)
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


    //     public static DataTable SelectData(string tableName, List<string> columns, string whereClause = "")
    //{
    //    DataTable dataTable = new DataTable();

    //    using (var connection = new SQLiteConnection(_connectionString))
    //    {
    //        connection.Open();
    //        string columnNames = string.Join(", ", columns);
    //        string query = String.Format("SELECT {0} FROM {1}",columnNames,tableName);

    //        if (!string.IsNullOrEmpty(whereClause))
    //        {
    //            query += String.Format(" WHERE {0}",whereClause);
    //        }

    //        using (var command = new SQLiteCommand(query, connection))
    //        {
    //            using (var adapter = new SQLiteDataAdapter(command))
    //            {
    //                adapter.Fill(dataTable);
    //            }
    //        }
    //    }

    //    return dataTable;
    //}


    }
}
