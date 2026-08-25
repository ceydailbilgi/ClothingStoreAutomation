using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClothingStoreAutomation.Froms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            lblWelcome.Text = "Welcome : " + DbConnection.AdminName;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            FrmProducts frm = new FrmProducts();
            frm.ShowDialog();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            FrmSales frm = new FrmSales();
            frm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            FrmCustomers frm = new FrmCustomers();
            frm.ShowDialog();
        }
    }
}
