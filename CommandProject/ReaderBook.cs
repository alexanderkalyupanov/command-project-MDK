using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CommandProject
{
    public partial class ReaderBook : Form
    {
        private List<string> bookPages;
        private int currentPage = 0;
        private string currentBookPath = string.Empty;
        private BookInfo currentBookInfo;

        public ReaderBook()
        {
            InitializeComponent();
            InitializeBookInfo();
        }

        private void InitializeBookInfo()
        {
            currentBookInfo = new BookInfo();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Книги (*.pdf;*.fb2;*.txt)|*.pdf;*.fb2;*.txt|PDF файлы (*.pdf)|*.pdf|FB2 файлы (*.fb2)|*.fb2|Текстовые файлы (*.txt)|*.txt";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadBook(openFileDialog.FileName);
                }
            }
        }

        private void LoadBook(string filePath)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                statusLabel.Text = "Загрузка книги...";

                currentBookPath = filePath;
                bookPages = new List<string>();
                currentPage = 0;

                string extension = Path.GetExtension(filePath).ToLower();

                switch(extension)
                {
                    case ".pdf":
                        LoadPdf(filePath);
                        break;
                    case ".fb2":
                        LoadFb2(filePath);
                        break;
                    case ".txt":
                        LoadTxt(filePath);
                        break;
                    default:
                        MessageBox.Show("Неподдерживаемый формат файла", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                if(bookPages.Count > 0)
                {
                    ShowPage(0);
                    statusLabel.Text = $"Загружено: {Path.GetFileName(filePath)} - Страниц: {bookPages.Count}";

                    // Показываем информацию о книге
                    ShowBookInfo();
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить содержимое книги", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    statusLabel.Text = "Ошибка загрузки книги";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки книги: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = $"Ошибка: {ex.Message}";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void LoadPdf(string filePath)
        {
            try
            {
                using(var pdfDoc = new PdfDocument(new PdfReader(filePath)))
                {
                    for(int pageNum = 1; pageNum <= pdfDoc.GetNumberOfPages(); pageNum++)
                    {
                        var page = pdfDoc.GetPage(pageNum);
                        var strategy = new SimpleTextExtractionStrategy();
                        var pageText = PdfTextExtractor.GetTextFromPage(page, strategy);
                        bookPages.Add(pageText?.Trim() ?? "");
                    }
                }

                // Для PDF файлов создаем базовую информацию
                currentBookInfo = new BookInfo
                {
                    Title = Path.GetFileNameWithoutExtension(filePath),
                    Author = "Неизвестен",
                    Genre = "PDF документ",
                    Language = "Русский",
                    Description = "PDF документ"
                };
            }
            catch(Exception ex)
            {
                throw new Exception($"Ошибка чтения PDF: {ex.Message}");
            }
        }

        private void LoadFb2(string filePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                // Добавляем пространство имен для FB2
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("fb", "http://www.gribuser.ru/xml/fictionbook/2.0");

                // Извлекаем информацию о книге
                ExtractFb2Info(xmlDoc, nsmgr);

                // Извлекаем основной текст
                XmlNode bodyNode = xmlDoc.SelectSingleNode("//fb:body[1]", nsmgr);
                if(bodyNode == null)
                {
                    throw new Exception("Не найден основной текст книги");
                }

                string fullText = ExtractTextFromNode(bodyNode);

                // Разбиваем на страницы
                int pageSize = 2000; // символов на страницу

                for(int i = 0; i < fullText.Length; i += pageSize)
                {
                    int length = Math.Min(pageSize, fullText.Length - i);
                    string pageText = fullText.Substring(i, length).Trim();
                    if(!string.IsNullOrEmpty(pageText))
                    {
                        bookPages.Add(pageText);
                    }
                }

                // Если книга пустая, добавляем заглушку
                if(bookPages.Count == 0)
                {
                    bookPages.Add("Содержимое книги пусто или не распознано.");
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Ошибка чтения FB2: {ex.Message}");
            }
        }

        private void ExtractFb2Info(XmlDocument xmlDoc, XmlNamespaceManager nsmgr)
        {
            currentBookInfo = new BookInfo();

            try
            {
                // Извлекаем название
                XmlNode titleNode = xmlDoc.SelectSingleNode("//fb:description/fb:title-info/fb:book-title", nsmgr);
                if(titleNode != null)
                    currentBookInfo.Title = titleNode.InnerText.Trim();

                // Извлекаем автора
                XmlNode firstNameNode = xmlDoc.SelectSingleNode("//fb:description/fb:title-info/fb:author/fb:first-name", nsmgr);
                XmlNode lastNameNode = xmlDoc.SelectSingleNode("//fb:description/fb:title-info/fb:author/fb:last-name", nsmgr);

                string author = "";
                if(firstNameNode != null)
                    author += firstNameNode.InnerText.Trim();
                if(lastNameNode != null)
                    author += " " + lastNameNode.InnerText.Trim();

                currentBookInfo.Author = string.IsNullOrEmpty(author.Trim()) ? "Неизвестен" : author.Trim();

                // Извлекаем жанр
                XmlNode genreNode = xmlDoc.SelectSingleNode("//fb:description/fb:title-info/fb:genre", nsmgr);
                if(genreNode != null)
                    currentBookInfo.Genre = genreNode.InnerText.Trim();

                // Извлекаем язык
                XmlNode langNode = xmlDoc.SelectSingleNode("//fb:description/fb:title-info/fb:lang", nsmgr);
                if(langNode != null)
                    currentBookInfo.Language = langNode.InnerText.Trim();

                // Извлекаем аннотацию
                XmlNode annotationNode = xmlDoc.SelectSingleNode("//fb:description/fb:title-info/fb:annotation", nsmgr);
                if(annotationNode != null)
                    currentBookInfo.Description = ExtractTextFromNode(annotationNode);
            }
            catch(Exception ex)
            {
                // Если не удалось извлечь информацию, используем значения по умолчанию
                currentBookInfo.Title = Path.GetFileNameWithoutExtension(currentBookPath);
                currentBookInfo.Author = "Неизвестен";
                currentBookInfo.Genre = "Книга";
                currentBookInfo.Language = "Русский";
                currentBookInfo.Description = "FB2 книга";
            }
        }

        private string ExtractTextFromNode(XmlNode node)
        {
            if(node == null)
                return string.Empty;

            StringBuilder text = new StringBuilder();

            foreach(XmlNode childNode in node.ChildNodes)
            {
                if(childNode.NodeType == XmlNodeType.Text)
                {
                    text.Append(childNode.Value?.Trim() + " ");
                }
                else if(childNode.Name == "p" || childNode.Name == "title")
                {
                    text.AppendLine(ExtractTextFromNode(childNode));
                    text.AppendLine();
                }
                else if(childNode.Name == "empty-line")
                {
                    text.AppendLine();
                    text.AppendLine();
                }
                else if(childNode.Name == "strong" || childNode.Name == "emphasis")
                {
                    text.Append(ExtractTextFromNode(childNode) + " ");
                }
                else
                {
                    text.Append(ExtractTextFromNode(childNode));
                }
            }

            return text.ToString().Trim();
        }

        private void LoadTxt(string filePath)
        {
            try
            {
                string text = File.ReadAllText(filePath, Encoding.UTF8);
                int pageSize = 2000; // символов на страницу

                for(int i = 0; i < text.Length; i += pageSize)
                {
                    int length = Math.Min(pageSize, text.Length - i);
                    string pageText = text.Substring(i, length).Trim();
                    if(!string.IsNullOrEmpty(pageText))
                    {
                        bookPages.Add(pageText);
                    }
                }

                // Для TXT файлов создаем базовую информацию
                currentBookInfo = new BookInfo
                {
                    Title = Path.GetFileNameWithoutExtension(filePath),
                    Author = "Неизвестен",
                    Genre = "Текстовый документ",
                    Language = "Русский",
                    Description = "Текстовый документ"
                };
            }
            catch(Exception ex)
            {
                throw new Exception($"Ошибка чтения TXT: {ex.Message}");
            }
        }

        private void ShowBookInfo()
        {
            string info = $"Название: {currentBookInfo.Title}\n" +
                         $"Автор: {currentBookInfo.Author}\n" +
                         $"Жанр: {currentBookInfo.Genre}\n" +
                         $"Язык: {currentBookInfo.Language}\n" +
                         $"Описание: {currentBookInfo.Description}";

            // Можно показать в MessageBox или в статусной строке
            statusLabel.Text = $"{currentBookInfo.Title} - {currentBookInfo.Author}";
        }

        private void ShowPage(int pageIndex)
        {
            if(bookPages == null || bookPages.Count == 0)
            {
                txtContent.Text = "Книга не загружена или пуста.";
                return;
            }

            // Проверяем границы
            if(pageIndex < 0)
                pageIndex = 0;
            if(pageIndex >= bookPages.Count)
                pageIndex = bookPages.Count - 1;

            txtContent.Text = bookPages[pageIndex];
            currentPage = pageIndex;

            // Обновляем информацию о странице
            UpdatePageInfo();
        }

        private void UpdatePageInfo()
        {
            if(bookPages == null || bookPages.Count == 0)
            {
                lblPageInfo.Text = "Страница: 0/0";
                txtPageNumber.Text = "0";
            }
            else
            {
                lblPageInfo.Text = $"Страница: {currentPage + 1}/{bookPages.Count}";
                txtPageNumber.Text = (currentPage + 1).ToString();
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if(bookPages != null && currentPage > 0)
            {
                ShowPage(currentPage - 1);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(bookPages != null && currentPage < bookPages.Count - 1)
            {
                ShowPage(currentPage + 1);
            }
        }

        private void btnGoToPage_Click(object sender, EventArgs e)
        {
            GoToPage();
        }

        private void txtPageNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                GoToPage();
                e.Handled = true;
            }
        }

        private void GoToPage()
        {
            if(bookPages == null || bookPages.Count == 0)
            {
                MessageBox.Show("Книга не загружена", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(int.TryParse(txtPageNumber.Text, out int pageNumber) && pageNumber > 0)
            {
                int pageIndex = pageNumber - 1;
                if(pageIndex >= 0 && pageIndex < bookPages.Count)
                {
                    ShowPage(pageIndex);
                }
                else
                {
                    MessageBox.Show($"Номер страницы должен быть от 1 до {bookPages.Count}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Введите корректный номер страницы", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Сохранение информации о книге или текущей позиции
                string saveInfo = $"Текущая книга: {currentBookInfo.Title}\n" +
                                 $"Текущая страница: {currentPage + 1} из {bookPages?.Count ?? 0}\n" +
                                 $"Автор: {currentBookInfo.Author}";

                // Можно сохранить в файл или просто показать сообщение
                MessageBox.Show(saveInfo, "Информация о книге",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            // Меню настроек
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("О программе", null, (s, args) =>
            {
                MessageBox.Show("Книгоfeel - Читалка книг\nВерсия 1.0\n\nПоддерживаемые форматы:\n- PDF\n- FB2\n- TXT", "О программе");
            });
            contextMenu.Items.Add("Информация о книге", null, (s, args) =>
            {
                ShowBookInfoMessage();
            });
            contextMenu.Items.Add("-"); // Разделитель
            contextMenu.Items.Add("Настройки", null, (s, args) =>
            {
                MessageBox.Show("Настройки будут доступны в следующей версии", "Настройки");
            });

            contextMenu.Show(buttonSettings, new Point(0, buttonSettings.Height));
        }

        private void ShowBookInfoMessage()
        {
            if(currentBookInfo != null)
            {
                string info = $"Название: {currentBookInfo.Title}\n" +
                             $"Автор: {currentBookInfo.Author}\n" +
                             $"Жанр: {currentBookInfo.Genre}\n" +
                             $"Язык: {currentBookInfo.Language}\n" +
                             $"Описание: {currentBookInfo.Description}\n\n" +
                             $"Файл: {Path.GetFileName(currentBookPath)}\n" +
                             $"Страниц: {bookPages?.Count ?? 0}";

                MessageBox.Show(info, "Информация о книге",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Книга не загружена", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Обработка горячих клавиш
            switch(keyData)
            {
                case Keys.Right:
                case Keys.Space:
                case Keys.PageDown:
                    btnNext_Click(null, null);
                    return true;
                case Keys.Left:
                case Keys.PageUp:
                    btnPrev_Click(null, null);
                    return true;
                case Keys.Home:
                    ShowPage(0);
                    return true;
                case Keys.End:
                    if(bookPages != null)
                        ShowPage(bookPages.Count - 1);
                    return true;
                case Keys.F1:
                    buttonSettings_Click(null, null);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        // Добавьте этот метод для загрузки книги по пути
        public void LoadBookFromPath(string filePath)
        {
            LoadBook(filePath);
        }

    }

    public class BookInfo
    {
        public string Title { get; set; } = "Неизвестно";
        public string Author { get; set; } = "Неизвестен";
        public string Genre { get; set; } = "Неизвестен";
        public string Language { get; set; } = "Русский";
        public string Description { get; set; } = "Описание отсутствует";
    }
}
