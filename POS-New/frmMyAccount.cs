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
    public partial class frmMyAccount : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        ClassDB db = new ClassDB();
        public frmMyAccount()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        public void GetInfo()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tbluser WHERE username=@username", cn);
            cm.Parameters.AddWithValue("@username", frmLogin.Username);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                txtUsername.Text = dr["username"].ToString();
                txtName.Text = dr["name"].ToString();
                txtPassword.Text = dr["password"].ToString();
                txtCPassword.Text = dr["password"].ToString();
            }
            else
            {
                txtUsername.Text = "";
                txtName.Text = "";
                txtPassword.Text = "";
                txtCPassword.Text = "";
            }
            dr.Close();
            cn.Close();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                txtUsername.Focus();
                MessageBox.Show("Username field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtName.Text == "")
            {
                txtName.Focus();
                MessageBox.Show("Product name field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtPassword.Text == "")
            {
                txtPassword.Focus();
                MessageBox.Show("Password field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtPassword.Text != txtCPassword.Text)
            {
                MessageBox.Show("Password do not match!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Update Account, Click Yes to Confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    cn.Open();
                    cm = new MySqlCommand("SELECT * FROM tbluser WHERE username=@username", cn);
                    cm.Parameters.AddWithValue("@username", txtUsername.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        dr.Close();
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("UPDATE tbluser SET username=@username,name=@name,password=@password WHERE username=@username", cn);
                        cm.Parameters.AddWithValue("@username", txtUsername.Text);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@password", txtPassword.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        txtUsername.Focus();
                        MessageBox.Show("Account has been updated successfully!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                    dr.Close();
                    cn.Close();

                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show("WARNING: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void frmMyAccount_Load(object sender, EventArgs e)
        {
            GetInfo();
        }
    }
}
