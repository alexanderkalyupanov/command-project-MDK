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
    public partial class BookReaderForm : Form
    {
        public BookReaderForm()
        {
            InitializeComponent();
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
