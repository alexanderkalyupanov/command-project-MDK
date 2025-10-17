namespace CommandProject.Forms.MainMenu
{
    partial class MainMenu
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelSeparator;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonFilters;
        private System.Windows.Forms.Button buttonProfile;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label labelLogo;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.PictureBox pictureBoxSettingsIcon;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelBooks;

        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.labelLogo = new System.Windows.Forms.Label();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonFilters = new System.Windows.Forms.Button();
            this.buttonProfile = new System.Windows.Forms.Button();
            this.pictureBoxSettingsIcon = new System.Windows.Forms.PictureBox();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.panelSeparator = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.flowLayoutPanelBooks = new System.Windows.Forms.FlowLayoutPanel();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettingsIcon)).BeginInit();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelTop.Controls.Add(this.pictureBoxLogo);
            this.panelTop.Controls.Add(this.labelLogo);
            this.panelTop.Controls.Add(this.searchPanel);
            this.panelTop.Controls.Add(this.buttonFilters);
            this.panelTop.Controls.Add(this.buttonProfile);
            this.panelTop.Controls.Add(this.pictureBoxSettingsIcon);
            this.panelTop.Controls.Add(this.buttonSettings);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1470, 90);
            this.panelTop.TabIndex = 2;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.BorderStyle = System.Windows.Forms.BorderStyle.None;
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
            // searchPanel
            // 
            this.searchPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.searchPanel.Controls.Add(this.textBoxSearch);
            this.searchPanel.Controls.Add(this.buttonSearch);
            this.searchPanel.Location = new System.Drawing.Point(300, 26);
            this.searchPanel.Margin = new System.Windows.Forms.Padding(2);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Padding = new System.Windows.Forms.Padding(10);
            this.searchPanel.Size = new System.Drawing.Size(760, 38);
            this.searchPanel.TabIndex = 2;
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.textBoxSearch.Location = new System.Drawing.Point(14, 9);
            this.textBoxSearch.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(620, 20);
            this.textBoxSearch.TabIndex = 0;
            this.textBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSearch_KeyDown);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.buttonSearch.FlatAppearance.BorderSize = 0;
            this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buttonSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.buttonSearch.Location = new System.Drawing.Point(648, 6);
            this.buttonSearch.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(96, 26);
            this.buttonSearch.TabIndex = 1;
            this.buttonSearch.Text = "🔍";
            this.buttonSearch.UseVisualStyleBackColor = false;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonFilters
            // 
            this.buttonFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.buttonFilters.FlatAppearance.BorderSize = 0;
            this.buttonFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFilters.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.buttonFilters.ForeColor = System.Drawing.Color.White;
            this.buttonFilters.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonFilters.Location = new System.Drawing.Point(1080, 27);
            this.buttonFilters.Margin = new System.Windows.Forms.Padding(2);
            this.buttonFilters.Name = "buttonFilters";
            this.buttonFilters.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.buttonFilters.Size = new System.Drawing.Size(140, 36);
            this.buttonFilters.TabIndex = 3;
            this.buttonFilters.Text = "Фильтры";
            this.buttonFilters.UseVisualStyleBackColor = false;
            this.buttonFilters.Click += new System.EventHandler(this.buttonFilters_Click);
            // 
            // buttonProfile
            // 
            this.buttonProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.buttonProfile.FlatAppearance.BorderSize = 0;
            this.buttonProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProfile.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.buttonProfile.ForeColor = System.Drawing.Color.White;
            this.buttonProfile.Location = new System.Drawing.Point(1244, 28);
            this.buttonProfile.Margin = new System.Windows.Forms.Padding(2);
            this.buttonProfile.Name = "buttonProfile";
            this.buttonProfile.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.buttonProfile.Size = new System.Drawing.Size(140, 36);
            this.buttonProfile.TabIndex = 5;
            this.buttonProfile.Text = "Профиль";
            this.buttonProfile.UseVisualStyleBackColor = false;
            this.buttonProfile.Click += new System.EventHandler(this.buttonProfile_Click);
            // 
            // pictureBoxSettingsIcon
            // 
            this.pictureBoxSettingsIcon.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxSettingsIcon.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pictureBoxSettingsIcon.Location = new System.Drawing.Point(1402, 12);
            this.pictureBoxSettingsIcon.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxSettingsIcon.Name = "pictureBoxSettingsIcon";
            this.pictureBoxSettingsIcon.Size = new System.Drawing.Size(52, 52);
            this.pictureBoxSettingsIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSettingsIcon.TabIndex = 6;
            this.pictureBoxSettingsIcon.TabStop = false;
            this.pictureBoxSettingsIcon.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // buttonSettings
            // 
            this.buttonSettings.BackColor = System.Drawing.Color.Transparent;
            this.buttonSettings.FlatAppearance.BorderSize = 0;
            this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSettings.Location = new System.Drawing.Point(1542, 27);
            this.buttonSettings.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(44, 36);
            this.buttonSettings.TabIndex = 7;
            this.buttonSettings.UseVisualStyleBackColor = false;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // panelSeparator
            // 
            this.panelSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.panelSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSeparator.Location = new System.Drawing.Point(0, 90);
            this.panelSeparator.Margin = new System.Windows.Forms.Padding(2);
            this.panelSeparator.Name = "panelSeparator";
            this.panelSeparator.Size = new System.Drawing.Size(1470, 8);
            this.panelSeparator.TabIndex = 1;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.White;
            this.panelContent.Controls.Add(this.flowLayoutPanelBooks);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 98);
            this.panelContent.Margin = new System.Windows.Forms.Padding(2);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panelContent.Size = new System.Drawing.Size(1470, 592);
            this.panelContent.TabIndex = 0;
            // 
            // flowLayoutPanelBooks
            // 
            this.flowLayoutPanelBooks.AutoScroll = true;
            this.flowLayoutPanelBooks.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanelBooks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelBooks.Location = new System.Drawing.Point(6, 5);
            this.flowLayoutPanelBooks.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanelBooks.Name = "flowLayoutPanelBooks";
            this.flowLayoutPanelBooks.Size = new System.Drawing.Size(1458, 582);
            this.flowLayoutPanelBooks.TabIndex = 0;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(131)))), ((int)(((byte)(58)))), ((int)(((byte)(180)))));
            this.ClientSize = new System.Drawing.Size(1470, 690);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelSeparator);
            this.Controls.Add(this.panelTop);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainMenu";
            this.Text = "Книгоfeel";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettingsIcon)).EndInit();
            this.panelContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}