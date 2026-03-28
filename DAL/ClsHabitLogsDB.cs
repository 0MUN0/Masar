using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static clsMain;
public class ClsHabitLogsDB
{
    public static DataTable GetAllHabitLogs()
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            using (SqlCommand command = new SqlCommand("GetAllHabitLogs", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) dt.Load(reader);
                }
            }
        }
        catch (Exception ex)
        {
            clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
        }
        return dt;
    }

    public static int? AddNewHabitLog( HabitLogsDTO HabitLogObject)
    {
        int? ID = null;
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("AddNewHabitLog", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Habit_ID", HabitLogObject.Habit_ID);
                command.Parameters.AddWithValue("@Date", HabitLogObject.Date);
                command.Parameters.AddWithValue("@Completed", HabitLogObject.Completed);
                command.Parameters.AddWithValue("@CompletedAt", HabitLogObject.CompletedAt);
                command.Parameters.AddWithValue("@Categories", HabitLogObject.Categories);
               
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

    public static int UpdateHabitLog(int ID, HabitLogsDTO HabitLogObject )
    {
        int rowAffected = 0;
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("UpdateHabitLog", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);
                command.Parameters.AddWithValue("@Date", HabitLogObject.Date);
                command.Parameters.AddWithValue("@Completed", HabitLogObject.Completed);
                command.Parameters.AddWithValue("@CompletedAt", HabitLogObject.CompletedAt);
                command.Parameters.AddWithValue("@Categories", HabitLogObject.Categories);

                

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

    public static int DeleteHabitLog(int ID)
    {
        int rowAffected = 0;
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("DeleteHabitLog", connection))
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
        }
        return rowAffected;
    }

    public static bool GetHabitLogByID(int ID, out HabitLogsDTO HabitLogObject)
    {
        bool isFound = false;
        HabitLogObject = null;

        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("GetHabitLogByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", ID);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    try
                    {
                        if (reader.Read())
                        {
                            HabitLogObject = new HabitLogsDTO
                            {
                                Habit_ID = Convert.ToInt32(reader["Habit_ID"]),
                                Date = DateOnly.FromDateTime((DateTime)reader["Date"]),
                                Completed = Convert.ToBoolean(reader["Completed"]),
                                CompletedAt = TimeOnly.FromDateTime((DateTime)reader["CompletedAt"]),
                                Categories = Convert.ToString(reader["Categories"])
                            };

                        }
                    }


                    catch (Exception ex)
                    {
                        clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                    }
                }
            }
        }
        
                isFound = true;

                        
      
        return isFound;
    }
}