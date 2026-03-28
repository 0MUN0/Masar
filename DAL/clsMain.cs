using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class clsMain
{

    public static string ConnictingStr = @"Server=DESKTOP-AKPUFO8; Database=MasarDB; Integrated Security=True; Connect Timeout=30; Packet Size=4096; TrustServerCertificate=True;";

    public enum enEventLogPlace { Application = 1, Security, System }

    public enum enEventLogType { Information, Error, Warning }

    // هنا مشكلة
    public static void ShowLogEvent(string message, enEventLogPlace Place, enEventLogType TypeOfLog)
    {
        string logName = "";

        string Source = " HabitManger.Application";
        switch (Place)
        {
            case enEventLogPlace.Application:
                logName = " Application";
                break;



            case enEventLogPlace.System:
                logName = " System";
                break;

        }



        if (!EventLog.SourceExists(Source))
        {
            EventLog.CreateEventSource(Source, logName);
        }

        switch (TypeOfLog)
        {
            case enEventLogType.Warning:
                EventLog.WriteEntry(Source, message, EventLogEntryType.Warning);
                break;

            case enEventLogType.Information:
                EventLog.WriteEntry(Source, message, EventLogEntryType.Information);
                break;

            case enEventLogType.Error:
                EventLog.WriteEntry(Source, message, EventLogEntryType.Error);
                break;
        }


    }


    public record TaskDTO
    {
        public string Name { get; set; }
        public bool TaskType { get; set; }
        public byte Status { get; set; }
        public short CreatedByUser { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTask { get; set; }
        public TimeOnly EndTask { get; set; }
        public short CategoryID { get; set; }


    }
    public record HabitFocusSessionDTO
    {
       public  int Habit_ID { get; set; }
       public TimeOnly StartTime { get; set; }
       public TimeOnly EndTime { get; set; }
    }
    public record HabitLogsDTO
    {
        public int Habit_ID { get; set; }
        public DateOnly Date { get; set; }
        public bool Completed { get; set; }
        public TimeOnly CompletedAt { get; set; }
        public string Categories { get; set; }

        public override string ToString()
        {
            return $"{Habit_ID} {Date.ToString("yyyy-MM-DD")} {Completed} ";
        }
    }
    public record HabitsDTO
    {
          public string Title { set; get; } 
          public  string Description { set; get; } 
          public byte Difficulty { set; get; } 
          public byte Category_ID { set; get; } 
          public decimal Frequency { set; get; } 
          public short Created_By_User { set; get; } 
          public bool isActive { set; get; } 
            
    }
    public record TaskFocusSessionsDTO
    {
        public int TaskID { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int DurationMinutes { get; set; }
        public float Rate { get; set; }
    }
  
    public record UsersDTO
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password{ get; set; }
        public string Email { get; set; }
        public int Permissions { get; set; }
        public bool IsActive { get; set; }

    }


}
