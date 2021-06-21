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
    public partial class frmProduct : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmProduct()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        public void GetCategory()
        {
            cboCategory.Items.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcategory ORDER BY name ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboCategory.Items.Add( dr["name"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void Clear()
        {
            txtQuantity.Clear();
            txtCode.Clear();
            txtDescription.Clear();
            txtName.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
            cboCategory.Text = "";
            GetID();
        }

        public void LoadRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct ORDER BY pcode ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["pcode"].ToString(), dr["pname"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["quantity"].ToString(), dr["date"].ToString(), dr["description"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void GetID()
        {
            string num;
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(*) FROM tblproduct", cn);
            num = cm.ExecuteScalar().ToString();
            cn.Close();

            txtCode.Text = "PRO-00" + num.ToString();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == "")
            {
                txtCode.Focus();
                MessageBox.Show("Product code field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtName.Text == "")
            {
                txtName.Focus();
                MessageBox.Show("Product name field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtPrice.Text == "")
            {
                txtPrice.Focus();
                MessageBox.Show("Price field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtQuantity.Text == "")
            {
                txtQuantity.Focus();
                MessageBox.Show("Quantity field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtDescription.Text == "")
            {
                txtDescription.Focus();
                MessageBox.Show("Description field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Save Product, Click Yes to Confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    cn.Open();
                    cm = new MySqlCommand("SELECT * FROM tblproduct WHERE pname=@pname", cn);
                    cm.Parameters.AddWithValue("@pname", txtName.Text);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        dr.Close();
                        cn.Close();
                        MessageBox.Show("Product name already exist!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                        cn.Open();
                        cm = new MySqlCommand("INSERT INTO tblproduct(pcode,pname,price,category,quantity,description,date) VALUES(@pcode,@pname,@price,@category,@quantity,@description,@date)", cn);
                        cm.Parameters.AddWithValue("@pcode", txtCode.Text);
                        cm.Parameters.AddWithValue("@pname", txtName.Text);
                        cm.Parameters.AddWithValue("@price", txtPrice.Text);
                        cm.Parameters.AddWithValue("@category", cboCategory.Text);
                        cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                        cm.Parameters.AddWithValue("@description", txtDescription.Text);
                        cm.Parameters.AddWithValue("@date", DateTime.Now.ToLongDateString());
                        cm.ExecuteNonQuery();
                        cn.Close();
                        LoadRecord();
                        Clear();
                        txtName.Focus();                       
                        MessageBox.Show("Product has been added successfully!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void frmProduct_Load(object sender, EventArgs e)
        {
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == "")
            {
                txtCode.Focus();
                MessageBox.Show("Product code field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtName.Text == "")
            {
                txtName.Focus();
                MessageBox.Show("Product name field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtPrice.Text == "")
            {
                txtPrice.Focus();
                MessageBox.Show("Price field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtQuantity.Text == "")
            {
                txtQuantity.Focus();
                MessageBox.Show("Quantity field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtDescription.Text == "")
            {
                txtDescription.Focus();
                MessageBox.Show("Description field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Save Product, Click Yes to Confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                        cn.Open();
                        cm = new MySqlCommand("UPDATE tblproduct SET pname=@pname,price=@price,quantity=@quantity,description=@description,date=@date WHERE pcode=@pcode", cn);
                        cm.Parameters.AddWithValue("@pcode", txtCode.Text);
                        cm.Parameters.AddWithValue("@pname", txtName.Text);
                        cm.Parameters.AddWithValue("@price", txtPrice.Text);
                        cm.Parameters.AddWithValue("@category", cboCategory.Text);
                        cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                        cm.Parameters.AddWithValue("@description", txtDescription.Text);
                        cm.Parameters.AddWithValue("@date", DateTime.Now.ToLongDateString());
                        cm.ExecuteNonQuery();
                        cn.Close();
                        LoadRecord();
                        Clear();
                        txtName.Focus();
                        MessageBox.Show("Product has been updated successfully!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show("WARNING: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE pname = @pname OR pcode = @pcode", cn);
            cm.Parameters.AddWithValue("@pname", txtSearch.Text);
            cm.Parameters.AddWithValue("@pcode", txtSearch.Text);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["pcode"].ToString(), dr["pname"].ToString(), dr["price"].ToString(), dr["quantity"].ToString(), dr["date"].ToString(), dr["description"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void cboCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColName == "ColEdit")
            {
                txtCode.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();               
                cboCategory.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtQuantity.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            }
            else if (ColName == "ColDelete")
            {
                if (MessageBox.Show("Delete Product, Click Yes to Confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tblproduct WHERE pcode = @pcode", cn);
                    cm.Parameters.AddWithValue("@pcode", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadRecord();
                    GetID();
                }
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                app.Visible = true;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "Records";

                try
                {
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            if (dataGridView1.Rows[i].Cells[j].Value != null)
                            {
                                worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[i + 2, j + 1] = "";
                            }
                        }
                    }

                    //Getting the location and file name of the excel to save from user. 
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    saveDialog.FilterIndex = 2;

                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        workbook.SaveAs(saveDialog.FileName);
                        MessageBox.Show("Export Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                finally
                {
                    app.Quit();
                    workbook = null;
                    worksheet = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
