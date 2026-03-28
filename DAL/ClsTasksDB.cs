using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static clsMain;


public class ClsTasksDB
{
    public static DataTable GetAllTasks()
    {
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            DataTable AllApplication = new DataTable();

            string StoredProcedure = "GetAllTasks";

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
    public static int? InsertTask(TaskDTO task)
    {
        int? insertedID = null;

        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            string StoredP = "InsertTask";

            using (SqlCommand command = new SqlCommand(StoredP, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                
                command.Parameters.AddWithValue("@Name", task.Name);
                command.Parameters.AddWithValue("@TaskType", task.TaskType);
                command.Parameters.AddWithValue("@Status", task.Status);
                command.Parameters.AddWithValue("@CreatedByUser", task.CreatedByUser);
                command.Parameters.AddWithValue("@Date", task.Date);  
                command.Parameters.AddWithValue("@StartDate", task.StartTask);
                command.Parameters.AddWithValue("@EndDate", task.EndTask);
                command.Parameters.AddWithValue("@CategoryID", task.CategoryID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        insertedID = id;
                    }
                }
                catch (Exception ex)
                {
                    clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                }
            }
        }

        return insertedID;
    }
    public static int DeleteTask(int ID)
    {
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {

            string StoredPro = "DeleteTask";

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
    public static int UpdateTask(int ID, TaskDTO task)
    {
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            int rowAffected = 0;
            string StoredPro = "UpdateTask";

            using (SqlCommand command = new SqlCommand(StoredPro, connection))
            {
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.AddWithValue("@TaskID", ID);
                command.Parameters.AddWithValue("@Name", task.Name);
                command.Parameters.AddWithValue("@Status", task.Status);
                command.Parameters.AddWithValue("@createdByUser", task.CreatedByUser);
                command.Parameters.AddWithValue("@Date", task.Date);
                command.Parameters.AddWithValue("@StartDate", task.StartTask);
                command.Parameters.AddWithValue("@EndDate", task.EndTask);
                command.Parameters.AddWithValue("@CategoryID", task.CategoryID);
                command.Parameters.AddWithValue("@TaskType", task.TaskType);

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
    public static bool GetTaskByID(int ID, out TaskDTO? task)
    {
        bool isFound = false;
        task = null;

        using (SqlConnection connection = new SqlConnection(ConnictingStr))
        {
            string cmdtext = "GetTaskByID";

            using (SqlCommand command = new SqlCommand(cmdtext, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@TaskID", SqlDbType.Int).Value = ID;

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            task = new TaskDTO
                            {

                                Name = (string)reader["Name"],
                                TaskType = (bool)reader["Task_Type"],
                                Status = (byte)reader["Status"],
                                CreatedByUser = Convert.ToInt16(reader["Created_By_User"]),
                                Date = DateOnly.FromDateTime((DateTime)reader["Date"]),
                                StartTask = TimeOnly.FromTimeSpan((TimeSpan)reader["Start_Task"]),
                                EndTask = TimeOnly.FromTimeSpan((TimeSpan)reader["End_Task"]),
                                CategoryID = Convert.ToInt16(reader["Catigory_ID"])
                            };
                           

                            isFound = true;
                        }







                        
                    }
                }
                catch (Exception ex)
                {
                    clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                }
            }
        }
        return isFound;
    }


}