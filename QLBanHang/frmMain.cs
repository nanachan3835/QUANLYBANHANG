using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBanHang.Class;

namespace QLBanHang
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Function.Connect();
        }

        private void mnuDanhmuc_Click(object sender, EventArgs e)
        {

        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            Function.Disconnect();    
            Application.Exit();
        }
    }
}
