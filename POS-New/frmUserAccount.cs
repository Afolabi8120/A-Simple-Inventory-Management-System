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
    public partial class frmUserAccount : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();
        public frmUserAccount()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        private void Clear()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtCPassword.Clear();
            txtName.Clear();
            cboUsertype.Text = "";
        }

        public void LoadRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tbluser ORDER BY username ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["name"].ToString(), dr["username"].ToString(), dr["usertype"].ToString(), dr["password"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                txtUsername.Focus();
                MessageBox.Show("Username field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (cboUsertype.Text == "")
            {
                cboUsertype.Focus();
                MessageBox.Show("Please select a valid option!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

            if (MessageBox.Show("Save Account, Click Yes to Confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                        MessageBox.Show("Username name already exist!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("INSERT INTO tbluser(username,name,usertype,password) VALUES(@username,@name,@usertype,@password)", cn);
                        cm.Parameters.AddWithValue("@username", txtUsername.Text);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@usertype", cboUsertype.Text);
                        cm.Parameters.AddWithValue("@password", txtPassword.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        LoadRecord();
                        Clear();
                        txtUsername.Focus();
                        MessageBox.Show("Account has been added successfully!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                txtUsername.Focus();
                MessageBox.Show("Username field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (cboUsertype.Text == "")
            {
                cboUsertype.Focus();
                MessageBox.Show("Please select a valid option!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

            if (MessageBox.Show("Save Account, Click Yes to Confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                        MessageBox.Show("Username name already exist!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("UPDATE tbluser SET username=@username,name=@name,usertype=@usertype,password=@password WHERE username=@username", cn);
                        cm.Parameters.AddWithValue("@username", txtUsername.Text);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@usertype", cboUsertype.Text);
                        cm.Parameters.AddWithValue("@password", txtPassword.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        LoadRecord();
                        Clear();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            txtUsername.Focus();
        }

        private void frmUserAccount_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColName == "ColEdit")
            {
                txtUsername.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                cboUsertype.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtPassword.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtCPassword.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
            else if (ColName == "ColDelete")
            {
                if (MessageBox.Show("Delete User Account, Click Yes to Confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tbluser WHERE username = @username", cn);
                    cm.Parameters.AddWithValue("@username", dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadRecord();
                }
            }
        }
    }
}
