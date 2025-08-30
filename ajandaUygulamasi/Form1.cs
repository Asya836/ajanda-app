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
    public partial class Form1 : Form
    {
        DatabaseHelper dbHelper = new DatabaseHelper();

        private void Listele()
        {
            dataGridView1.DataSource= dbHelper.Listele();
        }

        public Form1()
        {
            InitializeComponent();
            Listele();
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "MESAJ";
            dataGridView1.Columns[2].HeaderText = "TARİH";
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen güncellemek için bir not seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GuncelleForm guncelleForm = new GuncelleForm();
            guncelleForm.gelenId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
            guncelleForm.ShowDialog();
            Listele();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            EkleForm ekleForm = new EkleForm();
            ekleForm.ShowDialog();
            Listele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek için bir not seçiniz!","Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            int tiklananId= Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

            DialogResult sonuc= MessageBox.Show("Bu notu silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(sonuc == DialogResult.Yes)
            {
                dbHelper.Sil(tiklananId);
                Listele();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<DateTime> notTarih = dbHelper.tumTarihleriGetir();
            DateTime simdikiZaman= DateTime.Now;

            foreach (DateTime dt in notTarih) 
            {
                if(dt.Year==simdikiZaman.Year && dt.Month==simdikiZaman.Month && dt.Day==simdikiZaman.Day&& dt.Hour==simdikiZaman.Hour&& dt.Minute == simdikiZaman.Minute)
                {
                    string mesaj=dbHelper.mesajGetir(dt);
                    if (mesaj != null)
                    {
                        MessageBox.Show(mesaj, "Not süresi doldu.",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        dbHelper.Sil(durum : 1,tarih:dt);
                        Listele() ;
                    }
                }
            }
        }
    }
}
