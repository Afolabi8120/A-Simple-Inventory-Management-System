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
    public partial class frmPassword : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;

        ClassDB db = new ClassDB();
        public frmPassword()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
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
                        cm = new MySqlCommand("UPDATE tbluser SET password=@password WHERE username=@username", cn);
                        cm.Parameters.AddWithValue("@username", txtUsername.Text);
                        cm.Parameters.AddWithValue("@password", txtPassword.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        txtUsername.Focus();
                        MessageBox.Show("Password has been updated successfully!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Dispose();
                    }
                    else
                    {
                        cn.Close();
                        MessageBox.Show("Username not found!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
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
    }
}
