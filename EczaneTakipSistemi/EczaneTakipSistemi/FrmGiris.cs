using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EczaneTakipSistemi
{
    public partial class FrmGiris : Form
    {
        TextBox txtKullaniciAdi;
        TextBox txtSifre;
        Button btnGiris;
        Button btnKullaniciOlustur;

        SqlConnection baglanti = new SqlConnection(
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=EczaneTakipDB;Integrated Security=True"
        );

        public FrmGiris()
        {
            EkraniHazirla();
        }

        private void EkraniHazirla()
        {
            this.Text = "Eczane Takip Sistemi";
            this.Size = new Size(920, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(11, 18, 32);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Panel solAlan = new Panel();
            solAlan.Location = new Point(0, 0);
            solAlan.Size = new Size(420, 560);
            solAlan.BackColor = Color.FromArgb(15, 23, 42);
            this.Controls.Add(solAlan);

            PictureBox logo = new PictureBox();
            logo.Image = Properties.Resources.eczane_png;
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.Location = new Point(45, 45);
            logo.Size = new Size(120, 120);
            solAlan.Controls.Add(logo);

            Label baslikSol = new Label();
            baslikSol.Text = "Eczane\nYönetim Paneli";
            baslikSol.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            baslikSol.ForeColor = Color.White;
            baslikSol.Location = new Point(45, 160);
            baslikSol.Size = new Size(330, 120);
            solAlan.Controls.Add(baslikSol);

            Label aciklama = new Label();
            aciklama.Text = "Stok takibi, satış yönetimi,\nkullanıcı yetkilendirme ve raporlama.";
            aciklama.Font = new Font("Segoe UI", 11);
            aciklama.ForeColor = Color.LightGray;
            aciklama.Location = new Point(50, 300);
            aciklama.Size = new Size(330, 70);
            solAlan.Controls.Add(aciklama);

            Label roller = new Label();
            roller.Text = "Admin  •  Eczacı  •  Çalışan";
            roller.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            roller.ForeColor = Color.FromArgb(147, 197, 253);
            roller.Location = new Point(50, 425);
            roller.Size = new Size(300, 30);
            solAlan.Controls.Add(roller);

            Panel kart = new Panel();
            kart.Location = new Point(520, 70);
            kart.Size = new Size(320, 420);
            kart.BackColor = Color.FromArgb(248, 250, 252);
            this.Controls.Add(kart);

            Label baslik = new Label();
            baslik.Text = "Sisteme Giriş";
            baslik.Font = new Font("Segoe UI", 23, FontStyle.Bold);
            baslik.ForeColor = Color.FromArgb(15, 23, 42);
            baslik.Location = new Point(35, 30);
            baslik.Size = new Size(260, 45);
            kart.Controls.Add(baslik);

            Label altBaslik = new Label();
            altBaslik.Text = "Yetkili hesabınızla devam edin";
            altBaslik.Font = new Font("Segoe UI", 9);
            altBaslik.ForeColor = Color.Gray;
            altBaslik.Location = new Point(38, 78);
            altBaslik.Size = new Size(260, 25);
            kart.Controls.Add(altBaslik);

            Label lblKullanici = new Label();
            lblKullanici.Text = "Kullanıcı Adı";
            lblKullanici.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblKullanici.Location = new Point(40, 125);
            lblKullanici.AutoSize = true;
            kart.Controls.Add(lblKullanici);

            txtKullaniciAdi = new TextBox();
            txtKullaniciAdi.Location = new Point(40, 150);
            txtKullaniciAdi.Size = new Size(240, 30);
            txtKullaniciAdi.Font = new Font("Segoe UI", 11);
            kart.Controls.Add(txtKullaniciAdi);

            Label lblSifre = new Label();
            lblSifre.Text = "Şifre";
            lblSifre.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSifre.Location = new Point(40, 200);
            lblSifre.AutoSize = true;
            kart.Controls.Add(lblSifre);

            txtSifre = new TextBox();
            txtSifre.Location = new Point(40, 225);
            txtSifre.Size = new Size(240, 30);
            txtSifre.Font = new Font("Segoe UI", 11);
            txtSifre.PasswordChar = '*';
            kart.Controls.Add(txtSifre);

            btnGiris = new Button();
            btnGiris.Text = "GİRİŞ YAP";
            btnGiris.Location = new Point(40, 285);
            btnGiris.Size = new Size(240, 42);
            btnGiris.BackColor = Color.FromArgb(34, 197, 94);
            btnGiris.ForeColor = Color.White;
            btnGiris.FlatStyle = FlatStyle.Flat;
            btnGiris.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnGiris.Click += BtnGiris_Click;
            kart.Controls.Add(btnGiris);
            this.AcceptButton = btnGiris;

            btnKullaniciOlustur = new Button();
            btnKullaniciOlustur.Text = "Yeni Kullanıcı Oluştur";
            btnKullaniciOlustur.Location = new Point(40, 345);
            btnKullaniciOlustur.Size = new Size(240, 38);
            btnKullaniciOlustur.BackColor = Color.FromArgb(30, 41, 59);
            btnKullaniciOlustur.ForeColor = Color.White;
            btnKullaniciOlustur.FlatStyle = FlatStyle.Flat;
            btnKullaniciOlustur.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnKullaniciOlustur.Click += BtnKullaniciOlustur_Click;
            kart.Controls.Add(btnKullaniciOlustur);
        }

        private void BtnGiris_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            SqlCommand komut = new SqlCommand(
                "SELECT Rol FROM Kullanicilar WHERE KullaniciAdi=@kadi AND Sifre=@sifre",
                baglanti
            );

            komut.Parameters.AddWithValue("@kadi", txtKullaniciAdi.Text);
            komut.Parameters.AddWithValue("@sifre", txtSifre.Text);

            object sonuc = komut.ExecuteScalar();

            if (sonuc != null)
            {
                SqlCommand girisGuncelle = new SqlCommand(
                    "UPDATE Kullanicilar SET SonGirisTarihi=@tarih WHERE KullaniciAdi=@kadi",
                    baglanti
                );

                girisGuncelle.Parameters.AddWithValue("@tarih", DateTime.Now);
                girisGuncelle.Parameters.AddWithValue("@kadi", txtKullaniciAdi.Text);

                girisGuncelle.ExecuteNonQuery();

                baglanti.Close();

                MessageBox.Show("Giriş başarılı. Rol: " + sonuc.ToString());

                string rol = sonuc.ToString();

                if (rol == "Admin")
                {
                    FrmAdminPanel adminPanel = new FrmAdminPanel();
                    adminPanel.Show();
                }
                else
                {
                    FrmIlaclar ilacFormu = new FrmIlaclar(rol);
                    ilacFormu.Show();
                }

                this.Hide();
            }
            else
            {
                baglanti.Close();
                MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
            }
        }
        private void BtnKullaniciOlustur_Click(object sender, EventArgs e)
        {
            FrmKullaniciEkle frm = new FrmKullaniciEkle();
            frm.ShowDialog();
        }
    }
}
