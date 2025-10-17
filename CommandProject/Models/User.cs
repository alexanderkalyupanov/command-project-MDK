using System;

namespace CommandProject.Database
{
    /// <summary>
    /// Модель пользователя
    /// </summary>
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// Проверка, является ли пользователь администратором
        /// </summary>
        public bool IsAdmin
        {
            get { return RoleName == "Admin" || RoleID == 1; }
        }

        /// <summary>
        /// Проверка, является ли пользователь обычным пользователем
        /// </summary>
        public bool IsRegularUser
        {
            get { return RoleName == "User" || RoleID == 2; }
        }

        public override string ToString()
        {
            return $"{FullName} ({Username}) - {RoleName}";
        }
    }
}

