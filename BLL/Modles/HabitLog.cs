using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static clsMain;
using static MainClass;


public class HabitLog
    {
    public enSaveMode Mode = enSaveMode.AddNew;

    public int? Habit_ID { get; set; }
    public DateOnly Date { get; set; }
    public bool Completed { get; set; }
    public TimeOnly CompletedAt { get; set; }
    public string Categories { get; set; }

    public HabitLog()
    {
        this.Habit_ID = null;
        this.Date = new DateOnly();
        this.Completed = false;
        this.CompletedAt = new TimeOnly();
        this.Categories = string.Empty;

        Mode = enSaveMode.AddNew;
    }


    private HabitLog(int id, HabitLogsDTO dto)
    {
        this.Habit_ID = dto.Habit_ID;
        this.Date = dto.Date;
        this.Completed = dto.Completed;
        this.CompletedAt = dto.CompletedAt;
        this.Categories = dto.Categories;

        Mode = enSaveMode.Update;
    }


    public static HabitLog Find(int ID)
    {
        if (ClsHabitLogsDB.GetHabitLogByID(ID,out HabitLogsDTO Hlo))
        {
            return new HabitLog(ID, Hlo);
        }
        return null; // أو إرجاع كائن فارغ حسب تفضيلك في المعيار
    }


    private bool _AddNew()
    {
        this.Habit_ID = ClsHabitLogsDB.AddTaskFocusSession(new HabitLogsDTO
        {
            Habit_ID = this.Habit_ID,
            Categories = this.Categories,
            Completed = this.Completed,
            CompletedAt = this.CompletedAt,
            Date = this.Date
        });

        return (this.Habit_ID != null);
    }

    private bool _Update()
    {

        if (this.Habit_ID == null) return false;

        int rowsAffected = ClsTaskFocusSessionsDB.UpdateTaskFocusSession(Habit_ID, new HabitLogsDTO
        {
            Habit_ID = this.Habit_ID,
            Categories = this.Categories,
            Completed = this.Completed,
            CompletedAt = this.CompletedAt,
            Date = this.Date
        });

        return (rowsAffected > 0);
    }


    public bool Save()
    {
        switch (Mode)
        {
            case enSaveMode.AddNew:
                if (_AddNew())
                {
                    Mode = enSaveMode.Update;
                    return true;
                }
                return false;

            case enSaveMode.Update:
                return _Update();
        }
        return false;
    }


    public static DataTable GetAllHabitsLogs() => ClsHabitLogsDB.GetAllHabitLogs();

    public static bool Delete(int ID) => ClsHabitLogsDB.DeleteHabitLog(ID) > 0;
}
 
