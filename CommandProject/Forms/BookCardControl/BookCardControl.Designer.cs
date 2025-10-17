using System.ComponentModel;
using System.Windows.Forms;

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

            // BookCardControl
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new System.Drawing.Size(520, 220);
            this.Padding = new Padding(8);

            // pictureBoxCover
            this.pictureBoxCover.Location = new System.Drawing.Point(8, 8);
            this.pictureBoxCover.Size = new System.Drawing.Size(180, 204);
            this.pictureBoxCover.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxCover.BackColor = System.Drawing.Color.LightGray;
            this.pictureBoxCover.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.Controls.Add(this.pictureBoxCover);

            // panelRight
            this.panelRight.Location = new System.Drawing.Point(200, 8);
            this.panelRight.Size = new System.Drawing.Size(304, 204);
            this.panelRight.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelRight);

            // labelTitle
            this.labelTitle.AutoSize = false;
            this.labelTitle.Text = "Название книги";
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.FromArgb(255, 87, 34);
            this.labelTitle.Dock = DockStyle.Top;
            this.labelTitle.Height = 44;
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelRight.Controls.Add(this.labelTitle);

            // linkLabelAuthor
            this.linkLabelAuthor.Text = "Автор книги";
            this.linkLabelAuthor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.linkLabelAuthor.LinkColor = System.Drawing.Color.RoyalBlue;
            this.linkLabelAuthor.Location = new System.Drawing.Point(8, 50);
            this.linkLabelAuthor.AutoSize = true;
            this.panelRight.Controls.Add(this.linkLabelAuthor);

            // labelRating
            this.labelRating.Text = "0.0/5.0";
            this.labelRating.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelRating.Location = new System.Drawing.Point(8, 78);
            this.labelRating.AutoSize = true;
            this.panelRight.Controls.Add(this.labelRating);

            // labelDescription
            this.labelDescription.Text = "Краткое описание книги";
            this.labelDescription.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelDescription.Location = new System.Drawing.Point(8, 108);
            this.labelDescription.Size = new System.Drawing.Size(288, 64);
            this.labelDescription.AutoEllipsis = true;
            this.panelRight.Controls.Add(this.labelDescription);

            // buttonDetails
            this.buttonDetails.Text = "Подробнее";
            this.buttonDetails.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.buttonDetails.BackColor = System.Drawing.Color.FromArgb(255, 87, 34);
            this.buttonDetails.ForeColor = System.Drawing.Color.White;
            this.buttonDetails.FlatStyle = FlatStyle.Flat;
            this.buttonDetails.FlatAppearance.BorderSize = 0;
            this.buttonDetails.Size = new System.Drawing.Size(160, 40);
            /*this.buttonDetails.Location = new System.Drawing.Point((this.panelRight.Width - this.buttonDetails.Width) 
                / 2, this.panelRight.Height - this.buttonDetails.Height - 8);*/
            this.buttonDetails.Anchor = AnchorStyles.Bottom;
            this.buttonDetails.Click += new System.EventHandler(this.ButtonDetails_Click);
            this.panelRight.Controls.Add(this.buttonDetails);

            // Resize handler for responsive layout
            this.panelRight.Resize += (s, e) =>
            {
                this.buttonDetails.Left = System.Math.Max(8, (this.panelRight.ClientSize.Width - this.buttonDetails.Width) / 2);
                this.labelDescription.Width = System.Math.Max(48, this.panelRight.ClientSize.Width - 16);
            };
        }

        #endregion
    }
}
