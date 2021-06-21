using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace POS_New
{
    public partial class frmAdminHome : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmAdminHome()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            var frm1 = new frmSalesRecord();
            frm1.GetAll();
            frm1.ShowDialog();
        }

        private void btnMyAccount_Click(object sender, EventArgs e)
        {
            var frm1 = new frmMyAccount();
            frm1.GetInfo();
            frm1.ShowDialog();
        }

        private void btnUserAccount_Click(object sender, EventArgs e)
        {
            var frm1 = new frmUserAccount();
            frm1.LoadRecord();
            frm1.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm1 = new frmCategory();
            frm1.LoadRecord();
            frm1.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var frm1 = new frmProduct();
            frm1.GetID();
            frm1.GetCategory();
            frm1.LoadRecord();
            frm1.ShowDialog();
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            //var frm1 = new frmMain();
            //frm1.GetProductName();
            //frm1.GetTransactionID();
            //frm1.GetTotal();
            //frm1.LoadCart();            
            //frm1.CountItems();
            //frm1.btnSave.Enabled = false;
            //frm1.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Logout? Click Yes to Confirm!", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {              
                var frm1 = new frmLogin();
                frm1.Show();
                this.Hide();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var time = DateTime.Now;
            lblTime.Text = time.ToString("h:mm:ss tt");

            lblDate.Text = time.ToLongDateString();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            var f1 = new frmSystemSettings();
            f1.GetInfo();
            f1.ShowDialog();
        }

        private void btnAbout_Click_1(object sender, EventArgs e)
        {
            var f = new frmAbout();
            f.ShowDialog();
        }
    }
}
