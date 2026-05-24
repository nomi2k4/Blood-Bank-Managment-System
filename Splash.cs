using System;
using System.Windows.Forms;

namespace Blood_Bank
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            timer1.Tick += timer1_Tick;
            this.Load += Splash_Load;
            label5.Click += (s, e) => Application.Exit();
        }

        int startpoint = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            startpoint += 2;
            guna2CircleProgressBar1.Value = startpoint;
            if (guna2CircleProgressBar1.Value == 100)
            {
                guna2CircleProgressBar1.Value = 0;
                timer1.Stop();
                Login log = new Login();
                log.Show();
                this.Hide();
            }
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
