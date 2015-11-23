using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.ServiceProcess;

namespace NetPrint_LpdQueueError
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "NetPrint LpdQueueError Remover";

            string dbc = @"Data Source=" + Properties.Settings.Default.dbNetPath;
            Console.WriteLine(Properties.Settings.Default.dbNetPath);

            using (SQLiteConnection conn = new SQLiteConnection(dbc))
            {
                try { conn.Open(); }
                catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }

                SQLiteCommand cmd = new SQLiteCommand(conn);

                // delete all records in table
                try
                {
                    cmd.CommandText = "\r\ndelete from LpdQueueErrorTable where FileNumber > 0";
                    Console.WriteLine(cmd.CommandText);
                    cmd.ExecuteScalar();
                }
                catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }

                // show all records in table
                string sql = "select * from LpdQueueErrorTable";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    int count = 0;
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("Index: " + reader[0] + "\tName: " + reader[1] + "\tFileNumber: " + reader[2]);
                        count++;
                    }
                    Console.WriteLine("\r\nTotal records: " + count);
                }

                // close connection
                try { conn.Close(); }
                catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }

                // create 4 second pause
                int pauseTime = 4000;
                System.Threading.Thread.Sleep(pauseTime);
            }
        }
    }
}
