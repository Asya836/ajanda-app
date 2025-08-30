using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ajandaUygulamasi
{
    internal class DatabaseHelper
    {
        private OleDbConnection connection;
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\AjandaDB.mdb";

        public DatabaseHelper()
        {
            connection = new OleDbConnection(connectionString);
        }

        private void ExecuteQuery(OleDbCommand cmd)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }  
                cmd.ExecuteNonQuery();
                System.Windows.Forms.MessageBox.Show("İşlem başarılı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }     
            }
        }

        public DataTable Listele()
        {
            DataTable dt = new DataTable();
            string query = "select * from Ajanda";
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
            adapter.Fill(dt);
            return dt;
        }

        public DataTable IdGöreGetir(int id)
        {
            DataTable dt = new DataTable();
            string query = "select * from Ajanda where ID=@id";
            using (OleDbCommand cmd = new OleDbCommand(query,connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                connection.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;

        }

        public void Ekle(string tarih, string mesaj)
        {
            string query = "insert into Ajanda (Mesaj,Tarih) values (@mesaj,@tarih)";
            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.Parameters.AddWithValue("@mesaj", mesaj);
            cmd.Parameters.AddWithValue("@tarih", tarih);

            ExecuteQuery(cmd);
        }

        public void Guncelle(int id, string tarih, string mesaj)
        {
            string query = "update Ajanda set Mesaj=@mesaj, Tarih=@tarih where ID=@id";
            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.Parameters.AddWithValue("@mesaj", mesaj);
            cmd.Parameters.AddWithValue("@tarih", tarih);
            cmd.Parameters.AddWithValue("@id", id);

            ExecuteQuery(cmd);
        }

        public void Sil(int id=int.MinValue, int durum = 0, DateTime? tarih=null)
        {
            if (durum == 0)
            {
                string query = "delete from Ajanda where ID=@id";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);

                ExecuteQuery(cmd);
            }else if(durum == 1)
            {
                string query = "delete from Ajanda where Tarih=@Tarih";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.Parameters.AddWithValue("Tarih", tarih);

                ExecuteQuery(cmd);
            }
        }

        public List<DateTime> tumTarihleriGetir()
        {
            List<DateTime> list = new List<DateTime>();
            string query = "select Tarih from Ajanda";

            using(OleDbCommand cmd=new OleDbCommand(query, connection))
            {
                connection.Open();

                using(OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        if (reader["Tarih"]!=DBNull.Value && DateTime.TryParse(reader["Tarih"].ToString(),out DateTime dbTarih))
                        {
                            list.Add(dbTarih);
                        }
                    }
                }
                connection.Close();
            }
            return list;
        }

        public string mesajGetir(DateTime tarih)
        {
            string mesaj="";
            string query = "select Mesaj from Ajanda where Tarih=@Tarih";

            using(OleDbCommand cmd=new OleDbCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Tarih",tarih);

                connection.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    mesaj=result.ToString();
                }
                connection.Close();
            }
            return mesaj;
        }
    }
}
