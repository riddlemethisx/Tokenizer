using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tokenizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDegerUret_Click(object sender, EventArgs e)
        {
            Resetle();
            RegexAlgila();
        }

        private void Resetle()
        {
            regex = null;
            kelime = null;
        }






        string regex = null;
        string kelime = null;
        private void RegexAlgila()
        {
            string metin = txtRegex.Text;

            for (int i = 0; i < metin.Length; i++)
            {
                if (metin[i] != '(' && metin[i] != '@' && metin[i] != ')' && metin[i] != '*' && metin[i] != '+')
                {
                    kelime += metin[i];
                    regex += metin[i];
                }
                else
                {
                    if (metin[i] == '(')
                    {
                        int parantezBaslangic = i;

                        #region ")" karakterini bulma
                        bool bulundu = false;
                        int k = i + 1;
                        while (bulundu == false)
                        {
                            if (metin[k] == ')')
                                bulundu = true;
                            else
                                k++;
                        }
                        int parantezBitis = k;
                        //label5.Text = parantezBitis.ToString();
                        #endregion

                        int harfSayisi = parantezBitis - parantezBaslangic - 1;
                        string parantezIci = metin.Substring(parantezBaslangic + 1, harfSayisi);

                        //label5.Text = parantezIci;

                        #region YILDIZ KONTROLÜ
                        try
                        {
                            if (metin[parantezBitis + 1] != '*')
                            {
                                //1.1
                                kelime += ParantezIciCalistir(harfSayisi, parantezIci);
                                i += harfSayisi + 1;
                            }
                            else
                            {
                                //yıldız varsa
                                //parantezin dışında yıldız olmasa dahi yanlışlıkla bu else'in içersine giriyor

                                int olabilirlikDurumu = rastgele.Next(2);
                                if (olabilirlikDurumu == 1)
                                    kelime += ParantezIciCalistir(harfSayisi, parantezIci);

                                i += harfSayisi + 2;

                            }
                        }
                        catch
                        {
                            //hata veriyorsa demekki yıldız yoktur
                            //1.1
                            kelime += ParantezIciCalistir(harfSayisi, parantezIci);
                            i += harfSayisi + 1;
                        }
                        #endregion

                    }
                    else if (metin[i] == '@')
                    {
                        //bu kısım çalışıyor
                        kelime += OzelKarakterUret();
                        regex += "@";
                    }
                    else if (metin[i] == '*')
                    {
                        //bu kısım çalışıyor
                        int olabilirlikDurumu = rastgele.Next(2);
                        if (olabilirlikDurumu == 0)
                            kelime = kelime.Substring(0, i - 1);

                    }
                    else if (metin[i] == '+')
                    {


                        int rastgeleDeger = rastgele.Next(2);
                        if (rastgeleDeger == 1)
                        {
                            kelime = kelime.Substring(0, i - 1);
                            //eğer 1 seçilmişse önceki karakter silinir ve otomatik olarak +'dan sonraki karakteri zaten döngü yazdıracaktır. ekstra birşey yapmamıza gerek yok
                        }
                        else
                        {
                            //eğer 0 seçilmişse ilk karakter alınır +'dan sonraki karakter atlanır. BU KONTROL +'YA GELİNDİĞİNDE YAPILMAKTADIR
                            i++;
                        }

                    }



                }


            }

            Yazdir();
        }








        string ParantezIciCalistir(int harfSayisi, string parantezIci)
        {
            if (harfSayisi == 1 && parantezIci == "a")
            {
                return HarfUret();

                    
            }
            else
            {
                //if (harfSayisi > 1)

                if (parantezIci == "09")
                    return Uret09();
                else
                {
                    if (parantezIci.IndexOf("+") == -1)
                        return KelimeUret(parantezIci);
                    else
                        return YaDaIslemi(parantezIci);
                }
            }
        }





        string YaDaIslemi(string parantezIci)
        {
            string dondurulecekHarf = null;
            int artiIndex = parantezIci.IndexOf("+");

            string[] harfler = parantezIci.Split("+");
            //label5.Text = artiIndex.ToString();

            int rastgeleDeger = rastgele.Next(harfler.Length);
            dondurulecekHarf = harfler[rastgeleDeger];

            if (char.IsDigit(char.Parse(dondurulecekHarf) ) )
                regex += @"\d";
            else
                regex += @"\w";

            return dondurulecekHarf;
        }


















        string Uret09()
        {
            string uretilenDeger = null;

            int adet = rastgele.Next(2, 4);
            for (int i = 0; i < adet; i++)
            {
                int deger = rastgele.Next(9);
                uretilenDeger += deger.ToString();
            }

            regex += @"\d{"+adet+"}";
            return uretilenDeger;
        }

        string KelimeUret(string parantezIci)
        {
            string uretilenKelime = null;
            char[] harfler = parantezIci.ToCharArray();

            int adet = rastgele.Next(3, 5);
            for (int i = 0; i < adet; i++)
            {
                int rastgeleDeger = rastgele.Next(harfler.Length);
                uretilenKelime += harfler[rastgeleDeger];
            }

            
            if (char.IsDigit(harfler[0] ))
                regex += @"\d{" + adet + "}";
            else
                regex += @"\w{" + adet + "}";
            return uretilenKelime;
        }




        string HarfUret()
        {
            string uretilenKelime = null;
            string[] harfler = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "v", "y", "z", "w", "q", "x" };

            if (txtAlfabe.Text != "")
            {
                harfler = txtAlfabe.Text.Split(',');
            }

            int adet = rastgele.Next(3, 5);
            for (int i = 0; i < adet; i++)
            {
                int rastgeleDeger = rastgele.Next(harfler.Length);
                uretilenKelime += harfler[rastgeleDeger];
            }

            regex += @"\w{"+adet+"}";
            return uretilenKelime;
        }





        private void Yazdir()
        {
            txtKelimeler.Text += kelime + "\n";
            txtRegexYazdir.Text = regex;
        }

        Random rastgele = new Random();
        string OzelKarakterUret()
        {
            string ozelKarakter = null;

            string[] karakterler = { "_", "-", "/", "#", "<", ">", "|", "$", "&", "!" };
            int rastgeleDeger = rastgele.Next(karakterler.Length - 1);

            ozelKarakter = karakterler[rastgeleDeger];

            return ozelKarakter;
        }










        bool move;
        int mouse_x;
        int mouse_y;


        private void btnKapatma_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAsagiAlma_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
        }

        private void btnHarf_Click(object sender, EventArgs e)
        {
            txtRegex.Text += "(a)";
        }

        private void btnSayi_Click(object sender, EventArgs e)
        {
            txtRegex.Text += "(09)";
        }

        private void btnOlabilir_Click(object sender, EventArgs e)
        {
            txtRegex.Text += "*";
        }

        private void btnYada_Click(object sender, EventArgs e)
        {
            txtRegex.Text += "+";
        }

        private void btnOzelKrt_Click(object sender, EventArgs e)
        {
            txtRegex.Text += "@";
        }

        private void btnKontrolEt_Click(object sender, EventArgs e)
        {
            Regex reg = new Regex("^"+regex +"$");
            Match match = reg.Match(txtKelimeKontrol.Text);

            if (match.Success)
                label16.Text = "RegEx'e uymaktadır";
            else
                label16.Text = "RegEx'e uymamaktadır";
           
        }
    }
}
