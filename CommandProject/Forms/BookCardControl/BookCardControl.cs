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

        public BookCardControl()
        {
            InitializeComponent();
            ResetToDefaults();
        }

        private void ResetToDefaults()
        {
            BookId = 0;
            labelTitle.Text = string.Empty;
            linkLabelAuthor.Text = string.Empty;
            labelRating.Text = "0.0/5.0";
            labelDescription.Text = string.Empty;
            SetCoverPlaceholder();
        }

        // Simple model-less setter: pass values directly
        public void SetData(int bookId, string title, string authors, decimal? rating, string shortDescription, Image coverImage = null)
        {
            this.BookId = bookId;
            this.labelTitle.Text = title ?? string.Empty;
            this.linkLabelAuthor.Text = authors ?? string.Empty;
            this.labelDescription.Text = shortDescription ?? string.Empty;
            this.labelRating.Text = rating.HasValue ? $"{rating.Value:0.0}/5.0" : "0.0/5.0";

            if(coverImage != null)
                SetCoverImage(coverImage);
            else
                SetCoverPlaceholder();
        }

        // Alternative setter taking a cover image path (absolute or relative to app base)
        public void SetData(int bookId, string title, string authors, decimal? rating, string shortDescription, string coverImagePath)
        {
            this.BookId = bookId;
            this.labelTitle.Text = title ?? string.Empty;
            this.linkLabelAuthor.Text = authors ?? string.Empty;
            this.labelDescription.Text = shortDescription ?? string.Empty;
            this.labelRating.Text = rating.HasValue ? $"{rating.Value:0.0}/5.0" : "0.0/5.0";

            if(!string.IsNullOrWhiteSpace(coverImagePath))
                SetCoverImageFromPath(coverImagePath);
            else
                SetCoverPlaceholder();
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
            // Show placeholder message for 'Подробнее' button on the card
            try
            {
                MessageBox.Show("Данная функциональность будет реализована в следующих версиях приложения.", "Подробнее", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                // ignore any UI errors
            }

            // Raise event for external handlers if any
            DetailsClicked?.Invoke(this, this.BookId);
        }
    }
}