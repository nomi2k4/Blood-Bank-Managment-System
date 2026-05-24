using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Blood_Bank
{
    public partial class BloodTransfer : Form
    {
        public BloodTransfer()
        {
            InitializeComponent();
            WireUpSidebar();
            this.Load += BloodTransfer_Load;
            comboBox1.SelectionChangeCommitted += comboBox1_SelectionChangeCommitted;
            button2.Click += button2_Click;
            label17.Click += (s, e) => Application.Exit();
            pictureBox1.Click += (s, e) => { new Mainform().Show(); this.Hide(); };
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            // Add Transfer Date label
            Label lblDate = new Label();
            lblDate.Text = "Transfer Date";
            lblDate.Font = new Font("Impact", 15.75F, FontStyle.Regular);
            lblDate.ForeColor = Color.Red;
            lblDate.Location = new System.Drawing.Point(612, 380);
            lblDate.AutoSize = true;
            this.Controls.Add(lblDate);
            lblDate.BringToFront();
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

        private void BloodTransfer_Load(object sender, EventArgs e)
        {
            FillPatientComboBox();
        }

        private void FillPatientComboBox()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT PNum FROM PatientTbl";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("PNum", typeof(int));
                        dt.Load(reader);
                        comboBox1.DisplayMember = "PNum";
                        comboBox1.ValueMember = "PNum";
                        comboBox1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int stock = 0;

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null) return;

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM PatientTbl WHERE PNum=@pnum";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@pnum", comboBox1.SelectedValue.ToString());
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        foreach (DataRow dr in dt.Rows)
                        {
                            textBox1.Text = dr["PName"].ToString();
                            textBox2.Text = dr["PBGroup"].ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(textBox2.Text)) return;

                    string stockQuery = "SELECT * FROM BloodTbl WHERE BGroup=@bgroup";
                    using (SqlCommand cmdStock = new SqlCommand(stockQuery, conn))
                    {
                        cmdStock.Parameters.AddWithValue("@bgroup", textBox2.Text);
                        SqlDataAdapter sdaStock = new SqlDataAdapter(cmdStock);
                        DataTable dtStock = new DataTable();
                        sdaStock.Fill(dtStock);
                        foreach (DataRow dr in dtStock.Rows)
                        {
                            stock = Convert.ToInt32(dr["BStock"].ToString());
                        }

                        if (stock > 0)
                        {
                            label14.Text = "Available Stock: " + stock;
                            label14.ForeColor = Color.Green;
                            button2.Visible = true;
                        }
                        else
                        {
                            label14.Text = "Stock Not Available";
                            label14.ForeColor = Color.Red;
                            button2.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Missing Information");
                return;
            }

            if (stock > 0)
            {
                try
                {
                    using (SqlConnection conn = DbConnection.GetConnection())
                    {
                        conn.Open();
                        string queryUpdate = "UPDATE BloodTbl SET BStock = BStock - 1 WHERE BGroup = @bgroup";
                        using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@bgroup", textBox2.Text);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        string queryInsert = "INSERT INTO TransferTbl (PName, BGroup, TDate) VALUES (@pname, @bgroup, @tdate)";
                        using (SqlCommand cmdInsert = new SqlCommand(queryInsert, conn))
                        {
                            cmdInsert.Parameters.AddWithValue("@pname", textBox1.Text);
                            cmdInsert.Parameters.AddWithValue("@bgroup", textBox2.Text);
                            cmdInsert.Parameters.AddWithValue("@tdate", guna2DateTimePicker1.Value);
                            cmdInsert.ExecuteNonQuery();
                        }

                        MessageBox.Show("Successfully Transferred");

                        textBox1.Text = "";
                        textBox2.Text = "";
                        label14.Text = "Availability?";
                        label14.ForeColor = Color.Red;
                        button2.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Not enough stock available.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
