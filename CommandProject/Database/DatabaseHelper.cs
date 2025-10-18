using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using CommandProject; // for ClassConnectDB

namespace CommandProject.Database
{
    /// <summary>
    /// Класс для работы с базой данных
    /// </summary>
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            // Попытка получить строку подключения из конфигурации через reflection, чтобы избежать жесткой зависимости на System.Configuration сборку
            string connFromConfig = null;
            try
            {
                Type configManagerType = Type.GetType("System.Configuration.ConfigurationManager, System.Configuration");
                if (configManagerType != null)
                {
                    PropertyInfo connStringsProp = configManagerType.GetProperty("ConnectionStrings", BindingFlags.Static | BindingFlags.Public);
                    var connStrings = connStringsProp?.GetValue(null);
                    if (connStrings != null)
                    {
                        // попробовать получить элемент по индексу с именем
                        PropertyInfo itemProp = connStrings.GetType().GetProperty("Item", new[] { typeof(string) });
                        var setting = itemProp?.GetValue(connStrings, new object[] { "OnlineLibraryDB" });
                        if (setting != null)
                        {
                            PropertyInfo connProp = setting.GetType().GetProperty("ConnectionString");
                            connFromConfig = connProp?.GetValue(setting) as string;
                        }
                    }
                }
            }
            catch
            {
                // игнорируем любые ошибки чтения конфигурации
            }

            if (!string.IsNullOrEmpty(connFromConfig))
            {
                connectionString = connFromConfig;
            }
            else
            {
                // Если строка подключения не найдена, используем значение по умолчанию
                connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\OnlineLibraryDB.mdf;Integrated Security=True";
            }
        }

        public DatabaseHelper(string customConnectionString)
        {
            connectionString = customConnectionString;
        }

        /// <summary>
        /// Проверка подключения к базе данных
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        public (bool Success, string Message, int UserID) RegisterUser(string username, string email, string password, string fullName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_RegisterUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Входные параметры
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", HashPassword(password));
                        command.Parameters.AddWithValue("@FullName", fullName);

                        // Выходные параметры
                        SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(resultParam);

                        SqlParameter messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(messageParam);

                        command.ExecuteNonQuery();

                        int result = (int)resultParam.Value;
                        string message = messageParam.Value.ToString();

                        return (result > 0, message, result);
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка: {ex.Message}", -999);
            }
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        public User LoginUser(string username, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_LoginUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", HashPassword(password));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserID = (int)reader["UserID"],
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    RoleID = (int)reader["RoleID"],
                                    RoleName = reader["RoleName"].ToString(),
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    LastLogin = reader["LastLogin"] != DBNull.Value ? (DateTime?)reader["LastLogin"] : null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка авторизации: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Получение информации о пользователе по ID
        /// </summary>
        public User GetUserInfo(int userID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_GetUserInfo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserID", userID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserID = (int)reader["UserID"],
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    RoleID = (int)reader["RoleID"],
                                    RoleName = reader["RoleName"].ToString(),
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    LastLogin = reader["LastLogin"] != DBNull.Value ? (DateTime?)reader["LastLogin"] : null,
                                    IsActive = (bool)reader["IsActive"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения данных пользователя: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Получение всех пользователей (для администратора)
        /// </summary>
        public DataTable GetAllUsers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_GetAllUsers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения списка пользователей: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение списка книг из базы данных.
        /// Возвращает DataTable с колонками: ID, Title, Description, CoverPath, Author, Rating
        /// Использует ClassConnectDB.GetOpenConnection чтобы подключиться к серверу, избегая попыток автоподключения файла .mdf.
        /// </summary>
        public DataTable GetAllBooks()
        {
            try
            {
                // Use the application's central connection helper to avoid AttachDbFilename conflicts
                using (SqlConnection connection = ClassConnectDB.GetOpenConnection())
                {
                    string sql = @"
SELECT b.BookID AS ID,
       b.Title,
       b.Description,
       b.CoverImagePath AS CoverPath,
       -- Список авторов через подзапрос (имена через запятую)
       STUFF((
           SELECT ', ' + a.FirstName + ' ' + a.LastName
           FROM BookAuthors ba
           JOIN Authors a ON ba.AuthorID = a.AuthorID
           WHERE ba.BookID = b.BookID
           FOR XML PATH('')
       ), 1, 2, '') AS Author,
       b.Rating
FROM Books b
ORDER BY b.Title;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения списка книг: {ex.Message}");
            }
        }

        /// <summary>
        /// Изменение роли пользователя
        /// </summary>
        public bool UpdateUserRole(int userID, int roleID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_UpdateUserRole", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@RoleID", roleID);

                        SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(resultParam);

                        command.ExecuteNonQuery();

                        return (bool)resultParam.Value;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Деактивация пользователя
        /// </summary>
        public bool DeactivateUser(int userID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_DeactivateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserID", userID);

                        SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(resultParam);

                        command.ExecuteNonQuery();

                        return (bool)resultParam.Value;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Активация пользователя
        /// </summary>
        public bool ActivateUser(int userID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_ActivateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserID", userID);

                        SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(resultParam);

                        command.ExecuteNonQuery();

                        return (bool)resultParam.Value;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Хеширование пароля с использованием SHA256
        /// </summary>
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Проверка существования пользователя по имени
        /// </summary>
        public bool UserExists(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка существования email
        /// </summary>
        public bool EmailExists(string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email = @Email", connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

