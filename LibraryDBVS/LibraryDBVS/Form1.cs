using System;
using System.Windows.Forms;
using System.IO;

namespace LibraryDBVS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

            string path = "users.txt";
            string[] lines = File.ReadAllLines(path);

            bool found = false;
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts[0] == username && parts[1] == password)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                MainForm main = new MainForm();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong username or password.");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
