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
    public partial class frmCategory : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmCategory()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        public void LoadRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcategory ORDER BY name ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["name"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void lblClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Please enter a category name!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Save Category Name, Click Yes to Confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    cn.Open();
                    cm = new MySqlCommand("SELECT * FROM tblcategory WHERE name=@name", cn);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        dr.Close();
                        cn.Close();
                        MessageBox.Show("Category name already exist!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("INSERT INTO tblcategory(name) VALUES(@name)", cn);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        LoadRecord();
                        txtName.Clear();
                        txtName.Focus();
                        MessageBox.Show("Category has been added successfully!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void frmCategory_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
                if (ColName == "ColDelete")
                {
                    if (MessageBox.Show("Delete Category Name, Click Yes to Confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cn.Open();
                        cm = new MySqlCommand("DELETE FROM tblcategory WHERE name = @name", cn);
                        cm.Parameters.AddWithValue("@name", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                        cm.ExecuteNonQuery();
                        cn.Close();
                        LoadRecord();
                    }
                }
        }
    }
}
