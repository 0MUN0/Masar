using System;
using System.Data;
using static clsMain;
using static MainClass;

public class Task
{
    
    
    public enSaveMode Mode = enSaveMode.AddNew;

  
    public int? ID { get; set; }
    public string Name { get; set; }
    public bool TaskType { get; set; }
    public byte Status { get; set; }
    public int CreatedByUser { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTask { get; set; }
    public TimeOnly EndTask { get; set; }
    public int CategoryID { get; set; }

   
    public Task()
    {
        this.ID = null;
        this.Name = string.Empty;
        this.TaskType = false;
        this.Status = 0;
        this.CreatedByUser = 0;
        this.Date = DateOnly.FromDateTime(DateTime.Now);  
        this.StartTask = TimeOnly.MinValue;
        this.EndTask = TimeOnly.MinValue;
        this.CategoryID = 0;

        Mode = enSaveMode.AddNew;
    }

    
    private Task(int id, TaskDTO dto)
    {
        this.ID = id;
        this.Name = dto.Name;
        this.TaskType = dto.TaskType;
        this.Status = dto.Status;
        this.CreatedByUser = dto.CreatedByUser;
        this.Date = dto.Date;
        this.StartTask = dto.StartTask;
        this.EndTask = dto.EndTask;
        this.CategoryID = dto.CategoryID;

        Mode = enSaveMode.Update;
    }

     public static Task Find(int ID)
    {
        if (ClsTasksDB.GetTaskByID(ID, out TaskDTO dto))
        {
            return new Task(ID, dto);
        }
        return null;  
    }

    
    private bool _AddNew()
    {
        this.ID = ClsTasksDB.InsertTask(new TaskDTO
        {
            Name = this.Name,
            TaskType = this.TaskType,
            Status = this.Status,
            CreatedByUser = this.CreatedByUser,
            Date = this.Date,
            StartTask = this.StartTask,
            EndTask = this.EndTask,
            CategoryID = this.CategoryID
        });

        return (this.ID != null);
    }

    
    private bool _Update()
    {
        
        if (this.ID == null) return false;

        int rowsAffected = ClsTasksDB.UpdateTask((int)this.ID, new TaskDTO
        {
            Name = this.Name,
            TaskType = this.TaskType,
            Status = this.Status,
            CreatedByUser = this.CreatedByUser,
            Date = this.Date,
            StartTask = this.StartTask,
            EndTask = this.EndTask,
            CategoryID = this.CategoryID
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

    
    public static DataTable GetAllTasks() => ClsTasksDB.GetAllTasks();

   
    public static bool Delete(int ID) => ClsTasksDB.DeleteTask(ID) > 0;
}