using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;//System.Data.OleDb kütüphanesinin yüklenmesi

namespace personel_takip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //veri tabanı dosya yolu ve provider nesnesinin belrlenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OleDb.12.0;Data Source=" +
        Application.StartupPath + "\\personel.accdb");

        //Formalar arası veri aktarımında kullanılacak değişkenler
        public static string tcno, adi, soyadi, yetki;
        //yerel yani sadece bu formda kullanılacak değişkenler
        int hak_degeri = 3; bool durum = false; // veri tabanında böyle bir kullanıcı var mı yok mu varsa true olacak yoksa false olacak.
                                                // başlangıç değeri false belirlendi(yani yok)

        private void button1_Click(object sender, EventArgs e)
        {
            if (hak_degeri!=0)//kullanıcının hala girme hakkı varsa
            {
                baglantim.Open();
                OleDbCommand eklemesorgusu = new OleDbCommand("select * from kullanicilar",baglantim);//kullanicilar tablosundaki bütün verileri getirme sorgusu oluşturduk
                                                                                                       // sorgunun sonuçlarını kayitokuma adlı data readerda sakladık.
                OleDbDataReader kayitokuma = eklemesorgusu.ExecuteReader();//kullanıcılar tablosundaki bütün verileri getir. burada access tablosunn klonu şeklinde bellekte durur.
                while (kayitokuma.Read())//tabloda herhang bir değer var mı(true mu. kaç tane kayıt varsa while döngüsü o kadar döner
                {
                    if (radioButton1.Checked)//yönetici seçilmişse
                    {
                        if (kayitokuma["kullaniciadi"].ToString()==textBox1.Text && kayitokuma["parola"].ToString()==textBox2.Text && kayitokuma["yetki"].ToString()=="Yönetici")
                        {
                            durum = true;// dogru bir kullanıcı grişi olduğu çin drurum=true oldu
                            tcno = kayitokuma.GetValue(0).ToString();//kaydın sıfırıncı alanını al. yanni 1. alan tc no değerini al
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            yetki = kayitokuma.GetValue(3).ToString();
                            this.Hide();
                            Form frm2 = new Form2();
                            frm2.Show();
                            break;
                                
                        }

                    }

                    if (radioButton2.Checked)//yönetici seçilmişse
                    {
                        if (kayitokuma["kullaniciadi"].ToString() == textBox1.Text && kayitokuma["parola"].ToString() == textBox2.Text && kayitokuma["yetki"].ToString() == "Kullanıcı")
                        {
                            durum = true;// dogru bir kullanıcı grişi olduğu çin drurum=true oldu
                            tcno = kayitokuma.GetValue(0).ToString();//kaydın sıfırıncı alanını al. yanni 1. alan tc no değerini al
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            yetki = kayitokuma.GetValue(3).ToString();
                            this.Hide();
                            Form frm3 = new Form3();
                            frm3.Show();
                            break;

                        }
                    }
                }
            }

            if (durum==false)//kullanıcı yanlış giriş denemesi yapmışsa(durum değeri hala false) değeri bir azalt
            {
                hak_degeri--;
                baglantim.Close();
                label5.Text = hak_degeri.ToString();
            }

            if (hak_degeri==0)
            {
                button1.Enabled = false;
                MessageBox.Show("Giriş Hakkınız Kalmadı","SKY Personel Takip Programı",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
            }
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text="Kullanıcı Girişi...";
            this.AcceptButton = button1;// enter tuşuna basıldığı anda buton1 bir(giriş butonu) çalışır
            this.CancelButton = button2;// esc tusuna basildii zaman buton2(çıkış dügmesi) çalışır.
            label5.Text = Convert.ToString(hak_degeri);// hak değerini label 5 e atadık
            radioButton1.Checked = true;
            this.StartPosition = FormStartPosition.CenterScreen;// form ekranın merkezinde gelsin
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;// ekranı tam kaplama ve minimize etme tuşları pasif olacak
        }
    }
}
