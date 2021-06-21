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
    public partial class frmLogin : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public static string Username;
        public static string Password;
        public static string Fullname;
        public static string Usertype;

        public frmLogin()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void picClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var time = DateTime.Now;
            lblTime.Text = time.ToString("h:mm:ss tt");

            lblDate.Text = time.ToLongDateString();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                txtUsername.Focus();
                MessageBox.Show("Username text field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtPassword.Text == "")
            {
                txtPassword.Focus();
                MessageBox.Show("Password text field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            else
            {
                cn.Open();
                cm = new MySqlCommand("SELECT * FROM tbluser WHERE username = @username AND password = @password", cn);
                cm.Parameters.AddWithValue("@username", txtUsername.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    Username = dr["username"].ToString();
                    Password = dr["password"].ToString();
                    Fullname = dr["name"].ToString();
                    Usertype = dr["usertype"].ToString();

                    if (Usertype == "Administrator")
                    {
                        MessageBox.Show("WELCOME, " + frmLogin.Fullname, "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var frm1 = new frmAdminHome();
                        frm1.lblFullName.Text = frmLogin.Fullname;
                        frm1.lblFullName.Text.ToUpper();
                        frm1.lblUsertype.Text = frmLogin.Usertype;
                        frm1.Show();
                        this.Hide();
                    }

                    else if (Usertype == "Cashier")
                    {
                        MessageBox.Show("WELCOME, " + frmLogin.Fullname, "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var frm1 = new frmMain();
                        frm1.GetProductName();
                        frm1.GetTransactionID();
                        frm1.LoadCart();
                        frm1.GetTotal();
                        frm1.CountItems();
                        frm1.lblFullName.Text = frmLogin.Fullname;
                        frm1.lblFullName.Text.ToUpper();
                        frm1.lblUsertype.Text = frmLogin.Usertype;
                        frm1.btnSave.Enabled = false;
                        frm1.Show();
                        this.Hide();
                    }
                }
                else
                {
                    cn.Close();
                    MessageBox.Show("User does not exist", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                dr.Close();
                cn.Close();

                
            }
        }

        private void lblForgetPassword_Click(object sender, EventArgs e)
        {
            var f = new frmPassword();
            f.ShowDialog();
        }
    }
}
