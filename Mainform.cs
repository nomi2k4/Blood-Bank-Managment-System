using System;
using System.Windows.Forms;

namespace Blood_Bank
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
            WireUpSidebar();
            WireUpCenterButtons();
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

        private void WireUpCenterButtons()
        {
            label10.Click += (s, e) => { new Donor().Show(); this.Hide(); };
            label11.Click += (s, e) => { new ViewDonor().Show(); this.Hide(); };
            label12.Click += (s, e) => { new BloodTransfer().Show(); this.Hide(); };
            label13.Click += (s, e) => { new Patient().Show(); this.Hide(); };
            label14.Click += (s, e) => { new ViewPatients().Show(); this.Hide(); };
            label15.Click += (s, e) => { new BloodStock().Show(); this.Hide(); };
            label16.Click += (s, e) => { new Dashboard().Show(); this.Hide(); };
        }
    }
}
