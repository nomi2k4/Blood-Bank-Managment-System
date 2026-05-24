using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            button1.Click += button1_Click;
            label5.Click += label5_Click;
            // Hide password characters
            textBox2.PasswordChar = '*';
            // Enter key navigation
            textBox1.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; textBox2.Focus(); } };
            textBox2.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; button1_Click(null, null); } };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Enter Username and Password");
                return;
            }
            
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM EmployeeTbl WHERE EmpId = @user AND EmpPass = @pass";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", textBox1.Text);
                        cmd.Parameters.AddWithValue("@pass", textBox2.Text);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0)
                        {
                            Mainform main = new Mainform();
                            main.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Wrong Username or Password");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
