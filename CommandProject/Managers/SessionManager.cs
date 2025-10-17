using System;
using CommandProject.Database;

namespace CommandProject.Managers
{
    /// <summary>
    /// Менеджер сессий для управления текущим пользователем
    /// </summary>
    public static class SessionManager
    {
        private static User currentUser;

        /// <summary>
        /// Текущий авторизованный пользователь
        /// </summary>
        public static User CurrentUser
        {
            get { return currentUser; }
            private set { currentUser = value; }
        }

        /// <summary>
        /// Проверка, авторизован ли пользователь
        /// </summary>
        public static bool IsLoggedIn
        {
            get { return currentUser != null; }
        }

        /// <summary>
        /// Проверка, является ли текущий пользователь администратором
        /// </summary>
        public static bool IsAdmin
        {
            get { return currentUser != null && currentUser.IsAdmin; }
        }

        /// <summary>
        /// Вход пользователя в систему
        /// </summary>
        public static void Login(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            currentUser = user;
            OnUserLoggedIn?.Invoke(null, new UserEventArgs(user));
        }

        /// <summary>
        /// Выход пользователя из системы
        /// </summary>
        public static void Logout()
        {
            User previousUser = currentUser;
            currentUser = null;
            OnUserLoggedOut?.Invoke(null, new UserEventArgs(previousUser));
        }

        /// <summary>
        /// Событие входа пользователя
        /// </summary>
        public static event EventHandler<UserEventArgs> OnUserLoggedIn;

        /// <summary>
        /// Событие выхода пользователя
        /// </summary>
        public static event EventHandler<UserEventArgs> OnUserLoggedOut;
    }

    /// <summary>
    /// Аргументы события пользователя
    /// </summary>
    public class UserEventArgs : EventArgs
    {
        public User User { get; set; }

        public UserEventArgs(User user)
        {
            User = user;
        }
    }
}

