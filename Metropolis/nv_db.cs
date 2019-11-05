using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SQLite;
using System.Configuration;
using System.Windows.Forms;   // только ради messagebox - потом надо убрать. сделать невизуально
 

namespace Metropolis
{
    static public class prvCommon
    {
        public enum     DBName : int { MySQL, SQLite, Oracle };
        static public   DBName curDB = DBName.SQLite;  // текущая БД, ее надо устанавливать в своей программе при старте
        static public DbConnection f_GetDBConnection(DBName ai_DBName)
        {
            //    DbConnection conn = new DbConnection();

            // чтобы не подкл.доп.using, пока во всех case стоит  SQLiteConnection
            switch (ai_DBName)
            {
                case DBName.MySQL:          // BaseName.MySQL:
                    SQLiteConnection connMySQL = new SQLiteConnection();
                    return connMySQL;
                case DBName.SQLite:    // BaseName.SQLite:
                           // получаем строку подключения
                    string connectionString = ConfigurationManager.ConnectionStrings["SQLiteConn"].ConnectionString;
                    SQLiteConnection sqlite_conn = new SQLiteConnection(connectionString);
                    //                        SQLiteConnection connSQLite = new SQLiteConnection();
                    // create a new database connection:
                    if (sqlite_conn != null)
                    {
                        try
                        {
                            sqlite_conn.Open();     // Открываем подключение
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                    return sqlite_conn;
                case DBName.Oracle:     //  BaseName.Oracle:
                    SQLiteConnection connOracle = new SQLiteConnection();
                    return connOracle;
                default:
                    return null;
            }
        }
        static public DbDataAdapter f_GetDBAdapter(DBName ai_DBName, string as_Select, DbConnection l_con)
        {
            switch (ai_DBName)
            {
                case DBName.MySQL:          // BaseName.MySQL:

                    return null;
                case DBName.SQLite:    // BaseName.SQLite:
                    SQLiteDataAdapter l_adapterSQLite = new SQLiteDataAdapter(as_Select, l_con as SQLiteConnection);
                    return l_adapterSQLite;
                default:
                    return null;
            }
        }
        static public DbCommand f_GetSQLCommandVar(DBName ai_DBName, DbConnection l_con)
        {
            switch (ai_DBName)
            {
                case DBName.MySQL:          // BaseName.MySQL:

                    return null;
                case DBName.SQLite:    // BaseName.SQLite:
                    SQLiteCommand SQLiteCmd = new SQLiteCommand(l_con as SQLiteConnection);
                    return SQLiteCmd;
                default:
                    return null;
            }
        }
        static public DbDataReader f_GetDataReader(DBName ai_DBName, DbCommand a_cmd, DbConnection l_con)
        {
            switch (ai_DBName)
            {
                case DBName.MySQL:          // BaseName.MySQL:

                    return null;
                case DBName.SQLite:    // BaseName.SQLite:
                    SQLiteDataReader rd = (a_cmd as SQLiteCommand).ExecuteReader();
                    return rd;
                default:
                    return null;
            }
        }
        
        static public int f_AddParm(DBName ai_DBName, DbCommand a_cmd, string as_parmName, object aob_parmValue, string as_DbType)
        {
            switch (ai_DBName)
            {
                case DBName.MySQL:          // BaseName.MySQL:

                    return 0;
                case DBName.SQLite:    // BaseName.SQLite:
                    if (as_DbType.ToUpper() == "INT" || as_DbType.ToUpper() == "INT32" )   
                            (a_cmd as SQLiteCommand).Parameters.AddWithValue(as_parmName,  Convert.ToInt32(aob_parmValue));
                    else if (as_DbType.ToUpper() == "STRING")
                        (a_cmd as SQLiteCommand).Parameters.AddWithValue(as_parmName, Convert.ToString(aob_parmValue) );
                    return 1;
            }
            return 1;
        }
    }

    /*
public class f_SQLite
    {
        public SQLiteConnection ConnDB(string as_DBName)
        {
            int li_rc = 0;
            // create a new database connection:
            SQLiteConnection parm_sqlite_conn =
            new SQLiteConnection("Data Source=" + as_DBName + ";Version=3;");
            // create a new database connection:
            // SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=:memory:;Version=3;");
            // The above statement creates a SQLite database in-memory.Each in-memory database instance is unique and ceases to exist when the connection is closed.
            // if (parm_sqlite_conn != null) li_rc = 1;
            return parm_sqlite_conn;
        }
    }
    */
}
