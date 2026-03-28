using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static clsMain;


    public  static class ClsUsersDB
    {
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            {
                using (SqlCommand command = new SqlCommand("GetAllUsers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) dt.Load(reader);
                        }

                    }


                    catch (Exception ex)
                    {
                        clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                    }
                    return dt;
                }

            }
        }
        public static int? AddUser(UsersDTO UserObject)
        {
            int? ID = null;

            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            {
                using (SqlCommand command = new SqlCommand("AddUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FullName", UserObject.FullName);
                    command.Parameters.AddWithValue("@UserName", UserObject.UserName);
                    command.Parameters.AddWithValue("@Password", UserObject.Password);
                    command.Parameters.AddWithValue("@Permissions", UserObject.Permissions);
                    command.Parameters.AddWithValue("@Email", UserObject.Email);
                    command.Parameters.AddWithValue("@IsActive", UserObject.IsActive);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID)) ID = insertedID;


                    }

                    catch (Exception ex)
                    {
                        clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                    }
                }
            }
            return ID;
        }
        public static int UpdateUser(int ID, UsersDTO UserObject)
        {
            int rowAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            {
                using (SqlCommand command = new SqlCommand("UpdateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@User_ID", ID);

                    command.Parameters.AddWithValue("@FullName", UserObject.FullName);
                    command.Parameters.AddWithValue("@UserName", UserObject.UserName);
                    command.Parameters.AddWithValue("@Password", UserObject.Password);
                    command.Parameters.AddWithValue("@Permissions", UserObject.Permissions);
                    command.Parameters.AddWithValue("@Email", UserObject.Email);
                    command.Parameters.AddWithValue("@IsActive", UserObject.IsActive);
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
            }
            return rowAffected;
        }

        public static int DeleteUser(int ID)
        {
            int rowAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            {
                using (SqlCommand command = new SqlCommand("DeleteUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@User_ID", ID);
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

        public static bool GetUserByID(int ID, out UsersDTO UserObject)
        {
            bool isFound = false;
            UserObject = null;
            using (SqlConnection connection = new SqlConnection(ConnictingStr))
            {
                using (SqlCommand command = new SqlCommand("GetUserByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@User_ID", ID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserObject = new UsersDTO
                                {
                                    FullName = (string)reader["FullName"],
                                    UserName = (string)reader["UserName"],
                                    Password = (string)reader["Password"],
                                    Permissions = Convert.ToInt32(reader["Permissions "]),
                                    Email = (string)reader["Email"],
                                    IsActive = (bool)reader["IsActive"]

                                };

                                isFound = true;
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                    }
                    return isFound;
                }
            }
        }

        public static int? CheckIsUserExsistByUserName(string UserName)
        {
        
            int? ID = null;

            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            {
                using (SqlCommand command = new SqlCommand("CheckUserExistsByUserName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserName", UserName);
                
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID)) ID = insertedID;


                    }

                    catch (Exception ex)
                    {
                        clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                    }
                }
            }
            return ID;

        }

        public static bool loginUser(string UserName ,string PassWord,out int? UserID)
        {
            bool Result = false;
            UserID = null;
            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            {
                using (SqlCommand command = new SqlCommand("LoginUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", PassWord);

                    try
                    {
                            connection.Open();
                        
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            UserID = Convert.ToInt32(result);
                            Result = true;
                        }
                    }

                    catch (Exception ex)
                    {
                        clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                    }
                }
            }
            return Result;
        }

        //public static int EnableUse()
        //{

        //}

        //public static int DisableUse()
        //{

        //}
}

