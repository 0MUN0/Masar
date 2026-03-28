using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static clsMain;

public class ClsHabitFocusSessionsDB
{
    public static DataTable GetAllHabitFocusSessions()
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            using (SqlCommand command = new SqlCommand("GetAllHabitFocusSessions", connection))
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

    public static int? AddNewHabitFocusSession(HabitFocusSessionDTO HFS)
    {
        int? ID = null;
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {

            using (SqlCommand command = new SqlCommand("AddNewHabitFocusSession", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Habit_ID", HFS.Habit_ID);
                command.Parameters.AddWithValue("@StartTime", HFS.StartTime);
                command.Parameters.AddWithValue("@EndTime", HFS.EndTime);




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

    public static int UpdateHabitFocusSession(int ID, HabitFocusSessionDTO HFS)
    {
        int rowAffected = 0;
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("UpdateHabitFocusSession", connection))
            {
                try
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", ID);
                    command.Parameters.AddWithValue("@StartTime", HFS.StartTime);
                    command.Parameters.AddWithValue("@EndTime", HFS.EndTime);

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

    public static int DeleteHabitFocusSession(int ID)
    {
        int rowAffected = 0;
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("DeleteHabitFocusSession", connection))
            {
                try
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", ID);

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

    public static bool GetHabitFocusSessionByID(int ID, out HabitFocusSessionDTO HFS)
    {
        bool isFound = false;

        HFS = new HabitFocusSessionDTO();

        try
        {
            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            {
                using (SqlCommand command = new SqlCommand("GetHabitFocusSessionByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", ID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           HFS.Habit_ID = Convert.ToInt32(reader["Habit_ID"]);
                           HFS.StartTime = TimeOnly.FromTimeSpan((TimeSpan)reader["StartTime"]);
                           HFS.EndTime = TimeOnly.FromTimeSpan((TimeSpan)reader["EndTime"]);

                            isFound = true;
                        }
                    }
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
