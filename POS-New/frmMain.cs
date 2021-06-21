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
    public partial class frmMain : Form
    {
        MySqlConnection cn;
        MySqlCommand cm;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmMain()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        public void GetProductName()
        {
            cboProductName.Items.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct ORDER BY pname ASC", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cboProductName.Items.Add(dr["pname"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void GetStock()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE pcode = @pcode", cn);
            cm.Parameters.AddWithValue("@pcode", txtPCode.Text);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {                
                txtStock.Text = dr["quantity"].ToString();
            }
            dr.Close();
            cn.Close();
        }

        public void LoadCart()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblcart WHERE transactionid = @transactionid", cn);
            cm.Parameters.AddWithValue("@transactionid", lblTransactionID.Text);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["pname"].ToString(), dr["price"].ToString(), dr["quantity"].ToString(), dr["total"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void GetTotal()
        {
            string num;
            cn.Open();
            cm = new MySqlCommand("SELECT SUM(total) FROM tblcart WHERE transactionid = @transactionid", cn);
            cm.Parameters.AddWithValue("@transactionid", lblTransactionID.Text);
            num = cm.ExecuteScalar().ToString();
            cn.Close();

            lblTotal.Text = num;
        }

        public void GetTransactionID()
        {
            string num;
            cn.Open();
            cm = new MySqlCommand("SELECT COUNT(DISTINCT(transactionid)) FROM tblcart", cn);
            num = cm.ExecuteScalar().ToString();
            cn.Close();

            string _year = DateTime.Now.Year.ToString();
            string _month = DateTime.Now.Month.ToString();

            lblTransactionID.Text = _year + "-" + _month + "-P1" + num.ToString();
        }

        public void GetProductInfo()
        {
            cn.Close();
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblproduct WHERE pname = @pname", cn);
            cm.Parameters.AddWithValue("@pname", cboProductName.Text);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {                
                txtPCode.Text = dr["pcode"].ToString();
                txtName.Text = dr["pname"].ToString();
                txtPrice.Text = dr["price"].ToString();
                txtStock.Text = dr["quantity"].ToString();
                txtDescription.Text = dr["description"].ToString();
                txtCategory.Text = dr["category"].ToString();
            }
            else
            {
                txtPCode.Text = "";
                txtName.Text = "";
                txtPrice.Text = "";
                txtStock.Text = "";
                txtDescription.Text = "";
                txtCategory.Text = "";
            }
            dr.Close();
            cn.Close();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblTotal.Text = "0.00";
        }

        private void cboProductName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var time = DateTime.Now;
            lblTime.Text = time.ToString("h:mm:ss tt");

            lblDate.Text = time.ToLongDateString();
        }

        private void cboProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProductInfo();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (Convert.ToInt32(txtQuantity.Text) > Convert.ToInt32(txtStock.Text))
            {
                MessageBox.Show("Unable to Proceed!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtQuantity.Text == "")
            {
                txtQuantity.Text = "1";
            }

            if (txtPCode.Text == "")
            {
                txtPCode.Focus();
                MessageBox.Show("Product code not found!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                MessageBox.Show("Product Price field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtStock.Text == "")
            {
                txtStock.Focus();
                MessageBox.Show("Product Stock field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtDescription.Text == "")
            {
                txtDescription.Focus();
                MessageBox.Show("Product Description field is empty!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Convert.ToInt32(txtQuantity.Text) <= 0)
            {
                txtQuantity.Focus();
                MessageBox.Show("Please select a valid quantity!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Convert.ToInt32(txtStock.Text) <= 0)
            {
                txtStock.Focus();
                MessageBox.Show("You are out of Stock!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Add Item! Click Yes to confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            {
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        cn.Open();
                        cm = new MySqlCommand("SELECT * FROM tblcart WHERE transactionid = @transactionid AND pcode = @pcode", cn);
                        cm.Parameters.AddWithValue("@transactionid", lblTransactionID.Text);
                        cm.Parameters.AddWithValue("@pcode", txtPCode.Text);
                        dr = cm.ExecuteReader();
                        dr.Read();
                        if (dr.HasRows)
                        {
                            dr.Close();
                            cm = new MySqlCommand("UPDATE tblcart SET quantity = quantity + '" + txtQuantity.Text + "',total = price * quantity WHERE transactionid = @transactionid AND pcode = @pcode", cn);
                            cm.Parameters.AddWithValue("@transactionid", lblTransactionID.Text);
                            cm.Parameters.AddWithValue("@pcode", txtPCode.Text);
                            cm.ExecuteNonQuery();
                            cn.Close();

                            cn.Open();
                            cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity - '" + txtQuantity.Text + "' WHERE pcode = @pcode", cn);
                            cm.Parameters.AddWithValue("@pcode", txtPCode.Text);
                            cm.ExecuteNonQuery();
                            cn.Close();

                            CountItems();
                            LoadCart();
                            GetStock();
                            GetTotal();
                        }
                        else
                        {
                            cn.Close();
                            cn.Open();
                            cm = new MySqlCommand("INSERT INTO tblcart (transactionid,pcode,pname,price,quantity,total,date,time,cashiername,status) VALUES(@transactionid,@pcode,@pname,@price,@quantity,@total,@date,@time,@cashiername,@status)", cn);
                            cm.Parameters.AddWithValue("@transactionid", lblTransactionID.Text);
                            cm.Parameters.AddWithValue("@pcode", txtPCode.Text);
                            cm.Parameters.AddWithValue("@pname", txtName.Text);
                            cm.Parameters.AddWithValue("@price", txtPrice.Text);
                            cm.Parameters.AddWithValue("@quantity", txtQuantity.Text);
                            cm.Parameters.AddWithValue("@total", double.Parse(txtPrice.Text) * double.Parse(txtQuantity.Text));
                            cm.Parameters.AddWithValue("@date", lblDate.Text);
                            cm.Parameters.AddWithValue("@time", lblTime.Text);
                            cm.Parameters.AddWithValue("@cashiername", lblFullName.Text);
                            cm.Parameters.AddWithValue("@status", "Pending");
                            cm.ExecuteNonQuery();
                            cn.Close();

                            cn.Open();
                            cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity - '" + txtQuantity.Text + "' WHERE pcode = @pcode", cn);
                            cm.Parameters.AddWithValue("@pcode", txtPCode.Text);
                            cm.ExecuteNonQuery();
                            cn.Close();

                            CountItems();
                            LoadCart();
                            GetStock();
                            GetTotal();
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

        public void CountItems()
        {
            string num = dataGridView1.Rows.Count.ToString();

            if (dataGridView1.Rows.Count.ToString() != "")
            {
                btnSave.Enabled = true;
                //string a = " List of Items in cart: " + dataGridView1.Rows.Count.ToString();
                //lblCount.Text = a.ToString();
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
            string _qty = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            string _name = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            if (ColName == "ColDelete")
            {
                cn.Open();
                cm = new MySqlCommand("DELETE FROM tblcart WHERE pname = @pname AND transactionid = @transactionid", cn);
                cm.Parameters.AddWithValue("@pname", _name);
                cm.Parameters.AddWithValue("@transactionid", lblTransactionID.Text);
                cm.ExecuteNonQuery();
                cn.Close();

                cn.Open();
                cm = new MySqlCommand("UPDATE tblproduct SET quantity = quantity + '" + _qty + "' WHERE pname = @pname", cn);
                cm.Parameters.AddWithValue("@pname", _name);
                cm.ExecuteNonQuery();
                cn.Close();
                LoadCart();
                CountItems();
                GetStock();
                GetTotal();

                if (dataGridView1.Rows.Count == 0)
                {
                    btnSave.Enabled = false;
                    string a = " List of Items in cart: " + "0";
                    lblCount.Text = a.ToString();
                }
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            double mychange = Convert.ToDouble(txtChange.Text);
             mychange = Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(lblTotal.Text);

            txtChange.Text = mychange.ToString("#,0.00");

            if (cboPaymentType.Text == "")
            {
                MessageBox.Show("Please select payment type", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtAmount.Text == "")
            {
                MessageBox.Show("Please enter an amount to pay", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToDouble(txtAmount.Text) < Convert.ToDouble(lblTotal.Text))
            {
                MessageBox.Show("Unable to process Transaction!\n Insufficient Amount", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Continue with Transaction? Click Yes to confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                {
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            cn.Open();
                            cm = new MySqlCommand("INSERT INTO tblpayment (transactionid,amountpaid,mchange,paymenttype,date,time,total,cname) VALUES(@transactionid,@amountpaid,@mchange,@paymenttype,@date,@time,@total,@cname)", cn);
                            cm.Parameters.AddWithValue("@transactionid", lblTransactionID.Text);
                            cm.Parameters.AddWithValue("@amountpaid", txtAmount.Text);
                            cm.Parameters.AddWithValue("@mchange", txtChange.Text);
                            cm.Parameters.AddWithValue("@paymenttype", cboPaymentType.Text);
                            cm.Parameters.AddWithValue("@date", lblDate.Text);
                            cm.Parameters.AddWithValue("@time", lblTime.Text);
                            cm.Parameters.AddWithValue("@total", lblTotal.Text);
                            cm.Parameters.AddWithValue("@cname", frmLogin.Username);
                            cm.ExecuteNonQuery();
                            cn.Close();

                            cn.Open();
                            cm = new MySqlCommand("UPDATE tblcart SET status = @status WHERE transactionid = @transactionid", cn);
                            cm.Parameters.AddWithValue("@status", "Completed");
                            cm.Parameters.AddWithValue("@transactionid", lblTransactionID.Text);
                            cm.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Payment Successful", "SUCCESS", MessageBoxButtons.OK, MessageBoxIcon.Information);                           
                            cboPaymentType.Text = " ";
                            GetTransactionID();
                            LoadCart();
                            CountItems();
                            txtAmount.Text = "0.00";
                            txtChange.Text = "0.00";
                            lblTotal.Text = "0.00";
                            dataGridView1.Rows.Clear();
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

        private void btnMyAccount_Click(object sender, EventArgs e)
        {
            var frm1 = new frmMyAccount();
            frm1.GetInfo();
            frm1.ShowDialog();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            var frm1 = new frmSalesRecord();
            frm1.GetRecord();
            frm1.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (frmLogin.Usertype == "Administrator")
            {
                this.Dispose();
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to Logout? Click Yes to Confirm!", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var frm1 = new frmLogin();
                    frm1.Show();
                    this.Hide();
                }
            }
        }

    }
}
