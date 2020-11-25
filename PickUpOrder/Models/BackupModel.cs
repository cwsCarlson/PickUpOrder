using System;
using System.Data.SqlClient;
using System.IO;

namespace PickUpOrder.Models
{
    public class BackupModel
    {
        private static DateTime lastBackup;

        // MakeBackup - If lastBackup > 7 days ago, make a backup.
        public static void MakeBackup()
        {
            // If there is a backup and it was too recent, leave.
            if (lastBackup != null &&
                lastBackup.CompareTo(DateTime.Today.AddDays(-7)) >= 0)
                return;

            // Open a database connection.
            var conn = new SqlConnection("Data Source=(localdb)\\" +
                                         "ProjectsV13;" +
                                         "Initial Catalog=PickUpOrderDB;" +
                                         "Integrated Security=True;" +
                                         "Pooling=False;" +
                                         "Connect Timeout=30");
            conn.Open();

            // Save the current day and convert it to a string.
            lastBackup = DateTime.Today;
            var dateStr =
                lastBackup.Year + "-" + lastBackup.Month +
                "-" + lastBackup.Day;

            // Create the save location if it does not yet exist.
            Directory.CreateDirectory("C:\\PickUpOrderDB");

            // Create a backup command and run it.
            var sqlCommand = new SqlCommand
                { CommandText = "BACKUP DATABASE @DBName TO DISK = @PATH" };
            sqlCommand.Parameters.AddWithValue("@DBName", "PickUpOrderDB");
            sqlCommand.Parameters.AddWithValue("@PATH", "C:\\PickUpOrderDB\\" +
                                               "Backup-" + dateStr + ".bak");
            sqlCommand.Connection = conn;
            sqlCommand.ExecuteNonQuery();

            // Close the connection.
            conn.Close();
        }
    }
}