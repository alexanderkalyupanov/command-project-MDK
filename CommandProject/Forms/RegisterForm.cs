using System;
using System.Drawing;
using System.Windows.Forms;
using CommandProject.Database;
using CommandProject.Utils;
using CommandProject; // for ClassConnectDB

namespace CommandProject.Forms
{
    public partial class RegisterForm : Form
    {
        private DatabaseHelper dbHelper;

        // UI Controls
        private Label lblTitle;
        private Label lblUsername;
        private Label lblEmail;
        private Label lblFullName;
        private Label lblPassword;
        private Label lblConfirmPassword;
        private TextBox txtUsername;
        private TextBox txtEmail;
        private TextBox txtFullName;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnRegister;
        private LinkLabel linkLogin;
        private Label lblErrorMessage;

        public RegisterForm()
        {
            InitializeComponent();
            try
            {
                // prefer the application's central connection helper to get correct connection string
                using (var conn = ClassConnectDB.GetOpenConnection())
                {
                    dbHelper = new DatabaseHelper(conn.ConnectionString);
                }
            }
            catch
            {
                // fallback to default DatabaseHelper behavior
                dbHelper = new DatabaseHelper();
            }
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.btnRegister = new System.Windows.Forms.Button();
            this.linkLogin = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(107)))), ((int)(((byte)(53)))));
            this.lblTitle.Location = new System.Drawing.Point(126, 31);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(215, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Регистрация";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.lblUsername.Location = new System.Drawing.Point(50, 100);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(125, 19);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Имя пользователя";
            // 
            // txtUsername
            // 
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtUsername.Location = new System.Drawing.Point(50, 125);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(350, 27);
            this.txtUsername.TabIndex = 2;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.lblEmail.Location = new System.Drawing.Point(50, 170);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(41, 19);
            this.lblEmail.TabIndex = 3;
            this.lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtEmail.Location = new System.Drawing.Point(50, 195);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(350, 27);
            this.txtEmail.TabIndex = 4;
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFullName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.lblFullName.Location = new System.Drawing.Point(50, 240);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(86, 19);
            this.lblFullName.TabIndex = 5;
            this.lblFullName.Text = "Полное имя";
            // 
            // txtFullName
            // 
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtFullName.Location = new System.Drawing.Point(50, 265);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(350, 27);
            this.txtFullName.TabIndex = 6;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.lblPassword.Location = new System.Drawing.Point(50, 310);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 19);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "Пароль";
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPassword.Location = new System.Drawing.Point(50, 335);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '•';
            this.txtPassword.Size = new System.Drawing.Size(350, 27);
            this.txtPassword.TabIndex = 8;
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblConfirmPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.lblConfirmPassword.Location = new System.Drawing.Point(50, 380);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(141, 19);
            this.lblConfirmPassword.TabIndex = 9;
            this.lblConfirmPassword.Text = "Подтвердите пароль";
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtConfirmPassword.Location = new System.Drawing.Point(50, 405);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '•';
            this.txtConfirmPassword.Size = new System.Drawing.Size(350, 27);
            this.txtConfirmPassword.TabIndex = 10;
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.lblErrorMessage.Location = new System.Drawing.Point(50, 450);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(350, 40);
            this.lblErrorMessage.TabIndex = 11;
            this.lblErrorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(107)))), ((int)(((byte)(53)))));
            this.btnRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(50, 500);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(350, 45);
            this.btnRegister.TabIndex = 12;
            this.btnRegister.Text = "Зарегистрироваться";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.BtnRegister_Click);
            // 
            // linkLogin
            // 
            this.linkLogin.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(90)))), ((int)(((byte)(40)))));
            this.linkLogin.AutoSize = true;
            this.linkLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLogin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.linkLogin.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(107)))), ((int)(((byte)(53)))));
            this.linkLogin.Location = new System.Drawing.Point(130, 560);
            this.linkLogin.Name = "linkLogin";
            this.linkLogin.Size = new System.Drawing.Size(165, 19);
            this.linkLogin.TabIndex = 13;
            this.linkLogin.TabStop = true;
            this.linkLogin.Text = "Уже есть аккаунт? Войти";
            this.linkLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLogin_LinkClicked);
            // 
            // RegisterForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(434, 611);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.linkLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Регистрация";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";

            // Валидация имени пользователя
            var usernameValidation = ValidationHelper.ValidateUsername(txtUsername.Text);
            if (!usernameValidation.IsValid)
            {
                ShowError(usernameValidation.Message);
                txtUsername.Focus();
                return;
            }

            // Валидация email
            var emailValidation = ValidationHelper.ValidateEmail(txtEmail.Text);
            if (!emailValidation.IsValid)
            {
                ShowError(emailValidation.Message);
                txtEmail.Focus();
                return;
            }

            // Валидация полного имени
            var fullNameValidation = ValidationHelper.ValidateFullName(txtFullName.Text);
            if (!fullNameValidation.IsValid)
            {
                ShowError(fullNameValidation.Message);
                txtFullName.Focus();
                return;
            }

            // Валидация пароля
            var passwordValidation = ValidationHelper.ValidatePassword(txtPassword.Text);
            if (!passwordValidation.IsValid)
            {
                ShowError(passwordValidation.Message);
                txtPassword.Focus();
                return;
            }

            // Проверка совпадения паролей
            var passwordMatchValidation = ValidationHelper.ValidatePasswordMatch(txtPassword.Text, txtConfirmPassword.Text);
            if (!passwordMatchValidation.IsValid)
            {
                ShowError(passwordMatchValidation.Message);
                txtConfirmPassword.Focus();
                return;
            }

            // Регистрация пользователя
            try
            {
                btnRegister.Enabled = false;
                btnRegister.Text = "Регистрация...";

                var result = dbHelper.RegisterUser(
                    txtUsername.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtPassword.Text,
                    txtFullName.Text.Trim()
                );

                if (result.Success)
                {
                    MessageBox.Show(
                        "Регистрация прошла успешно!\nТеперь вы можете войти в систему.",
                        "Успех",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // Переход на форму входа
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ShowError(result.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка регистрации: {ex.Message}");
            }
            finally
            {
                btnRegister.Enabled = true;
                btnRegister.Text = "Зарегистрироваться";
            }
        }

        private void LinkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ShowError(string message)
        {
            lblErrorMessage.Text = message;
            lblErrorMessage.ForeColor = Color.Red;
        }
    }
}

