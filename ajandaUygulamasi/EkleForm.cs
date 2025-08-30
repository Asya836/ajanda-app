using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ajandaUygulamasi
{
    public partial class EkleForm : Form
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();


        public EkleForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EkleForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (txtMesaj.Text != "")
            {
                dbHelper.Ekle(secilenTarih.Value.ToString("dd.MM.yyyy HH:mm:ss"), txtMesaj.Text);
                this.Close();

            }
            else
            {
                MessageBox.Show("Lütfen mesaj alanını doldurunuz!","Uyarı",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EkleForm_Load(object sender, EventArgs e)
        {
            secilenTarih.MinDate = DateTime.Now;
        }
    }
}
