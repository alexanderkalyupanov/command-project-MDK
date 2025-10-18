using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CommandProject.Database;

namespace CommandProject.Forms
{
    public class FilterForm : Form
    {
        private ComboBox cbAuthors;
        private ComboBox cbGenres;
        private TrackBar trackMinRating;
        private TrackBar trackMaxRating;
        private Label lblMinRatingValue;
        private Label lblMaxRatingValue;
        private Button btnApply;
        private Label lblMessage;

        private bool suppressComboEvents = false;
        // Flags to indicate user explicitly selected an author or a genre
        private bool authorUserSelected = false;
        private bool genreUserSelected = false;

        public int? SelectedAuthorId { get; private set; }
        public int? SelectedGenreId { get; private set; }
        public decimal? MinRating { get; private set; }
        public decimal? MaxRating { get; private set; }

        public FilterForm()
        {
            InitializeComponents();
            LoadData();
        }

        private void InitializeComponents()
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(480, 560);
            this.Text = "Фильтры";

            // Ensure standard close (ControlBox) is used from base Form (no custom close button)
            this.ControlBox = true;

            lblMessage = new Label() { AutoSize = false, ForeColor = Color.Red, Height = 40, Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter };

            var lblAuthor = new Label() { Text = "Автор", Font = new Font("Segoe UI", 12F, FontStyle.Bold), AutoSize = true, Top = 60, Left = 20 };
            cbAuthors = new ComboBox() { Left = 20, Top = 100, Width = 420, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblGenre = new Label() { Text = "Жанр", Font = new Font("Segoe UI", 12F, FontStyle.Bold), AutoSize = true, Top = 150, Left = 20 };
            cbGenres = new ComboBox() { Left = 20, Top = 190, Width = 420, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblRange = new Label() { Text = "Диапазон рейтинга", Font = new Font("Segoe UI", 12F, FontStyle.Bold), AutoSize = true, Top = 240, Left = 20 };

            // TrackBars represent 0.0 - 5.0 with step 0.1 -> 0..50
            trackMinRating = new TrackBar() { Left = 20, Top = 280, Width = 200, Minimum = 0, Maximum = 50, TickFrequency = 5, SmallChange = 1, LargeChange = 5 };
            trackMaxRating = new TrackBar() { Left = 240, Top = 280, Width = 200, Minimum = 0, Maximum = 50, TickFrequency = 5, SmallChange = 1, LargeChange = 5 };

            lblMinRatingValue = new Label() { Left = 20, Top = 320, Width = 200, Text = "0.0", TextAlign = ContentAlignment.MiddleCenter };
            lblMaxRatingValue = new Label() { Left = 240, Top = 320, Width = 200, Text = "5.0", TextAlign = ContentAlignment.MiddleCenter };

            // set sensible initial values
            trackMinRating.Value = 0;
            trackMaxRating.Value = 50;
            lblMinRatingValue.Text = (trackMinRating.Value / 10.0m).ToString("F1");
            lblMaxRatingValue.Text = (trackMaxRating.Value / 10.0m).ToString("F1");

            trackMinRating.ValueChanged += TrackMinRating_ValueChanged;
            trackMaxRating.ValueChanged += TrackMaxRating_ValueChanged;

            btnApply = new Button() { Text = "Применить", Left = 140, Top = 360, Width = 200, Height = 48, BackColor = Color.FromArgb(255,87,34), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.Click += BtnApply_Click;

            // Use SelectionChangeCommitted to detect user-made changes only
            cbAuthors.SelectionChangeCommitted += CbAuthors_SelectionCommitted;
            cbGenres.SelectionChangeCommitted += CbGenres_SelectionCommitted;

            this.Controls.Add(lblMessage);
            this.Controls.Add(lblAuthor);
            this.Controls.Add(cbAuthors);
            this.Controls.Add(lblGenre);
            this.Controls.Add(cbGenres);
            this.Controls.Add(lblRange);
            this.Controls.Add(trackMinRating);
            this.Controls.Add(trackMaxRating);
            this.Controls.Add(lblMinRatingValue);
            this.Controls.Add(lblMaxRatingValue);
            this.Controls.Add(btnApply);
        }

        private void TrackMinRating_ValueChanged(object sender, EventArgs e)
        {
            if (trackMinRating.Value >= trackMaxRating.Value)
            {
                int newVal = Math.Max(trackMinRating.Minimum, trackMaxRating.Value - 1);
                if (newVal != trackMinRating.Value)
                    trackMinRating.Value = newVal;
            }
            lblMinRatingValue.Text = (trackMinRating.Value / 10.0m).ToString("F1");
        }

        private void TrackMaxRating_ValueChanged(object sender, EventArgs e)
        {
            if (trackMaxRating.Value <= trackMinRating.Value)
            {
                int newVal = Math.Min(trackMaxRating.Maximum, trackMinRating.Value + 1);
                if (newVal != trackMaxRating.Value)
                    trackMaxRating.Value = newVal;
            }
            lblMaxRatingValue.Text = (trackMaxRating.Value / 10.0m).ToString("F1");
        }

        private void LoadData()
        {
            try
            {
                LoadAllAuthors();
                LoadAllGenres();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных фильтра: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllAuthors()
        {
            var db = new DatabaseHelper();
            // use GetAllAuthors to return (AuthorID, Name)
            var authors = db.GetAllAuthors();

            suppressComboEvents = true;
            try
            {
                cbAuthors.Items.Clear();
                cbAuthors.Items.Add(new ComboboxItem { Text = "--- любой ---", Value = null });
                foreach (DataRow r in authors.Rows)
                {
                    cbAuthors.Items.Add(new ComboboxItem { Text = r["Name"].ToString(), Value = r["AuthorID"] });
                }
                cbAuthors.SelectedIndex = 0;
                authorUserSelected = false;
            }
            finally { suppressComboEvents = false; }
        }

        private void LoadAllGenres()
        {
            var db = new DatabaseHelper();
            var genres = db.GetAllGenres();

            suppressComboEvents = true;
            try
            {
                cbGenres.Items.Clear();
                cbGenres.Items.Add(new ComboboxItem { Text = "--- любой ---", Value = null });
                foreach (DataRow r in genres.Rows)
                {
                    cbGenres.Items.Add(new ComboboxItem { Text = r["GenreName"].ToString(), Value = r["GenreID"] });
                }
                cbGenres.SelectedIndex = 0;
                genreUserSelected = false;
            }
            finally { suppressComboEvents = false; }
        }

        private void LoadGenresForAuthor(int authorId)
        {
            try
            {
                var db = new DatabaseHelper();
                var genres = db.GetGenresByAuthor(authorId);

                suppressComboEvents = true;
                try
                {
                    var prev = (cbGenres.SelectedItem as ComboboxItem)?.Value as int?;

                    cbGenres.Items.Clear();
                    cbGenres.Items.Add(new ComboboxItem { Text = "--- любой ---", Value = null });
                    foreach (DataRow r in genres.Rows)
                    {
                        cbGenres.Items.Add(new ComboboxItem { Text = r["GenreName"].ToString(), Value = r["GenreID"] });
                    }

                    // restore if still present
                    if (prev.HasValue)
                    {
                        for (int i = 0; i < cbGenres.Items.Count; i++)
                        {
                            if ((cbGenres.Items[i] as ComboboxItem)?.Value as int? == prev.Value)
                            {
                                cbGenres.SelectedIndex = i; break;
                            }
                        }
                    }

                    if (cbGenres.SelectedIndex < 0) cbGenres.SelectedIndex = 0;
                }
                finally { suppressComboEvents = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке жанров для автора: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAuthorsForGenre(int genreId)
        {
            try
            {
                var db = new DatabaseHelper();
                var authors = db.GetAuthorsByGenre(genreId);

                suppressComboEvents = true;
                try
                {
                    var prev = (cbAuthors.SelectedItem as ComboboxItem)?.Value as int?;

                    cbAuthors.Items.Clear();
                    cbAuthors.Items.Add(new ComboboxItem { Text = "--- любой ---", Value = null });
                    foreach (DataRow r in authors.Rows)
                    {
                        cbAuthors.Items.Add(new ComboboxItem { Text = r["Name"].ToString(), Value = r["AuthorID"] });
                    }

                    if (prev.HasValue)
                    {
                        for (int i = 0; i < cbAuthors.Items.Count; i++)
                        {
                            if ((cbAuthors.Items[i] as ComboboxItem)?.Value as int? == prev.Value)
                            {
                                cbAuthors.SelectedIndex = i; break;
                            }
                        }
                    }

                    if (cbAuthors.SelectedIndex < 0) cbAuthors.SelectedIndex = 0;
                }
                finally { suppressComboEvents = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке авторов для жанра: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Called when user commits a selection in authors combobox
        private void CbAuthors_SelectionCommitted(object sender, EventArgs e)
        {
            if (suppressComboEvents) return;

            var selectedAuthor = (cbAuthors.SelectedItem as ComboboxItem)?.Value as int?;

            // If user previously selected a genre and locked it, do not refresh genres — just accept author selection
            if (genreUserSelected)
            {
                authorUserSelected = selectedAuthor.HasValue;
                return;
            }

            authorUserSelected = selectedAuthor.HasValue;
            if (authorUserSelected)
            {
                // selecting an author clears any previous genre lock
                genreUserSelected = false;
                LoadGenresForAuthor(selectedAuthor.Value);
            }
            else
            {
                LoadAllGenres();
            }
        }

        // Called when user commits a selection in genres combobox
        private void CbGenres_SelectionCommitted(object sender, EventArgs e)
        {
            if (suppressComboEvents) return;

            var selectedGenre = (cbGenres.SelectedItem as ComboboxItem)?.Value as int?;

            // If user previously selected an author and locked it, do not refresh authors — just accept genre selection
            if (authorUserSelected)
            {
                genreUserSelected = selectedGenre.HasValue;
                return;
            }

            genreUserSelected = selectedGenre.HasValue;
            if (genreUserSelected)
            {
                // selecting a genre clears any previous author lock
                authorUserSelected = false;
                LoadAuthorsForGenre(selectedGenre.Value);
            }
            else
            {
                LoadAllAuthors();
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            SelectedAuthorId = (cbAuthors.SelectedItem as ComboboxItem)?.Value as int?;
            SelectedGenreId = (cbGenres.SelectedItem as ComboboxItem)?.Value as int?;

            // Read slider values (0..50 -> 0.0..5.0)
            MinRating = trackMinRating.Value / 10.0m;
            MaxRating = trackMaxRating.Value / 10.0m;

            // Now check if there are books matching
            try
            {
                var db = new DatabaseHelper();
                var dt = db.GetBooksByFilter(SelectedAuthorId, SelectedGenreId, MinRating, MaxRating);
                if (dt.Rows.Count == 0)
                {
                    lblMessage.Text = "По заданным критериям книг не найдено";
                    return; // do not close
                }

                // store results in Tag and close with DialogResult OK
                this.Tag = dt;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public override string ToString() => Text;
        }
    }
}
