using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Blood_Bank
{
    public partial class Donor : Form
    {
        public int DonorId { get; set; }

        public Donor()
        {
            InitializeComponent();
            WireUpSidebar();
            button1.Click += button1_Click;
            this.Load += Donor_Load;
            label1.Click += (s, e) => Application.Exit();
            pictureBox1.Click += (s, e) => { new Mainform().Show(); this.Hide(); };
        }

        private void Donor_Load(object sender, EventArgs e)
        {
            if (DonorId > 0)
                LoadDonorForEdit();
        }

        private void LoadDonorForEdit()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM DonorTbl WHERE DNum=@id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", DonorId);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                textBox1.Text = rdr["DName"].ToString();
                                textBox2.Text = rdr["DAge"].ToString();
                                textBox3.Text = rdr["DPhone"].ToString();
                                textBox4.Text = rdr["DAddress"].ToString();
                                comboBox1.SelectedItem = rdr["DGen"].ToString();
                                comboBox2.SelectedItem = rdr["DBGroup"].ToString();
                                if (rdr["DDate"] != DBNull.Value)
                                    guna2DateTimePicker1.Value = Convert.ToDateTime(rdr["DDate"]);
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
                    if (DonorId > 0)
                    {
                        string oldGroupQuery = "SELECT DBGroup FROM DonorTbl WHERE DNum=@id";
                        string oldGroup = "";
                        using (SqlCommand cmdOld = new SqlCommand(oldGroupQuery, conn))
                        {
                            cmdOld.Parameters.AddWithValue("@id", DonorId);
                            oldGroup = cmdOld.ExecuteScalar()?.ToString();
                        }

                        string query = "UPDATE DonorTbl SET DName=@name, DAge=@age, DGen=@gen, DPhone=@phone, DAddress=@address, DBGroup=@bgroup, DDate=@ddate WHERE DNum=@id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", DonorId);
                            cmd.Parameters.AddWithValue("@name", textBox1.Text);
                            cmd.Parameters.AddWithValue("@age", age);
                            cmd.Parameters.AddWithValue("@gen", comboBox1.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@phone", textBox3.Text);
                            cmd.Parameters.AddWithValue("@address", textBox4.Text);
                            cmd.Parameters.AddWithValue("@bgroup", comboBox2.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@ddate", guna2DateTimePicker1.Value);
                            cmd.ExecuteNonQuery();
                        }

                        string newGroup = comboBox2.SelectedItem.ToString();
                        if (oldGroup != newGroup)
                        {
                            string decStock = "UPDATE BloodTbl SET BStock = BStock - 1 WHERE BGroup = @oldGroup";
                            using (SqlCommand cmdDec = new SqlCommand(decStock, conn))
                            {
                                cmdDec.Parameters.AddWithValue("@oldGroup", oldGroup);
                                cmdDec.ExecuteNonQuery();
                            }
                            string incStock = "UPDATE BloodTbl SET BStock = BStock + 1 WHERE BGroup = @newGroup";
                            using (SqlCommand cmdInc = new SqlCommand(incStock, conn))
                            {
                                cmdInc.Parameters.AddWithValue("@newGroup", newGroup);
                                cmdInc.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Donor Successfully Updated");
                    }
                    else
                    {
                        string query = "INSERT INTO DonorTbl (DName, DAge, DGen, DPhone, DAddress, DBGroup, DDate) VALUES (@name, @age, @gen, @phone, @address, @bgroup, @ddate)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@name", textBox1.Text);
                            cmd.Parameters.AddWithValue("@age", age);
                            cmd.Parameters.AddWithValue("@gen", comboBox1.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@phone", textBox3.Text);
                            cmd.Parameters.AddWithValue("@address", textBox4.Text);
                            cmd.Parameters.AddWithValue("@bgroup", comboBox2.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@ddate", guna2DateTimePicker1.Value);
                            cmd.ExecuteNonQuery();
                        }

                        string stockQuery = "UPDATE BloodTbl SET BStock = BStock + 1 WHERE BGroup = @bgroupStock";
                        using (SqlCommand cmdStock = new SqlCommand(stockQuery, conn))
                        {
                            cmdStock.Parameters.AddWithValue("@bgroupStock", comboBox2.SelectedItem.ToString());
                            cmdStock.ExecuteNonQuery();
                        }

                        MessageBox.Show("Donor Successfully Saved");
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
