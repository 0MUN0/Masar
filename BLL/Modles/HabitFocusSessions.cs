using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static clsMain;
using static MainClass;
public class HabitFocusSessions
{
 
    
    public enSaveMode Mode = enSaveMode.AddNew;

    // الخصائص (Properties)
    public int? ID { get; set; }
    public int TaskID { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int DurationMinutes { get; set; }
    public float Rate { get; set; }

    
    public HabitFocusSessions()
    {
        this.ID = null;
        this.TaskID = 0;
        this.StartTime = TimeOnly.MinValue;
        this.EndTime = TimeOnly.MinValue;
        this.DurationMinutes = 0;
        this.Rate = 0.0f;
        Mode = enSaveMode.AddNew;
    }

  
    private HabitFocusSessions(int id, TaskFocusSessionsDTO dto)
    {
        this.ID = id;
        this.TaskID = dto.TaskID;
        this.StartTime = dto.StartTime;
        this.EndTime = dto.EndTime;
        this.DurationMinutes = dto.DurationMinutes;
        this.Rate = dto.Rate;
        Mode = enSaveMode.Update;
    }

    
    public static HabitFocusSessions Find(int ID)
    {
        if (ClsTaskFocusSessionsDB.GetTaskFocusSessionByID(ID, out TaskFocusSessionsDTO dto))
        {
            return new HabitFocusSessions(ID, dto);
        }
        return null; // أو إرجاع كائن فارغ حسب تفضيلك في المعيار
    }

    
    private bool _AddNew()
    {
        this.ID = ClsTaskFocusSessionsDB.AddTaskFocusSession(new TaskFocusSessionsDTO
        {
            TaskID = this.TaskID,
            StartTime = this.StartTime,
            EndTime = this.EndTime,
            DurationMinutes = this.DurationMinutes,
            Rate = this.Rate
        });

        return (this.ID != null);
    }

    private bool _Update()
    {
        
        if (this.ID == null) return false;

        int rowsAffected = ClsTaskFocusSessionsDB.UpdateTaskFocusSession((int)this.ID, new TaskFocusSessionsDTO
        {
            TaskID = this.TaskID,
            StartTime = this.StartTime,
            EndTime = this.EndTime,
            DurationMinutes = this.DurationMinutes,
            Rate = this.Rate
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

    
    public static DataTable GetAllSessions() => ClsTaskFocusSessionsDB.GetAllTaskFocusSessions();

    public static bool Delete(int ID) => ClsTaskFocusSessionsDB.DeleteTaskFocusSession(ID) > 0;
}

