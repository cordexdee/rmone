using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using uGovernIT.DAL.Infratructure;
using Microsoft.EntityFrameworkCore;

namespace uGovernIT.DAL
{
    public class DBHelper<T> where T: class
    {   
        private string _query;
        private SqlCommand _cmd;
        private string connectionString = ConfigurationManager.ConnectionStrings["cnn"].ToString();
        public DBHelper(string query)
        {
            _query = query;          
        }
        //Changes Made By Mayank Singh
        public DBHelper(SqlCommand cmd)
        {
            _cmd = cmd;
        }

        public IEnumerable<T> GetData()
        {
            CustomDbContext context = new CustomDbContext(connectionString);
            using (DatabaseContext ctx = new DatabaseContext(context))
            {
                try
                {
                                     
                    IEnumerable<T> result;
                    string query = _query;
                    result = ctx.Set<T>().FromSql(query);
                    if (result == null)
                        return Enumerable.Empty<T>();
                    else
                        return result;
                }
                catch (Exception ex)
                {
                    return Enumerable.Empty<T>();
                }
            }
        }

        public int? UpdateOrInsertData(T t)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                  
                    string query = _query;

                    // return con.Execute(query, t);
                    return 1;
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    return null;
                }
            }
        }
        public int? ExecuteCommand(T t)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = _cmd;
                cmd.Connection = connection;
                cmd.Connection.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        public int? ExecuteCommandDP(T t)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                //int rowsAffected = db.Execute(_cmd.ToString());
                return 1;
                
            }

        }
    }

}
