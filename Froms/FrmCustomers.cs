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
    public partial class FrmCustomers : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ClothingStoreDB;Integrated Security=True");

        public FrmCustomers()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            this.Text = "Müşteri Yönetim Paneli";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel pnlInputs = new Panel();
            pnlInputs.Size = new Size(300, 450);
            pnlInputs.Location = new Point(10, 10);
            this.Controls.Add(pnlInputs);

            // Müşteri için sadece Ad Soyad ve Telefon istenecek
            string[] labels = { "Müşteri Adı:", "Telefon:" };
            string[] names = { "txtCustomerName", "txtPhone" };

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label() { Text = labels[i], Location = new Point(10, 20 + (i * 40)), Width = 100 };
                TextBox txt = new TextBox() { Name = names[i], Location = new Point(110, 20 + (i * 40)), Width = 150 };
                pnlInputs.Controls.Add(lbl);
                pnlInputs.Controls.Add(txt);
            }

            // Butonlar
            Button btnList = new Button() { Text = "Listele", Location = new Point(10, 150), Width = 65, Height = 30, BackColor = Color.LightBlue };
            btnList.Click += MusterileriListele;
            pnlInputs.Controls.Add(btnList);

            Button btnAdd = new Button() { Text = "Ekle", Location = new Point(80, 150), Width = 65, Height = 30, BackColor = Color.LightGreen };
            btnAdd.Click += MusteriEkle;
            pnlInputs.Controls.Add(btnAdd);

            Button btnDelete = new Button() { Text = "Sil", Location = new Point(150, 150), Width = 65, Height = 30, BackColor = Color.LightCoral };
            btnDelete.Click += MusteriSil;
            pnlInputs.Controls.Add(btnDelete);

            DataGridView dgw = new DataGridView();
            dgw.Name = "dataGridView1";
            dgw.Location = new Point(320, 20);
            dgw.Size = new Size(450, 420);
            dgw.BackgroundColor = Color.White;
            this.Controls.Add(dgw);
        }

        private void MusterileriListele(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customers", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataGridView dgw = (DataGridView)this.Controls["dataGridView1"];
            dgw.DataSource = dt;
            baglanti.Close();
            MessageBox.Show("Müşteriler başarıyla listelendi!", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MusteriEkle(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO Customers (CustomerName, Phone) VALUES (@p1, @p2)", baglanti);
            
            Panel pnl = (Panel)this.Controls[0];
            komut.Parameters.AddWithValue("@p1", pnl.Controls["txtCustomerName"].Text);
            komut.Parameters.AddWithValue("@p2", pnl.Controls["txtPhone"].Text);
            
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Yeni müşteri başarıyla eklendi!", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MusterileriListele(null, null);
        }

        private void MusteriSil(object sender, EventArgs e)
        {
            DataGridView dgw = (DataGridView)this.Controls["dataGridView1"];
            if (dgw.SelectedRows.Count > 0)
            {
                int secilenId = Convert.ToInt32(dgw.SelectedRows[0].Cells[0].Value);
                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE FROM Customers WHERE CustomerID=@id", baglanti);
                komut.Parameters.AddWithValue("@id", secilenId);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Müşteri başarıyla silindi!", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MusterileriListele(null, null);
            }
            else
            {
                MessageBox.Show("Lütfen silmek için tablodan bir müşteri seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}