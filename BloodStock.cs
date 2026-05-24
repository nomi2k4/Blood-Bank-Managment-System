using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Blood_Bank
{
    public partial class BloodStock : Form
    {
        public BloodStock()
        {
            InitializeComponent();
            WireUpSidebar();
            this.Load += BloodStock_Load;
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

        private void BloodStock_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT BGroup AS 'Blood Group', BStock AS 'Quantity' FROM BloodTbl";
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                    SqlCommandBuilder builder = new SqlCommandBuilder(sda);
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

    }
}
