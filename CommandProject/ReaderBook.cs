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
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить содержимое книги", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки книги: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPdf(string filePath)
        {
            try
            {
                using(PdfReader reader = new PdfReader(filePath))
                using(PdfDocument pdfDoc = new PdfDocument(reader))
                {
                    for(int pageNum = 1; pageNum <= pdfDoc.GetNumberOfPages(); pageNum++)
                    {
                        var strategy = new SimpleTextExtractionStrategy();
                        var pageText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(pageNum), strategy);
                        bookPages.Add(pageText);
                    }
                }
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

                // Извлекаем все текстовые элементы
                XmlNodeList bodyNodes = xmlDoc.SelectNodes("//fb:body", nsmgr);

                StringBuilder bookText = new StringBuilder();

                foreach(XmlNode bodyNode in bodyNodes)
                {
                    bookText.AppendLine(ExtractTextFromNode(bodyNode));
                }

                // Разбиваем на страницы
                string fullText = bookText.ToString();
                int pageSize = 3000; // символов на страницу

                for(int i = 0; i < fullText.Length; i += pageSize)
                {
                    int length = Math.Min(pageSize, fullText.Length - i);
                    bookPages.Add(fullText.Substring(i, length));
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

            // Извлекаем название
            XmlNode titleNode = xmlDoc.SelectSingleNode("//fb:book-title", nsmgr);
            if(titleNode != null)
                currentBookInfo.Title = titleNode.InnerText;

            // Извлекаем автора
            XmlNode authorNode = xmlDoc.SelectSingleNode("//fb:author/fb:first-name", nsmgr);
            if(authorNode != null)
                currentBookInfo.Author = authorNode.InnerText;

            // Извлекаем жанр
            XmlNode genreNode = xmlDoc.SelectSingleNode("//fb:genre", nsmgr);
            if(genreNode != null)
                currentBookInfo.Genre = genreNode.InnerText;

            // Извлекаем язык
            XmlNode langNode = xmlDoc.SelectSingleNode("//fb:lang", nsmgr);
            if(langNode != null)
                currentBookInfo.Language = langNode.InnerText;

            // Извлекаем аннотацию
            XmlNode annotationNode = xmlDoc.SelectSingleNode("//fb:annotation", nsmgr);
            if(annotationNode != null)
                currentBookInfo.Description = ExtractTextFromNode(annotationNode);
        }

        private string ExtractTextFromNode(XmlNode node)
        {
            StringBuilder text = new StringBuilder();

            foreach(XmlNode childNode in node.ChildNodes)
            {
                if(childNode.NodeType == XmlNodeType.Text)
                {
                    text.Append(childNode.Value);
                }
                else if(childNode.Name == "p" || childNode.Name == "title")
                {
                    text.AppendLine(ExtractTextFromNode(childNode));
                    text.AppendLine();
                }
                else if(childNode.Name == "empty-line")
                {
                    text.AppendLine();
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
                int pageSize = 3000; // символов на страницу

                for(int i = 0; i < text.Length; i += pageSize)
                {
                    int length = Math.Min(pageSize, text.Length - i);
                    bookPages.Add(text.Substring(i, length));
                }

                // Для TXT файлов создаем базовую информацию
                currentBookInfo = new BookInfo
                {
                    Title = Path.GetFileNameWithoutExtension(filePath),
                    Author = "Неизвестен",
                    Genre = "Текст",
                    Language = "Русский",
                    Description = "Текстовый документ"
                };
            }
            catch(Exception ex)
            {
                throw new Exception($"Ошибка чтения TXT: {ex.Message}");
            }
        }


        private void ShowPage(int pageIndex)
        {
            if(bookPages == null || pageIndex < 0 || pageIndex >= bookPages.Count)
                return;

            txtContent.Text = bookPages[pageIndex];
            currentPage = pageIndex;

            // Обновляем информацию о странице
            UpdatePageInfo();
        }

        private void UpdatePageInfo()
        {
            lblPageInfo.Text = $"Страница: {currentPage + 1}/{bookPages.Count}";
            txtPageNumber.Text = (currentPage + 1).ToString();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if(currentPage > 0)
            {
                ShowPage(currentPage - 1);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(currentPage < bookPages.Count - 1)
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
            if(int.TryParse(txtPageNumber.Text, out int pageNumber))
            {
                int pageIndex = pageNumber - 1;
                if(pageIndex >= 0 && pageIndex < bookPages.Count)
                {
                    ShowPage(pageIndex);
                }
                else
                {
                    MessageBox.Show("Недопустимый номер страницы", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Сохранение информации о книге или текущей позиции
            MessageBox.Show("Информация о книге сохранена", "Сохранение",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Книгоfeel - Читалка книг\nВерсия 1.0", "О программе");
            });
            contextMenu.Items.Add("Настройки", null, (s, args) =>
            {
                MessageBox.Show("Настройки будут доступны в следующей версии", "Настройки");
            });

            contextMenu.Show(buttonSettings, new Point(0, buttonSettings.Height));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Обработка горячих клавиш
            switch(keyData)
            {
                case Keys.Right:
                case Keys.Space:
                    btnNext_Click(null, null);
                    return true;
                case Keys.Left:
                    btnPrev_Click(null, null);
                    return true;
                case Keys.Home:
                    ShowPage(0);
                    return true;
                case Keys.End:
                    if(bookPages != null)
                        ShowPage(bookPages.Count - 1);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        public void LoadBookFromPath(string filePath)
        {
            try
            {
                if(string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("Путь к файлу не указан.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if(!File.Exists(filePath))
                {
                    MessageBox.Show($"Файл не найден:\n{filePath}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Используем основной метод загрузки
                LoadBook(filePath);

                // Обновляем заголовок формы
                this.Text = $"Чтение: {Path.GetFileName(filePath)} - Книгоfeel";

                // Показываем сообщение об успешной загрузке
                statusLabel.Text = $"Загружено: {Path.GetFileName(filePath)}";
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
