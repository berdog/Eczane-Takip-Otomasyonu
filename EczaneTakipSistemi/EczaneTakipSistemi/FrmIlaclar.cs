using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace EczaneTakipSistemi
{
    public partial class FrmIlaclar : Form
    {
        TextBox txtIlacAdi, txtBarkod, txtKategori, txtStok, txtFiyat, txtAra;
        ComboBox cmbKategoriFiltre;
        DateTimePicker dtpSonKullanma;

        Button btnEkle, btnListele, btnGuncelle, btnSil;
        Button btnSatisGecmisi;
        Button btnSatisYap;
        Button btnGunlukKazanc;
        Button btnKategoriFiltreleme;
        Button btnCikis;

        Panel formPanel;
        Panel pnlGunlukKazanc;

        Label lblAra;
        Label lblKategoriFiltre;
        Label lblHosgeldiniz;
        Label lblHosgeldinizAlt;

        DataGridView dataGridView1;

        string aktifSayfa = "AnaSayfa";
        string kullaniciRolu;

        SqlConnection baglanti = new SqlConnection(
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=EczaneTakipDB;Integrated Security=True"
        );

        public FrmIlaclar(string rol)
        {
            kullaniciRolu = rol;
            SayfayiHazirla();
            YetkiAyarla();
        }

        private void SayfayiHazirla()
        {
            this.Text = "Eczane Takip Sistemi - İlaç İşlemleri";
            this.Size = new Size(1300, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 30, 48);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;

            Panel menu = new Panel();
            menu.Location = new Point(0, 0);
            menu.Size = new Size(300, this.ClientSize.Height);
            menu.BackColor = Color.FromArgb(17, 24, 39);
            this.Controls.Add(menu);

            Label logo = new Label();
            logo.Text = "💊";
            logo.Font = new Font("Segoe UI Emoji", 42, FontStyle.Bold);
            logo.Location = new Point(75, 35);
            logo.Size = new Size(100, 80);
            menu.Controls.Add(logo);

            Label menuBaslik = new Label();
            menuBaslik.Text = "Eczane\nTakip Sistemi";
            menuBaslik.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            menuBaslik.ForeColor = Color.White;
            menuBaslik.Location = new Point(35, 130);
            menuBaslik.Size = new Size(200, 90);
            menu.Controls.Add(menuBaslik);

            Label menuAlt = new Label();
            menuAlt.Text = "İlaç kayıt ve stok yönetimi";
            menuAlt.Font = new Font("Segoe UI", 9);
            menuAlt.ForeColor = Color.LightGray;
            menuAlt.Location = new Point(35, 235);
            menuAlt.Size = new Size(190, 30);
            menu.Controls.Add(menuAlt);

            Button btnAnaSayfa = MenuButonu("Ana Sayfa", 285);
            btnAnaSayfa.BackColor = Color.FromArgb(37, 99, 235);
            btnAnaSayfa.Click += BtnAnaSayfa_Click;
            menu.Controls.Add(btnAnaSayfa);

            Button btnIlacEkleme = MenuButonu("İlaç Ekleme", 330);
            btnIlacEkleme.Click += BtnIlacEkleme_Click;
            menu.Controls.Add(btnIlacEkleme);

            Button btnStokListesi = MenuButonu("Stok Listesi", 375);
            btnStokListesi.Click += BtnStokListesi_Click;
            menu.Controls.Add(btnStokListesi);

            Button btnIlacArama = MenuButonu("İlaç Arama", 420);
            btnIlacArama.Click += BtnIlacArama_Click;
            menu.Controls.Add(btnIlacArama);

            Button btnSKT = MenuButonu("Son Kullanma Tarihleri", 465);
            btnSKT.Click += BtnSKT_Click;
            menu.Controls.Add(btnSKT);

            btnSatisGecmisi = MenuButonu("Satış Kayıt Geçmişi", 510);
            btnSatisGecmisi.Click += BtnSatisGecmisi_Click;
            menu.Controls.Add(btnSatisGecmisi);

            btnSatisYap = MenuButonu("Satış Yap", 555);
            btnSatisYap.Click += BtnSatisYap_Click;
            menu.Controls.Add(btnSatisYap);

            btnKategoriFiltreleme = MenuButonu("Kategori Filtreleme", 600);
            btnKategoriFiltreleme.Click += BtnKategoriFiltreleme_Click;
            menu.Controls.Add(btnKategoriFiltreleme);

            btnGunlukKazanc = MenuButonu("Günlük Kazanç", 645);
            btnGunlukKazanc.Click += BtnGunlukKazanc_Click;
            menu.Controls.Add(btnGunlukKazanc);

            btnCikis = MenuButonu("Çıkış Yap", 690);
            btnCikis.BackColor = Color.FromArgb(127, 29, 29);
            btnCikis.Click += BtnCikis_Click;
            menu.Controls.Add(btnCikis);

            IlacFormPaneliOlustur();
            AramaAlaniOlustur();
            KategoriFiltreAlaniOlustur();
            DataGridOlustur();

            AnaSayfayiGoster();
        }

        private Button MenuButonu(string yazi, int y)
        {
            Button btn = new Button();
            btn.Text = yazi;
            btn.Location = new Point(35, y);
            btn.Size = new Size(170, 38);
            btn.BackColor = Color.FromArgb(31, 41, 55);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            return btn;
        }

        private void IlacFormPaneliOlustur()
        {
            formPanel = new Panel();
            formPanel.Location = new Point(350, 120);
            formPanel.Size = new Size(340, 560);
            formPanel.BackColor = Color.White;
            this.Controls.Add(formPanel);

            Label formBaslik = new Label();
            formBaslik.Text = "İlaç Bilgileri";
            formBaslik.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            formBaslik.ForeColor = Color.FromArgb(17, 24, 39);
            formBaslik.Location = new Point(25, 20);
            formBaslik.Size = new Size(250, 30);
            formPanel.Controls.Add(formBaslik);

            Label lblIlacAdi = Etiket("İlaç Adı", 70);
            formPanel.Controls.Add(lblIlacAdi);

            txtIlacAdi = TextKutusu(92);
            formPanel.Controls.Add(txtIlacAdi);

            Label lblBarkod = Etiket("Barkod No", 125);
            formPanel.Controls.Add(lblBarkod);

            txtBarkod = TextKutusu(147);
            formPanel.Controls.Add(txtBarkod);

            Label lblKategori = Etiket("Kategori", 180);
            formPanel.Controls.Add(lblKategori);

            txtKategori = TextKutusu(202);
            formPanel.Controls.Add(txtKategori);

            Label lblStok = Etiket("Stok", 235);
            formPanel.Controls.Add(lblStok);

            txtStok = TextKutusu(257);
            formPanel.Controls.Add(txtStok);

            Label lblFiyat = Etiket("Fiyat", 290);
            formPanel.Controls.Add(lblFiyat);

            txtFiyat = TextKutusu(312);
            formPanel.Controls.Add(txtFiyat);

            Label lblSKT = Etiket("Son Kullanma Tarihi", 345);
            formPanel.Controls.Add(lblSKT);

            dtpSonKullanma = new DateTimePicker();
            dtpSonKullanma.Location = new Point(25, 370);
            dtpSonKullanma.Size = new Size(280, 25);
            formPanel.Controls.Add(dtpSonKullanma);

            btnEkle = IslemButonu("Ekle", 25, 445, Color.SeaGreen);
            btnEkle.Click += BtnEkle_Click;
            formPanel.Controls.Add(btnEkle);

            btnListele = IslemButonu("Listele", 175, 445, Color.RoyalBlue);
            btnListele.Click += BtnListele_Click;
            formPanel.Controls.Add(btnListele);

            btnGuncelle = IslemButonu("Güncelle", 25, 495, Color.DarkOrange);
            btnGuncelle.Click += BtnGuncelle_Click;
            formPanel.Controls.Add(btnGuncelle);

            btnSil = IslemButonu("Sil", 175, 495, Color.Firebrick);
            btnSil.Click += BtnSil_Click;
            formPanel.Controls.Add(btnSil);
        }

        private Label Etiket(string yazi, int y)
        {
            Label lbl = new Label();
            lbl.Text = yazi;
            lbl.Location = new Point(25, y);
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            return lbl;
        }

        private TextBox TextKutusu(int y)
        {
            TextBox txt = new TextBox();
            txt.Location = new Point(25, y);
            txt.Size = new Size(280, 25);
            return txt;
        }

        private Button IslemButonu(string yazi, int x, int y, Color renk)
        {
            Button btn = new Button();
            btn.Text = yazi;
            btn.Location = new Point(x, y);
            btn.Size = new Size(130, 35);
            btn.BackColor = renk;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            return btn;
        }

        private void AramaAlaniOlustur()
        {
            lblAra = new Label();
            lblAra.Text = "İlaç Ara";
            lblAra.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblAra.ForeColor = Color.White;
            lblAra.Location = new Point(780, 120);
            lblAra.AutoSize = true;
            this.Controls.Add(lblAra);

            txtAra = new TextBox();
            txtAra.Location = new Point(780, 145);
            txtAra.Size = new Size(500, 25);
            txtAra.Font = new Font("Segoe UI", 10);
            txtAra.TextChanged += TxtAra_TextChanged;
            this.Controls.Add(txtAra);
        }

        private void KategoriFiltreAlaniOlustur()
        {
            lblKategoriFiltre = new Label();
            lblKategoriFiltre.Text = "Kategori Filtrele";
            lblKategoriFiltre.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblKategoriFiltre.ForeColor = Color.White;
            lblKategoriFiltre.Location = new Point(330, 110);
            lblKategoriFiltre.AutoSize = true;
            this.Controls.Add(lblKategoriFiltre);

            cmbKategoriFiltre = new ComboBox();
            cmbKategoriFiltre.Location = new Point(330, 140);
            cmbKategoriFiltre.Size = new Size(250, 30);
            cmbKategoriFiltre.Font = new Font("Segoe UI", 10);
            cmbKategoriFiltre.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbKategoriFiltre.Items.Add("Tümü");
            cmbKategoriFiltre.Items.Add("Ağrı Kesici");
            cmbKategoriFiltre.Items.Add("Antibiyotik");
            cmbKategoriFiltre.Items.Add("Vitamin");
            cmbKategoriFiltre.Items.Add("Şurup");
            cmbKategoriFiltre.Items.Add("Krem");

            cmbKategoriFiltre.SelectedIndex = 0;
            cmbKategoriFiltre.SelectedIndexChanged += CmbKategoriFiltre_SelectedIndexChanged;

            this.Controls.Add(cmbKategoriFiltre);
        }

        private void DataGridOlustur()
        {
            dataGridView1 = new DataGridView();
            dataGridView1.Location = new Point(780, 185);
            dataGridView1.Size = new Size(500, 480);
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.CellClick += DataGridView1_CellClick;
            this.Controls.Add(dataGridView1);
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtStok.Text, out int stok))
            {
                MessageBox.Show("Stok alanına sayı giriniz.");
                return;
            }

            if (!decimal.TryParse(txtFiyat.Text.Replace(".", ","), out decimal fiyat))
            {
                MessageBox.Show("Fiyat alanına sayı giriniz.");
                return;
            }

            baglanti.Open();

            SqlCommand barkodKontrol = new SqlCommand(
                "SELECT COUNT(*) FROM Ilaclar WHERE BarkodNo=@barkod",
                baglanti
            );

            barkodKontrol.Parameters.AddWithValue("@barkod", txtBarkod.Text);

            int barkodSayisi = (int)barkodKontrol.ExecuteScalar();

            if (barkodSayisi > 0)
            {
                MessageBox.Show("Bu barkod numarasında ilaç var. Lütfen farklı barkod no giriniz.");
                baglanti.Close();
                return;
            }

            SqlCommand komut = new SqlCommand(
                "INSERT INTO Ilaclar (IlacAdi, BarkodNo, Kategori, Stok, Fiyat, SonKullanmaTarihi) VALUES (@p1,@p2,@p3,@p4,@p5,@p6)",
                baglanti
            );

            komut.Parameters.AddWithValue("@p1", txtIlacAdi.Text);
            komut.Parameters.AddWithValue("@p2", txtBarkod.Text);
            komut.Parameters.AddWithValue("@p3", txtKategori.Text);
            komut.Parameters.AddWithValue("@p4", stok);
            komut.Parameters.AddWithValue("@p5", fiyat);
            komut.Parameters.AddWithValue("@p6", dtpSonKullanma.Value.Date);

            komut.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("İlaç başarıyla eklendi.");
            BtnListele_Click(null, null);
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Ilaclar", baglanti);
            DataTable tablo = new DataTable();
            da.Fill(tablo);

            dataGridView1.DataSource = tablo;

            baglanti.Close();

            StokUyarisiRenklendir();

            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;

            string stokMesaj = "";
            string sktMesaj = "";

            foreach (DataGridViewRow satir in dataGridView1.Rows)
            {
                if (satir.Cells["IlacAdi"].Value == null)
                    continue;

                string ilacAdi = satir.Cells["IlacAdi"].Value.ToString();

                // STOK KONTROL
                int stok = Convert.ToInt32(satir.Cells["Stok"].Value);

                if (stok <= 10)
                {
                    stokMesaj += "• " + ilacAdi + " adlı ilacın stoğu azalıyor!\n";
                }

                // SON KULLANMA TARİHİ KONTROL
                DateTime skt = Convert.ToDateTime(
                    satir.Cells["SonKullanmaTarihi"].Value
                );

                if (skt < DateTime.Now.Date)
                {
                    sktMesaj += "• " + ilacAdi + " adlı ilacın son kullanma tarihi geçti!\n";
                }
            }

            // STOK UYARISI
            if (stokMesaj != "")
            {
                MessageBox.Show(
                    stokMesaj,
                    "Kritik Stok Uyarısı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

            // SON KULLANIM TARİHİ UYARISI
            if (sktMesaj != "")
            {
                MessageBox.Show(
                    sktMesaj,
                    "Son Kullanma Tarihi Uyarısı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void StokUyarisiRenklendir()
        {
            foreach (DataGridViewRow satir in dataGridView1.Rows)
            {
                if (satir.Cells["Stok"].Value != null)
                {
                    int stok = Convert.ToInt32(satir.Cells["Stok"].Value);

                    if (stok <= 10)
                    {
                        satir.DefaultCellStyle.BackColor = Color.Firebrick;
                        satir.DefaultCellStyle.ForeColor = Color.White;
                    }
                    else
                    {
                        satir.DefaultCellStyle.BackColor = Color.White;
                        satir.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (aktifSayfa != "Ilac")
                return;

            if (e.RowIndex < 0)
                return;

            if (!dataGridView1.Columns.Contains("BarkodNo"))
                return;

            DataGridViewRow satir = dataGridView1.Rows[e.RowIndex];

            txtIlacAdi.Text = satir.Cells["IlacAdi"].Value.ToString();
            txtBarkod.Text = satir.Cells["BarkodNo"].Value.ToString();
            txtKategori.Text = satir.Cells["Kategori"].Value.ToString();
            txtStok.Text = satir.Cells["Stok"].Value.ToString();
            txtFiyat.Text = satir.Cells["Fiyat"].Value.ToString();

            if (satir.Cells["SonKullanmaTarihi"].Value != DBNull.Value &&
                satir.Cells["SonKullanmaTarihi"].Value != null)
            {
                dtpSonKullanma.Value = Convert.ToDateTime(satir.Cells["SonKullanmaTarihi"].Value);
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silinecek ilacı seçiniz.");
                return;
            }

            int ilacID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["IlacID"].Value);

            DialogResult cevap = MessageBox.Show(
                "Seçili ilacı silmek istiyor musunuz?",
                "Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (cevap == DialogResult.Yes)
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand(
                    "DELETE FROM Ilaclar WHERE IlacID=@id",
                    baglanti
                );

                komut.Parameters.AddWithValue("@id", ilacID);
                komut.ExecuteNonQuery();

                baglanti.Close();

                MessageBox.Show("İlaç silindi.");
                BtnListele_Click(null, null);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen güncellenecek ilacı seçiniz.");
                return;
            }

            if (!int.TryParse(txtStok.Text, out int stok))
            {
                MessageBox.Show("Stok alanına sayı giriniz.");
                return;
            }

            if (!decimal.TryParse(txtFiyat.Text.Replace(".", ","), out decimal fiyat))
            {
                MessageBox.Show("Fiyat alanına sayı giriniz.");
                return;
            }

            int ilacID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["IlacID"].Value);

            baglanti.Open();

            SqlCommand komut = new SqlCommand(
                "UPDATE Ilaclar SET IlacAdi=@p1, BarkodNo=@p2, Kategori=@p3, Stok=@p4, Fiyat=@p5, SonKullanmaTarihi=@p6 WHERE IlacID=@id",
                baglanti
            );

            komut.Parameters.AddWithValue("@p1", txtIlacAdi.Text);
            komut.Parameters.AddWithValue("@p2", txtBarkod.Text);
            komut.Parameters.AddWithValue("@p3", txtKategori.Text);
            komut.Parameters.AddWithValue("@p4", stok);
            komut.Parameters.AddWithValue("@p5", fiyat);
            komut.Parameters.AddWithValue("@p6", dtpSonKullanma.Value.Date);
            komut.Parameters.AddWithValue("@id", ilacID);

            komut.ExecuteNonQuery();

            baglanti.Close();

            MessageBox.Show("İlaç güncellendi.");
            BtnListele_Click(null, null);
        }

        private void YetkiAyarla()
        {
            if (kullaniciRolu == "Eczacı")
            {
                this.Text = "Eczane Paneli - Eczacı";

                btnEkle.Enabled = true;
                btnGuncelle.Enabled = true;
                btnSil.Enabled = true;

                btnSatisGecmisi.Visible = true;
                btnSatisYap.Visible = true;
                btnKategoriFiltreleme.Visible = true;
                btnGunlukKazanc.Visible = true;

                btnSatisGecmisi.Location = new Point(35, 510);
                btnSatisYap.Location = new Point(35, 555);
                btnKategoriFiltreleme.Location = new Point(35, 600);
                btnGunlukKazanc.Location = new Point(35, 645);
                btnCikis.Location = new Point(35, 690);
            }
            else if (kullaniciRolu == "Çalışan")
            {
                this.Text = "Eczane Paneli - Çalışan";

                btnEkle.Enabled = true;
                btnGuncelle.Enabled = false;
                btnSil.Enabled = false;

                btnSatisGecmisi.Visible = false;
                btnGunlukKazanc.Visible = false;

                btnSatisYap.Visible = true;
                btnKategoriFiltreleme.Visible = true;

                btnSatisYap.Location = new Point(35, 510);
                btnKategoriFiltreleme.Location = new Point(35, 555);
                btnCikis.Location = new Point(35, 600);
            }
        }

        private void BtnAnaSayfa_Click(object sender, EventArgs e)
        {
            AnaSayfayiGoster();
        }

        private void BtnIlacEkleme_Click(object sender, EventArgs e)
        {
            aktifSayfa = "Ilac";

            TumSayfalariGizle();

            lblAra.Location = new Point(780, 120);
            txtAra.Location = new Point(780, 145);
            txtAra.Size = new Size(500, 25);

            dataGridView1.Location = new Point(780, 185);
            dataGridView1.Size = new Size(500, 480);

            formPanel.Visible = true;
            dataGridView1.Visible = true;
        }

        private void BtnStokListesi_Click(object sender, EventArgs e)
        {
            aktifSayfa = "Ilac";

            TumSayfalariGizle();

            dataGridView1.Location = new Point(330, 140);
            dataGridView1.Size = new Size(900, 500);
            dataGridView1.Visible = true;

            BtnListele_Click(null, null);
        }

        private void BtnIlacArama_Click(object sender, EventArgs e)
        {
            aktifSayfa = "Ilac";

            TumSayfalariGizle();

            lblAra.Location = new Point(330, 110);
            txtAra.Location = new Point(330, 140);
            txtAra.Size = new Size(900, 30);

            dataGridView1.Location = new Point(330, 190);
            dataGridView1.Size = new Size(900, 450);

            lblAra.Visible = true;
            txtAra.Visible = true;
            dataGridView1.Visible = true;

            txtAra.Focus();
        }

        private void BtnSKT_Click(object sender, EventArgs e)
        {
            aktifSayfa = "SKT";

            TumSayfalariGizle();

            dataGridView1.Location = new Point(330, 140);
            dataGridView1.Size = new Size(900, 500);
            dataGridView1.Visible = true;

            baglanti.Open();

            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT IlacID, IlacAdi, BarkodNo, Kategori, Stok, Fiyat, SonKullanmaTarihi " +
                "FROM Ilaclar " +
                "WHERE SonKullanmaTarihi <= DATEADD(day, 30, GETDATE()) " +
                "ORDER BY SonKullanmaTarihi ASC",
                baglanti
            );

            DataTable tablo = new DataTable();
            da.Fill(tablo);

            dataGridView1.DataSource = tablo;

            baglanti.Close();

            if (tablo.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Son kullanma tarihi geçmiş veya 30 gün içinde dolacak ilaç bulunamadı.",
                    "Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void BtnSatisGecmisi_Click(object sender, EventArgs e)
        {
            aktifSayfa = "Satis";

            TumSayfalariGizle();

            dataGridView1.Location = new Point(330, 140);
            dataGridView1.Size = new Size(900, 500);
            dataGridView1.Visible = true;

            baglanti.Open();

            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM SatisGecmisi ORDER BY SatisTarihi DESC",
                baglanti
            );

            DataTable tablo = new DataTable();
            da.Fill(tablo);

            dataGridView1.DataSource = tablo;

            baglanti.Close();

            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;
        }

        private void BtnSatisYap_Click(object sender, EventArgs e)
        {
            if (!dataGridView1.Columns.Contains("Stok"))
            {
                MessageBox.Show("Satış yapmak için önce Stok Listesi veya İlaç Ekleme sayfasından bir ilaç seçiniz.");
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen satış yapılacak ilacı listeden seçiniz.");
                return;
            }

            DataGridViewRow seciliSatir = dataGridView1.SelectedRows[0];

            int ilacID = Convert.ToInt32(seciliSatir.Cells["IlacID"].Value);
            string ilacAdi = seciliSatir.Cells["IlacAdi"].Value.ToString();
            int stok = Convert.ToInt32(seciliSatir.Cells["Stok"].Value);
            decimal fiyat = Convert.ToDecimal(seciliSatir.Cells["Fiyat"].Value);

            Form adetFormu = new Form();
            adetFormu.Text = "Satış Adedi";
            adetFormu.Size = new Size(300, 180);
            adetFormu.StartPosition = FormStartPosition.CenterParent;
            adetFormu.BackColor = Color.WhiteSmoke;

            Label lblAdet = new Label();
            lblAdet.Text = "Satılan adet:";
            lblAdet.Location = new Point(30, 25);
            lblAdet.AutoSize = true;
            adetFormu.Controls.Add(lblAdet);

            TextBox txtAdet = new TextBox();
            txtAdet.Location = new Point(30, 55);
            txtAdet.Size = new Size(220, 25);
            txtAdet.Text = "1";
            adetFormu.Controls.Add(txtAdet);

            Button btnOnay = new Button();
            btnOnay.Text = "Onayla";
            btnOnay.Location = new Point(30, 95);
            btnOnay.Size = new Size(220, 35);
            btnOnay.BackColor = Color.RoyalBlue;
            btnOnay.ForeColor = Color.White;
            btnOnay.DialogResult = DialogResult.OK;
            adetFormu.Controls.Add(btnOnay);

            adetFormu.AcceptButton = btnOnay;

            if (adetFormu.ShowDialog() != DialogResult.OK)
                return;

            if (!int.TryParse(txtAdet.Text, out int adet) || adet <= 0)
            {
                MessageBox.Show("Geçerli bir adet giriniz.");
                return;
            }

            if (adet > stok)
            {
                MessageBox.Show("Yetersiz stok. Mevcut stok: " + stok);
                return;
            }

            decimal toplamFiyat = adet * fiyat;

            baglanti.Open();

            SqlCommand satisKomut = new SqlCommand(
                "INSERT INTO SatisGecmisi (IlacID, IlacAdi, Adet, ToplamFiyat) VALUES (@id,@ad,@adet,@toplam)",
                baglanti
            );

            satisKomut.Parameters.AddWithValue("@id", ilacID);
            satisKomut.Parameters.AddWithValue("@ad", ilacAdi);
            satisKomut.Parameters.AddWithValue("@adet", adet);
            satisKomut.Parameters.AddWithValue("@toplam", toplamFiyat);
            satisKomut.ExecuteNonQuery();

            SqlCommand stokKomut = new SqlCommand(
                "UPDATE Ilaclar SET Stok = Stok - @adet WHERE IlacID=@id",
                baglanti
            );

            stokKomut.Parameters.AddWithValue("@adet", adet);
            stokKomut.Parameters.AddWithValue("@id", ilacID);
            stokKomut.ExecuteNonQuery();

            baglanti.Close();

            MessageBox.Show("Satış tamamlandı. Toplam: " + toplamFiyat + " TL");

            BtnListele_Click(null, null);
        }

        private void BtnKategoriFiltreleme_Click(object sender, EventArgs e)
        {
            aktifSayfa = "Ilac";

            TumSayfalariGizle();

            lblKategoriFiltre.Visible = true;
            cmbKategoriFiltre.Visible = true;

            dataGridView1.Location = new Point(330, 190);
            dataGridView1.Size = new Size(900, 450);
            dataGridView1.Visible = true;

            CmbKategoriFiltre_SelectedIndexChanged(null, null);
        }

        private void CmbKategoriFiltre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKategoriFiltre == null || dataGridView1 == null)
                return;

            baglanti.Open();

            SqlDataAdapter da;

            if (cmbKategoriFiltre.Text == "Tümü")
            {
                da = new SqlDataAdapter("SELECT * FROM Ilaclar", baglanti);
            }
            else
            {
                da = new SqlDataAdapter(
                    "SELECT * FROM Ilaclar WHERE Kategori=@kategori",
                    baglanti
                );

                da.SelectCommand.Parameters.AddWithValue("@kategori", cmbKategoriFiltre.Text);
            }

            DataTable tablo = new DataTable();
            da.Fill(tablo);

            dataGridView1.DataSource = tablo;

            baglanti.Close();

            StokUyarisiRenklendir();
        }

        private void BtnGunlukKazanc_Click(object sender, EventArgs e)
        {
            TumSayfalariGizle();

            baglanti.Open();

            SqlCommand komut = new SqlCommand(
                "SELECT ISNULL(SUM(ToplamFiyat),0) FROM SatisGecmisi WHERE CONVERT(date, SatisTarihi)=CONVERT(date, GETDATE())",
                baglanti
            );

            decimal toplamKazanc = Convert.ToDecimal(komut.ExecuteScalar());

            baglanti.Close();

            pnlGunlukKazanc = new Panel();
            pnlGunlukKazanc.Size = new Size(500, 250);
            pnlGunlukKazanc.Location = new Point(500, 220);
            pnlGunlukKazanc.BackColor = Color.FromArgb(31, 41, 55);

            Label lblBaslik = new Label();
            lblBaslik.Text = "Günlük Kazanç";
            lblBaslik.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblBaslik.ForeColor = Color.White;
            lblBaslik.Location = new Point(120, 40);
            lblBaslik.AutoSize = true;

            Label lblTutar = new Label();
            lblTutar.Text = toplamKazanc.ToString("0.00") + " TL";
            lblTutar.Font = new Font("Segoe UI", 34, FontStyle.Bold);
            lblTutar.ForeColor = Color.LimeGreen;
            lblTutar.Location = new Point(95, 110);
            lblTutar.AutoSize = true;

            pnlGunlukKazanc.Controls.Add(lblBaslik);
            pnlGunlukKazanc.Controls.Add(lblTutar);

            this.Controls.Add(pnlGunlukKazanc);
        }

        private void AnaSayfayiGoster()
        {
            TumSayfalariGizle();

            if (lblHosgeldiniz == null)
            {
                lblHosgeldiniz = new Label();
                lblHosgeldiniz.Text = "Eczane Takip Sistemine\nHoş Geldiniz";
                lblHosgeldiniz.Font = new Font("Segoe UI", 30, FontStyle.Bold);
                lblHosgeldiniz.ForeColor = Color.White;
                lblHosgeldiniz.Location = new Point(250, 230);
                lblHosgeldiniz.Size = new Size(this.ClientSize.Width - 250, 120);
                lblHosgeldiniz.TextAlign = ContentAlignment.MiddleCenter;
                this.Controls.Add(lblHosgeldiniz);

                lblHosgeldinizAlt = new Label();
                lblHosgeldinizAlt.Text = "Sol menüden yapmak istediğiniz işlemi seçebilirsiniz.";
                lblHosgeldinizAlt.Font = new Font("Segoe UI", 12);
                lblHosgeldinizAlt.ForeColor = Color.LightGray;
                lblHosgeldinizAlt.Location = new Point(250, 360);
                lblHosgeldinizAlt.Size = new Size(this.ClientSize.Width - 250, 40);
                lblHosgeldinizAlt.TextAlign = ContentAlignment.MiddleCenter;
                this.Controls.Add(lblHosgeldinizAlt);
            }

            lblHosgeldiniz.Visible = true;
            lblHosgeldinizAlt.Visible = true;
        }

        private void TxtAra_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1 == null)
                return;

            baglanti.Open();

            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM Ilaclar WHERE IlacAdi LIKE @arama OR BarkodNo LIKE @arama OR Kategori LIKE @arama",
                baglanti
            );

            da.SelectCommand.Parameters.AddWithValue("@arama", "%" + txtAra.Text + "%");

            DataTable tablo = new DataTable();
            da.Fill(tablo);

            dataGridView1.DataSource = tablo;

            baglanti.Close();

            StokUyarisiRenklendir();
        }

        private void TumSayfalariGizle()
        {
            if (formPanel != null)
                formPanel.Visible = false;

            if (dataGridView1 != null)
                dataGridView1.Visible = false;

            if (txtAra != null)
                txtAra.Visible = false;

            if (lblAra != null)
                lblAra.Visible = false;

            if (cmbKategoriFiltre != null)
                cmbKategoriFiltre.Visible = false;

            if (lblKategoriFiltre != null)
                lblKategoriFiltre.Visible = false;

            if (lblHosgeldiniz != null)
                lblHosgeldiniz.Visible = false;

            if (lblHosgeldinizAlt != null)
                lblHosgeldinizAlt.Visible = false;

            if (pnlGunlukKazanc != null)
                pnlGunlukKazanc.Visible = false;
        }
        private void BtnCikis_Click(object sender, EventArgs e)
        {
            DialogResult cevap = MessageBox.Show(
                "Hesaptan çıkış yapmak istiyor musunuz?",
                "Çıkış Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (cevap == DialogResult.Yes)
            {
                FrmGiris giris = new FrmGiris();
                giris.Show();
                this.Close();
            }
        }
    }
}