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

namespace StajProjesi1
{
    public partial class Form1 : Form
    {

        SqlConnection baglanti;
        SqlCommand komut;
        SqlDataAdapter da;


        public Form1()
        {
            InitializeComponent();
        }

        void MusteriGetir()
        {
            baglanti = new SqlConnection("Data Source=DESKTOP-B2JKE9F\\SQLEXPRESS;Initial Catalog=menu;Integrated Security=True");
            baglanti.Open();
            da = new SqlDataAdapter("SELECT *FROM menu", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            MusteriGetir();
        }




        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textAnayemek.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textArayemek.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textTatli.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textIcecek.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textAnayemek.Text == "" || textArayemek.Text == "" || textTatli.Text == "" || textIcecek.Text == "")

                MessageBox.Show("Lüften tarihi seçiniz yada\nverileri girdiğinizden emin olun.");
            else
            {
                string sorgu = "INSERT INTO menu(tarih,anayemek,arayemek,tatli,icecek) VALUES (@tarih,@anayemek,@arayemek,@tatli,@icecek)";
                komut = new SqlCommand(sorgu, baglanti);
                //  komut = new SqlCommand("TRUNCATE TABLE menu", baglanti);
                komut.Parameters.AddWithValue("@tarih", textBox1.Text);
                komut.Parameters.AddWithValue("@anayemek", textAnayemek.Text);
                komut.Parameters.AddWithValue("@arayemek", textArayemek.Text);
                komut.Parameters.AddWithValue("@tatli", textTatli.Text);
                komut.Parameters.AddWithValue("@icecek", textIcecek.Text);
                MessageBox.Show("kayıt eklendi");
                textBox1.Text = "";
                textAnayemek.Text = "";
                textArayemek.Text = "";
                textTatli.Text = "";
                textIcecek.Text = "";
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                MusteriGetir();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            string sorgu = "SELECT * from menu where tarih=@tarih";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@tarih", textBox1.Text);
            da = new SqlDataAdapter(komut);
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                string date = dr["tarih"].ToString() + " " ;
                dr.Close();
                DialogResult durum = MessageBox.Show(date + " Tarihinde kayıtlı olan menüyü silmek istediğinizden eminmisiniz? ", "Silme Onayı", MessageBoxButtons.YesNo);
                if (DialogResult.Yes == durum)// Eğer evet seçilmişse kaydı silecek kodlar çalıştırılır. 
                {
                    string silme = "DELETE from menu where tarih=@tarih";// girilen tarihteki menüyü siler. 
                    SqlCommand silmeKomutu = new SqlCommand(silme, baglanti);// veritabanı üzerinde sorgulama, ekleme, güncelleme, silme işlemlerini yapmak için kullanılmaktadır. 
                    silmeKomutu.Parameters.AddWithValue("@tarih", textBox1.Text);// girilen tarihle textboxtaki tarih arasında ilişkilendirir. 
                    silmeKomutu.ExecuteNonQuery();//Yazdığımız Verileri Çalıştıran ve İşleve Sokan parametre  

                    MessageBox.Show("Kayıt Silindi.");

                    textBox1.Text = "";
                    textAnayemek.Text = "";
                    textArayemek.Text = ""; // bir sonraki işlemler için textboxlar boşaltıldı. 
                    textTatli.Text = "";
                    textIcecek.Text = "";

                }
                else
                    MessageBox.Show("Kayıt Bulunamadı.");// kayıt bulunamadığı durumlarda kullanıcı bilgilendirildi 
                baglanti.Close();

                SqlDataAdapter daa = new SqlDataAdapter("Select * From menu", baglanti);// menu adındaki tablo databaseden çekildi. 
                DataSet ds = new DataSet();// bir kez bağlandıktan sonra veriyi alır ve bağlantıyı keser. 
                daa.Fill(ds, "menu");  // datasetten alınan veriler dolduruldu 
                DataTable menu = new DataTable();// doldurulan veriler menu adında bir datatable a atandı 
                daa.Fill(menu); // menu adındaki değişkene veriler dolduruldu  
                dataGridView1.DataSource = menu;// menu adındaki değişkene atanan veriler datagride aktarılarak gösterilmesi sağlandı. 
                baglanti.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sorgu = "UPDATE menu SET tarih=@tarih,anayemek=@anayemek,arayemek=@arayemek,tatli=@tatli,icecek=@icecek WHERE id=@id";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@id", Convert.ToInt32(textBox2.Text));
            komut.Parameters.AddWithValue("@tarih", textBox1.Text);
            komut.Parameters.AddWithValue("@anayemek", textAnayemek.Text);
            komut.Parameters.AddWithValue("@arayemek", textArayemek.Text);
            komut.Parameters.AddWithValue("@tatli", textTatli.Text);
            komut.Parameters.AddWithValue("@icecek", textIcecek.Text);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            MusteriGetir();
        }
    }
}
