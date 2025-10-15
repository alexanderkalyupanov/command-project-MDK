using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Globalization;

namespace CommandProject.Forms.MainMenu
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();

            // Apply rounded corners to search panel and buttons after initialization
            // Use Resize events to handle designer/layout changes
            this.searchPanel.Resize += (s, e) => RoundControl(this.searchPanel, 24);
            this.buttonSearch.Resize += (s, e) => RoundControl(this.buttonSearch, 16);
            this.buttonFilters.Resize += (s, e) => RoundControl(this.buttonFilters, 20);
            this.buttonProfile.Resize += (s, e) => RoundControl(this.buttonProfile, 20);
            this.buttonSettings.Resize += (s, e) => RoundControl(this.buttonSettings, 16);

            // Ensure initial rounding
            RoundControl(this.searchPanel, 24);
            RoundControl(this.buttonSearch, 16);
            RoundControl(this.buttonFilters, 20);
            RoundControl(this.buttonProfile, 20);
            RoundControl(this.buttonSettings, 16);

            // Optional: make buttons flat and visually consistent
            ConfigureButtonStyle(this.buttonSearch);
            ConfigureButtonStyle(this.buttonFilters);
            ConfigureButtonStyle(this.buttonProfile);
            ConfigureButtonStyle(this.buttonSettings);

            // Load icons for logo and settings
            LoadIcons();
        }

        private void ConfigureButtonStyle(Button btn)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            // Keep BackColor as set in designer
        }

        private void RoundControl(Control ctrl, int radius)
        {
            if (ctrl == null) return;
            int w = Math.Max(ctrl.Width, 1);
            int h = Math.Max(ctrl.Height, 1);

            using (GraphicsPath path = new GraphicsPath())
            {
                int r = Math.Min(radius, Math.Min(w, h));
                path.StartFigure();
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(w - r, 0, r, r, 270, 90);
                path.AddArc(w - r, h - r, r, r, 0, 90);
                path.AddArc(0, h - r, r, r, 90, 90);
                path.CloseFigure();
                // Dispose previous region if any
                try
                {
                    var old = ctrl.Region;
                    ctrl.Region = new Region(path);
                    old?.Dispose();
                }
                catch
                {
                    // ignore region set failures
                }
            }
        }

        // Загрузка иконок: сначала пытаемся загрузить из папки Resources/Icons рядом с exe,
        // затем пробуем найти изображения в Properties.Resources по ключам (если добавлены в .resx).
        private void LoadIcons()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string[] logoCandidates = new[] {
                    Path.Combine(baseDir, "Resources", "Icons", "logo.png"),
                    Path.Combine(baseDir, "Resources", "Icons", "logo.jpg"),
                    Path.Combine(baseDir, "Resources", "Icons", "logo.bmp"),
                    Path.Combine(baseDir, "Resources", "Icons", "logo.ico")
                };
                string[] settingsCandidates = new[] {
                    Path.Combine(baseDir, "Resources", "Icons", "settings.png"),
                    Path.Combine(baseDir, "Resources", "Icons", "settings.jpg"),
                    Path.Combine(baseDir, "Resources", "Icons", "settings.bmp"),
                    Path.Combine(baseDir, "Resources", "Icons", "settings.ico")
                };

                bool loadedLogo = false;
                foreach (var p in logoCandidates)
                {
                    if (File.Exists(p))
                    {
                        try
                        {
                            this.pictureBoxLogo.Image = Image.FromFile(p);
                            loadedLogo = true;
                            break;
                        }
                        catch { }
                    }
                }

                bool loadedSettings = false;
                foreach (var p in settingsCandidates)
                {
                    if (File.Exists(p))
                    {
                        try
                        {
                            this.pictureBoxSettingsIcon.Image = Image.FromFile(p);
                            loadedSettings = true;
                            break;
                        }
                        catch { }
                    }
                }

                // Если не найдено на диске — попробуем найти в Properties.Resources
                if (!loadedLogo || !loadedSettings)
                {
                    try
                    {
                        var rm = Properties.Resources.ResourceManager;

                        // Попробуем напрямую получить по ожидаемым ключам, если вы добавили изображения в Resources.resx
                        if (!loadedLogo)
                        {
                            var imgObj = rm.GetObject("app_logo");
                            if (imgObj is Image img)
                            {
                                this.pictureBoxLogo.Image = img;
                                loadedLogo = true;
                            }
                        }
                        if (!loadedSettings)
                        {
                            var imgObj = rm.GetObject("icon_settins");
                            if (imgObj is Image img)
                            {
                                this.pictureBoxSettingsIcon.Image = img;
                                loadedSettings = true;
                            }
                        }

                        // Если прямые имена не сработали, попробуем перебрать ResourceSet и найти по подстроке
                        if (!loadedLogo || !loadedSettings)
                        {
                            var set = rm.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                            if (set != null)
                            {
                                foreach (DictionaryEntry entry in set)
                                {
                                    string key = (entry.Key as string) ?? string.Empty;
                                    object val = entry.Value;
                                    if (!loadedLogo && key.IndexOf("logo", StringComparison.OrdinalIgnoreCase) >= 0 && val is Image)
                                    {
                                        this.pictureBoxLogo.Image = (Image)val;
                                        loadedLogo = true;
                                    }
                                    if (!loadedSettings && (key.IndexOf("settings", StringComparison.OrdinalIgnoreCase) >= 0 || key.IndexOf("setting", StringComparison.OrdinalIgnoreCase) >= 0 || key.IndexOf("settin", StringComparison.OrdinalIgnoreCase) >= 0) && val is Image)
                                    {
                                        this.pictureBoxSettingsIcon.Image = (Image)val;
                                        loadedSettings = true;
                                    }
                                    if (loadedLogo && loadedSettings) break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        // игнорируем ошибки при чтении .resx
                    }
                }

                // Небольшая настройка: если картинка установлена, сделаем Background белым и рамку как у PictureBox
                if (this.pictureBoxLogo.Image != null)
                {
                    this.pictureBoxLogo.BackColor = Color.White;
                    this.pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
                }
                if (this.pictureBoxSettingsIcon.Image != null)
                {
                    this.pictureBoxSettingsIcon.BackColor = Color.White;
                    this.pictureBoxSettingsIcon.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch
            {
                // не критично — просто не показываем иконки
            }
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная функциональность будет реализована в следующих версиях.", "Настройки");
        }

        private void buttonProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная функциональность будет реализована в следующих версиях.", "Профиль");
        }
        private void buttonFilters_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная функциональность будет реализована в следующих версиях.", "Фильтры");
        }

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("Данная функциональность будет реализована в следующих версиях.", "Поиск");
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная функциональность будет реализована в следующих версиях.", "Поиск");
        }
    }
}
