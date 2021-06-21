using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace POS_New
{
    public partial class frmSystemSettings : Form
    {
        MySqlCommand cm;
        MySqlConnection cn;
        MySqlDataReader dr;
        ClassDB db = new ClassDB();

        public frmSystemSettings()
        {
            InitializeComponent();
            cn = new MySqlConnection();
            cn.ConnectionString = db.GetConnection();
        }

        public void GetInfo()
        {
            cn.Open();
            cm = new MySqlCommand("SELECT * FROM tblinfo", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                txtName.Text = dr["name"].ToString();
                txtPhone.Text = dr["phone"].ToString();
                txtEmail.Text = dr["email"].ToString();
                txtRegNo.Text = dr["regno"].ToString();
                txtAddress.Text = dr["address"].ToString();

                byte[] data = (byte[])dr["picture"];
                MemoryStream ms = new MemoryStream(data);
                picLogo.Image = Image.FromStream(ms);
            }
            else
            {
                txtName.Text = "";
                txtPhone.Text = "";
                txtEmail.Text = "";
                txtRegNo.Text = "";
                txtAddress.Text = "";

                picLogo.Image = picLogo.InitialImage;
            }
            dr.Close();
            cn.Close();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "image files(*.bmp;*.jpg;*.png;*.gif)|*.bmp*;*.jpg;*.png;*.gif;";

            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                picLogo.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void btnRemovePic_Click(object sender, EventArgs e)
        {
            picLogo.Image = picLogo.InitialImage;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAddress.Text == "" || txtEmail.Text == "" || txtName.Text == "" || txtPhone.Text == "")
            {
                MessageBox.Show("All Field are Required!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("Please check well before submitting? Click Yes to Confirm", "ALERT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    byte[] bytImage = new byte[0];
                    MemoryStream ms = new System.IO.MemoryStream();
                    Bitmap bmpImage = new Bitmap(picLogo.Image);

                    bmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ms.Seek(0, 0);
                    bytImage = ms.ToArray();
                    ms.Close();

                    cn.Open();
                    cm = new MySqlCommand("DELETE FROM tblinfo", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new MySqlCommand("INSERT INTO tblinfo (name,phone,email,regno,picture,address) VALUES(@name,@phone,@email,@regno,@picture,@address)", cn);
                    cm.Parameters.AddWithValue("name", txtName.Text);
                    cm.Parameters.AddWithValue("phone", txtPhone.Text);
                    cm.Parameters.AddWithValue("email", txtEmail.Text);
                    cm.Parameters.AddWithValue("regno", txtRegNo.Text);
                    cm.Parameters.AddWithValue("picture", bytImage);
                    cm.Parameters.AddWithValue("address", txtAddress.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    GetInfo();
                    MessageBox.Show("Information has been Saved!", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show("WARNING: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtAddress.Clear();
            txtEmail.Clear();
            txtName.Clear();
            txtPhone.Clear();
            txtRegNo.Clear();
            picClose.Image = picClose.InitialImage;
            GetInfo();
        }
    }
}
