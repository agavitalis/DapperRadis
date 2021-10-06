using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PrepApp.Interface
{
    public class DapperrRepo : IDapperr
    {
        private IDbConnection GetConnection()
        {
            // for SQLite connections
            // install Microsoft.Data.Sqlite nuget package
            // return new SqliteConnection("Data Source=app.db");

            // for MySql connections
            // install MySql.Data nuget package
            // return new MySqlConnection("Server=localhost;Port=3306;Uid=admin;Password=Abcd$1234;Database=mydb");

            // for SQL Server connections
            // install Microsoft.Data.SqlClient nuget package
            return new SqlConnection("Server=localhost;User name=admin;Password=Abcd$1234;Initial Catalog=mydb");
        }

        public int Execute(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            int result;

            // get connection
            using IDbConnection connection = GetConnection();

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            try
            {
                // start transaction
                using var transaction = connection.BeginTransaction();

                try
                {
                    // execute command within transaction
                    result = connection.Execute(sql, dp, transaction);

                    // commit changes
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // rollback if exception
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // finally close connection
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return result;
        }

        public IEnumerable<T> Query<T>(string sql, DynamicParameters dp, CommandType commandType = CommandType.Text)
        {
            IEnumerable<T> result;

            using IDbConnection db = GetConnection();

            if (db.State == ConnectionState.Closed)
                db.Open();

            try
            {
                // start transaction
                using var transaction = db.BeginTransaction();
                try
                {
                    // execute query within transaction
                    result = db.Query<T>(sql, dp, commandType: commandType, transaction: transaction);

                    // commit changes
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // rollback if exception
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }
    }
}
