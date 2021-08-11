using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//System.Data.OleDb; kütüphanesinin tanımlanması
using System.Data.OleDb;

// system.text.regularexpression(regex) kütüphanesinin tanımlanması
using System.Text.RegularExpressions;// bu kütüphanenin kullanım amacı güvenli parola oluşturmak için hazır yapıları barındırmasıdır.
// giriş çıkış işlemlerin ilişkin kütüphanenin tanımlanması
using System.IO;// yeni klasörler oluşturulacağız. bir klasörün var olup olmayacağın denetleyeceğiz. resim kopyalama işlemeri yapacağız.

namespace personel_takip
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        // veri tabanı dosya yolu ve provider nesnesinin belirlenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OleDb.12.0;Data Source=personel.accdb");

        private void kullanicilari_goster()// veri tabanındaki kullanıcıları yeni kllanıcı ekledğimde, sildiğimde, güncellediğimde çağıralacaktır.
        {
            try
            {
                baglantim.Open();
                
                // order by ad asc:kulanıcı tablosunu ad alanına göre  yöntemiyle sıralar. 
                // kullanıcılar tablosunda alanda tcno ise datagridview de AS deyimiyle TC KMLİK no görülecek.
                OleDbDataAdapter kullanicilariGosterSorgu = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO], ad AS[AD], soyad AS[SOYAD]," +
                    "yetki AS[YETKİ], kullaniciadi AS[KULLANICI ADI], parola AS[PAROLA] from kullanicilar order By ad ASC",baglantim);
                DataSet dshafiza = new DataSet();

                kullanicilariGosterSorgu.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglantim.Close();
            }
            catch (Exception errmsg)
            {

                MessageBox.Show(errmsg.Message,"SKY Personel Takip Programı",MessageBoxButtons.OK,MessageBoxIcon.Error);
                baglantim.Close();
            }
           

        }

        public void personelleri_goster()// veri tabanındaki kullanıcıları yeni kllanıcı ekledğimde, sildiğimde, güncellediğimde çağıralacaktır.
        {
            try
            {
                baglantim.Open();

                // order by ad asc:kulanıcı tablosunu ad alanına göre  yöntemiyle sıralar. 
                // kullanıcılar tablosunda alanda tcnı ise datagridview de TC KMLİK no görülecek.
                // ,mezuniyet AS[MEZUNİYET],dogumtarihi AS[DOĞUM TARİHİ],gorevi AS[GÖREVİ], +
                   // "maasi AS[MAAŞI] 
                OleDbDataAdapter personelleriGosterSorgu = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO],ad AS[AD], soyad AS[SOYAD], cinsiyet AS[CİNSİYET]," +
                    "mezun AS[MEZUNİYET], dogumtarihi AS[DOĞUM TARİHİ],gorevi AS[GÖREVİ], gorevyeri AS[GÖREV YERİ], maasi AS[MAAŞI] from personeller order By ad ASC", baglantim);
                DataSet dshafiza = new DataSet();

                personelleriGosterSorgu.Fill(dshafiza);
                dataGridView2.DataSource = dshafiza.Tables[0];
                baglantim.Close();
            }
            catch (Exception errmsg)
            {

                MessageBox.Show(errmsg.Message, "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }


        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //kullanicilari_goster();
            // personelleri_goster();
            pictureBox1.Height = 150;pictureBox1.Width = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;//Reasmi picturebox a göre ayarla

            try
            {
                // form1 de giriş yapmak isteyen kullanıcının giriş yaptığında  tc kimlik numarası.jpg uzantılı dosya resim olacaktır.
                // kim giriş yapmışsa onun tcsi gelecek public static string tcno, adi, soyadi, yetki; daki tc no gelecek
                
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullanici_resimler\\"+Form1.tcno+".jpg");

            }
            catch 
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullanici_resimler\\resimyok.jpg");

            }
            string buyukmetingoster2 = textBox2.Text;
            //Kullanıcı sekmesinin işlemleri yapılmaktadır.
            this.Text = "YÖNETİCİ İŞLEMLERİ";
            label10.ForeColor = Color.DarkRed;
            label10.Text = Form1.adi + " " + Form1.soyadi;// label10 da giriş yapan kullanıcının adı ve soyadı form1 den getirilecek
            textBox1.MaxLength = 11;
            textBox4.MaxLength = 11;
            toolTip1.SetToolTip(this.textBox1, "TC Kimlik No 11 Haneli Olmalı!");// imleci textbox1 e getirisen bu uyarıyı verir
            radioButton1.Checked = true;

            textBox5.MaxLength = 10;
            textBox6.MaxLength = 10;
            progressBar1.Maximum = 100; progressBar1.Value = 0;
            kullanicilari_goster();
            personelleri_goster();

            // Personel İşlemleri sekmesinin işlemleri yapılmaktadır.
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Width = 100; pictureBox2.Height = 100;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            maskedTextBox1.Mask = "00000000000";// 11 tane rakam girmek zorunda. harf giremez
            maskedTextBox2.Mask = "LL??????????????????????";// en az iki karakter olsun(LL) ama 22 karakterdden fazla(?) olmasın ve sadece karakter olsun
            maskedTextBox3.Mask = "LL??????????????????????";// en az iki karakter olsun(LL) ama 22 karakterdden fazla(?) olmasın ve sadece karakter olsun
            maskedTextBox4.Mask = "0000"; // 1000 ile 9999 tl arasında bir maaş girişi zorunludur.
            maskedTextBox2.Text.ToUpper();/////girilen metinleri büyük harfe dönüştürür
            maskedTextBox3.Text.ToUpper();/////girilen metinleri büyük harfe dönüştürür

            comboBox1.Items.Add("İlköğretim");comboBox1.Items.Add("Ortaöğretim");comboBox1.Items.Add("Lise");comboBox1.Items.Add("Lisans");
            comboBox1.Items.Add("Yüksek Lisans"); comboBox1.Items.Add("Doktora ");

            comboBox2.Items.Add("Yönetici");comboBox2.Items.Add("Memur");comboBox2.Items.Add("Şoför");comboBox2.Items.Add("İşçiler");

            comboBox3.Items.Add("ARGE");comboBox3.Items.Add("Bilgi İşlem");comboBox3.Items.Add("Üretim");comboBox3.Items.Add("Nakliye");
            comboBox3.Items.Add("Paketleme");comboBox3.Items.Add("Muhasebe");

            DateTime zaman = DateTime.Now;// şimdiki zamanı aldık
            int yil = int.Parse(zaman.ToString("yyyy"));//bugünkü tarih ne issse onun yılını aldık.
            int ay = int.Parse(zaman.ToString("MM"));
            int gun = int.Parse(zaman.ToString("dd"));

            dateTimePicker1.MinDate = new DateTime(1960,1,1);// 1960 tan aşağısını göstermeyecek
            dateTimePicker1.MaxDate = new DateTime(yil-18,ay,gun); // 18 yaşından kücüklerin çalışamayacağı için yil-18 yaptık; ay,gun farketmez
            dateTimePicker1.Format = DateTimePickerFormat.Short; // kısa tarih olarak göster.
            radioButton3.Checked = true;

        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length<11)
            {
                errorProvider1.SetError(textBox1,"TC KimliK No 11 Haneli Olmalı");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)// klavyeye basıldığı zamanki olayı gösterir
        {
            //ASCII tablosuna göre Kalvyeden basılan tuş 48 ile 57 arasında mı (0-9 arası rakamlara tekabul eder). silme tuşuna da basılabilir(e.KeyChar=8)
            //e.KeyChar: bu iafde klavyeden girilen karakter veya  sayı anlamına gelir.

            
                
            if ((((int)(e.KeyChar)>=48 && (int)(e.KeyChar)<=57) || (int) e.KeyChar==8))  
            {
                if (textBox1.Text.Length==0 && e.KeyChar==48)// TC numarasının sıfır başlamasının önüne geçer
                { 
                    e.Handled = true;
                    MessageBox.Show("TC kimlik numarası sıfır ile başlayamaz");


                }
                else
                {
                     e.Handled = false;// 0-9 arasındaki rakamlaa ve silme tusunun basılmasına izin ver
                }

            }
            else
            {
                e.Handled = true; // 0-9 arasındaki rakamlaa ve silme tusunun basılmasına izin vermez.
               
            }

        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // klavyeden basılan tuş karakterse harfse backspace tusu basıldıysa seperator tusu basıldıysaaşagıdaki işlemi yap
            if (char.IsLetter(e.KeyChar)==true || char.IsControl(e.KeyChar)==true  || char.IsSeparator(e.KeyChar)==true)
            {   
                e.Handled = false;

                e.KeyChar= Char.ToUpper(e.KeyChar);//girilen harfleri büyük yapar
            }

            else
            {
                e.Handled = true;
            }

        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // klavyeden basılan tuş karakterse harfse backspace tusu basıldıysa seperator tusu basıldıysaaşagıdaki işlemi yap
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
            {
                e.Handled = false;
                e.KeyChar = Char.ToUpper(e.KeyChar);//girilen harfleri büyük yapar
            }

            else
            {
                e.Handled = true;
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length != 8)
            {
                errorProvider1.SetError(textBox4, "Kullanıcı Adı 8 KArakter Olmalı");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // klavyeden karaktere basılmışsa veya backspace (IsControl) tuşuna basılmışsa veya sayıya basılmışsa(IsDigit) aşağıdaki işlemleri yap
            if (char.IsLetter(e.KeyChar)==true || char.IsControl(e.KeyChar)==true || char.IsDigit(e.KeyChar)==true)
            {
                e.Handled = false;
            }

            else
            {
                e.Handled = true;
            }
       
        }

        int parolaskoru = 0;// 0-1000 arasında parala skoru belirlenecek.
                            // Kullanıcının olusturdugu parolanın güvenlik seviyesine göre bu  değişken ceşitli değerler alabilecek gösterecek
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string parola_seviyesi= "";//zayıf,orta,güçlü,çok güçlü olarak gösterecek
            int kucuk_harf_skoru = 0, buyuk_harf_skoru = 0, rakam_skoru = 0,sembol_skoru = 0;

            string sifre = textBox5.Text;
            //regex kütüphanesi ing kelimleri baz aldıgından Turkce karakterlerde sorun yasamamak için sifre string ifadesindeki
            //türkce karakterleri ingilizce karakterlere dönüşştürmemiz gerekiyor
            string duzeltilmis_sifre = "";
            duzeltilmis_sifre = sifre;// ilk basta sifre alanına Türkçe karakterler girilmeden şifre girilirse sifre duzeeltimiş sifreye eşit olur.
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('İ', 'I');// replace yer değiştir demek. Türkçedeki harflari ingilizce dönüştükdük
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ı', 'i');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ç', 'C');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ç', 'c');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ş', 'S');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ş', 's');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ğ', 'G');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ğ', 'g');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ö', 'O');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ö', 'o');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ü', 'U');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ü', 'u');
            

            // Türkçe karkaterler kullanılmışsa türkçe karakterler ingilizce karakterlere dönüştürür. ve yeni şifreye eşit olur
            if (sifre!=duzeltilmis_sifre)
            {
                sifre = duzeltilmis_sifre;
                textBox5.Text = sifre;
                MessageBox.Show("Paroladaki Türkçe karakterler İngilizce karakterlere dönüştürülmüştür");
            }

            // bir küçük harf 10 puan 2 küçük ve daha fazla küçük harf 20 puan
            // örnegin : +Anil785 ---> 8 karakter
            //Regex.Replace(sifre,["a-z"]," ").Length--> sifre den küçük harkeri çıkar
            // Regex.Replace(sifre,["a-z"]," ").Length ---->+A785 -->5 karakter  küçük karakterlerin yerini boşluklara dönüştürür
            //  +Anil785(sifre. length) -->8 karakter
            //az_karakter_sayisi = sifre.Length - Regex.Replace(sifre,["a-z"]," ").Length= 8-5 = 3 küçük karakterlerin sayısı
            int az_karakter_sayisi = sifre.Length - Regex.Replace(sifre,"[a-z]","").Length;//kucuk harf sayısı
            //ör: 1 küçük harf olursa min(2,1) olur (minimum sayı 1)ve 1*10 dan 10 puan olur
            //ör: 2 küçük harf olursa min(2,2)(minimum sayı 2) olur ve 2*10 dan 20 puan olur
            //ör 3 küçük harf olursa min(2,3) (minimum sayı 2)olur ve 2*10 dan 20 puan olur
            kucuk_harf_skoru = Math.Min(2, az_karakter_sayisi)*10; // max 20 puan alır.

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // bir büyük harf 10 puan 2 büyük ve daha fazla büyük harf 20 puan
            // örnegin : +ANIl785 ---> 8 karakter
            //Regex.Replace(sifre,"[A-Z]"," ").Length--> sifre den büyük harkeri çıkar
            // Regex.Replace(sifre,"[A-Z]"," ").Length ---->+l785-->5 karakter// büyük karakterlerin yerini boşlukla dönüştürür
            //  +ANIl785(sifre. length) -->8 karakter
            //AZ_karakter_sayisi = sifre.Length - Regex.Replace(sifre,"[A-Z]"," ").Length= 8-5 = 3 küçük karakterlerin sayısı


            int AZ_karakter_sayisi = sifre.Length - Regex.Replace(sifre, "[A-Z]", "").Length;//büyük harf sayısı
            //ör: 1 büyük harf olursa min(2,1) olur (minimum sayı 1)ve 1*10 dan 10 puan olur
            //ör: 2 büyük harf olursa min(2,2)(minimum sayı 2) olur ve 2*10 dan 20 puan olur
            //ör 3 büyük harf olursa min(2,3) (minimum sayı 2)olur ve 2*10 dan 20 puan olur
            buyuk_harf_skoru = Math.Min(2, AZ_karakter_sayisi) * 10; // max 20 puan alır.

           //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
           // 1 rakam 10 puan 2 ve üstü rakamlar 20 puan

            int rakam_sayisi = sifre.Length - Regex.Replace(sifre, "[0-9]", "").Length;//rakam sayısı
            
            rakam_skoru = Math.Min(2, rakam_sayisi) * 10; // max 20 puan alır.

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 1 sembol 10 puan 2 ve üstü sembol 20 puan

            int sembol_sayisi = sifre.Length - az_karakter_sayisi - AZ_karakter_sayisi - rakam_sayisi;//sembol sayısı

            sembol_skoru = Math.Min(2, sembol_sayisi) * 10; // max 20 puan alır.


            parolaskoru = buyuk_harf_skoru + kucuk_harf_skoru + rakam_skoru + sembol_skoru;

            // 2 rakam 2 küçük harf 2 büyük harf 2 sembol kullanan kişi 80 puan alır.
            // 9 karakter olursa ne olursa olsun parlo skoruna 10 eklenir
            //10 karakter olursa ne olursa olsun parlo skoruna 20 eklenir
            if (sifre.Length==9)
            {
                parolaskoru += 10;
            }

            else if (sifre.Length==10)
            {
                parolaskoru += 20;
            }

            if (kucuk_harf_skoru==0 || buyuk_harf_skoru==0 || rakam_skoru==0 || sembol_skoru==0)
            {
                label23.Text = "Büyük Harf,Küçük Harf, Rakam, Sembol kullanmalısın";
            }

            if (kucuk_harf_skoru != 0 && buyuk_harf_skoru != 0 && rakam_skoru != 0 && sembol_skoru != 0)
            {
                label23.Text = "";
            }

            if (parolaskoru<70)
            {
                parola_seviyesi = "Kabul edilemez";
            }

            else if(parolaskoru ==70 || parolaskoru == 80)
            {
                parola_seviyesi = "Güçlü";
            }

            else if (parolaskoru == 90 || parolaskoru == 100)
            {
                parola_seviyesi = "Çok Güçlü";
            }

            label20.Text = "%" + Convert.ToString(parolaskoru);//skor labeli
            label21.Text = parola_seviyesi;

            progressBar1.Value = parolaskoru;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if(textBox6.Text != textBox5.Text)
            {
                errorProvider1.SetError(textBox6, "Parola Tekrarı Eşlemiyor!");
            }

            else
            {
                errorProvider1.Clear();
            }
        }

        private void maskedTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);//girilen harfleri büyük yapar
        }

        private void maskedTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);//girilen harfleri büyük yapar
        }
        private void tabPage1Clear()
        {
            textBox1.Clear();textBox2.Clear();textBox3.Clear();textBox4.Clear();textBox5.Clear(); textBox6.Clear(); 
            
        }
        private void tabPage2Clear()
        {
            pictureBox2.Image = null; maskedTextBox1.Clear(); maskedTextBox2.Clear(); maskedTextBox3.Clear(); maskedTextBox4.Clear();
            comboBox1.SelectedIndex = -1;// Combobox ta herhangi bir index değerini boş yapar.
            comboBox2.SelectedIndex = -1;// Combobox ta herhangi bir index değerini boş yapar.
            comboBox3.SelectedIndex = -1;// Combobox ta herhangi bir index değerini boş yapar. 

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string yetki = "";// kaydedilecek kişinin yönetici mi yoksa kullanıcı mı olacak onu belirler.
            bool kayitkontrol = false;// kayıtlı olan  kullanıcıdan ,aynı kişi kayıt edileceği zaman kayıtlı mı değil mi bakarız.
                                      // başlangıç olarak acces tablosunda böyle bir kaydın olmadığını(false) varsayıyoruz

            baglantim.Open();

            OleDbCommand selectsorgu = new OleDbCommand("select * from kullanicilar where tcno='"+textBox1.Text+"'",baglantim);
            OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();//selectsorgu sonuçları kayitokuma datareader da mevcut

            // kayıt  gerçekleşti mi yani textbox1 e girilen TC değri access tabloaunda mevcut mu? while döngüsü kullanılacak
            while (kayitokuma.Read())
            {
                kayitkontrol = true;//herhangi bir kayıt varsa böyle bir kaydın olmamasının önüne geçmiş olacağız.
                break;
                
            }
            baglantim.Close();

            if (kayitkontrol==false)// texboxa girilen tc kimlik numarası access tablosunda yoksa anlamına gelir
            {
                //tc kimlik no kontrolü
                if (textBox1.Text.Length<11 || textBox1.Text=="")
                {
                    label1.ForeColor = Color.Red;
                }
                else
                {
                    label1.ForeColor = Color.Black;
                }

                //adi veri kontrolu.bi kinini adı 2 karakterden küçük olamaz
                if (textBox2.Text.Length < 2 || textBox2.Text == "")
                {
                    label8.ForeColor = Color.Red;
                }
                else
                {
                    label8.ForeColor = Color.Black;
                }
                //soyadi veri kontrolu.bi kisinini soyadı 2 karakterden küçük olamaz
                if (textBox3.Text.Length < 2 || textBox3.Text == "")
                {
                    label3.ForeColor = Color.Red;
                }
                else
                {
                    label3.ForeColor = Color.Black;
                }

                //kullanıcı adı veri kontrolu.bi kisinin kullanıcı adi 8 karakter olmalı
                if (textBox4.Text.Length != 8 || textBox4.Text == "")
                {
                    label5.ForeColor = Color.Red;
                }
                else
                {
                    label5.ForeColor = Color.Black;
                }

                //parola veri kontrolu.
                if (parolaskoru<70 || textBox5.Text == "")
                {
                    label6.ForeColor = Color.Red;
                }
                else
                {
                    label6.ForeColor = Color.Black;
                }

                //parola tekrar veri kontrolu.
                if (textBox5.Text != textBox6.Text || textBox6.Text == "")
                {
                    label9.ForeColor = Color.Red;
                }
                else
                {
                    label9.ForeColor = Color.Black;
                }

                if (textBox1.Text.Length == 11 && textBox1.Text != "" && textBox2.Text.Length >= 2 && textBox2.Text != ""
                    && textBox3.Text.Length >= 2 && textBox3.Text != "" && textBox4.Text.Length == 8 && textBox4.Text != "" 
                    && parolaskoru >= 70 && textBox5.Text != "" && textBox5.Text == textBox6.Text && textBox6.Text != "")
                   
                {
                    if (radioButton1.Checked==true)
                    {
                        yetki = "Yönetici";
                    }
                    else if(radioButton2.Checked==true)
                    {
                        yetki = "Kullanıcı";
                    }

                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekoomutu = new OleDbCommand("insert into kullanicilar values('"+textBox1.Text+ "','" + textBox2.Text + "'," +
                            "'" + textBox3.Text + "','"+yetki+"','" + textBox4.Text + "','" + textBox5.Text + "')",baglantim);

                        eklekoomutu.ExecuteReader();// eklekomutu adlı sorgunun sonuçlarını access tablosuna işle
                        baglantim.Close();
                        MessageBox.Show("Yeni Kullanıcı kaydı Oluşturuldu","SKY Personel Takip Programı",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);

                        tabPage1Clear();

                    }
                    catch (Exception errmsg)
                    {
                        MessageBox.Show(errmsg.Message);
                        baglantim.Close();
                    }
                }

                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz!!! ", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            else
            {
                MessageBox.Show("Girilen TC Kimlik Numarası daha önceden kayıtlıdır","SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            kullanicilari_goster();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            bool kayit_arama_durumu = false; // listede kayıt olmadığını kontrol eder. böyle bir kayıt yok anlamında(false)


            if (textBox1.Text.Length == 11)
            {
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from kullanicilar where tcno= '" + textBox1.Text + "'", baglantim);

                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();

                while (kayitokuma.Read())//herhangi bir kayıt var mı diyedöner eğer varsa kayit_arama_durumu true döner
                {
                    kayit_arama_durumu = true;
                    textBox2.Text = kayitokuma.GetValue(1).ToString();//ad alanını aldı.
                    textBox3.Text = kayitokuma.GetValue(2).ToString();//soyad alanını aldı.
                    if (kayitokuma.GetValue(3).ToString() == "Yönetici")
                    {
                        radioButton1.Checked = true;
                    }

                    if (kayitokuma.GetValue(3).ToString() == "Kullanıcı")
                    {
                        radioButton2.Checked = true;
                    }

                    textBox4.Text = kayitokuma.GetValue(4).ToString();//kullanıcıadı alanını aldı.
                    textBox5.Text = kayitokuma.GetValue(5).ToString();//parola
                    textBox6.Text = kayitokuma.GetValue(5).ToString();//parola tekrar alan adı acceste yok
                    break;

                }
                if (kayit_arama_durumu == false)
                {
                    MessageBox.Show("Aranan Kayıt Bulunamadı", "SKY Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }

                baglantim.Close();

            }

            else
            {
                MessageBox.Show("lütfen 11 haneli TC Kimlik Numarası Giriniz..", "SKY Personel Takip", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabPage1Clear();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string yetki = "";// güncellenecek kişinin yönetici mi yoksa kullanıcı mı olacak onu belirler.
          
            //tc kimlik no kontrolü
            if (textBox1.Text.Length < 11 || textBox1.Text == "")
            {
                label1.ForeColor = Color.Red;
            }
            else
            {
                label1.ForeColor = Color.Black;
            }

            //adi veri kontrolu.bi kinini adı 2 karakterden küçük olamaz
            if (textBox2.Text.Length < 2 || textBox2.Text == "")
            {
                label8.ForeColor = Color.Red;
            }
            else
            {
                label8.ForeColor = Color.Black;
            }
            //soyadi veri kontrolu.bi kisinini soyadı 2 karakterden küçük olamaz
            if (textBox3.Text.Length < 2 || textBox3.Text == "")
            {
                label3.ForeColor = Color.Red;
            }
            else
            {
                label3.ForeColor = Color.Black;
            }

            //kullanıcı adı veri kontrolu.bi kisinin kullanıcı adi 8 karakter olmalı
            if (textBox4.Text.Length != 8 || textBox4.Text == "")
            {
                label5.ForeColor = Color.Red;
            }
            else
            {
                label5.ForeColor = Color.Black;
            }

            //parola veri kontrolu.
            if (parolaskoru < 70 || textBox5.Text == "")
            {
                label6.ForeColor = Color.Red;
            }
            else
            {
                label6.ForeColor = Color.Black;
            }

            //parola tekrar veri kontrolu.
            if (textBox5.Text != textBox6.Text || textBox6.Text == "")
            {
                label9.ForeColor = Color.Red;
            }
            else
            {
                label9.ForeColor = Color.Black;
            }

            if (textBox1.Text.Length == 11 && textBox1.Text != "" && textBox2.Text.Length >= 2 && textBox2.Text != ""
                && textBox3.Text.Length >= 2 && textBox3.Text != "" && textBox4.Text.Length == 8 && textBox4.Text != ""
                && parolaskoru >= 70 && textBox5.Text != "" && textBox5.Text == textBox6.Text && textBox6.Text != "")

            {
                if (radioButton1.Checked == true)
                {
                    yetki = "Yönetici";
                }
                else if (radioButton2.Checked == true)
                {
                    yetki = "Kullanıcı";
                }

                try
                {
                    baglantim.Open();
                    // where deyimi tc kimlik numarasına eşit olan kayıt güncellernir. aksi takdirde bütün tablo aynı olur
                    OleDbCommand guncellekoomutu = new OleDbCommand("update kullanicilar set ad='"+textBox2.Text+"',soyad='"+textBox3.Text+"'," +
                        "yetki='"+yetki+"',kullaniciadi='"+textBox4.Text+"',parola='"+textBox5.Text+"' where tcno='"+textBox1.Text+"'", baglantim);

                    guncellekoomutu.ExecuteReader();// guncellekomutu adlı sorgunun sonuçlarını access tablosuna işle
                    baglantim.Close();
                    MessageBox.Show("Kullanıcı Bilgileri Güncellendi", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    //tabPage1Clear();
                    kullanicilari_goster();
                }

                catch (Exception errmsg)
                {
                    MessageBox.Show(errmsg.Message, "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   
                    baglantim.Close();
                }

            }

            else
            {
                MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz!!! ", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length==11)//tc kimlik numarası 11 e eşit oln kayıt sistemde var mı
            {
                bool kayit_arama_durumu = false;
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from kullanicilar where tcno='" + textBox1.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();

                while (kayitokuma.Read())//kayıt okudu mu
                {
                    kayit_arama_durumu = true;//böylr bit kayıt mecut anlamına gelir 
                    //textbox1 e girilen değerin silinmesini sağlar
                    OleDbCommand deletesorgu = new OleDbCommand("delete from kullanicilar where tcno='"+textBox1.Text+"'",baglantim);

                    deletesorgu.ExecuteNonQuery();// silme işlemini access tablosunda gerçklesştir
                    MessageBox.Show("Kullanıcı kaydı Başarıyla Silindi","SKY Personel Takip Programı",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    baglantim.Close();
                    kullanicilari_goster();
                    tabPage1Clear();
                    break;
                }

                if (kayit_arama_durumu==false)// tc kimlik numarası bulunamadıysa
                {
                    MessageBox.Show("Silinecek Kayıt Bulunamadı", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    baglantim.Close();
                }
                    

                tabPage1Clear();

            }
            else
            {
                MessageBox.Show("Lütfrn 11 Karakterden Oluşan T Kimlik Numarası Giriniz.", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabPage1Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog resimsec = new OpenFileDialog();
            resimsec.Title = "Personel Resmi Seçiniz";
            resimsec.Filter = "JPG Dosyalar (*.jpg) | *.jpg";

            if (resimsec.ShowDialog()==DialogResult.OK)// reşim secme(resimsec.ShowDialog()) kullanıcıya gösterildiyse
            {
                this.pictureBox2.Image = new Bitmap(resimsec.OpenFile());//seçilen resmi picturebox2 ye yükle
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string cinsiyet = ""; bool kayitkontrol = false;//daha önce böyle bir kayıt var mı?
            baglantim.Open();
            //access  tablosundaki tcno alanı maskedTextBox1 nessenesine eşit olan kayıtları getir
            OleDbCommand selectsorgu = new OleDbCommand("select * from personeller where tcno='"+maskedTextBox1.Text+"'",baglantim);

            OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();// bu sorgu ile kayıtokuma alanının içini doldurduk.
            while (kayitokuma.Read())//eğer kayıt gelmişse sorgu okuma sonucunda tc alanına yazılan kayıt accceste kayıtlıysa
            {
                kayitkontrol = true;// true döner. yani acceste kayıt var.
                MessageBox.Show("Bu TC Kimlik Numarasına ait kayıt mevcuttur.");
                break;

            }
            baglantim.Close();

            if (kayitkontrol==false)// maxedtextbox a girilen bir personeli kaydı accesste yoksa, ilk kez personel kaydı yapılacaksa
            {
                if (pictureBox2.Image == null)//pictureBox2 boşsa
                    button6.ForeColor = Color.Red;
                else
                    button6.ForeColor = Color.Black;

                if (maskedTextBox1.MaskCompleted == false)//maske tamamlandı mı?//form1_load olayında tanımaldığımız şart sağlamıyorsa
                    label14.ForeColor = Color.Red;
                else
                    label14.ForeColor = Color.Black;

                if (maskedTextBox2.MaskCompleted == false)//maske tamamlandı mı? form1_load olayında tanımaldığımız şart sağlamıyorsa
                    label15.ForeColor = Color.Red;
                else
                    label15.ForeColor = Color.Black;

                if (maskedTextBox3.MaskCompleted == false)//maske tamamlandı mı? form1_load olayında tanımaldığımız şart sağlamıyorsa
                    label16.ForeColor = Color.Red;
                else
                    label16.ForeColor = Color.Black;

                if (comboBox1.Text == "")
                    label18.ForeColor = Color.Red;
                else
                    label18.ForeColor = Color.Black;

                if (comboBox2.Text == "")
                    label11.ForeColor = Color.Red;
                else
                    label11.ForeColor = Color.Black;

                if (comboBox3.Text == "")
                    label12.ForeColor = Color.Red;
                else
                    label12.ForeColor = Color.Black;

                if (maskedTextBox4.MaskCompleted == false || int.Parse(maskedTextBox4.Text) < 1000)//maske tamamlandı mı? form1_load olayında tanımaldığımız şart sağlamıyorsa

                {
                    errorProvider1.SetError(maskedTextBox4, "Maaş Bilgisi 1000 TL den aşağıya olmamlı");
                    label13.ForeColor = Color.Red;
                }
                else
                {
                    label13.ForeColor = Color.Black;
                    errorProvider1.Clear();

                }

                if (pictureBox2.Image!=null && maskedTextBox1.MaskCompleted == true && maskedTextBox2.MaskCompleted == true && maskedTextBox3.MaskCompleted == true
                    && maskedTextBox4.MaskCompleted == true && comboBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != "")
                {
                    baglantim.Open();
                    if (radioButton3.Checked)
                        cinsiyet = "Bay";
                    if (radioButton4.Checked)
                        cinsiyet = "Bayan";
                    try
                    {
                        OleDbCommand kayitekle = new OleDbCommand("insert into personeller values('" + maskedTextBox1.Text + "','" + maskedTextBox2.Text + "','" + maskedTextBox3.Text + "'," +
                                                               " '" + cinsiyet + "','" + comboBox1.Text + "','" + dateTimePicker1.Text + "','" + comboBox2.Text + "'," +
                                                               "'" + comboBox3.Text + "','" + maskedTextBox4.Text + "')", baglantim);
                        kayitekle.ExecuteNonQuery();
                        baglantim.Close();

                        //Application.StartupPath+"\\personelresimler yani bindeki debug klasörü(Application.StartupPath) içinde personelresimler adlı klasör yoksa(!)
                        if (!Directory.Exists(Application.StartupPath + "\\personelresimler")) // "!" işareti yoksa anlamına gelir
                        
                            Directory.CreateDirectory(Application.StartupPath + "\\personelresimler");

                        //resmi personelresimler klasörünün altına tc kimlik numarası ile(maskedTextBox1.Text) kaydeder.
                        pictureBox2.Image.Save(Application.StartupPath + "\\personelresimler\\"+maskedTextBox1.Text+".jpg");//resmi personelresimler klasörünün altına kaydeder

                        
                            

                        MessageBox.Show("Yeni Personel Başarıyla Kaydedildi", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        personelleri_goster();
                        tabPage2Clear();
                    }
                    catch (Exception errmsg)
                    {
                        MessageBox.Show(errmsg.Message,"SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }

                }

                else
                {
                    MessageBox.Show("Kırmızı Olan Bölgerleri Kontrol ediniz", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else
            {
                MessageBox.Show("Bu TC Kimlik Numarası Kayıtlıdır.", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

           




        }
        private void button7_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (maskedTextBox1.Text.Length==11)
            {
                baglantim.Open();
                //maskedTextBox1.Text ile eşleşen kayıtları access  tablosundan getir
                OleDbCommand selectsorgu = new OleDbCommand("select * from personeller where tcno='" + maskedTextBox1.Text + "'",baglantim);
                //OleDbDataReader Ram bellkte oluşturulan bir alan ve select sorgu ile alınan verileri orya yazar
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())// tc alanına girilen değer access tablosunda bulunmuş ve kayitokuma DataReader a aktarılmışsa
                {
                    kayit_arama_durumu = true;
                    try
                    {
                        //Bindeki debug  dosyasının içerisinderki debug klasörünün içindeki personeller klasörüne bak.
                        //kayitokuma.GetValue(0).ToString() değeri access tablosundaki personeller tablosundaki 1 alana olan tcno alanına aittir
                        //tcno.jpg dosyasını al.
                        pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\" + kayitokuma.GetValue(0).ToString() + ".jpg");

                    }

                    catch 
                    {
                        if (pictureBox2.Image==null)
                        {
                            pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\resimyok.jpg");
                        }

                       
                        baglantim.Close();
                    }

                    maskedTextBox2.Text = kayitokuma.GetValue(1).ToString();// access deki ad alanı
                    maskedTextBox3.Text = kayitokuma.GetValue(2).ToString();// access deki soyad alanı

                    if (kayitokuma.GetValue(3).ToString() == "Bay") // access deki cinsiyet alanı
                        radioButton3.Checked = true;
                    else
                        radioButton4.Checked = true;

                    comboBox1.Text = kayitokuma.GetValue(4).ToString(); // access deki mezuniyet alanı

                    dateTimePicker1.Text = kayitokuma.GetValue(5).ToString();//access deki doğumyili alanı

                    comboBox2.Text = kayitokuma.GetValue(6).ToString();//access deki görevi alanı
                    comboBox3.Text = kayitokuma.GetValue(7).ToString();//access deki görevyeri alanı

                    maskedTextBox4.Text = kayitokuma.GetValue(8).ToString();//access deki maasi alanı

                    break;

                }

                if (kayit_arama_durumu==false)
                {
                    MessageBox.Show("Böyle bir TC Kimlik Numarası Mevcut değildir.", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                baglantim.Close();
                personelleri_goster();
            }


            else
            {
                MessageBox.Show("11 Haneli TC no giriniz.", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                tabPage2Clear();
            }

        }
        private void button9_Click(object sender, EventArgs e)
        {
            string cinsiyet = ""; 
            
            if (pictureBox2.Image == null)//pictureBox2 boşsa
                button6.ForeColor = Color.Red;
            else
                button6.ForeColor = Color.Black;

            if (maskedTextBox1.MaskCompleted == false)//maske tamamlandı mı?//form1_load olayında tanımaldığımız şart sağlamıyorsa
                label14.ForeColor = Color.Red;
            else
                label14.ForeColor = Color.Black;

            if (maskedTextBox2.MaskCompleted == false)//maske tamamlandı mı? form1_load olayında tanımaldığımız şart sağlamıyorsa
                label15.ForeColor = Color.Red;
            else
                label15.ForeColor = Color.Black;

            if (maskedTextBox3.MaskCompleted == false)//maske tamamlandı mı? form1_load olayında tanımaldığımız şart sağlamıyorsa
                label16.ForeColor = Color.Red;
            else
                label16.ForeColor = Color.Black;

            if (comboBox1.Text == "")
                label18.ForeColor = Color.Red;
            else
                label18.ForeColor = Color.Black;

            if (comboBox2.Text == "")
                label11.ForeColor = Color.Red;
            else
                label11.ForeColor = Color.Black;

            if (comboBox3.Text == "")
                label12.ForeColor = Color.Red;
            else
                label12.ForeColor = Color.Black;

            if (maskedTextBox4.MaskCompleted == false || int.Parse(maskedTextBox4.Text) < 1000)//maske tamamlandı mı? form1_load olayında tanımaldığımız şart sağlamıyorsa

            {
                errorProvider1.SetError(maskedTextBox4, "Maaş Bilgisi 1000 TL den aşağıya olmamlı");
                label13.ForeColor = Color.Red;
            }
            else
            {
                label13.ForeColor = Color.Black;
                errorProvider1.Clear();

            }

            if (pictureBox2.Image != null && maskedTextBox1.MaskCompleted == true && maskedTextBox2.MaskCompleted == true && maskedTextBox3.MaskCompleted == true
                && maskedTextBox4.MaskCompleted == true && comboBox1.Text != "" && comboBox2.Text != "" && comboBox3.Text != "")
            {
                baglantim.Open();
                if (radioButton3.Checked)
                    cinsiyet = "Bay";
                if (radioButton4.Checked)
                    cinsiyet = "Bayan";
                try
                {
                    OleDbCommand guncelle = new OleDbCommand("update personeller set ad='" + maskedTextBox2.Text + "',soyad='" + maskedTextBox3.Text + "'," +
                                                            "cinsiyet='" + cinsiyet + "',mezun= '" + comboBox1.Text + "',dogumtarihi='" + dateTimePicker1.Text + "',gorevi='" + comboBox2.Text + "'," +
                                                            "gorevyeri='" + comboBox3.Text + "',maasi='" + maskedTextBox4.Text + "' where tcno='"+maskedTextBox1.Text+"'", baglantim);
                    guncelle.ExecuteNonQuery();
                    baglantim.Close();

                    ////Application.StartupPath+"\\personelresimler yani bindeki debug klasörü(Application.StartupPath) içinde personelresimler adlı klasör yoksa(!)
                    //if (!Directory.Exists(Application.StartupPath + "\\personelresimler")) // "!" işareti yoksa anlamına gelir

                    //    Directory.CreateDirectory(Application.StartupPath + "\\personelresimler");

                    ////resmi personelresimler klasörünün altına tc kimlik numarası ile(maskedTextBox1.Text) kaydeder.
                    //pictureBox2.Image.Save(Application.StartupPath + "\\personelresimler\\" + maskedTextBox1.Text + ".jpg");//resmi personelresimler klasörünün altına kaydeder


                    MessageBox.Show("TC Numarası Girilen Kişinin Bilgileri Başarıyla Güncellenmiştir.", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    personelleri_goster();
                    tabPage2Clear();
                }
                catch (Exception errmsg)
                {
                    MessageBox.Show(errmsg.Message, "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    baglantim.Close();
                }

            }

            else
            {
                MessageBox.Show("Kırmızı Olan Bölgerleri Kontrol ediniz", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tabPage2Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.MaskCompleted==true)//11 haneli tc kimlik numrası girilmişse
            {
                baglantim.Open();
                bool kayitarama = false;// girilen tc kimlik numrasına ait kayıt olmadığını varsayıyoruz
                OleDbCommand aramasorgusu = new OleDbCommand("select * from personeller where tcno='"+maskedTextBox1.Text+"'",baglantim);
                OleDbDataReader kayitokuma = aramasorgusu.ExecuteReader();
                while (kayitokuma.Read())//tcno alanına girilen herhangi bir değer il accesteki değer aşleşiyorsa
                {
                    kayitarama = true;

                    OleDbCommand deletesorgu = new OleDbCommand("delete from personeller where tcno='" + maskedTextBox1.Text + "'", baglantim);
                    deletesorgu.ExecuteNonQuery();//bu işlemin sonucunu access e işle.
                    MessageBox.Show("Kayıt Başarıyla Silinmiştir.", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }

                if (kayitarama==false)
                {
                    MessageBox.Show("Silinecek Kayıt Bulunamadı", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                baglantim.Close();
                personelleri_goster();
                tabPage2Clear();
            }

            else
            {
                MessageBox.Show("Lütfen 11 karakterden oluşan TC Kimlik NUmarası Giriniz", "SKY Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
    
}
