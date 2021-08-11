using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace personel_takip
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        OleDbConnection baglantim = new OleDbConnection("Provider = Microsoft.Ace.OleDb.12.0; Data Source=personel.accdb");

        private void personellerigoster()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter personelleriListele = new OleDbDataAdapter("select tcno AS[TC KİMLİK NUMARASI], ad AS[ADI],soyad AS[SOYADI]," +
                    "cinsiyet AS[CİNSİYET],mezun AS[MEZUNİYET], dogumtarihi AS[DOĞUM TARİHİ], gorevi AS[GÖREVİ],gorevyeri AS[GÖREV YERİ],maasi AS[MAAŞI]" +
                    "from personeller Order By ad ASC",baglantim);
                DataSet dshafiza = new DataSet();
                personelleriListele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglantim.Close();

            }
            catch (Exception errmsg)
            {

                MessageBox.Show(errmsg.Message,"SKY Personel Takip Programı",MessageBoxButtons.OK,MessageBoxIcon.Error);
                baglantim.Close();
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            this.Text = "KULLANICI İŞLEMLERİ";
            label11.Text = Form1.adi + " " + Form1.soyadi;// Aktif Kullanıcı asını form1 deki adi değişkeninden aldık
            pictureBox1.Height = 150; pictureBox1.Width = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;//3 boyutlu çerçeve olarka gösterir.

            pictureBox2.Height = 150; pictureBox1.Width = 150;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;//3 boyutlu çerçeve olarka gösterir.

            try
            {
                //hangi kullanıcı giriş yapmışsa o kullanıcının resmini gösterir.
                pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\kullanici_resimler\\" + Form1.tcno + ".jpg");

            }
            catch 
            {
                pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\kullanici_resimler\\resimyok.jpg");

            }
            maskedTextBox1.Mask = "00000000000";//11 haneli TC kimlik numarasını girmesini zorunlu hale getirdik
            
            
            
            
            personellerigoster();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (maskedTextBox1.Text.Length==11)
            {
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from personeller where tcno='"+maskedTextBox1.Text+"'",baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();

                while (kayitokuma.Read())//true değeri dönerse çalışır
                {
                    kayit_arama_durumu = true;
                    try
                    {//kayitokuma.GetValue(0) değeri access tablosundaki tcno alanını temsil eder.
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\" + kayitokuma.GetValue(0) + ".jpg");


                    }
                    catch 
                    {
                        //kullanıcının resmi yüklenmezse resim yok dosyasını sisteme yükler
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\resimyok.jpg");
                    }

                    label12.Text = kayitokuma.GetValue(1).ToString();// accesteki ad alanını temsil eder.
                    label13.Text = kayitokuma.GetValue(2).ToString();// accesteki soyad alanını temsil eder.
                    if (kayitokuma.GetValue(3).ToString() == "Bay") //accesteki cinsiyet alanını temsil eder.
                        label14.Text = "Bay";
                    else
                        label14.Text = "Bayan";

                    label15.Text = kayitokuma.GetValue(4).ToString();// accesteki mezun alanını temsil eder.
                    label16.Text = kayitokuma.GetValue(5).ToString();// accesteki dogumtarihi alanını temsil eder.
                    label17.Text = kayitokuma.GetValue(6).ToString();// accesteki gorevi alanını temsil eder.
                    label18.Text = kayitokuma.GetValue(7).ToString();// accesteki gorevyeri alanını temsil eder.
                    label19.Text = kayitokuma.GetValue(8).ToString();// accesteki maasi alanını temsil eder.

                    
                    break;
                }

                if (kayit_arama_durumu==false)
                    MessageBox.Show("Aranan Kayıt Bulunamadı","SKY Personel Takip  Programı",MessageBoxButtons.OK,MessageBoxIcon.Error);

                baglantim.Close();

            }

            else
                MessageBox.Show("Lüütfen 11 Haneli TC Kimlik Numarası Giriniz", "SKY Personel Takip  Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);




        }
    
    }
}
