using System;
using System.Data;
using static clsMain;

public class Habit
{
    public enum enMode { AddNew = 0, Update = 1 }
    public enMode Mode = enMode.AddNew;

    public int? ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public byte Difficulty { get; set; }
    public byte Category_ID { get; set; }
    public decimal Frequency { get; set; }
    public short Created_By_User { get; set; }
    public bool isActive { get; set; }

    public Habit()
    {
        this.ID = null;
        this.Title = string.Empty;
        this.Description = string.Empty;
        this.Difficulty = 0;
        this.Category_ID = 0;
        this.Frequency = 0;
        this.Created_By_User = 0;
        this.isActive = false;

        Mode = enMode.AddNew;
    }

    private Habit(int id, HabitsDTO dto)
    {
        this.ID = id;
        this.Title = dto.Title;
        this.Description = dto.Description;
        this.Difficulty = dto.Difficulty;
        this.Category_ID = dto.Category_ID;
        this.Frequency = dto.Frequency;
        this.Created_By_User = dto.Created_By_User;
        this.isActive = dto.isActive;

        Mode = enMode.Update;
    }

    public static Habit Find(int ID)
    {
        if (ClsHabitsDB.GetHabitByID(ID, out HabitsDTO dto))
        {
            return new Habit(ID, dto);
        }
        return null;
    }

    private bool _AddNew()
    {
        this.ID = ClsHabitsDB.AddNewHabit(new HabitsDTO
        {
            Title = this.Title,
            Description = this.Description,
            Difficulty = this.Difficulty,
            Category_ID = this.Category_ID,
            Frequency = this.Frequency,
            Created_By_User = this.Created_By_User,
            isActive = this.isActive
        });

        return (this.ID != null);
    }

    private bool _Update()
    {
        if (this.ID == null) return false;

        int rowsAffected = ClsHabitsDB.UpdateHabit((int)this.ID, new HabitsDTO
        {
            Title = this.Title,
            Description = this.Description,
            Difficulty = this.Difficulty,
            Category_ID = this.Category_ID,
            Frequency = this.Frequency,
            Created_By_User = this.Created_By_User,
            isActive = this.isActive
        });

        return (rowsAffected > 0);
    }

    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNew())
                {
                    Mode = enMode.Update;
                    return true;
                }
                return false;

            case enMode.Update:
                return _Update();
        }
        return false;
    }

    public static DataTable GetAllHabits() => ClsHabitsDB.GetAllHabits();

    public static bool Delete(int ID) => ClsHabitsDB.DeleteHabit(ID) > 0;
}