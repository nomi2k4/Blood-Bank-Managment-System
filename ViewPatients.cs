using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank
{
    public partial class ViewPatients : Form
    {
        public ViewPatients()
        {
            InitializeComponent();
            WireUpSidebar();
            this.Load += ViewPatients_Load;
            textBox1.TextChanged += textBox1_TextChanged;
            button1.Click += button1_Click;
            button2.Click += button2_Click;
            label1.Click += (s, e) => Application.Exit();
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

        private void ViewPatients_Load(object sender, EventArgs e)
        {
            PopulatePatients();
        }

        private void LoadPatients(string filterName = null)
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT PNum AS 'ID', PName AS 'Name', PAge AS 'Age', PGen AS 'Gender', PPhone AS 'Phone', PAddress AS 'Address', PBGroup AS 'Blood Group' FROM PatientTbl";
                    if (filterName != null)
                        query += " WHERE PName LIKE @name";
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

        private void PopulatePatients()
        {
            LoadPatients();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadPatients(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a patient to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this patient?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
                return;

            int pnum = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["PNum"].Value);

            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM PatientTbl WHERE PNum=@id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", pnum);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Patient Deleted Successfully");
                PopulatePatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a patient to edit.");
                return;
            }

            int pnum = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["PNum"].Value);
            Patient patientForm = new Patient();
            patientForm.PatientId = pnum;
            patientForm.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
