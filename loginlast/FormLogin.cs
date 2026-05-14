using Npgsql;
using System;
using System.Windows.Forms;

namespace loginlast
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            string connection = "host=localhost;port=5432;username=postgres;password=navatak_21;database=postgres";

            using (var conn = new NpgsqlConnection(connection))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        u.userid, 
                        r.rolename,
                        u.username,
                        u.userpatronymic
                    FROM ""OBYV"".""User"" u 
                    JOIN ""OBYV"".""role"" r ON u.userrole = r.roleid 
                    WHERE u.userlogin = @username 
                      AND u.userpassword = @password";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("username", txtlogin.Text.Trim());
                    cmd.Parameters.AddWithValue("password", txtpassword.Text);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string fullName = reader.GetString(2); // username
                            if (!reader.IsDBNull(3))
                                fullName += " " + reader.GetString(3);

                            CurrentUser.Instance = new User
                            {
                                Id = reader.GetInt32(0),
                                Role = reader.GetString(1),
                                FullName = fullName.Trim()
                            };

                            OpenMainForm();
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            CurrentUser.Instance = new User
            {
                Id = 0,
                Role = "Гость",
                FullName = "Гость"
            };

            OpenMainForm();
        }

        private void OpenMainForm()
        {
            FormMain mainForm = new FormMain();
            mainForm.Show();
            this.Hide();
        }
    }
}