using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemPerpustakaan
{
    public interface IPeminjaman
    {
        void PinjamBuku(Pengguna pengguna, Buku buku);
        void KembalikanBuku(Pengguna pengguna, Buku buku);
        List<Buku> DaftarBukuDipinjam(Pengguna pengguna);
    }

    public abstract class Buku
    {
        protected string id;
        protected string judul;
        protected string penulis;
        protected int tahunTerbit;
        protected bool tersedia;

        public string ID { get => id; set => id = value; }
        public string Judul { get => judul; set => judul = value; }
        public string Penulis { get => penulis; set => penulis = value; }
        public int TahunTerbit { get => tahunTerbit; set => tahunTerbit = value; }
        public bool Tersedia { get => tersedia; set => tersedia = value; }

        public Buku(string id, string judul, string penulis, int tahunTerbit)
        {
            this.id = id;
            this.judul = judul;
            this.penulis = penulis;
            this.tahunTerbit = tahunTerbit;
            this.tersedia = true;
        }

        public abstract string DapatkanInfo();
    }

    public class BukuFiksi : Buku
    {
        private string genre;
        public string Genre { get => genre; set => genre = value; }

        public BukuFiksi(string id, string judul, string penulis, int tahunTerbit, string genre)
            : base(id, judul, penulis, tahunTerbit)
        {
            this.genre = genre;
        }

        public override string DapatkanInfo()
        {
            return $"Buku Fiksi - ID: {ID}, Judul: {Judul}, Penulis: {Penulis}, Tahun: {TahunTerbit}, Genre: {Genre}, Status: {(Tersedia ? "Tersedia" : "Dipinjam")}";
        }
    }

    public class BukuNonFiksi : Buku
    {
        private string subjek;
        public string Subjek { get => subjek; set => subjek = value; }

        public BukuNonFiksi(string id, string judul, string penulis, int tahunTerbit, string subjek)
            : base(id, judul, penulis, tahunTerbit)
        {
            this.subjek = subjek;
        }

        public override string DapatkanInfo()
        {
            return $"Buku Non-Fiksi - ID: {ID}, Judul: {Judul}, Penulis: {Penulis}, Tahun: {TahunTerbit}, Subjek: {Subjek}, Status: {(Tersedia ? "Tersedia" : "Dipinjam")}";
        }
    }

    public class Majalah : Buku
    {
        private string edisi;
        public string Edisi { get => edisi; set => edisi = value; }

        public Majalah(string id, string judul, string penulis, int tahunTerbit, string edisi)
            : base(id, judul, penulis, tahunTerbit)
        {
            this.edisi = edisi;
        }

        public override string DapatkanInfo()
        {
            return $"Majalah - ID: {ID}, Judul: {Judul}, Penulis: {Penulis}, Tahun: {TahunTerbit}, Edisi: {Edisi}, Status: {(Tersedia ? "Tersedia" : "Dipinjam")}";
        }
    }

    public class Pengguna
    {
        private string id;
        private string nama;
        private List<Buku> bukuDipinjam;

        public string ID { get => id; }
        public string Nama { get => nama; }
        public List<Buku> BukuDipinjam { get => bukuDipinjam; }

        public Pengguna(string id, string nama)
        {
            this.id = id;
            this.nama = nama;
            this.bukuDipinjam = new List<Buku>();
        }

        public string DapatkanInfo()
        {
            return $"Pengguna - ID: {ID}, Nama: {Nama}, Jumlah Buku Dipinjam: {BukuDipinjam.Count}";
        }
    }

    public class Perpustakaan : IPeminjaman
    {
        private List<Buku> daftarBuku;
        private List<Pengguna> daftarPengguna;

        public Perpustakaan()
        {
            daftarBuku = new List<Buku>();
            daftarPengguna = new List<Pengguna>();
        }

        public void TambahBuku(Buku buku)
        {
            daftarBuku.Add(buku);
        }

        public void UbahBuku(string id, Buku bukuBaru)
        {
            Buku bukuLama = daftarBuku.FirstOrDefault(b => b.ID == id);
            if (bukuLama != null)
            {
                daftarBuku.Remove(bukuLama);
                daftarBuku.Add(bukuBaru);
            }
        }

        public List<Buku> DapatkanSemuaBuku()
        {
            return daftarBuku;
        }

        public Buku DapatkanBuku(string id)
        {
            return daftarBuku.FirstOrDefault(b => b.ID == id);
        }

        public void TambahPengguna(Pengguna pengguna)
        {
            daftarPengguna.Add(pengguna);
        }

        public Pengguna DapatkanPengguna(string id)
        {
            return daftarPengguna.FirstOrDefault(p => p.ID == id);
        }

        public void PinjamBuku(Pengguna pengguna, Buku buku)
        {
            if (pengguna.BukuDipinjam.Count >= 3)
                throw new Exception("Pengguna sudah meminjam maksimal 3 buku.");

            if (!buku.Tersedia)
                throw new Exception("Buku tidak tersedia untuk dipinjam.");

            buku.Tersedia = false;
            pengguna.BukuDipinjam.Add(buku);
        }

        public void KembalikanBuku(Pengguna pengguna, Buku buku)
        {
            if (!pengguna.BukuDipinjam.Contains(buku))
                throw new Exception("Buku ini tidak dipinjam oleh pengguna ini.");

            buku.Tersedia = true;
            pengguna.BukuDipinjam.Remove(buku);
        }

        public List<Buku> DaftarBukuDipinjam(Pengguna pengguna)
        {
            return pengguna.BukuDipinjam;
        }
    }

    class Program
    {
        static Perpustakaan perpustakaan = new Perpustakaan();
        static Pengguna penggunaSaatIni = null;

        static void Main(string[] args)
        {
            InisialisasiData();
            bool lanjut = true;

            while (lanjut)
            {
                TampilkanMenu();
                string pilihan = Console.ReadLine();

                try
                {
                    switch (pilihan)
                    {
                        case "1": PilihPengguna(); break;
                        case "2": TambahBuku(); break;
                        case "3": UbahBuku(); break;
                        case "4": LihatDaftarBuku(); break;
                        case "5": PinjamBuku(); break;
                        case "6": KembalikanBuku(); break;
                        case "7": LihatBukuDipinjam(); break;
                        case "0": lanjut = false; break;
                        default: Console.WriteLine("Pilihan tidak valid."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                Console.WriteLine("\nTekan Enter untuk melanjutkan...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        static void InisialisasiData()
        {
            perpustakaan.TambahBuku(new BukuFiksi("F001", "Harry Potter", "J.K. Rowling", 1997, "Fantasi"));
            perpustakaan.TambahBuku(new BukuNonFiksi("NF001", "Sejarah Indonesia", "Moh. Hatta", 1980, "Sejarah"));
            perpustakaan.TambahBuku(new Majalah("M001", "National Geographic", "Various", 2023, "Januari 2023"));

            perpustakaan.TambahPengguna(new Pengguna("P001", "Budi Santoso"));
            perpustakaan.TambahPengguna(new Pengguna("P002", "Siti Rahma"));

            penggunaSaatIni = perpustakaan.DapatkanPengguna("P001");
        }

        static void TampilkanMenu()
        {
            Console.WriteLine("======== SISTEM MANAJEMEN PERPUSTAKAAN ========");
            if (penggunaSaatIni != null)
                Console.WriteLine($"Pengguna: {penggunaSaatIni.Nama} ({penggunaSaatIni.ID})");

            Console.WriteLine("1. Pilih Pengguna");
            Console.WriteLine("2. Tambah Buku");
            Console.WriteLine("3. Ubah Data Buku");
            Console.WriteLine("4. Lihat Daftar Buku");
            Console.WriteLine("5. Pinjam Buku");
            Console.WriteLine("6. Kembalikan Buku");
            Console.WriteLine("7. Lihat Buku yang Dipinjam");
            Console.WriteLine("0. Keluar");
            Console.Write("Pilihan Anda: ");
        }

        static void PilihPengguna()
        {
            Console.WriteLine("\n--- Daftar Pengguna ---");
            Console.WriteLine("P001 - Budi Santoso");
            Console.WriteLine("P002 - Siti Rahma");
            Console.Write("Masukkan ID Pengguna: ");
            string idPengguna = Console.ReadLine();

            Pengguna pengguna = perpustakaan.DapatkanPengguna(idPengguna);
            if (pengguna != null)
            {
                penggunaSaatIni = pengguna;
                Console.WriteLine($"Pengguna {penggunaSaatIni.Nama} dipilih.");
            }
            else
                Console.WriteLine("ID Pengguna tidak ditemukan.");
        }

        static void TambahBuku()
        {
            Console.WriteLine("\n--- Tambah Buku ---");
            Console.WriteLine("Pilih jenis buku:");
            Console.WriteLine("1. Buku Fiksi");
            Console.WriteLine("2. Buku Non-Fiksi");
            Console.WriteLine("3. Majalah");
            Console.Write("Pilihan: ");
            string jenisBuku = Console.ReadLine();

            Console.Write("ID: ");
            string id = Console.ReadLine();
            Console.Write("Judul: ");
            string judul = Console.ReadLine();
            Console.Write("Penulis: ");
            string penulis = Console.ReadLine();
            Console.Write("Tahun Terbit: ");
            int tahunTerbit = int.Parse(Console.ReadLine());

            Buku bukuBaru = null;

            switch (jenisBuku)
            {
                case "1":
                    Console.Write("Genre: ");
                    bukuBaru = new BukuFiksi(id, judul, penulis, tahunTerbit, Console.ReadLine());
                    break;
                case "2":
                    Console.Write("Subjek: ");
                    bukuBaru = new BukuNonFiksi(id, judul, penulis, tahunTerbit, Console.ReadLine());
                    break;
                case "3":
                    Console.Write("Edisi: ");
                    bukuBaru = new Majalah(id, judul, penulis, tahunTerbit, Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Jenis buku tidak valid.");
                    return;
            }

            perpustakaan.TambahBuku(bukuBaru);
            Console.WriteLine("Buku berhasil ditambahkan.");
        }

        static void UbahBuku()
        {
            Console.WriteLine("\n--- Ubah Data Buku ---");
            Console.Write("Masukkan ID buku yang akan diubah: ");
            string idBuku = Console.ReadLine();

            Buku bukuLama = perpustakaan.DapatkanBuku(idBuku);
            if (bukuLama == null)
            {
                Console.WriteLine("Buku tidak ditemukan.");
                return;
            }

            Console.Write("Judul baru: ");
            string judul = Console.ReadLine();
            Console.Write("Penulis baru: ");
            string penulis = Console.ReadLine();
            Console.Write("Tahun Terbit baru: ");
            int tahunTerbit = int.Parse(Console.ReadLine());

            Buku bukuBaru = null;

            if (bukuLama is BukuFiksi)
            {
                Console.Write("Genre baru: ");
                bukuBaru = new BukuFiksi(idBuku, judul, penulis, tahunTerbit, Console.ReadLine());
            }
            else if (bukuLama is BukuNonFiksi)
            {
                Console.Write("Subjek baru: ");
                bukuBaru = new BukuNonFiksi(idBuku, judul, penulis, tahunTerbit, Console.ReadLine());
            }
            else if (bukuLama is Majalah)
            {
                Console.Write("Edisi baru: ");
                bukuBaru = new Majalah(idBuku, judul, penulis, tahunTerbit, Console.ReadLine());
            }

            perpustakaan.UbahBuku(idBuku, bukuBaru);
            Console.WriteLine("Data buku berhasil diubah.");
        }

        static void LihatDaftarBuku()
        {
            Console.WriteLine("\n--- Daftar Buku ---");
            List<Buku> daftarBuku = perpustakaan.DapatkanSemuaBuku();

            if (daftarBuku.Count == 0)
            {
                Console.WriteLine("Tidak ada buku dalam daftar.");
                return;
            }

            foreach (Buku buku in daftarBuku)
                Console.WriteLine(buku.DapatkanInfo());
        }

        static void PinjamBuku()
        {
            if (penggunaSaatIni == null)
            {
                Console.WriteLine("Silakan pilih pengguna terlebih dahulu.");
                return;
            }

            Console.WriteLine("\n--- Pinjam Buku ---");
            List<Buku> bukuTersedia = perpustakaan.DapatkanSemuaBuku().Where(b => b.Tersedia).ToList();

            if (bukuTersedia.Count == 0)
            {
                Console.WriteLine("Tidak ada buku yang tersedia untuk dipinjam.");
                return;
            }

            foreach (Buku buku in bukuTersedia)
                Console.WriteLine(buku.DapatkanInfo());

            Console.Write("Masukkan ID buku yang ingin dipinjam: ");
            string idBuku = Console.ReadLine();

            Buku bukuPinjam = perpustakaan.DapatkanBuku(idBuku);
            if (bukuPinjam == null || !bukuPinjam.Tersedia)
            {
                Console.WriteLine("Buku tidak tersedia atau tidak ditemukan.");
                return;
            }

            perpustakaan.PinjamBuku(penggunaSaatIni, bukuPinjam);
            Console.WriteLine($"Buku '{bukuPinjam.Judul}' berhasil dipinjam.");
        }

        static void KembalikanBuku()
        {
            if (penggunaSaatIni == null)
            {
                Console.WriteLine("Silakan pilih pengguna terlebih dahulu.");
                return;
            }

            Console.WriteLine("\n--- Kembalikan Buku ---");
            List<Buku> bukuDipinjam = perpustakaan.DaftarBukuDipinjam(penggunaSaatIni);

            if (bukuDipinjam.Count == 0)
            {
                Console.WriteLine("Anda tidak memiliki buku yang dipinjam.");
                return;
            }

            for (int i = 0; i < bukuDipinjam.Count; i++)
                Console.WriteLine($"{i + 1}. {bukuDipinjam[i].DapatkanInfo()}");

            Console.Write("Pilih nomor buku yang ingin dikembalikan: ");
            int nomorBuku = int.Parse(Console.ReadLine());

            if (nomorBuku < 1 || nomorBuku > bukuDipinjam.Count)
            {
                Console.WriteLine("Nomor buku tidak valid.");
                return;
            }

            Buku bukuKembali = bukuDipinjam[nomorBuku - 1];
            perpustakaan.KembalikanBuku(penggunaSaatIni, bukuKembali);
            Console.WriteLine($"Buku '{bukuKembali.Judul}' berhasil dikembalikan.");
        }

        static void LihatBukuDipinjam()
        {
            if (penggunaSaatIni == null)
            {
                Console.WriteLine("Silakan pilih pengguna terlebih dahulu.");
                return;                                                                                                                                                    
            }

            Console.WriteLine("\n--- Buku yang Sedang Dipinjam ---");
            List<Buku> bukuDipinjam = perpustakaan.DaftarBukuDipinjam(penggunaSaatIni);

            if (bukuDipinjam.Count == 0)
                Console.WriteLine("Anda tidak memiliki buku yang dipinjam.");
            else
                foreach (Buku buku in bukuDipinjam)
                    Console.WriteLine(buku.DapatkanInfo());
        }
    }
}