using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace CommandProject.Forms.BookCardControlls
{
    partial class BookCardControl
    {
        private IContainer components = null;

        private PictureBox pictureBoxCover;
        private Panel panelRight;
        private Label labelTitle;
        private LinkLabel linkLabelAuthor;
        private Label labelRating;
        private Label labelDescription;
        private Button buttonDetails;
        private Label labelGenres;
        private Label labelYear;

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                // Dispose image first to avoid accessing controls after components.Dispose()
                if(pictureBoxCover?.Image != null)
                {
                    pictureBoxCover.Image.Dispose();
                    pictureBoxCover.Image = null;
                }

                if(components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new Container();
            this.pictureBoxCover = new PictureBox();
            this.panelRight = new Panel();
            this.labelTitle = new Label();
            this.linkLabelAuthor = new LinkLabel();
            this.labelRating = new Label();
            this.labelDescription = new Label();
            this.buttonDetails = new Button();
            this.labelGenres = new Label();
            this.labelYear = new Label();

            // BookCardControl
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            // Restore smaller card size
            this.Size = new System.Drawing.Size(560, 280);
            this.MinimumSize = new System.Drawing.Size(520, 240);
            this.Padding = new Padding(8);

            // pictureBoxCover
            this.pictureBoxCover.Location = new System.Drawing.Point(8, 8);
            this.pictureBoxCover.Size = new System.Drawing.Size(220, 264);
            this.pictureBoxCover.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxCover.BackColor = System.Drawing.Color.LightGray;
            this.pictureBoxCover.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.Controls.Add(this.pictureBoxCover);

            // panelRight
            this.panelRight.Location = new System.Drawing.Point(236, 8);
            this.panelRight.Size = new System.Drawing.Size(312, 264);
            this.panelRight.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            // smaller bottom padding to keep button closer but still leave gap
            this.panelRight.Padding = new Padding(0, 0, 0, 16);
            this.Controls.Add(this.panelRight);

            // labelTitle
            this.labelTitle.AutoSize = false;
            this.labelTitle.Text = "Название книги";
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(255, 87, 34);
            this.labelTitle.Dock = DockStyle.Top;
            this.labelTitle.Height = 48;
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelRight.Controls.Add(this.labelTitle);

            // linkLabelAuthor (reduced vertical gap)
            this.linkLabelAuthor.Text = "Автор книги";
            this.linkLabelAuthor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.linkLabelAuthor.LinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabelAuthor.Location = new System.Drawing.Point(8, 56);
            this.linkLabelAuthor.AutoSize = true;
            this.panelRight.Controls.Add(this.linkLabelAuthor);

            // labelYear (moved up slightly)
            this.labelYear.Text = "Год: ---";
            this.labelYear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            this.labelYear.Location = new System.Drawing.Point(8, 76);
            this.labelYear.AutoSize = true;
            this.panelRight.Controls.Add(this.labelYear);

            // labelRating (moved up)
            this.labelRating.Text = "0.0/5.0";
            this.labelRating.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelRating.Location = new System.Drawing.Point(8, 98);
            this.labelRating.AutoSize = true;
            this.panelRight.Controls.Add(this.labelRating);

            // labelGenres (moved up and slightly smaller height)
            this.labelGenres.Text = "Жанры: ---";
            this.labelGenres.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelGenres.Location = new System.Drawing.Point(8, 128);
            this.labelGenres.Size = new System.Drawing.Size(296, 36);
            this.labelGenres.AutoEllipsis = true;
            this.panelRight.Controls.Add(this.labelGenres);

            // labelDescription (moved up, reduced height so it's closer to genres)
            this.labelDescription.Text = "Краткое описание книги";
            this.labelDescription.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelDescription.Location = new System.Drawing.Point(8, 168);
            this.labelDescription.Size = new System.Drawing.Size(296, 48);
            this.labelDescription.AutoEllipsis = true;
            this.panelRight.Controls.Add(this.labelDescription);

            // buttonDetails
            this.buttonDetails.Text = "Подробнее";
            this.buttonDetails.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.buttonDetails.BackColor = System.Drawing.Color.FromArgb(255, 87, 34);
            this.buttonDetails.ForeColor = System.Drawing.Color.White;
            this.buttonDetails.FlatStyle = FlatStyle.Flat;
            this.buttonDetails.FlatAppearance.BorderSize = 0;
            this.buttonDetails.Size = new System.Drawing.Size(160, 44);
            // Initial location: centered horizontally and placed near the bottom of panelRight
            this.buttonDetails.Location = new System.Drawing.Point(Math.Max(8, (this.panelRight.Width - this.buttonDetails.Width) / 2), Math.Max(8, this.panelRight.Height - this.buttonDetails.Height - 16));
            this.buttonDetails.Anchor = AnchorStyles.Bottom;
            this.buttonDetails.Click += new System.EventHandler(this.ButtonDetails_Click);
            this.panelRight.Controls.Add(this.buttonDetails);
            // ensure button is on top
            this.buttonDetails.BringToFront();

            // Resize handler for responsive layout
            this.panelRight.Resize += (s, e) =>
            {
                this.buttonDetails.Left = Math.Max(8, (this.panelRight.ClientSize.Width - this.buttonDetails.Width) / 2);
                this.buttonDetails.Top = Math.Max(8, this.panelRight.ClientSize.Height - this.buttonDetails.Height - 16);

                // Adjust description height so it doesn't overlap the button (small margin)
                int descTop = this.labelDescription.Top;
                int spaceBelow = this.panelRight.ClientSize.Height - descTop - this.buttonDetails.Height - 24; // 24px margin
                this.labelDescription.Height = Math.Max(24, spaceBelow);

                this.labelDescription.Width = Math.Max(48, this.panelRight.ClientSize.Width - 16);
                this.labelGenres.Width = Math.Max(48, this.panelRight.ClientSize.Width - 16);

                // Bring button to front after layout changes
                this.buttonDetails.BringToFront();
            };
        }

        #endregion
    }
}
