-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Waktu pembuatan: 17 Bulan Mei 2024 pada 14.19
-- Versi server: 10.4.32-MariaDB
-- Versi PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `kelompok6`
--

-- --------------------------------------------------------

--
-- Struktur dari tabel `tbbentuk`
--

CREATE TABLE `tbbentuk` (
  `id_bentuk` int(11) NOT NULL,
  `nama_bentuk` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data untuk tabel `tbbentuk`
--

INSERT INTO `tbbentuk` (`id_bentuk`, `nama_bentuk`) VALUES
(1, 'Coin'),
(2, 'Bar'),
(3, 'Shell'),
(4, 'Love'),
(5, 'Truffle'),
(6, 'Figure'),
(7, 'Love'),
(8, 'Shell');

-- --------------------------------------------------------

--
-- Struktur dari tabel `tbkeranjang`
--

CREATE TABLE `tbkeranjang` (
  `id_keranjang` int(11) NOT NULL,
  `id_pemesan` int(11) NOT NULL,
  `bentuk` varchar(30) NOT NULL,
  `varian` varchar(30) NOT NULL,
  `ukuran` varchar(10) NOT NULL,
  `jumlah` int(11) NOT NULL,
  `totalHarga` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data untuk tabel `tbkeranjang`
--

INSERT INTO `tbkeranjang` (`id_keranjang`, `id_pemesan`, `bentuk`, `varian`, `ukuran`, `jumlah`, `totalHarga`) VALUES
(1, 1, 'Shell', 'White Hazelnut', 'Medium', 6, 420000),
(2, 1, 'Shell', 'White Hazelnut', 'Medium', 6, 420000),
(3, 1, 'Shell', 'Almond', 'Large', 11, 1155000),
(4, 1, 'Shell', 'Almond', 'Large', 11, 1155000),
(5, 4, 'Shell', 'Almond', 'Small', 23, 805000),
(7, 5, 'Bar', 'Dark Chocolate', 'Large', 12, 1080000),
(8, 5, 'Bar', 'Dark Chocolate', 'Small', 1, 30000),
(9, 5, 'Bar', 'Dark Chocolate', 'Small', 1, 30000),
(10, 4, 'Love', 'White Chocolate', 'Medium', 12, 720000),
(11, 4, 'Coin', 'Original', 'Medium', 12, 480000),
(12, 4, 'Coin', 'Original', 'Medium', 12, 480000),
(14, 4, 'Coin', 'Original', 'Medium', 12, 480000);

-- --------------------------------------------------------

--
-- Struktur dari tabel `tblogin`
--

CREATE TABLE `tblogin` (
  `idlogin` int(11) NOT NULL,
  `nama` varchar(30) NOT NULL,
  `alamat` text NOT NULL,
  `alamatemail` text NOT NULL,
  `katasandi` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data untuk tabel `tblogin`
--

INSERT INTO `tblogin` (`idlogin`, `nama`, `alamat`, `alamatemail`, `katasandi`) VALUES
(4, 'dinda', '2209106095', 'aa@a.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3'),
(5, 'vista', '12', 'v@a.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3');

-- --------------------------------------------------------

--
-- Struktur dari tabel `tbpesanan`
--

CREATE TABLE `tbpesanan` (
  `id_pesanan` int(11) NOT NULL,
  `id_pemesan` int(11) NOT NULL,
  `nama_pemesan` varchar(30) NOT NULL,
  `alamat_pemesan` text NOT NULL,
  `tgl_pesanan` date NOT NULL,
  `total_harga` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data untuk tabel `tbpesanan`
--

INSERT INTO `tbpesanan` (`id_pesanan`, `id_pemesan`, `nama_pemesan`, `alamat_pemesan`, `tgl_pesanan`, `total_harga`) VALUES
(1, 1, 'Dinda Ayu Aprilia', '2209106095', '2024-05-15', 3150000),
(2, 0, 'dinda', '2209106095', '2024-05-17', 1525000),
(3, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(4, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(5, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(6, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(7, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(8, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(9, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(10, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(11, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(12, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(13, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(14, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(15, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(16, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(17, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(18, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(19, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(20, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(21, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(22, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(23, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(24, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(25, 4, 'dinda', '2209106095', '2024-05-17', 1525000),
(26, 4, 'dinda', '2209106095', '2024-05-17', 3445000),
(27, 4, 'dinda', '2209106095', '2024-05-17', 2965000);

-- --------------------------------------------------------

--
-- Struktur dari tabel `tbvarian`
--

CREATE TABLE `tbvarian` (
  `id_varian` int(11) NOT NULL,
  `nama_varian` varchar(30) NOT NULL,
  `harga` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data untuk tabel `tbvarian`
--

INSERT INTO `tbvarian` (`id_varian`, `nama_varian`, `harga`) VALUES
(1, 'Original', 20000),
(2, 'Dark Chocolate', 30000),
(3, 'White Chocolate', 30000),
(4, 'White Hazelnut', 35000),
(5, 'Almond', 35000),
(6, 'Caramel', 35000),
(7, 'Matcha', 40000),
(8, 'Berries', 40000),
(9, 'Mint', 40000),
(10, 'Tropical Fruit', 50000),
(11, 'White Chocolate', 30000),
(12, 'Dark Chocolate', 30000);

--
-- Indexes for dumped tables
--

--
-- Indeks untuk tabel `tbbentuk`
--
ALTER TABLE `tbbentuk`
  ADD PRIMARY KEY (`id_bentuk`);

--
-- Indeks untuk tabel `tbkeranjang`
--
ALTER TABLE `tbkeranjang`
  ADD PRIMARY KEY (`id_keranjang`);

--
-- Indeks untuk tabel `tblogin`
--
ALTER TABLE `tblogin`
  ADD PRIMARY KEY (`idlogin`);

--
-- Indeks untuk tabel `tbpesanan`
--
ALTER TABLE `tbpesanan`
  ADD PRIMARY KEY (`id_pesanan`);

--
-- Indeks untuk tabel `tbvarian`
--
ALTER TABLE `tbvarian`
  ADD PRIMARY KEY (`id_varian`);

--
-- AUTO_INCREMENT untuk tabel yang dibuang
--

--
-- AUTO_INCREMENT untuk tabel `tbbentuk`
--
ALTER TABLE `tbbentuk`
  MODIFY `id_bentuk` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT untuk tabel `tbkeranjang`
--
ALTER TABLE `tbkeranjang`
  MODIFY `id_keranjang` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT untuk tabel `tblogin`
--
ALTER TABLE `tblogin`
  MODIFY `idlogin` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT untuk tabel `tbpesanan`
--
ALTER TABLE `tbpesanan`
  MODIFY `id_pesanan` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=28;

--
-- AUTO_INCREMENT untuk tabel `tbvarian`
--
ALTER TABLE `tbvarian`
  MODIFY `id_varian` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
