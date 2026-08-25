using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Veritabanı için bu şart!

namespace ClothingStoreAutomation.Froms
{
    public partial class FrmProducts : Form
    {
        // Bilgisayarındaki SQL Server veritabanı bağlantı adresi
        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ClothingStoreDB;Integrated Security=True");

        public FrmProducts()
        {
            InitializeComponent();
        }

        // Form açılırken kutuları, butonları çizen ve onlara görev veren kod
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            this.Text = "Ürün Yönetim Paneli";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel pnlInputs = new Panel();
            pnlInputs.Size = new Size(300, 450);
            pnlInputs.Location = new Point(10, 10);
            this.Controls.Add(pnlInputs);

            string[] labels = { "Ürün Adı:", "Kategori ID:", "Renk:", "Beden:", "Fiyat:", "Stok Miktarı:" };
            string[] names = { "txtProductName", "txtCategoryID", "txtColor", "txtSize", "txtPrice", "txtStock" };

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label() { Text = labels[i], Location = new Point(10, 20 + (i * 40)), Width = 100 };
                TextBox txt = new TextBox() { Name = names[i], Location = new Point(110, 20 + (i * 40)), Width = 150 };
                pnlInputs.Controls.Add(lbl);
                pnlInputs.Controls.Add(txt);
            }

            // Butonlar ve Tıklama Görevleri
            Button btnList = new Button() { Text = "Listele", Location = new Point(10, 280), Width = 65, Height = 30, BackColor = Color.LightBlue };
            btnList.Click += UrunleriListele;
            pnlInputs.Controls.Add(btnList);

            Button btnAdd = new Button() { Text = "Ekle", Location = new Point(80, 280), Width = 65, Height = 30, BackColor = Color.LightGreen };
            btnAdd.Click += UrunEkle;
            pnlInputs.Controls.Add(btnAdd);

            Button btnDelete = new Button() { Text = "Sil", Location = new Point(150, 280), Width = 65, Height = 30, BackColor = Color.LightCoral };
            btnDelete.Click += UrunSil;
            pnlInputs.Controls.Add(btnDelete);

            DataGridView dgw = new DataGridView();
            dgw.Name = "dataGridView1";
            dgw.Location = new Point(320, 20);
            dgw.Size = new Size(450, 420);
            dgw.BackgroundColor = Color.White;
            this.Controls.Add(dgw);
        }

        // 1. LİSTELEME FONKSİYONU
        private void UrunleriListele(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Products", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataGridView dgw = (DataGridView)this.Controls["dataGridView1"];
            dgw.DataSource = dt;
            baglanti.Close();
            MessageBox.Show("Ürünler başarıyla listelendi!", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 2. EKLEME FONKSİYONU
        private void UrunEkle(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO Products (ProductName, Color, Size) VALUES (@p1, @p2, @p3)", baglanti);
            
            // Panel içindeki kutulardan verileri çekiyoruz
            Panel pnl = (Panel)this.Controls[0];
            komut.Parameters.AddWithValue("@p1", pnl.Controls["txtProductName"].Text);
            komut.Parameters.AddWithValue("@p2", pnl.Controls["txtColor"].Text);
            komut.Parameters.AddWithValue("@p3", pnl.Controls["txtSize"].Text);
            
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Yeni ürün başarıyla eklendi!", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UrunleriListele(null, null); // Listeyi otomatik yenilesin
        }

        // 3. SİLME FONKSİYONU
        private void UrunSil(object sender, EventArgs e)
        {
            DataGridView dgw = (DataGridView)this.Controls["dataGridView1"];
            if (dgw.SelectedRows.Count > 0)
            {
                int secilenId = Convert.ToInt32(dgw.SelectedRows[0].Cells[0].Value);
                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE FROM Products WHERE ProductID=@id", baglanti);
                komut.Parameters.AddWithValue("@id", secilenId);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ürün başarıyla silindi!", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UrunleriListele(null, null);
            }
            else
            {
                MessageBox.Show("Lütfen silmek için tablodan bir satır seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}