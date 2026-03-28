using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static clsMain;

public class ClsTaskFocusSessionsDB
{
    public static DataTable GetAllTaskFocusSessions()
    {
        DataTable dt = new DataTable();
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("GetAllTaskFocusSessions", connection))
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
    public static int? AddTaskFocusSession(TaskFocusSessionsDTO TFSObject)
    {
        int? ID = null;

        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("AddTaskFocusSession", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TaskID", TFSObject.TaskID);
                command.Parameters.AddWithValue("@StartTime", TFSObject.StartTime);
                command.Parameters.AddWithValue("@EndTime", TFSObject.EndTime);
                command.Parameters.AddWithValue("@DurationMinutes", TFSObject.DurationMinutes);
                command.Parameters.AddWithValue("@Rate", TFSObject.Rate);

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

    public static int UpdateTaskFocusSession(int ID, TaskFocusSessionsDTO TFSObject )
    {
        int rowAffected = 0;

        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("UpdateTaskFocusSession", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);
                command.Parameters.AddWithValue("@StartTime", TFSObject.StartTime);
                command.Parameters.AddWithValue("@EndTime", TFSObject.EndTime);
                command.Parameters.AddWithValue("@DurationMinutes", TFSObject.DurationMinutes);
                command.Parameters.AddWithValue("@Rate", TFSObject.Rate);
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

    public static int DeleteTaskFocusSession(int ID)
    {
        int rowAffected = 0;

        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("DeleteTaskFocusSession", connection))
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
                return rowAffected;
            }

        }
    }

    public static bool GetTaskFocusSessionByID(int ID, out TaskFocusSessionsDTO TFSObject)
    {
        bool isFound = false;
        TFSObject = null;
        using (SqlConnection connection = new SqlConnection(ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("GetTaskFocusSessionByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TFSObject = new TaskFocusSessionsDTO
                            {
                                TaskID =Convert.ToInt32(reader["TaskID"]),
                                StartTime =TimeOnly.FromDateTime((DateTime)reader["StartTime"]),
                                EndTime = TimeOnly.FromDateTime((DateTime)reader["EndTime"]),
                                DurationMinutes = Convert.ToInt32(reader["DurationMinutes"]),
                                Rate = Convert.ToSingle(reader["Rate"])

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
}
