using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank
{
    public partial class ViewDonor : Form
    {
        public ViewDonor()
        {
            InitializeComponent();
            WireUpSidebar();
            this.Load += ViewDonor_Load;
            textBox1.TextChanged += textBox1_TextChanged;
            button1.Click += button1_Click;
            button2.Click += button2_Click;
            label17.Click += (s, e) => Application.Exit();
            pictureBox1.Click += (s, e) => { new Mainform().Show(); this.Hide(); };
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

        private void ViewDonor_Load(object sender, EventArgs e)
        {
            PopulateDonors();
        }

        private void LoadDonors(string filterName = null)
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT DNum AS 'ID', DName AS 'Name', DAge AS 'Age', DGen AS 'Gender', DPhone AS 'Phone', DAddress AS 'Address', DBGroup AS 'Blood Group' FROM DonorTbl";
                    if (filterName != null)
                        query += " WHERE DName LIKE @name";
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    if (filterName != null)
                        sda.SelectCommand.Parameters.AddWithValue("@name", "%" + filterName + "%");
                    var ds = new DataSet();
                    sda.Fill(ds);
                    guna2DataGridView1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateDonors()
        {
            LoadDonors();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadDonors(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a donor to edit.");
                return;
            }

            int dnum = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["DNum"].Value);
            Donor donorForm = new Donor();
            donorForm.DonorId = dnum;
            donorForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a donor to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this donor?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
                return;

            int dnum = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["DNum"].Value);

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM DonorTbl WHERE DNum=@id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", dnum);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Donor Deleted Successfully");
                PopulateDonors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
