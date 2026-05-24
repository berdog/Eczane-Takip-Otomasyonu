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
    public partial class FrmKullaniciEkle : Form
    {
        TextBox txtKullaniciAdi;
        TextBox txtSifre;
        ComboBox cmbRol;
        TextBox txtEczaciSifre;

        Button btnKaydet;

        SqlConnection baglanti = new SqlConnection(
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=EczaneTakipDB;Integrated Security=True"
        );

        public FrmKullaniciEkle()
        {
            FormHazirla();
        }

        private void FormHazirla()
        {
            this.Text = "Kullanıcı Oluştur";
            this.Size = new Size(520, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 30, 48);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Panel kart = new Panel();
            kart.Location = new Point(70, 45);
            kart.Size = new Size(360, 470);
            kart.BackColor = Color.White;
            this.Controls.Add(kart);

            Label ikon = new Label();
            ikon.Text = "👤";
            ikon.Font = new Font("Segoe UI Emoji", 32, FontStyle.Bold);
            ikon.Location = new Point(145, 15);
            ikon.Size = new Size(80, 55);
            kart.Controls.Add(ikon);

            Label lblBaslik = new Label();
            lblBaslik.Text = "Kullanıcı Oluştur";
            lblBaslik.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblBaslik.ForeColor = Color.FromArgb(17, 24, 39);
            lblBaslik.Location = new Point(45, 75);
            lblBaslik.Size = new Size(280, 40);
            lblBaslik.TextAlign = ContentAlignment.MiddleCenter;
            kart.Controls.Add(lblBaslik);

            Label lblAlt = new Label();
            lblAlt.Text = "Yeni eczacı veya çalışan hesabı ekleyin";
            lblAlt.Font = new Font("Segoe UI", 9);
            lblAlt.ForeColor = Color.Gray;
            lblAlt.Location = new Point(45, 115);
            lblAlt.Size = new Size(280, 25);
            lblAlt.TextAlign = ContentAlignment.MiddleCenter;
            kart.Controls.Add(lblAlt);

            Label lblKullanici = new Label();
            lblKullanici.Text = "Kullanıcı Adı";
            lblKullanici.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblKullanici.Location = new Point(45, 155);
            lblKullanici.AutoSize = true;
            kart.Controls.Add(lblKullanici);

            txtKullaniciAdi = new TextBox();
            txtKullaniciAdi.Location = new Point(45, 180);
            txtKullaniciAdi.Size = new Size(270, 25);
            txtKullaniciAdi.Font = new Font("Segoe UI", 10);
            kart.Controls.Add(txtKullaniciAdi);

            Label lblSifre = new Label();
            lblSifre.Text = "Şifre";
            lblSifre.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSifre.Location = new Point(45, 215);
            lblSifre.AutoSize = true;
            kart.Controls.Add(lblSifre);

            txtSifre = new TextBox();
            txtSifre.Location = new Point(45, 240);
            txtSifre.Size = new Size(270, 25);
            txtSifre.Font = new Font("Segoe UI", 10);
            txtSifre.PasswordChar = '*';
            kart.Controls.Add(txtSifre);

            Label lblRol = new Label();
            lblRol.Text = "Rol";
            lblRol.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblRol.Location = new Point(45, 275);
            lblRol.AutoSize = true;
            kart.Controls.Add(lblRol);

            cmbRol = new ComboBox();
            cmbRol.Location = new Point(45, 300);
            cmbRol.Size = new Size(270, 25);
            cmbRol.Font = new Font("Segoe UI", 10);
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Items.Add("Eczacı");
            cmbRol.Items.Add("Çalışan");
            cmbRol.SelectedIndex = 0;
            kart.Controls.Add(cmbRol);

            Label lblEczaci = new Label();
            lblEczaci.Text = "Eczacı Onay Şifresi";
            lblEczaci.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblEczaci.Location = new Point(45, 335);
            lblEczaci.AutoSize = true;
            kart.Controls.Add(lblEczaci);

            txtEczaciSifre = new TextBox();
            txtEczaciSifre.Location = new Point(45, 360);
            txtEczaciSifre.Size = new Size(270, 25);
            txtEczaciSifre.Font = new Font("Segoe UI", 10);
            txtEczaciSifre.PasswordChar = '*';
            kart.Controls.Add(txtEczaciSifre);

            btnKaydet = new Button();
            btnKaydet.Text = "KULLANICIYI KAYDET";
            btnKaydet.Location = new Point(45, 410);
            btnKaydet.Size = new Size(270, 40);
            btnKaydet.BackColor = Color.FromArgb(37, 99, 235);
            btnKaydet.ForeColor = Color.White;
            btnKaydet.FlatStyle = FlatStyle.Flat;
            btnKaydet.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnKaydet.Click += BtnKaydet_Click;
            kart.Controls.Add(btnKaydet);
            this.AcceptButton = btnKaydet;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            SqlCommand kontrol = new SqlCommand(
                "SELECT COUNT(*) FROM Kullanicilar WHERE Rol='Eczacı' AND Sifre=@sifre",
                baglanti
            );

            kontrol.Parameters.AddWithValue("@sifre", txtEczaciSifre.Text);

            int sonuc = (int)kontrol.ExecuteScalar();

            if (sonuc > 0)
            {
                SqlCommand komut = new SqlCommand(
                    "INSERT INTO Kullanicilar (KullaniciAdi, Sifre, Rol) VALUES (@kadi,@sifre,@rol)",
                    baglanti
                );

                komut.Parameters.AddWithValue("@kadi", txtKullaniciAdi.Text);
                komut.Parameters.AddWithValue("@sifre", txtSifre.Text);
                komut.Parameters.AddWithValue("@rol", cmbRol.Text);

                komut.ExecuteNonQuery();

                MessageBox.Show("Kullanıcı başarıyla eklendi.");
            }
            else
            {
                MessageBox.Show("Eczacı şifresi yanlış.");
            }

            baglanti.Close();
        }
    }
}
