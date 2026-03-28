using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


public class ClsCategoriesDB  
    {


        public static DataTable GetAllCategories()
        {
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            DataTable AllApplication = new DataTable();

            string StoredProcedure = "GetAllCategories";

            using (SqlCommand command = new SqlCommand(StoredProcedure, connection))
            {
                
                
                command.CommandType = CommandType.StoredProcedure;


                try
                {
                    connection.Open();
                    
                    SqlDataReader Reader = command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        AllApplication.Load(Reader);
                    }


                }
                catch (Exception EX)
                {
                   clsMain.ShowLogEvent(EX.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                }

            }

            return AllApplication;

        }



    }

        public static int? InsertCategory(string Name,string Color)
        {
            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            {

                int? ID = null;

                string StoredP = "InsertCategories";

                using (SqlCommand command = new SqlCommand(StoredP, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name",Name );
                    command.Parameters.AddWithValue("@Color", Color);
                   

                    try
                    {
                        connection.Open();
                        object Result = command.ExecuteScalar();

                        if (Result == null)
                            ID = null;
                        else
                            ID = Convert.ToInt32(Result);

                    }
                    catch (Exception ex)
                    {
                    clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                }

                }


                return ID;
            }
        }

        public static int DeleteCategory(int ID)
        {
            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            {

            string StoredPro = "DeleteCategories";

                int rowAffected = 0;

                using (SqlCommand command = new SqlCommand(StoredPro, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;    
                    command.Parameters.AddWithValue("@ID", ID);

                    try
                    {
                        connection.Open();

                        rowAffected = command.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                    clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                }
                }
                return rowAffected;
            }

        }

        public static int UpdateCategory(int ID, string CategoryName ,string colore)
    {
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            int rowAffected = 0;

            string StoredPro = "UpdateCategories";

            using (SqlCommand command = new SqlCommand(StoredPro, connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", ID);
                command.Parameters.AddWithValue("@Color", colore);
                command.Parameters.AddWithValue("@Name", CategoryName);

                 


                try
                {
                    connection.Open();
                    rowAffected = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                }

                return rowAffected;

            }

        }
    }

        public static bool GetCategoryByID(int ID,out string CategoryName, out string Color)
    {
        bool isfound = false;

         CategoryName = string.Empty;
             Color  = string.Empty;
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {

            string cmdtext = "GetOneCategoriesByID";

            using (SqlCommand command = new SqlCommand(cmdtext, connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;


                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {

                            CategoryName = (string)reader["Name"];
                            Color = (string)reader["color"];
                           

                            isfound = true;

                        }

                    }

                }
                catch (Exception ex)
                {
                    clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                }

                return isfound;

            }
        }
    }

        public static bool CheckIsCategoryExsistByName(string Name, out int? ID)
        {

             ID = null;

            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            {
                using (SqlCommand command = new SqlCommand("CheckCategoryExisitByName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserName", Name);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            ID = insertedID;
                            return true;
                        }

                    }

                    catch (Exception ex)
                    {
                        clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                    }
                }
            }
        return false;

        }

}

