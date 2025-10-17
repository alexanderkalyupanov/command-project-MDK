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

namespace CommandProject
{
    public partial class AddBookForms : Form
    {
        public AddBookForms()
        {
            InitializeComponent();
        }

        private void buttonFilters_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = ClassConnectDB.GetOpenConnection())
                {
                    // Проверка уникальности email
                    string checkQuery = "SELECT COUNT(*) FROM Books WHERE Title = @TitleEmail";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Title", TBTitle.Text.Trim());
                    int emailExists = (int)checkCmd.ExecuteScalar();

                    if (emailExists > 0)
                    {
                        MessageBox.Show("Книга с таким названием уже существует!",
                                      "Ошибка",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                        return;
                    }

                    // Добавление нового преподавателя
                    string insertQuery = @"INSERT INTO Books (Title, Description, PublishedYear, Publisher, Language) 
                                   VALUES (@Title, @Description, @PublishedYear, @Publisher, @Languages)";

                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@Title", TBTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Author", CBAuthor.SelectedValue);
                    cmd.Parameters.AddWithValue("@PublishedYear", TBPublishedYear.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", RTBDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Publisher", CBPublishes.SelectedValue);
                    cmd.Parameters.AddWithValue("@Languages", CBLanguages.SelectedValue);
                    cmd.Parameters.AddWithValue("@Genres", CBGenres.SelectedValue);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Книга успешно добавлена!",
                                  "Успешно",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении книги: {ex.Message}",
                               "Ошибка",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

    }
}
