using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DBConnection
/// </summary>
public class DBConnection
{
    static string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;

    public DBConnection()
    {
    }

    public static SqlConnection GetSqlConnection()
    {
        return new SqlConnection(connectionString);
    }

    public static int ExecuteStatement(SqlConnection connection, string sql)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();

        try
        {
            var command = connection.CreateCommand();
            command.CommandText = sql;

            int rows = command.ExecuteNonQuery();

            connection.Close();

            return rows;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }

    public static SqlDataReader ExecuteReader(SqlConnection connection, SqlCommand command)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();
        else
        {
            connection.Close();
            connection.Open();
        }

        return (SqlDataReader)command.ExecuteReader();
    }

    public static SqlDataReader ExecuteReader(SqlConnection connection, string sql)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();
        else
        {
            connection.Close();
            connection.Open();
        }

        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = sql;
            return (SqlDataReader)command.ExecuteReader();
        }
    }

    public static DataSet ExecuteDataSet(SqlConnection connection, string sql)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();

        SqlCommand command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandTimeout = 6000;

        SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
        DataSet ds = new DataSet();
        adapter.Fill(ds);

        connection.Close();

        return ds;
    }

    public static DataSet ExecuteDataSet(string sql)
    {
        return ExecuteDataSet(DBConnection.GetSqlConnection(), sql);
    }

    public static string ExecuteStatementWithReturn(SqlConnection connection, string sql)
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();

        SqlCommand command = connection.CreateCommand();
        command.CommandText = sql;

        SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            reader.Read();
            string val = reader.GetValue(0).ToString();

            reader.Close();
            connection.Close();

            return val;
        }
        else
            return null;

        //reader.Close();
        //connection.Close();
    }

    public static object ExecuteStatementWithReturn(string sql)
    {
        return ExecuteStatementWithReturn(GetSqlConnection(), sql);
    }

    public static void ExecuteStatement(string sql)
    {
        ExecuteStatement(GetSqlConnection(), sql);
    }

    public static SqlDataReader ExecuteReader(string sql)
    {
        return ExecuteReader(GetSqlConnection(), sql);
    }

    public static void ExecuteCommand(SqlCommand cmd)
    {
        if (cmd == null)
            return;

        using (SqlConnection connection = GetSqlConnection())
        {
            cmd.Connection = connection;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public static int InsertCommand(SqlCommand cmd)
    {
        int val = 0;
        if (cmd == null)
        {
            return val;
        }

        using (SqlConnection connection = GetSqlConnection())
        {
            cmd.Connection = connection;
            cmd.Connection.Open();
            val = (int)cmd.ExecuteScalar();
        }
        return val;
    }
}
