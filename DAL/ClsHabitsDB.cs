using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static clsMain;
public class ClsHabitsDB
{
    public static DataTable GetAllHabits()
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
            using (SqlCommand command = new SqlCommand("GetAllHabits", connection))
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
    public static int? AddNewHabit(HabitsDTO HabitObject)
    {
        int? ID = null;

        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("AddNewHabit", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Title", HabitObject.Title);
                command.Parameters.AddWithValue("@Description", HabitObject.Description);
                command.Parameters.AddWithValue("@Difficulty", HabitObject.Difficulty);
                command.Parameters.AddWithValue("@Category_ID", HabitObject.Category_ID);
                command.Parameters.AddWithValue("@Frequency", HabitObject.Frequency);
                command.Parameters.AddWithValue("@Created_By_User", HabitObject.Created_By_User);
                command.Parameters.AddWithValue("@isActive", HabitObject.isActive);
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        ID = insertedID;
                    }
                }


                catch (Exception ex)
                {
                    clsMain.ShowLogEvent(ex.Message, clsMain.enEventLogPlace.Application, clsMain.enEventLogType.Error);
                }
            }
        }
        return ID;
    }
    public static int UpdateHabit(int Habit_ID, HabitsDTO HabitObject)
    {
        int rowAffected = 0;

        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("UpdateHabit", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Habit_ID", Habit_ID);
                command.Parameters.AddWithValue("@Title", HabitObject.Title);
                command.Parameters.AddWithValue("@Description", HabitObject.Description);
                command.Parameters.AddWithValue("@Difficulty", HabitObject.Difficulty);
                command.Parameters.AddWithValue("@Category_ID", HabitObject.Category_ID);
                command.Parameters.AddWithValue("@Frequency", HabitObject.Frequency);
                command.Parameters.AddWithValue("@Created_By_User", HabitObject.Created_By_User);
                command.Parameters.AddWithValue("@isActive", HabitObject.isActive);


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
    public static int DeleteHabit(int Habit_ID)
    {
        int rowAffected = 0;

        using (SqlConnection connection = new SqlConnection(ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("DeleteHabit", connection))
            {
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Habit_ID", Habit_ID);
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
    public static bool GetHabitByID(int Habit_ID, out HabitsDTO HabitObject)
    {
        bool isFound = false;
        HabitObject = null;
        using (SqlConnection connection = new SqlConnection(clsMain.ConnictingStr))
        {
            using (SqlCommand command = new SqlCommand("GetHabitByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Habit_ID", Habit_ID);
                try
                {

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            HabitObject = new HabitsDTO
                            {
                                Title = Convert.ToString(reader["Title"]),
                                Description = Convert.ToString(reader["Description"]),
                                Difficulty = Convert.ToByte(reader["Difficulty"]),
                                Category_ID = Convert.ToByte(reader["Category_ID"]),
                                Frequency = Convert.ToDecimal(reader["Frequency"]),
                                Created_By_User = Convert.ToInt16(reader["Created_By_User"]),
                                isActive = Convert.ToBoolean(reader["isActive"])
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
