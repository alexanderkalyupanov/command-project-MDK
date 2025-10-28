namespace CommandProject
{
    partial class BookReaderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.CBGenres = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CBLanguages = new System.Windows.Forms.ComboBox();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.labelLogo = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TBTitle = new System.Windows.Forms.TextBox();
            this.RTBDescription = new System.Windows.Forms.RichTextBox();
            this.buttonFilters = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonRead = new System.Windows.Forms.Button();
            this.buttonManageFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.panel1.Location = new System.Drawing.Point(2, 95);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(945, 10);
            this.panel1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(0, 310);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.button1.Size = new System.Drawing.Size(946, 36);
            this.button1.TabIndex = 18;
            this.button1.Text = "Отменить";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Жанр книги";
            // 
            // CBGenres
            // 
            this.CBGenres.FormattingEnabled = true;
            this.CBGenres.Location = new System.Drawing.Point(107, 79);
            this.CBGenres.Name = "CBGenres";
            this.CBGenres.Size = new System.Drawing.Size(289, 21);
            this.CBGenres.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Язык книги";
            // 
            // CBLanguages
            // 
            this.CBLanguages.FormattingEnabled = true;
            this.CBLanguages.Location = new System.Drawing.Point(107, 52);
            this.CBLanguages.Name = "CBLanguages";
            this.CBLanguages.Size = new System.Drawing.Size(289, 21);
            this.CBLanguages.TabIndex = 14;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.Image = global::CommandProject.Properties.Resources.app_logo;
            this.pictureBoxLogo.Location = new System.Drawing.Point(9, 20);
            this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(56, 56);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // labelLogo
            // 
            this.labelLogo.AutoSize = true;
            this.labelLogo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.labelLogo.ForeColor = System.Drawing.Color.Black;
            this.labelLogo.Location = new System.Drawing.Point(76, 30);
            this.labelLogo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLogo.Name = "labelLogo";
            this.labelLogo.Size = new System.Drawing.Size(127, 32);
            this.labelLogo.TabIndex = 1;
            this.labelLogo.Text = "Книгоfeel";
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelTop.Controls.Add(this.buttonManageFile);
            this.panelTop.Controls.Add(this.pictureBoxLogo);
            this.panelTop.Controls.Add(this.labelLogo);
            this.panelTop.Controls.Add(this.buttonSettings);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(946, 90);
            this.panelTop.TabIndex = 6;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettings.BackColor = System.Drawing.Color.Transparent;
            this.buttonSettings.FlatAppearance.BorderSize = 0;
            this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSettings.Location = new System.Drawing.Point(890, 20);
            this.buttonSettings.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(44, 36);
            this.buttonSettings.TabIndex = 7;
            this.buttonSettings.Text = "⚙";
            this.buttonSettings.UseVisualStyleBackColor = false;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Описание книги";
            // 
            // TBTitle
            // 
            this.TBTitle.Location = new System.Drawing.Point(107, 26);
            this.TBTitle.Name = "TBTitle";
            this.TBTitle.Size = new System.Drawing.Size(289, 20);
            this.TBTitle.TabIndex = 5;
            // 
            // RTBDescription
            // 
            this.RTBDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RTBDescription.Location = new System.Drawing.Point(0, 135);
            this.RTBDescription.Name = "RTBDescription";
            this.RTBDescription.Size = new System.Drawing.Size(946, 165);
            this.RTBDescription.TabIndex = 4;
            this.RTBDescription.Text = "";
            // 
            // buttonFilters
            // 
            this.buttonFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.buttonFilters.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonFilters.FlatAppearance.BorderSize = 0;
            this.buttonFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFilters.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.buttonFilters.ForeColor = System.Drawing.Color.White;
            this.buttonFilters.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonFilters.Location = new System.Drawing.Point(0, 346);
            this.buttonFilters.Margin = new System.Windows.Forms.Padding(2);
            this.buttonFilters.Name = "buttonFilters";
            this.buttonFilters.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.buttonFilters.Size = new System.Drawing.Size(946, 36);
            this.buttonFilters.TabIndex = 3;
            this.buttonFilters.Text = "Сохранить";
            this.buttonFilters.UseVisualStyleBackColor = false;
            this.buttonFilters.Click += new System.EventHandler(this.buttonFilters_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Название книги";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.buttonRead);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.CBGenres);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.CBLanguages);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.TBTitle);
            this.panel2.Controls.Add(this.RTBDescription);
            this.panel2.Controls.Add(this.buttonFilters);
            this.panel2.Location = new System.Drawing.Point(0, 111);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(946, 382);
            this.panel2.TabIndex = 5;
            // 
            // buttonRead
            // 
            this.buttonRead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.buttonRead.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonRead.FlatAppearance.BorderSize = 0;
            this.buttonRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRead.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.buttonRead.ForeColor = System.Drawing.Color.White;
            this.buttonRead.Location = new System.Drawing.Point(0, 238);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(946, 72);
            this.buttonRead.TabIndex = 19;
            this.buttonRead.Text = "Начать читать";
            this.buttonRead.UseVisualStyleBackColor = false;
            this.buttonRead.Visible = false;
            this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
            // 
            // buttonManageFile
            // 
            this.buttonManageFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonManageFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.buttonManageFile.FlatAppearance.BorderSize = 0;
            this.buttonManageFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonManageFile.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.buttonManageFile.ForeColor = System.Drawing.Color.White;
            this.buttonManageFile.Location = new System.Drawing.Point(725, 48);
            this.buttonManageFile.Name = "buttonManageFile";
            this.buttonManageFile.Size = new System.Drawing.Size(209, 28);
            this.buttonManageFile.TabIndex = 20;
            this.buttonManageFile.Text = "Локальный поиск книг";
            this.buttonManageFile.UseVisualStyleBackColor = false;
            this.buttonManageFile.Click += new System.EventHandler(this.buttonManageFile_Click);
            // 
            // BookReaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 494);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panel2);
            this.Name = "BookReaderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BookReaderForm";
            this.Load += new System.EventHandler(this.BookReaderForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CBGenres;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CBLanguages;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label labelLogo;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBTitle;
        private System.Windows.Forms.RichTextBox RTBDescription;
        private System.Windows.Forms.Button buttonFilters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonRead;
        private System.Windows.Forms.Button buttonManageFile;
    }
}