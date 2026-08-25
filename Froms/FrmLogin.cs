using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ClothingStoreAutomation.Froms
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                DbConnection.con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = DbConnection.con;
                cmd.CommandText ="SELECT * FROM Admins WHERE Username=@u AND Password=@p";
                cmd.Parameters.AddWithValue("@u", txtUsername.Text);
                cmd.Parameters.AddWithValue("@p", txtPassword.Text);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read()) 
                {
                    MessageBox.Show("Login Successful");
                    FrmMain frm = new FrmMain();
                    frm.Show();
                    this.Hide();
                }
                else 
                {
                    MessageBox.Show("Wrong Username or Password");
                }

                DbConnection.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DbConnection.con.Close();
            }
        }
    }
}
