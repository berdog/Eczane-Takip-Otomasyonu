using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace EczaneTakipSistemi
{
    public partial class FrmAdminPanel : Form
    {
        SqlConnection baglanti = new SqlConnection(
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog=EczaneTakipDB;Integrated Security=True"
        );

        DataGridView dataGridView1;
        ComboBox cmbRol;
        Button btnRolGuncelle;

        public FrmAdminPanel()
        {
            InitializeComponent();
            PanelHazirla();
            KullanicilariListele();
        }

        private void PanelHazirla()
        {
            this.Text = "Admin Paneli";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 30, 48);

            Label baslik = new Label();
            baslik.Text = "👑 Admin Yönetim Paneli";
            baslik.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            baslik.ForeColor = Color.White;
            baslik.Location = new Point(30, 30);
            baslik.AutoSize = true;
            this.Controls.Add(baslik);

            Button btnYenile = new Button();
            btnYenile.Text = "Kullanıcıları Yenile";
            btnYenile.Location = new Point(35, 100);
            btnYenile.Size = new Size(220, 40);
            btnYenile.BackColor = Color.RoyalBlue;
            btnYenile.ForeColor = Color.White;
            btnYenile.FlatStyle = FlatStyle.Flat;
            btnYenile.Click += BtnYenile_Click;
            this.Controls.Add(btnYenile);

            Button btnSil = new Button();
            btnSil.Text = "Kullanıcı Sil";
            btnSil.Location = new Point(270, 100);
            btnSil.Size = new Size(220, 40);
            btnSil.BackColor = Color.Firebrick;
            btnSil.ForeColor = Color.White;
            btnSil.FlatStyle = FlatStyle.Flat;
            btnSil.Click += BtnSil_Click;
            this.Controls.Add(btnSil);

            cmbRol = new ComboBox();
            cmbRol.Location = new Point(505, 100);
            cmbRol.Size = new Size(180, 40);
            cmbRol.Font = new Font("Segoe UI", 10);
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.Items.Add("Admin");
            cmbRol.Items.Add("Eczacı");
            cmbRol.Items.Add("Çalışan");
            cmbRol.SelectedIndex = 1;
            this.Controls.Add(cmbRol);

            btnRolGuncelle = new Button();
            btnRolGuncelle.Text = "Rol Güncelle";
            btnRolGuncelle.Location = new Point(700, 100);
            btnRolGuncelle.Size = new Size(220, 40);
            btnRolGuncelle.BackColor = Color.DarkOrange;
            btnRolGuncelle.ForeColor = Color.White;
            btnRolGuncelle.FlatStyle = FlatStyle.Flat;
            btnRolGuncelle.Click += BtnRolGuncelle_Click;
            this.Controls.Add(btnRolGuncelle);

            dataGridView1 = new DataGridView();
            dataGridView1.Location = new Point(35, 170);
            dataGridView1.Size = new Size(1000, 400);
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.RowHeadersVisible = false;
            this.Controls.Add(dataGridView1);
        }

        private void KullanicilariListele()
        {
            baglanti.Open();

            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT KullaniciID, KullaniciAdi, Sifre, Rol, SonGirisTarihi FROM Kullanicilar",
                baglanti
            );

            DataTable tablo = new DataTable();
            da.Fill(tablo);

            dataGridView1.DataSource = tablo;

            baglanti.Close();
        }

        private void BtnYenile_Click(object sender, EventArgs e)
        {
            KullanicilariListele();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen kullanıcı seçiniz.");
                return;
            }

            int kullaniciID = Convert.ToInt32(
                dataGridView1.SelectedRows[0].Cells["KullaniciID"].Value
            );

            DialogResult cevap = MessageBox.Show(
                "Kullanıcı silinsin mi?",
                "Onay",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (cevap == DialogResult.Yes)
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand(
                    "DELETE FROM Kullanicilar WHERE KullaniciID=@id",
                    baglanti
                );

                komut.Parameters.AddWithValue("@id", kullaniciID);
                komut.ExecuteNonQuery();

                baglanti.Close();

                MessageBox.Show("Kullanıcı silindi.");

                KullanicilariListele();
            }
        }
        private void BtnRolGuncelle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen rolü güncellenecek kullanıcıyı seçiniz.");
                return;
            }

            int kullaniciID = Convert.ToInt32(
                dataGridView1.SelectedRows[0].Cells["KullaniciID"].Value
            );

            string yeniRol = cmbRol.Text;

            DialogResult cevap = MessageBox.Show(
                "Seçili kullanıcının rolü " + yeniRol + " olarak güncellensin mi?",
                "Rol Güncelleme",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (cevap == DialogResult.Yes)
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand(
                    "UPDATE Kullanicilar SET Rol=@rol WHERE KullaniciID=@id",
                    baglanti
                );

                komut.Parameters.AddWithValue("@rol", yeniRol);
                komut.Parameters.AddWithValue("@id", kullaniciID);

                komut.ExecuteNonQuery();

                baglanti.Close();

                MessageBox.Show("Kullanıcı rolü güncellendi.");

                KullanicilariListele();
            }
        }
    }
}
