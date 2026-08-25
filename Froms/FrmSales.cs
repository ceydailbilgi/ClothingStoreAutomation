using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ClothingStoreAutomation.Froms
{
    public partial class FrmSales : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ClothingStoreDB;Integrated Security=True");

        public FrmSales()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            this.Text = "Satış Yönetim Paneli";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel pnlInputs = new Panel();
            pnlInputs.Size = new Size(300, 450);
            pnlInputs.Location = new Point(10, 10);
            this.Controls.Add(pnlInputs);

            // Satış için Ürün, Müşteri ve Toplam Tutar istenecek
            string[] labels = { "Ürün ID:", "Müşteri ID:", "Toplam Tutar:" };
            string[] names = { "txtProductID", "txtCustomerID", "txtTotalAmount" };

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label() { Text = labels[i], Location = new Point(10, 20 + (i * 40)), Width = 100 };
                TextBox txt = new TextBox() { Name = names[i], Location = new Point(110, 20 + (i * 40)), Width = 150 };
                pnlInputs.Controls.Add(lbl);
                pnlInputs.Controls.Add(txt);
            }

            // Butonlar
            Button btnList = new Button() { Text = "Satışları Listele", Location = new Point(10, 180), Width = 100, Height = 30, BackColor = Color.LightBlue };
            btnList.Click += SatislariListele;
            pnlInputs.Controls.Add(btnList);

            Button btnAdd = new Button() { Text = "Satış Yap", Location = new Point(120, 180), Width = 80, Height = 30, BackColor = Color.LightGreen };
            btnAdd.Click += SatisYap;
            pnlInputs.Controls.Add(btnAdd);

            DataGridView dgw = new DataGridView();
            dgw.Name = "dataGridView1";
            dgw.Location = new Point(320, 20);
            dgw.Size = new Size(450, 420);
            dgw.BackgroundColor = Color.White;
            this.Controls.Add(dgw);
        }

        private void SatislariListele(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Sales", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataGridView dgw = (DataGridView)this.Controls["dataGridView1"];
            dgw.DataSource = dt;
            baglanti.Close();
            MessageBox.Show("Satış kayıtları başarıyla listelendi!", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SatisYap(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO Sales (ProductID, CustomerID, TotalAmount, SaleDate) VALUES (@p1, @p2, @p3, @p4)", baglanti);
            
            Panel pnl = (Panel)this.Controls[0];
            komut.Parameters.AddWithValue("@p1", Convert.ToInt32(pnl.Controls["txtProductID"].Text));
            komut.Parameters.AddWithValue("@p2", Convert.ToInt32(pnl.Controls["txtCustomerID"].Text));
            komut.Parameters.AddWithValue("@p3", Convert.ToDecimal(pnl.Controls["txtTotalAmount"].Text));
            komut.Parameters.AddWithValue("@p4", DateTime.Now); // Satış tarihini o anki zaman yapar
            
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Satış işlemi başarıyla kaydedildi!", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SatislariListele(null, null);
        }
    }
}