
using CommandProject.Database;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandProject
{
    public partial class BookReaderForm : Form
    {
        private int? loadedBookId;
        private bool isAdminMode;
        private string currentUserRole;
        private string bookFilePath;

        // Конструктор для администратора (редактирование)
        public BookReaderForm(int bookId, bool isAdmin = false, string userRole = "Admin")
        {
            InitializeComponent();
            loadedBookId = bookId;
            isAdminMode = isAdmin;
            currentUserRole = userRole;

            ConfigureFormForUserRole();
            try
            {
                LoadBook(bookId);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке книги: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Конструктор для пользователя (только чтение)
        public BookReaderForm(int bookId, string userRole = "User")
        {
            InitializeComponent();
            loadedBookId = bookId;
            isAdminMode = false;
            currentUserRole = userRole;

            ConfigureFormForUserRole();
            try
            {
                LoadBook(bookId);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке книги: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureFormForUserRole()
        {
            if(isAdminMode || currentUserRole == "Admin")
            {
                // Режим администратора - все элементы доступны
                SetAdminMode();
                this.Text = "Редактирование книги - Книгоfeel (Администратор)";
            }
            else
            {
                // Режим пользователя - только описание и кнопка чтения
                SetUserMode();
                this.Text = "Чтение книги - Книгоfeel";
            }
        }

        private void SetAdminMode()
        {
            // Показываем все элементы для админа
            label1.Visible = true;
            TBTitle.Visible = true;
            label6.Visible = true;
            CBLanguages.Visible = true;
            label7.Visible = true;
            CBGenres.Visible = true;
            label2.Visible = true;
            RTBDescription.Visible = true;

            // Кнопки админа
            buttonFilters.Visible = true;
            buttonFilters.Text = "Сохранить изменения";
            button1.Visible = true;
            button1.Text = "Отменить";

            // Скрываем кнопку чтения
            buttonRead.Visible = false;

            // Делаем элементы доступными для редактирования
            SetControlsEnabled(true);
        }

        private void SetUserMode()
        {
            // Скрываем ненужные элементы для пользователя
            label1.Visible = false;
            TBTitle.Visible = false;
            label6.Visible = false;
            CBLanguages.Visible = false;
            label7.Visible = false;
            CBGenres.Visible = false;

            // Оставляем только описание
            label2.Visible = true;
            label2.Text = "Описание книги";
            label2.Location = new Point(12, 20);

            RTBDescription.Visible = true;
            RTBDescription.Location = new Point(12, 40);
            RTBDescription.Height = 250;
            RTBDescription.ReadOnly = true;
            RTBDescription.BackColor = SystemColors.Control;

            // Скрываем кнопки админа
            buttonFilters.Visible = false;
            button1.Visible = false;

            // Показываем кнопку чтения
            buttonRead.Visible = true;

            // Настраиваем панель для пользовательского режима
            panel2.Height = 350;
        }

        private void SetControlsEnabled(bool enabled)
        {
            TBTitle.ReadOnly = !enabled;
            RTBDescription.ReadOnly = !enabled;
            CBLanguages.Enabled = enabled;
            CBGenres.Enabled = enabled;

            if(!enabled)
            {
                TBTitle.BackColor = SystemColors.Control;
                RTBDescription.BackColor = SystemColors.Control;
                CBLanguages.BackColor = SystemColors.Control;
                CBGenres.BackColor = SystemColors.Control;
            }
            else
            {
                TBTitle.BackColor = SystemColors.Window;
                RTBDescription.BackColor = SystemColors.Window;
                CBLanguages.BackColor = SystemColors.Window;
                CBGenres.BackColor = SystemColors.Window;
            }
        }

        private void LoadBook(int bookId)
        {
            var db = new DatabaseHelper();
            DataTable dt = db.GetBookById(bookId);
            if(dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Книга не найдена в базе данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dt.Rows[0];
            try
            {
                TBTitle.Text = row.Table.Columns.Contains("Title") && row["Title"] != DBNull.Value ? row["Title"].ToString() : string.Empty;
                RTBDescription.Text = row.Table.Columns.Contains("Description") && row["Description"] != DBNull.Value ? row["Description"].ToString() : string.Empty;

                // Получаем путь к файлу книги из базы данных
                if(row.Table.Columns.Contains("FilePath") && row["FilePath"] != DBNull.Value)
                {
                    bookFilePath = row["FilePath"].ToString();

                    // Проверяем существует ли файл
                    if(!File.Exists(bookFilePath))
                    {
                        MessageBox.Show($"Файл книги не найден по указанному пути:\n{bookFilePath}\n\nВы можете выбрать файл вручную при чтении.",
                            "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // Если путь не указан в БД, ищем файл в папке Resources/Books
                    bookFilePath = FindBookFile(TBTitle.Text);
                }

                // Загрузка жанров
                if(row.Table.Columns.Contains("Genres") && row["Genres"] != DBNull.Value)
                {
                    string genres = row["Genres"].ToString();
                    if(!string.IsNullOrEmpty(genres))
                    {
                        CBGenres.Text = genres;
                    }
                }

                // Загрузка языков
                if(row.Table.Columns.Contains("Language") && row["Language"] != DBNull.Value)
                {
                    string language = row["Language"].ToString();
                    if(!string.IsNullOrEmpty(language))
                    {
                        CBLanguages.Text = language;
                    }
                }

                // Обновляем заголовок формы
                if(!string.IsNullOrEmpty(TBTitle.Text))
                {
                    if(isAdminMode || currentUserRole == "Admin")
                    {
                        this.Text = $"Редактирование: {TBTitle.Text} - Книгоfeel";
                    }
                    else
                    {
                        this.Text = $"Чтение: {TBTitle.Text} - Книгоfeel";
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных книги: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FindBookFile(string bookTitle)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string booksFolder = Path.Combine(baseDir, "Resources", "Books");

                // Создаем папку если её нет
                if(!Directory.Exists(booksFolder))
                {
                    Directory.CreateDirectory(booksFolder);
                    MessageBox.Show($"Папка для книг создана: {booksFolder}\nПожалуйста, поместите файлы книг в эту папку.",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }

                // Показываем какие файлы есть в папке (для отладки)
                string[] allFiles = Directory.GetFiles(booksFolder, "*.*", SearchOption.AllDirectories);
                string filesInfo = $"Найдено файлов в {booksFolder}: {allFiles.Length}\n";
                foreach(string file in allFiles.Take(10)) // покажем первые 10 файлов
                {
                    filesInfo += $"- {Path.GetFileName(file)}\n";
                }
                if(allFiles.Length > 10)
                    filesInfo += "... и другие\n";

                // Ищем файлы с разными вариантами названий
                string[] searchPatterns = {
                    bookTitle,
                    bookTitle.Replace(" ", "_"),
                    bookTitle.Replace(" ", "-"),
                    bookTitle.Replace(":", ""),
                    bookTitle.Replace("?", ""),
                    bookTitle.Replace("!", ""),
                    loadedBookId?.ToString() ?? "0"
                };

                string[] supportedFormats = { "*.pdf", "*.fb2", "*.txt", "*.epub", "*.doc", "*.docx" };

                foreach(string format in supportedFormats)
                {
                    try
                    {
                        var files = Directory.GetFiles(booksFolder, format)
                            .Where(f => searchPatterns.Any(pattern =>
                                Path.GetFileNameWithoutExtension(f).ToLower().Contains(pattern.ToLower()) ||
                                f.ToLower().Contains(pattern.ToLower())))
                            .ToArray();

                        if(files.Length > 0)
                        {
                            MessageBox.Show($"Найден файл: {Path.GetFileName(files[0])}\nПо шаблону: {bookTitle}",
                                "Файл найден", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return files[0];
                        }
                    }
                    catch(Exception ex)
                    {
                        // Продолжаем поиск с другими форматами
                        continue;
                    }
                }

                // Если не нашли по названию, покажем все доступные файлы
                if(allFiles.Length > 0)
                {
                    string availableFiles = "Доступные файлы в папке книг:\n";
                    foreach(string file in allFiles.Take(20))
                    {
                        availableFiles += $"- {Path.GetFileName(file)}\n";
                    }

                    MessageBox.Show($"Файл для книги '{bookTitle}' не найден.\n\n{availableFiles}\n\nПапка: {booksFolder}",
                        "Файл не найден", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"В папке книг нет файлов.\n\nПапка: {booksFolder}\n\nПожалуйста, поместите файлы книг в эту папку.",
                        "Папка пуста", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                return null;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске файла книги: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        private string SelectBookFileManually()
        {
            using(OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Все поддерживаемые форматы (*.pdf;*.fb2;*.txt;*.epub;*.doc;*.docx)|*.pdf;*.fb2;*.txt;*.epub;*.doc;*.docx|PDF файлы (*.pdf)|*.pdf|FB2 файлы (*.fb2)|*.fb2|Текстовые файлы (*.txt)|*.txt|EPUB файлы (*.epub)|*.epub|Документы Word (*.doc;*.docx)|*.doc;*.docx|Все файлы (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Title = "Выберите файл книги";
                openFileDialog.RestoreDirectory = true;

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Сохраняем путь в базу данных для будущего использования
                    SaveBookFilePathToDatabase(openFileDialog.FileName);
                    return openFileDialog.FileName;
                }
            }
            return null;
        }

        private void SaveBookFilePathToDatabase(string filePath)
        {
            try
            {
                if(loadedBookId.HasValue)
                {
                    var db = new DatabaseHelper();
                    db.UpdateBookFilePath(loadedBookId.Value, filePath);
                    MessageBox.Show($"Путь к файлу сохранен в базе данных.", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить путь в базу данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonFilters_Click(object sender, EventArgs e)
        {
            if(!isAdminMode && currentUserRole != "Admin")
            {
                MessageBox.Show("У вас нет прав для редактирования книг.", "Доступ запрещен",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(string.IsNullOrWhiteSpace(TBTitle.Text))
            {
                MessageBox.Show("Название книги не может быть пустым!", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TBTitle.Focus();
                return;
            }

            if(CBLanguages.SelectedItem == null && string.IsNullOrWhiteSpace(CBLanguages.Text))
            {
                MessageBox.Show("Выберите язык книги!", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CBLanguages.Focus();
                return;
            }

            if(CBGenres.SelectedItem == null && string.IsNullOrWhiteSpace(CBGenres.Text))
            {
                MessageBox.Show("Выберите жанр книги!", "Ошибка ввода",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CBGenres.Focus();
                return;
            }

            try
            {
                using(SqlConnection conn = ClassConnectDB.GetOpenConnection())
                {
                    string query = @"UPDATE Books SET 
                                   Title = @Title,
                                   Language = @Language,
                                   Genres = @Genres,
                                   Description = @Description
                                   WHERE BookID = @BookID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Title", TBTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Language", CBLanguages.Text.Trim());
                    cmd.Parameters.AddWithValue("@Genres", CBGenres.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", RTBDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@BookID", loadedBookId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if(rowsAffected > 0)
                    {
                        MessageBox.Show("Изменения сохранены успешно!",
                                      "Успешно",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось сохранить изменения. Книга не найдена.",
                                      "Ошибка",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}",
                               "Ошибка",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!isAdminMode && currentUserRole != "Admin")
            {
                this.Close();
                return;
            }

            var result = MessageBox.Show("Отменить все изменения и закрыть форму?",
                                       "Подтверждение",
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            try
            {
                // Если путь не указан или файл не существует, предлагаем выбрать вручную
                if(string.IsNullOrEmpty(bookFilePath) || !File.Exists(bookFilePath))
                {
                    var dialogResult = MessageBox.Show($"Файл книги не найден по указанному пути.\n\nТекущий путь: {bookFilePath}\n\nХотите выбрать файл вручную?",
                        "Файл не найден", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if(dialogResult == DialogResult.Yes)
                    {
                        string selectedPath = SelectBookFileManually();
                        if(!string.IsNullOrEmpty(selectedPath))
                        {
                            bookFilePath = selectedPath;
                        }
                        else
                        {
                            return; // Пользователь отменил выбор
                        }
                    }
                    else
                    {
                        return; // Пользователь отказался выбирать файл
                    }
                }

                // Проверяем существование файла после выбора
                if(!File.Exists(bookFilePath))
                {
                    MessageBox.Show($"Файл не существует: {bookFilePath}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Показываем информацию о файле
                FileInfo fileInfo = new FileInfo(bookFilePath);
                string fileInfoText = $"Открываем файл:\n" +
                                     $"Название: {Path.GetFileName(bookFilePath)}\n" +
                                     $"Размер: {fileInfo.Length / 1024} KB\n" +
                                     $"Путь: {bookFilePath}";

                // Определяем тип файла и открываем соответствующую читалку
                string extension = Path.GetExtension(bookFilePath).ToLower();

                switch(extension)
                {
                    case ".pdf":
                    case ".fb2":
                    case ".txt":
                        OpenInBookReader(bookFilePath);
                        break;
                    default:
                        OpenBookWithDefaultProgram(bookFilePath);
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии книги: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenInBookReader(string filePath)
        {
            try
            {
                var readingForm = new ReaderBook();

                // Используем метод LoadBookFromPath
                readingForm.LoadBookFromPath(filePath);
                readingForm.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии книги в читалке: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenBookWithDefaultProgram(string filePath)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл: {ex.Message}\n\n" +
                               $"Попробуйте открыть файл вручную: {filePath}",
                               "Ошибка открытия", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BookReaderForm_Load(object sender, EventArgs e)
        {
            // Загрузка списков для комбобоксов (только для админа)
            if(isAdminMode || currentUserRole == "Admin")
            {
                LoadComboBoxData();
            }
        }

        private void LoadComboBoxData()
        {
            try
            {
                using(SqlConnection conn = ClassConnectDB.GetOpenConnection())
                {
                    string languageQuery = "SELECT DISTINCT Language FROM Books WHERE Language IS NOT NULL AND Language != ''";
                    SqlCommand languageCmd = new SqlCommand(languageQuery, conn);
                    SqlDataReader languageReader = languageCmd.ExecuteReader();

                    CBLanguages.Items.Clear();
                    while(languageReader.Read())
                    {
                        CBLanguages.Items.Add(languageReader["Language"]);
                    }
                    languageReader.Close();

                    string genreQuery = "SELECT DISTINCT Genres FROM Books WHERE Genres IS NOT NULL AND Genres != ''";
                    SqlCommand genreCmd = new SqlCommand(genreQuery, conn);
                    SqlDataReader genreReader = genreCmd.ExecuteReader();

                    CBGenres.Items.Clear();
                    while(genreReader.Read())
                    {
                        CBGenres.Items.Add(genreReader["Genres"]);
                    }
                    genreReader.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке справочников: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            var contextMenu = new ContextMenuStrip();

            if(isAdminMode || currentUserRole == "Admin")
            {
                contextMenu.Items.Add("Режим администратора", null, (s, args) =>
                {
                    MessageBox.Show("Вы работаете в режиме администратора", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
                contextMenu.Items.Add("-");
            }

            contextMenu.Items.Add("О программе", null, (s, args) =>
            {
                MessageBox.Show("Книгоfeel - Система чтения книг\nВерсия 2.0\n\nРежим: " +
                    (isAdminMode || currentUserRole == "Admin" ? "Администратор" : "Пользователь"),
                    "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });

            contextMenu.Items.Add("Информация о книге", null, (s, args) =>
            {
                ShowBookInfo();
            });

            contextMenu.Show(buttonSettings, new Point(0, buttonSettings.Height));
        }

        private void ShowBookInfo()
        {
            string fileInfo = string.IsNullOrEmpty(bookFilePath) ? "Файл не найден" : Path.GetFileName(bookFilePath);

            string info = $"Название: {TBTitle.Text}\n" +
                         $"Язык: {CBLanguages.Text}\n" +
                         $"Жанр: {CBGenres.Text}\n" +
                         $"Файл: {fileInfo}\n" +
                         $"Путь к файлу: {bookFilePath}\n" +
                         $"Описание: {RTBDescription.Text}\n\n" +
                         $"Режим: {(isAdminMode || currentUserRole == "Admin" ? "Администратор" : "Чтение")}";

            MessageBox.Show(info, "Информация о книге",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Метод для проверки прав доступа
        public static bool CanUserEditBooks(string userRole)
        {
            return userRole == "Admin";
        }

        // Метод для создания формы в зависимости от роли пользователя
        public static void ShowBookForm(int bookId, string userRole, Form parentForm = null)
        {
            bool isAdmin = CanUserEditBooks(userRole);
            BookReaderForm form;

            if(isAdmin)
            {
                form = new BookReaderForm(bookId, true, userRole);
            }
            else
            {
                form = new BookReaderForm(bookId, userRole);
            }

            if(parentForm != null)
            {
                form.ShowDialog(parentForm);
            }
            else
            {
                form.ShowDialog();
            }
        }

        private void buttonManageFile_Click(object sender, EventArgs e)
        {
            ManageBookFile();
        }

        private void ManageBookFile()
        {
            var menu = new ContextMenuStrip();

            menu.Items.Add("Показать текущий путь", null, (s, args) =>
            {
                MessageBox.Show($"Текущий путь к файлу: {bookFilePath}\nФайл существует: {File.Exists(bookFilePath)}",
                    "Информация о файле", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });

            menu.Items.Add("Изменить путь к файлу", null, (s, args) =>
            {
                string newPath = SelectBookFileManually();
                if(!string.IsNullOrEmpty(newPath))
                {
                    bookFilePath = newPath;
                    MessageBox.Show("Путь к файлу обновлен", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });

            menu.Items.Add("Показать папку книг", null, (s, args) =>
            {
                string booksFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Books");
                if(Directory.Exists(booksFolder))
                {
                    System.Diagnostics.Process.Start("explorer.exe", booksFolder);
                }
                else
                {
                    MessageBox.Show($"Папка не существует: {booksFolder}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });

            menu.Items.Add("Проверить все файлы", null, (s, args) =>
            {
                CheckAllBookFiles();
            });

            menu.Show(buttonManageFile, new Point(0, buttonManageFile.Height));
        }

        private void CheckAllBookFiles()
        {
            try
            {
                string booksFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Books");
                string report = $"Проверка папки книг: {booksFolder}\n\n";

                if(Directory.Exists(booksFolder))
                {
                    var allFiles = Directory.GetFiles(booksFolder, "*.*", SearchOption.AllDirectories);
                    report += $"Найдено файлов: {allFiles.Length}\n\n";

                    foreach(var file in allFiles.Take(50)) // Покажем первые 50 файлов
                    {
                        FileInfo fi = new FileInfo(file);
                        report += $"{Path.GetFileName(file)} ({fi.Length / 1024} KB)\n";
                    }

                    if(allFiles.Length > 50)
                        report += $"\n... и еще {allFiles.Length - 50} файлов";
                }
                else
                {
                    report += "Папка не существует!";
                }

                MessageBox.Show(report, "Отчет о файлах", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке файлов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReaderBook bookReaders = new ReaderBook();
            bookReaders.Show();
        }
    }
}