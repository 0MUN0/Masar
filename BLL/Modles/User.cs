


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MainClass;

    public class User
    {
    
    /// <summary>
    /// this class is Represent The User on System
    /// </summary>

    // ضروري هنا تحط اتريبوتس
    public int? UserID { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int Permissions { get; set; }
    public bool IsActive { get; set; }

    public enSaveMode Mode = enSaveMode.AddNew;


        public User(int? userID, string fullName, string userName, string password, string email, int permissions, bool isActive)
        {
            UserID = userID;
            FullName = fullName;
            UserName = userName;
            Password = password;
            Email = email;
            Permissions = permissions;
            IsActive = isActive;

            enSaveMode Mode = enSaveMode.Update;
        }
        public User(int? ID, clsMain.UsersDTO UserDTO)
        {
            UserID = ID;
            FullName = UserDTO.FullName;
            UserName = UserDTO.FullName;
            Password = UserDTO.Password;
            Email = UserDTO.Email;
            Permissions = UserDTO.Permissions;
            IsActive = UserDTO.IsActive;

            enSaveMode Mode = enSaveMode.Update;
        }
        public User(clsMain.UsersDTO UserDTO)
        {
            UserID = null;
            FullName = UserDTO.FullName;
            UserName = UserDTO.FullName;
            Password = UserDTO.Password;
            Email = UserDTO.Email;
            Permissions = UserDTO.Permissions;
            IsActive = UserDTO.IsActive;

            enSaveMode Mode = enSaveMode.Update;
        }
        public User()
        {
            UserID = null;
            FullName = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Permissions = 0;
            IsActive = false;

            enSaveMode Mode = enSaveMode.AddNew;
        }


       
        public static DataTable AllUsers() =>ClsUsersDB.GetAllUsers();
        public  static bool DeleteUser(int UserID) => ClsUsersDB.DeleteUser(UserID);

        private bool _AddNewUser()
        {
            this.UserID = ClsUsersDB.AddUser(new clsMain.UsersDTO
            {
                FullName = FullName,
                UserName = UserName,
                Email = Email,
                IsActive = IsActive,
                Password = Password,
                Permissions = Permissions
            });

            return this.UserID != null;
        }

        private bool _UpdateNewUser()
        {
            int Result = ClsUsersDB.UpdateUser(this.UserID, new clsMain.UsersDTO
            {
                FullName = FullName,
                UserName = UserName,
                Email = Email,
                IsActive = IsActive,
                Password = Password,
                Permissions = Permissions
            });
            return Result > 0;
        }

        public User Find(int UserID)
        {
            if(ClsUsersDB.GetUserByID(UserID, out clsMain.UsersDTO Object))
            {
                return new User(UserID, Object);
            }
            return new User();
        }

        public bool Save()
    {
        switch (Mode)
        {
            case enSaveMode.AddNew:
                if (_AddNewUser())
                {

                    Mode = enSaveMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enSaveMode.Update:

                return _UpdateNewUser();

        }

        return false;

    }

        public static bool IsUserExisit(string UserName)
        {
            return ClsUsersDB.CheckIsUserExsistByUserName(UserName);
        }

        public bool IsUserExisit()
        {
            return ClsUsersDB.CheckIsUserExsistByUserName(this.UserName);
        }

        public bool TryLogin(out int? id)
    {
        return ClsUsersDB.loginUser(this.UserName, this.Password, out id);
    }
}

