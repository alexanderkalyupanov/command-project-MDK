using System;
using System.Text.RegularExpressions;

namespace CommandProject.Utils
{
    /// <summary>
    /// Вспомогательный класс для валидации данных
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Проверка валидности email
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Регулярное выражение для проверки email
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка валидности имени пользователя
        /// </summary>
        public static (bool IsValid, string Message) ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (false, "Имя пользователя не может быть пустым");

            if (username.Length < 3)
                return (false, "Имя пользователя должно содержать минимум 3 символа");

            if (username.Length > 50)
                return (false, "Имя пользователя не может превышать 50 символов");

            // Проверка на допустимые символы (буквы, цифры, подчеркивание)
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_а-яА-ЯёЁ]+$"))
                return (false, "Имя пользователя может содержать только буквы, цифры и знак подчеркивания");

            return (true, string.Empty);
        }

        /// <summary>
        /// Проверка валидности пароля
        /// </summary>
        public static (bool IsValid, string Message) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Пароль не может быть пустым");

            if (password.Length < 6)
                return (false, "Пароль должен содержать минимум 6 символов");

            if (password.Length > 100)
                return (false, "Пароль не может превышать 100 символов");

            // Проверка на наличие хотя бы одной буквы и одной цифры
            bool hasLetter = Regex.IsMatch(password, @"[a-zA-Zа-яА-ЯёЁ]");
            bool hasDigit = Regex.IsMatch(password, @"\d");

            if (!hasLetter || !hasDigit)
                return (false, "Пароль должен содержать хотя бы одну букву и одну цифру");

            return (true, string.Empty);
        }

        /// <summary>
        /// Проверка совпадения паролей
        /// </summary>
        public static (bool IsValid, string Message) ValidatePasswordMatch(string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(confirmPassword))
                return (false, "Подтверждение пароля не может быть пустым");

            if (password != confirmPassword)
                return (false, "Пароли не совпадают");

            return (true, string.Empty);
        }

        /// <summary>
        /// Проверка валидности полного имени
        /// </summary>
        public static (bool IsValid, string Message) ValidateFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return (false, "Полное имя не может быть пустым");

            if (fullName.Length < 2)
                return (false, "Полное имя должно содержать минимум 2 символа");

            if (fullName.Length > 100)
                return (false, "Полное имя не может превышать 100 символов");

            return (true, string.Empty);
        }

        /// <summary>
        /// Проверка валидности email с сообщением
        /// </summary>
        public static (bool IsValid, string Message) ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (false, "Email не может быть пустым");

            if (!IsValidEmail(email))
                return (false, "Введите корректный email адрес");

            if (email.Length > 100)
                return (false, "Email не может превышать 100 символов");

            return (true, string.Empty);
        }
    }
}

