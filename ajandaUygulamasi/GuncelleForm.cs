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
    public partial class GuncelleForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        DatabaseHelper dbHelper = new DatabaseHelper();
        public int gelenId;

        public GuncelleForm()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GuncelleForm_Load(object sender, EventArgs e)
        {
            secilenTarih.MinDate = DateTime.Now;
            DataTable dt = dbHelper.IdGöreGetir(gelenId);

            if (dt.Rows.Count > 0)
            {
                txtMesaj.Text = dt.Rows[0]["MESAJ"].ToString();
                if (DateTime.TryParse(dt.Rows[0]["TARİH"].ToString(),out DateTime result))
                {
                    secilenTarih.Value = result;
                }               
            }
        }

        private void GuncelleForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtMesaj.Text))
            {
                dbHelper.Guncelle(gelenId, secilenTarih.Value.ToString("dd.MM.yyyy  HH:mm:ss"),txtMesaj.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show("Lütfen mesaj alanını doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
