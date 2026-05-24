CREATE DATABASE EczaneTakipDB;
GO

USE EczaneTakipDB;
GO

CREATE TABLE Kullanicilar (
    KullaniciID INT PRIMARY KEY IDENTITY(1,1),
    KullaniciAdi NVARCHAR(50) NOT NULL,
    Sifre NVARCHAR(50) NOT NULL,
    Rol NVARCHAR(20) NOT NULL,
    SonGirisTarihi DATETIME NULL
);

CREATE TABLE Ilaclar (
    IlacID INT PRIMARY KEY IDENTITY(1,1),
    IlacAdi NVARCHAR(100) NOT NULL,
    BarkodNo NVARCHAR(50),
    Kategori NVARCHAR(50),
    Stok INT NOT NULL,
    Fiyat DECIMAL(10,2) NOT NULL,
    SonKullanmaTarihi DATE
);

CREATE TABLE SatisGecmisi (
    SatisID INT PRIMARY KEY IDENTITY(1,1),
    IlacID INT,
    IlacAdi NVARCHAR(100),
    Adet INT,
    ToplamFiyat DECIMAL(10,2),
    SatisTarihi DATETIME DEFAULT GETDATE()
);

INSERT INTO Kullanicilar (KullaniciAdi, Sifre, Rol)
VALUES
('admin', '1234', 'Admin'),
('eczaci', '1234', N'Eczacı'),
('calisan', '1234', N'Çalışan');