using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Blood_Bank
{
    public static class DbConnection
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BloodBankDbConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }

        public static void EnsureDateColumns()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    string[] alters = new string[]
                    {
                        "ALTER TABLE DonorTbl ADD DDate DATE DEFAULT GETDATE()",
                        "ALTER TABLE PatientTbl ADD PDate DATE DEFAULT GETDATE()",
                        "ALTER TABLE TransferTbl ADD TDate DATE DEFAULT GETDATE()"
                    };
                    foreach (string sql in alters)
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                                cmd.ExecuteNonQuery();
                        }
                        catch
                        {
                            // Column may already exist — ignore
                        }
                    }
                    string[] updates = new string[]
                    {
                        "UPDATE DonorTbl SET DDate = DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETDATE()) WHERE DDate IS NULL",
                        "UPDATE PatientTbl SET PDate = DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETDATE()) WHERE PDate IS NULL",
                        "UPDATE TransferTbl SET TDate = DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETDATE()) WHERE TDate IS NULL"
                    };
                    foreach (string sql in updates)
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                            cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // Migration failed silently — tables may not exist yet
            }
        }
    }
}
