using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CommandProject.Forms.BookCardControlls
{
    public partial class BookCardControl : UserControl
    {
        public int BookId { get; private set; }

        // Raised when Details button is clicked. Argument = BookId (or 0 if not set).
        public event EventHandler<int> DetailsClicked;
        // Raised when delete is requested from this card
        public event EventHandler<int> DeleteRequested;

        private ContextMenuStrip cardContextMenu;

        public BookCardControl()
        {
            InitializeComponent();
            ResetToDefaults();
            InitializeContextMenu();
            // enable key preview via parent form; we will not set KeyPreview here
        }

        private void InitializeContextMenu()
        {
            cardContextMenu = new ContextMenuStrip();
            var miDetails = new ToolStripMenuItem("Подробнее");
            miDetails.Click += (s, e) => {
                ButtonDetails_Click(this, EventArgs.Empty);
            };
            var miDelete = new ToolStripMenuItem("Удалить");
            miDelete.Name = "miDelete";
            miDelete.Click += (s, e) => {
                DeleteRequested?.Invoke(this, this.BookId);
            };
            cardContextMenu.Items.Add(miDetails);
            cardContextMenu.Items.Add(miDelete);

            // Show context menu on right click
            this.MouseUp += (s, e) => {
                if (e.Button == MouseButtons.Right)
                {
                    // enable/disable delete item based on session (main menu will set visibility too)
                    var deleteItem = cardContextMenu.Items[1] as ToolStripMenuItem;
                    try
                    {
                        deleteItem.Enabled = CommandProject.Managers.SessionManager.IsAdmin;
                    }
                    catch { deleteItem.Enabled = false; }

                    cardContextMenu.Show(this, e.Location);
                }
            };

            // also show when right-click on inner controls
            foreach (Control c in this.Controls)
            {
                c.MouseUp += (s, e) => {
                    if (e.Button == MouseButtons.Right)
                    {
                        var deleteItem = cardContextMenu.Items[1] as ToolStripMenuItem;
                        try
                        {
                            deleteItem.Enabled = CommandProject.Managers.SessionManager.IsAdmin;
                        }
                        catch { deleteItem.Enabled = false; }

                        cardContextMenu.Show((Control)s, e.Location);
                    }
                };
            }
        }

        private void ResetToDefaults()
        {
            BookId = 0;
            labelTitle.Text = string.Empty;
            linkLabelAuthor.Text = string.Empty;
            labelRating.Text = "0.0/5.0";
            labelDescription.Text = string.Empty;
            labelGenres.Text = "Жанры: ---";
            labelYear.Text = "Год: ---";
            SetCoverPlaceholder();
        }

        // Simple model-less setter: pass values directly (new overload with genres and year)
        public void SetData(int bookId, string title, string authors, decimal? rating, string shortDescription, string genres, int? year, Image coverImage = null)
        {
            this.BookId = bookId;
            this.labelTitle.Text = title ?? string.Empty;
            this.linkLabelAuthor.Text = authors ?? string.Empty;
            this.labelDescription.Text = shortDescription ?? string.Empty;
            this.labelRating.Text = rating.HasValue ? $"{rating.Value:0.0}/5.0" : "0.0/5.0";
            this.labelGenres.Text = string.IsNullOrWhiteSpace(genres) ? "Жанры: ---" : $"Жанры: {genres}";
            this.labelYear.Text = year.HasValue ? $"Год: {year.Value}" : "Год: ---";

            if(coverImage != null)
                SetCoverImage(coverImage);
            else
                SetCoverPlaceholder();
        }

        // Alternative setter taking a cover image path (absolute or relative to app base)
        public void SetData(int bookId, string title, string authors, decimal? rating, string shortDescription, string coverImagePath)
        {
            // keep backward compatibility: call newer overload without genres and year
            SetData(bookId, title, authors, rating, shortDescription, null, null, null);

            if(!string.IsNullOrWhiteSpace(coverImagePath))
                SetCoverImageFromPath(coverImagePath);
            else
                SetCoverPlaceholder();
        }

        // Existing old overload without cover path - update to call new overload
        public void SetData(int bookId, string title, string authors, decimal? rating, string shortDescription, Image coverImage = null)
        {
            SetData(bookId, title, authors, rating, shortDescription, null, null, coverImage);
        }

        public void SetCoverImage(Image image)
        {
            try
            {
                var old = pictureBoxCover.Image;
                pictureBoxCover.Image = (Image)image.Clone();
                old?.Dispose();
            }
            catch
            {
                SetCoverPlaceholder();
            }
        }

        public void SetCoverImageFromPath(string path)
        {
            try
            {
                string tryPath = path;
                if(!Path.IsPathRooted(tryPath))
                    tryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

                if(!File.Exists(tryPath))
                {
                    SetCoverPlaceholder();
                    return;
                }

                using(var fs = new FileStream(tryPath, FileMode.Open, FileAccess.Read))
                {
                    var img = Image.FromStream(fs);
                    SetCoverImage(img);
                    img.Dispose();
                }
            }
            catch
            {
                SetCoverPlaceholder();
            }
        }

        private void SetCoverPlaceholder()
        {
            var old = pictureBoxCover.Image;
            var bmp = new Bitmap(Math.Max(1, pictureBoxCover.Width), Math.Max(1, pictureBoxCover.Height));
            using(var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
            }
            pictureBoxCover.Image = bmp;
            old?.Dispose();
        }

        private void ButtonDetails_Click(object sender, EventArgs e)
        {

            // Raise event for external handlers if any
            DetailsClicked?.Invoke(this, this.BookId);
        }
    }
}