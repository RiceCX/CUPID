using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace CUPID {
    public class DatabaseCache {
        public SQLiteConnection m_dbConnection;
        public DatabaseCache() {
            if (!File.Exists("cache.sqlite")) {
                SQLiteConnection.CreateFile("cache.sqlite");
                m_dbConnection = new SQLiteConnection("Data Source=cache.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "CREATE TABLE cache (date DATETIME, url VARCHAR(512), delURL VARCHAR(512), thumbnail VARCHAR(512))";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            } else {
                m_dbConnection = new SQLiteConnection("Data Source=cache.sqlite;Version=3;");
                m_dbConnection.Open();
            }
        }
        public void updateTables(string url, string delURL, string thumbnail) {
            string sql = "INSERT INTO cache (date, url, delURL, thumbnail) values (@DATE, @URL, @DELURL, @THUMBNAIL)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.Parameters.Add("@DATE", System.Data.DbType.DateTime);
            command.Parameters.Add("@URL", System.Data.DbType.String);
            command.Parameters.Add("@DELURL", System.Data.DbType.String);
            command.Parameters.Add("@THUMBNAIL", System.Data.DbType.String);
            command.Parameters["@DATE"].Value = DateTime.Now;
            command.Parameters["@URL"].Value = url;
            command.Parameters["@DELURL"].Value = delURL;
            command.Parameters["@THUMBNAIL"].Value = thumbnail;
            try {
                Int32 rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("RowsAffected: {0}", rowsAffected);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void deleteRow(string url) {
            throw new NotImplementedException();
        }
        ~DatabaseCache() {
            m_dbConnection.Close();
        }
    }
}
