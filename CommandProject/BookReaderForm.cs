using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandProject.Database;

namespace CommandProject
{
    public partial class BookReaderForm : Form
    {
        private int? loadedBookId;

        public BookReaderForm()
        {
            InitializeComponent();
        }

        // New constructor that accepts book id and loads book data
        public BookReaderForm(int bookId)
        {
            InitializeComponent();
            loadedBookId = bookId;
            try
            {
                LoadBook(bookId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке книги: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBook(int bookId)
        {
            var db = new DatabaseHelper();
            DataTable dt = db.GetBookById(bookId);
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Книга не найдена.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dt.Rows[0];
            try
            {
                TBTitle.Text = row.Table.Columns.Contains("Title") && row["Title"] != DBNull.Value ? row["Title"].ToString() : string.Empty;
                RTBDescription.Text = row.Table.Columns.Contains("Description") && row["Description"] != DBNull.Value ? row["Description"].ToString() : string.Empty;

                // Genres and languages in DB are returned as comma separated strings; place into ComboBoxes' Text
                if (row.Table.Columns.Contains("Genres") && row["Genres"] != DBNull.Value)
                {
                    CBGenres.Text = row["Genres"].ToString();
                }

                // Languages may not be present in query; leave CBLanguages untouched if not present
                if (row.Table.Columns.Contains("Language") && row["Language"] != DBNull.Value)
                {
                    CBLanguages.Text = row["Language"].ToString();
                }

                this.Text = TBTitle.Text;
            }
            catch
            {
                // ignore individual field parse errors
            }
        }

        private void buttonFilters_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBTitle.Text) ||
               CBLanguages.SelectedValue == null ||
               CBGenres.SelectedValue == null)
               
            {
                MessageBox.Show("Заполните все обязательные поля!",
                               "Ошибка ввода",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }            

            try
            {
                using (SqlConnection conn = ClassConnectDB.GetOpenConnection())
                {
                    string query = @"UPDATE Book SET 
                                   Title = @Title,
                                   Lsnguages = @Language,
                                   Description = @Description,                                   
                                   WHERE ID = @ID_Book";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Title", TBTitle.Text.Trim());                   
                    cmd.Parameters.AddWithValue("@Language", CBLanguages.SelectedValue);
                    cmd.Parameters.AddWithValue("@Genres", CBGenres.SelectedValue);
                    cmd.Parameters.AddWithValue("@Description", RTBDescription.Text.Trim());
                    //cmd.Parameters.AddWithValue("@ID_Book");

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Изменения сохранены успешно!",
                                  "Успешно",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}",
                               "Ошибка",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }
    }
}
