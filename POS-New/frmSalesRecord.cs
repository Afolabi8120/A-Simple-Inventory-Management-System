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
    public partial class frmSalesRecord : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmSalesRecord()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        public void GetAll()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblpayment ORDER BY transactionid ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["transactionid"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["mchange"].ToString(), dr["paymenttype"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["cname"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void GetTotal()
        {
            if(frmLogin.Usertype == "Administrator")
            {
                string num;
                cn.Open();
                cm = new MySqlCommand("SELECT SUM(total) FROM tblpayment", cn);
                num = cm.ExecuteScalar().ToString();
                cn.Close();

                lblTotal.Text = num;
            }
            else
            {
                string num;
                cn.Open();
                cm = new MySqlCommand("SELECT SUM(total) FROM tblpayment WHERE cname = @cname", cn);
                cm.Parameters.AddWithValue("@cname", frmLogin.Username);
                num = cm.ExecuteScalar().ToString();
                cn.Close();

                lblTotal.Text = num;
            }
        }

        public void GetRecord()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblpayment WHERE cname = @cname ORDER BY transactionid ASC", cn);
            cm.Parameters.AddWithValue("@cname", frmLogin.Username);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["transactionid"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["mchange"].ToString(), dr["paymenttype"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["cname"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblpayment WHERE transactionid = @transactionid", cn);
            cm.Parameters.AddWithValue("@transactionid", txtName.Text);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["transactionid"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["mchange"].ToString(), dr["paymenttype"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["cname"].ToString());
            }
            dr.Close();
            cn.Close();

            string num;
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(total) FROM tblpayment WHERE transactionid = @transactionid", cn);
            cm.Parameters.AddWithValue("@transactionid", txtName.Text);
            num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblTotal.Text = num;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcart WHERE date BETWEEN '" + dtFrome.Value + "' AND '" + dtFrome.Value + "'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["transactionid"].ToString(), dr["total"].ToString(), dr["amountpaid"].ToString(), dr["mchange"].ToString(), dr["paymenttype"].ToString(), dr["date"].ToString(), dr["time"].ToString(), dr["cname"].ToString());
            }
            dr.Close();
            cn.Close();

            GetTotal();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetRecord();
            GetTotal();
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

        private void frmSalesRecord_Load(object sender, EventArgs e)
        {
            GetTotal();
        }
    }
}
