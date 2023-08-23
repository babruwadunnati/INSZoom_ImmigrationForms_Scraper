using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Dapper;

namespace ZoomScraper
{
    public class DapperORM
    { static string strConnectionString = @"Data Source=DESKTOP-35MGC6G\UNNATISQL;Initial Catalog=InsZoom;User ID=sa;Password=UNNATI";
        
        public static IEnumerable<T> GetDetails<T>(string Procedure, DynamicParameters param = null)
        {
            // List<T> list = new List<T>();

            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                return con.Query<T>(Procedure, param, commandType: CommandType.StoredProcedure);
            }


        }

        public static void ExecuteWithoutReturn(string Procedure, DynamicParameters param = null)
        {
            // List<T> list = new List<T>();

            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                con.Execute(Procedure, param, commandType: CommandType.StoredProcedure);
            }


        }

        //private static string ConnectionString = @"Data Source=DESKTOP-9TNQL6V\SAGAR;Initial Catalog=InsZoom;User ID=sa;Password=sagar";

       
        public static T ExecuteReturnScalar<T>(string ProcedureName, DynamicParameters parameters)
        {
            using (SqlConnection sqlCon = new SqlConnection(strConnectionString))
            {
                sqlCon.Open();
                return (T)Convert.ChangeType(sqlCon.Execute(ProcedureName, parameters, commandType: CommandType.StoredProcedure), typeof(T));
            }
        }

        //Dapper ORM  ReturnList
        public static IEnumerable<T> ReutrnList<T>(String ProcedureName, DynamicParameters parameters = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(strConnectionString))
            {
                sqlCon.Open();
                return sqlCon.Query<T>(ProcedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public static IEnumerable<T> ReturnChange<T>(String ProcedureName, DynamicParameters parameters = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(strConnectionString))
            {
                sqlCon.Open();
                return sqlCon.Query<T>(ProcedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }



    }
}
