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
                // Try to obtain central configured connection string from ClassConnectDB (preferred)
                try
                {
                    var cs = ClassConnectDB.GetConnectionString();
                    if (!string.IsNullOrEmpty(cs))
                    {
                        connectionString = cs;
                        return;
                    }
                }
                catch
                {
                    // ignore
                }

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
                // Use central connection helper to avoid AttachDbFilename issues
                using (SqlConnection connection = ClassConnectDB.GetOpenConnection())
                {
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
       -- Список жанров через подзапрос
       STUFF((
           SELECT ', ' + g.GenreName
           FROM BookGenres bg
           JOIN Genres g ON bg.GenreID = g.GenreID
           WHERE bg.BookID = b.BookID
           FOR XML PATH('')
       ), 1, 2, '') AS Genres,
       b.PublishedYear AS PublishedYear,
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
        /// Возвращает список авторов (AuthorID, Name)
        /// </summary>
        public DataTable GetAuthors()
        {
            try
            {
                using (SqlConnection connection = ClassConnectDB.GetOpenConnection())
                {
                    string sql = "SELECT AuthorID, FirstName + ' ' + LastName AS Name FROM Authors ORDER BY LastName, FirstName";
                    using (var cmd = new SqlCommand(sql, connection))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения списка авторов: {ex.Message}");
            }
        }

        /// <summary>
        /// Возвращает список жанров (GenreID, GenreName)
        /// </summary>
        public DataTable GetGenres()
        {
            try
            {
                using (SqlConnection connection = ClassConnectDB.GetOpenConnection())
                {
                    string sql = "SELECT GenreID, GenreName FROM Genres ORDER BY GenreName";
                    using (var cmd = new SqlCommand(sql, connection))
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения списка жанров: {ex.Message}");
            }
        }

        /// <summary>
        /// Возвращает список книг по фильтру. Все параметры опциональны.
        /// </summary>
        public DataTable GetBooksByFilter(int? authorId, int? genreId, decimal? minRating, decimal? maxRating)
        {
            try
            {
                using (SqlConnection connection = ClassConnectDB.GetOpenConnection())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(@"SELECT b.BookID AS ID,
       b.Title,
       b.Description,
       b.CoverImagePath AS CoverPath,
       STUFF((
           SELECT ', ' + a.FirstName + ' ' + a.LastName
           FROM BookAuthors ba
           JOIN Authors a ON ba.AuthorID = a.AuthorID
           WHERE ba.BookID = b.BookID
           FOR XML PATH('')
       ), 1, 2, '') AS Author,
       STUFF((
           SELECT ', ' + g.GenreName
           FROM BookGenres bg
           JOIN Genres g ON bg.GenreID = g.GenreID
           WHERE bg.BookID = b.BookID
           FOR XML PATH('')
       ), 1, 2, '') AS Genres,
       b.PublishedYear AS PublishedYear,
       b.Rating
FROM Books b
");
                    sb.Append("WHERE 1=1\n");

                    if (authorId.HasValue)
                        sb.Append("AND EXISTS (SELECT 1 FROM BookAuthors ba WHERE ba.BookID = b.BookID AND ba.AuthorID = @AuthorID)\n");
                    if (genreId.HasValue)
                        sb.Append("AND EXISTS (SELECT 1 FROM BookGenres bg WHERE bg.BookID = b.BookID AND bg.GenreID = @GenreID)\n");
                    if (minRating.HasValue)
                        sb.Append("AND b.Rating >= @MinRating\n");
                    if (maxRating.HasValue)
                        sb.Append("AND b.Rating <= @MaxRating\n");

                    sb.Append("ORDER BY b.Title;");

                    using (var cmd = new SqlCommand(sb.ToString(), connection))
                    {
                        if (authorId.HasValue)
                            cmd.Parameters.AddWithValue("@AuthorID", authorId.Value);
                        if (genreId.HasValue)
                            cmd.Parameters.AddWithValue("@GenreID", genreId.Value);
                        if (minRating.HasValue)
                            cmd.Parameters.AddWithValue("@MinRating", minRating.Value);
                        if (maxRating.HasValue)
                            cmd.Parameters.AddWithValue("@MaxRating", maxRating.Value);

                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения списка книг по фильтру: {ex.Message}");
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

        /// <summary>
        /// Возвращает список жанров, в которых есть книги выбранного автора (GenreID, GenreName)
        /// </summary>
        public DataTable GetGenresByAuthor(int authorId)
        {
            try
            {
                using (SqlConnection connection = ClassConnectDB.GetOpenConnection())
                {
                    string sql = @"SELECT DISTINCT g.GenreID, g.GenreName
FROM Genres g
JOIN BookGenres bg ON g.GenreID = bg.GenreID
JOIN BookAuthors ba ON bg.BookID = ba.BookID
WHERE ba.AuthorID = @AuthorID
ORDER BY g.GenreName";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@AuthorID", authorId);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения жанров для автора: {ex.Message}");
            }
        }

        /// <summary>
        /// Возвращает список авторов, которые писали в выбранном жанре (AuthorID, Name)
        /// </summary>
        public DataTable GetAuthorsByGenre(int genreId)
        {
            try
            {
                using (SqlConnection connection = ClassConnectDB.GetOpenConnection())
                {
                    // Include LastName and FirstName in select because DISTINCT + ORDER BY by those columns requires them to be in the select list
                    string sql = @"SELECT DISTINCT a.AuthorID,
       a.FirstName + ' ' + a.LastName AS Name,
       a.LastName,
       a.FirstName
FROM Authors a
JOIN BookAuthors ba ON a.AuthorID = ba.AuthorID
JOIN BookGenres bg ON ba.BookID = bg.BookID
WHERE bg.GenreID = @GenreID
ORDER BY a.LastName, a.FirstName";
                    using (var cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@GenreID", genreId);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения авторов для жанра: {ex.Message}");
            }
        }

        /// <summary>
        /// Алиасы для совместимости: возвращает список авторов (AuthorID, Name)
        /// Для фильтров могут вызываться GetAllAuthors() — сделать совместимый метод.
        /// </summary>
        public DataTable GetAllAuthors()
        {
            // reuse existing implementation
            return GetAuthors();
        }

        /// <summary>
        /// Алиас для получения всех жанров (GenreID, GenreName)
        /// </summary>
        public DataTable GetAllGenres()
        {
            return GetGenres();
        }
    }
}

