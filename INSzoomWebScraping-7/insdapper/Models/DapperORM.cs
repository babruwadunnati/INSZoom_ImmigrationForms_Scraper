using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;


namespace insdapper.Models
{
    public class DapperORM
    {
        //Connection Establishment
        static string strConnectionString = @"Data Source=DESKTOP-35MGC6G\UNNATISQL;Initial Catalog=InsZoom;User ID=sa;Password=UNNATI";
        //Function to the Procedure which inserts Label into the database
        public static int AddLabel(Label Labeldata)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page_id", Labeldata.Page_id);
                parameters.Add("@Label_answer_id", Labeldata.Label_answer_id);
                parameters.Add("@Label_text", Labeldata.Label_text);
                parameters.Add("@Formid", Labeldata.Formid);


                rowAffected = con.Execute("InsertLabel", parameters, commandType: CommandType.StoredProcedure);
            }

            return rowAffected;
        }

        public static int AddOption(Option answer)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page_id", answer.Page_id);
                parameters.Add("@Label_answer_id", answer.Label_answer_id);
                parameters.Add("@Option_text", answer.Option_text);
                parameters.Add("@Option_value", answer.Option_value);
                parameters.Add("@Formid", answer.Formid);

                rowAffected = con.Execute("InsertOption", parameters, commandType: CommandType.StoredProcedure);
            }

            return rowAffected;
        }

        public static Label CheckAdd(Label Labeldata)
        {
            Label count = new Label();
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

            
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page_id", Labeldata.Page_id);
                parameters.Add("@Label_answer_id", Labeldata.Label_answer_id);
                parameters.Add("@Label_text", Labeldata.Label_text);
                parameters.Add("@Formid", Labeldata.Formid);

                count =con.Query<Label>("CheckAdd", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            
            System.Diagnostics.Debug.WriteLine(count);
            return count;
        }


       public static Int32 UpdateLabelText(Label Labeldata)
       {
           String flag;

            int retflag = -1 ;
           using (SqlConnection con = new SqlConnection(strConnectionString))
           {
               if (con.State == ConnectionState.Closed)
                   con.Open();

               SqlCommand abc = new SqlCommand("SELECT dbo.UpdateLabelText(@Label_answer_id,@Page_id,@Formid)", con);
               SqlParameter labelid = new SqlParameter("@Label_answer_id", Labeldata.Label_answer_id);
                SqlParameter pageid = new SqlParameter("@Page_id", Labeldata.Page_id);
                SqlParameter formid = new SqlParameter("@Formid", Labeldata.Formid);

                abc.Parameters.Add(labelid);
                abc.Parameters.Add(pageid);
                abc.Parameters.Add(formid);

                if (abc.ExecuteScalar() != DBNull.Value)
                    flag = (String)abc.ExecuteScalar();
                else
                    return 2;

                //Compare
                flag=flag.Trim();
                Labeldata.Label_text = Labeldata.Label_text.Trim();
                if (string.Compare(Labeldata.Label_text,flag) == 0)
                {
                    retflag = 0;
                    System.Console.WriteLine("RETT:" + retflag);
                }
                else
                {
                    retflag = 1;
                }
           }

           System.Diagnostics.Debug.WriteLine("Flag:------"+flag+"Retflag:"+retflag);
            System.Console.WriteLine("RETTout:" + retflag);
            //  System.Diagnostics.Debug.WriteLine(flag);
            return retflag;
       }

        public static int AddUpdatedLabel(Label Labeldata)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page_id", Labeldata.Page_id);
                parameters.Add("@Label_answer_id", Labeldata.Label_answer_id);
                parameters.Add("@Label_text", Labeldata.Label_text);
                parameters.Add("@Formid", Labeldata.Formid);


                rowAffected = con.Execute("UlabelText", parameters, commandType: CommandType.StoredProcedure);
            }

            return rowAffected;
        }


        public static Option CheckAddOption(Option answer)
        {
            Option count = new Option();
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();


                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page_id", answer.Page_id);
                parameters.Add("@Label_answer_id", answer.Label_answer_id);
                parameters.Add("@Option_text", answer.Option_text);
                parameters.Add("@Option_value", answer.Option_value);
                parameters.Add("@Formid", answer.Formid);



                count = con.Query<Option>("CheckAddOption", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
          
            System.Diagnostics.Debug.WriteLine(count);
            return count;
        }

        public static int ChangeAdd(CheckAdd Labeldata)
        {
            int rowAffected = 0;
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@New_id", Labeldata.New_id);
                parameters.Add("@Change_type", Labeldata.Change_type);
                parameters.Add("@Old_id", Labeldata.Old_id);
                parameters.Add("@Page_id", Labeldata.Page_id);
                parameters.Add("@Change_time", Labeldata.Change_time);
                parameters.Add("@Option_value", Labeldata.Option_value);
                parameters.Add("@Formid", Labeldata.Formid);
                rowAffected = con.Execute("ChangeAdd", parameters, commandType: CommandType.StoredProcedure);
            }

            return rowAffected;
        }

        public static IEnumerable<T>Return_list<T>(String Procedurename,DynamicParameters parameters=null)
        {

            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                return con.Query<T>(Procedurename, parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public static List<Label> List_With_Pageid<Label>(String Page_id)
        {

            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page_id",Page_id);
                return con.Query< Label>("PageList", parameters, commandType: CommandType.StoredProcedure).ToList();
            }

        }

        public static List<FormUrlData> url<FormUrlData>()
        {
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                return con.Query< FormUrlData > ("FetchUrl", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static List<CrawlerInstruction> instruction<CrawlerInstruction>(int FormId)
        {
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@FormId", FormId);
                return con.Query<CrawlerInstruction>("FetchInst", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static void Delete(Label Labeldata)
        {
            Label count = new Label();
            using (IDbConnection con = new SqlConnection(strConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();


                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page_id", Labeldata.Page_id);
                parameters.Add("@Label_answer_id", Labeldata.Label_answer_id);
                parameters.Add("@Formid", Labeldata.Formid);


                con.Execute("Delete", parameters, commandType: CommandType.StoredProcedure);
            }
            System.Diagnostics.Debug.WriteLine("Delete");
            System.Diagnostics.Debug.WriteLine(count);
        }



    }
}
