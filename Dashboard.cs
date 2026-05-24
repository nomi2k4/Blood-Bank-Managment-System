using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Blood_Bank
{
    public partial class Dashboard : Form
    {
        private Label lblDonors;
        private Label lblPatients;
        private Label lblStock;
        private Label lblTransfers;
        private DateTimePicker monthPicker;

        public Dashboard()
        {
            InitializeComponent();
            WireUpSidebar();
            CreateStatCards();
            this.Load += Dashboard_Load;
            label17.Click += (s, e) => Application.Exit();
            pictureBox1.Click += (s, e) => { new Mainform().Show(); this.Hide(); };
            AddReportButton();
        }

        private void CreateStatCards()
        {
            string[][] statData = new string[][] {
                new string[] { "Total Donors", "lblDonors" },
                new string[] { "Total Patients", "lblPatients" },
                new string[] { "Blood Stock (Units)", "lblStock" },
                new string[] { "Total Transfers", "lblTransfers" }
            };
            int[] xs = { 260, 670, 260, 670 };
            int[] ys = { 280, 280, 460, 460 };
            Color[] colors = {
                Color.FromArgb(100, 88, 255),
                Color.FromArgb(0, 192, 128),
                Color.FromArgb(220, 80, 80),
                Color.FromArgb(255, 160, 0)
            };

            for (int i = 0; i < 4; i++)
            {
                Panel card = new Panel();
                card.Size = new Size(370, 150);
                card.Location = new Point(xs[i], ys[i]);
                card.BackColor = colors[i];

                Label desc = new Label();
                desc.Text = statData[i][0];
                desc.ForeColor = Color.White;
                desc.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                desc.Location = new Point(15, 15);
                desc.AutoSize = true;

                Label value = new Label();
                value.Name = statData[i][1];
                value.Text = "0";
                value.ForeColor = Color.White;
                value.Font = new Font("Segoe UI", 48F, FontStyle.Bold);
                value.Location = new Point(15, 50);
                value.AutoSize = true;

                card.Controls.Add(desc);
                card.Controls.Add(value);
                this.Controls.Add(card);
                card.BringToFront();
            }

            lblDonors = (Label)Controls.Find("lblDonors", true)[0];
            lblPatients = (Label)Controls.Find("lblPatients", true)[0];
            lblStock = (Label)Controls.Find("lblStock", true)[0];
            lblTransfers = (Label)Controls.Find("lblTransfers", true)[0];
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM DonorTbl", conn))
                        lblDonors.Text = cmd.ExecuteScalar().ToString();
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PatientTbl", conn))
                        lblPatients.Text = cmd.ExecuteScalar().ToString();
                    using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(BStock), 0) FROM BloodTbl", conn))
                        lblStock.Text = cmd.ExecuteScalar().ToString();
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM TransferTbl", conn))
                        lblTransfers.Text = cmd.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddReportButton()
        {
            Button btnReport = new Button();
            btnReport.Text = "Generate Report";
            btnReport.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnReport.BackColor = Color.FromArgb(231, 76, 60);
            btnReport.ForeColor = Color.White;
            btnReport.FlatStyle = FlatStyle.Flat;
            btnReport.FlatAppearance.BorderSize = 0;
            btnReport.Size = new Size(180, 60);
            btnReport.Location = new Point(1055, 280);
            btnReport.Click += btnReport_Click;
            this.Controls.Add(btnReport);
            btnReport.BringToFront();

            monthPicker = new DateTimePicker();
            monthPicker.Format = DateTimePickerFormat.Custom;
            monthPicker.CustomFormat = "yyyy-MM";
            monthPicker.ShowUpDown = true;
            monthPicker.Size = new Size(180, 30);
            monthPicker.Location = new Point(1055, 370);
            monthPicker.Font = new Font("Segoe UI", 12F);
            monthPicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            this.Controls.Add(monthPicker);
            monthPicker.BringToFront();

            Label lblMonth = new Label();
            lblMonth.Text = "Select Month";
            lblMonth.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMonth.ForeColor = Color.Red;
            lblMonth.Location = new Point(1055, 345);
            lblMonth.AutoSize = true;
            this.Controls.Add(lblMonth);
            lblMonth.BringToFront();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                string donorData = "", patientData = "", stockData = "", transferData = "";
                DateTime monthStart = new DateTime(monthPicker.Value.Year, monthPicker.Value.Month, 1);
                DateTime monthEnd = monthStart.AddMonths(1);

                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    SqlDataAdapter daDonor = new SqlDataAdapter("SELECT DNum, DName, DAge, DGen, DPhone, DAddress, DBGroup, DDate FROM DonorTbl WHERE DDate >= @start AND DDate < @end", conn);
                    daDonor.SelectCommand.Parameters.AddWithValue("@start", monthStart);
                    daDonor.SelectCommand.Parameters.AddWithValue("@end", monthEnd);
                    DataTable dtDonor = new DataTable();
                    daDonor.Fill(dtDonor);
                    foreach (DataRow r in dtDonor.Rows)
                        donorData += "<tr><td>" + r["DNum"] + "</td><td>" + r["DName"] + "</td><td>" + r["DAge"] + "</td><td>" + r["DGen"] + "</td><td>" + r["DPhone"] + "</td><td>" + r["DAddress"] + "</td><td>" + r["DBGroup"] + "</td><td>" + Convert.ToDateTime(r["DDate"]).ToString("yyyy-MM-dd") + "</td></tr>";

                    SqlDataAdapter daPatient = new SqlDataAdapter("SELECT PNum, PName, PAge, PGen, PAddress, PBGroup, PDate FROM PatientTbl WHERE PDate >= @start AND PDate < @end", conn);
                    daPatient.SelectCommand.Parameters.AddWithValue("@start", monthStart);
                    daPatient.SelectCommand.Parameters.AddWithValue("@end", monthEnd);
                    DataTable dtPatient = new DataTable();
                    daPatient.Fill(dtPatient);
                    foreach (DataRow r in dtPatient.Rows)
                        patientData += "<tr><td>" + r["PNum"] + "</td><td>" + r["PName"] + "</td><td>" + r["PAge"] + "</td><td>" + r["PGen"] + "</td><td>" + r["PAddress"] + "</td><td>" + r["PBGroup"] + "</td><td>" + Convert.ToDateTime(r["PDate"]).ToString("yyyy-MM-dd") + "</td></tr>";

                    SqlDataAdapter daStock = new SqlDataAdapter("SELECT BGroup, BStock FROM BloodTbl", conn);
                    DataTable dtStock = new DataTable();
                    daStock.Fill(dtStock);
                    foreach (DataRow r in dtStock.Rows)
                        stockData += "<tr><td>" + r["BGroup"] + "</td><td>" + r["BStock"] + "</td></tr>";

                    SqlDataAdapter daTransfer = new SqlDataAdapter("SELECT TNum, PName, BGroup FROM TransferTbl WHERE TDate >= @start AND TDate < @end", conn);
                    daTransfer.SelectCommand.Parameters.AddWithValue("@start", monthStart);
                    daTransfer.SelectCommand.Parameters.AddWithValue("@end", monthEnd);
                    DataTable dtTransfer = new DataTable();
                    daTransfer.Fill(dtTransfer);
                    foreach (DataRow r in dtTransfer.Rows)
                        transferData += "<tr><td>" + r["TNum"] + "</td><td>" + r["PName"] + "</td><td>" + r["BGroup"] + "</td></tr>";
                }

                string logoBase64 = "";
                if (pictureBox1.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBox1.Image.Save(ms, ImageFormat.Png);
                        logoBase64 = Convert.ToBase64String(ms.ToArray());
                    }
                }

                string monthLabel = monthStart.ToString("MMMM yyyy");
                string date = DateTime.Now.ToString("dddd, MMMM dd, yyyy hh:mm tt");
                string html = @"<!DOCTYPE html>
<html><head><meta charset='UTF-8'>
<title>Blood Bank Report - " + monthLabel + @"</title>
<style>
body{font-family:Arial,sans-serif;margin:30px;color:#333;}
.logo{text-align:center;margin-bottom:10px;}
.logo img{width:120px;height:120px;}
h1{color:#c0392b;text-align:center;border-bottom:3px solid #c0392b;padding-bottom:10px;}
h2{color:#e74c3c;margin-top:30px;}
.date{color:#888;margin-bottom:20px;text-align:center;}
table{width:100%;border-collapse:collapse;margin:10px 0 20px;}
th{background:#e74c3c;color:#fff;padding:10px;text-align:left;}
td{padding:8px 10px;border:1px solid #ddd;}
tr:nth-child(even){background:#f9f9f9;}
.footer{margin-top:40px;padding-top:15px;border-top:1px solid #ccc;font-size:12px;color:#888;text-align:center;}
</style></head><body>
<div class='logo'><img src='data:image/png;base64," + logoBase64 + @"' alt='Logo'></div>
<h1>Blood Bank Management System : Monthly Report</h1>
<div class='date'>Month: " + monthLabel + " | Generated: " + date + @"</div>
<h2>Donors (This Month)</h2>
<table><thead><tr><th>ID</th><th>Name</th><th>Age</th><th>Gender</th><th>Phone</th><th>Address</th><th>Blood Group</th><th>Date</th></tr></thead><tbody>" + donorData + @"</tbody></table>
<h2>Patients (This Month)</h2>
<table><thead><tr><th>ID</th><th>Name</th><th>Age</th><th>Gender</th><th>Address</th><th>Blood Group</th><th>Date</th></tr></thead><tbody>" + patientData + @"</tbody></table>
<h2>Blood Stock</h2>
<table><thead><tr><th>Blood Group</th><th>Quantity</th></tr></thead><tbody>" + stockData + @"</tbody></table>
<h2>Transfers (This Month)</h2>
<table><thead><tr><th>ID</th><th>Patient Name</th><th>Blood Group</th></tr></thead><tbody>" + transferData + @"</tbody></table>
<div class='footer'>Blood Bank Management System &copy; " + DateTime.Now.Year + @"</div>
</body></html>";

                string htmlPath = Path.Combine(Path.GetTempPath(), "BloodBankReport.html");
                File.WriteAllText(htmlPath, html);
                Process.Start(htmlPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message);
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

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
