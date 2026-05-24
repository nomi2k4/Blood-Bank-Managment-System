using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Blood_Bank
{
    public partial class Patient : Form
    {
        public int PatientId { get; set; }

        public Patient()
        {
            InitializeComponent();
            WireUpSidebar();
            button1.Click += button1_Click;
            this.Load += Patient_Load;
            label1.Click += (s, e) => Application.Exit();
            pictureBox1.Click += (s, e) => { new Mainform().Show(); this.Hide(); };
        }

        private void Patient_Load(object sender, EventArgs e)
        {
            if (PatientId > 0)
                LoadPatientForEdit();
        }

        private void LoadPatientForEdit()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM PatientTbl WHERE PNum=@id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", PatientId);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                textBox1.Text = rdr["PName"].ToString();
                                textBox2.Text = rdr["PAge"].ToString();
                                textBox3.Text = rdr["PPhone"].ToString();
                                textBox4.Text = rdr["PAddress"].ToString();
                                comboBox1.SelectedItem = rdr["PGen"].ToString();
                                comboBox2.SelectedItem = rdr["PBGroup"].ToString();
                                if (rdr["PDate"] != DBNull.Value)
                                    guna2DateTimePicker1.Value = Convert.ToDateTime(rdr["PDate"]);
                            }
                        }
                    }
                }
                button1.Text = "Update";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WireUpSidebar()
        {
            label2.Click += (s, e) => { new Donor().Show(); this.Hide(); };
            label3.Click += (s, e) => { new ViewDonor().Show(); this.Hide(); };
            label4.Click += (s, e) => { new Patient().Show(); this.Hide(); };
            label6.Click += (s, e) => { new ViewPatients().Show(); this.Hide(); };
            label5.Click += (s, e) => { new BloodStock().Show(); this.Hide(); };
            label7.Click += (s, e) => { new BloodTransfer().Show(); this.Hide(); };
            label8.Click += (s, e) => { new Dashboard().Show(); this.Hide(); };
            label9.Click += (s, e) => { new Login().Show(); this.Hide(); };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
                return;
            }

            int age;
            if (!int.TryParse(textBox2.Text, out age) || age < 0 || age > 150)
            {
                MessageBox.Show("Please enter a valid age (0-150).");
                return;
            }

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    if (PatientId > 0)
                    {
                        string query = "UPDATE PatientTbl SET PName=@name, PAge=@age, PGen=@gen, PPhone=@phone, PAddress=@address, PBGroup=@bgroup, PDate=@pdate WHERE PNum=@id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", PatientId);
                            cmd.Parameters.AddWithValue("@name", textBox1.Text);
                            cmd.Parameters.AddWithValue("@age", age);
                            cmd.Parameters.AddWithValue("@gen", comboBox1.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@phone", textBox3.Text);
                            cmd.Parameters.AddWithValue("@address", textBox4.Text);
                            cmd.Parameters.AddWithValue("@bgroup", comboBox2.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@pdate", guna2DateTimePicker1.Value);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Patient Successfully Updated");
                    }
                    else
                    {
                        string query = "INSERT INTO PatientTbl (PName, PAge, PGen, PPhone, PAddress, PBGroup, PDate) VALUES (@name, @age, @gen, @phone, @address, @bgroup, @pdate)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@name", textBox1.Text);
                            cmd.Parameters.AddWithValue("@age", age);
                            cmd.Parameters.AddWithValue("@gen", comboBox1.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@phone", textBox3.Text);
                            cmd.Parameters.AddWithValue("@address", textBox4.Text);
                            cmd.Parameters.AddWithValue("@bgroup", comboBox2.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@pdate", guna2DateTimePicker1.Value);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Patient Successfully Saved");
                    }

                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
