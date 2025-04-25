using System;
using System.Collections.Generic;

public abstract class KoleksiBuku
{
    public string Nama { get; set; }
    public string Pengarang { get; set; }
    public int TahunTerbit { get; set; }

    public KoleksiBuku(string nama, string pengarang, int tahun)
    {
        Nama = nama;
        Pengarang = pengarang;
        TahunTerbit = tahun;
    }

    public abstract void InfoBuku();
}

public class BukuFiksi : KoleksiBuku
{
    public BukuFiksi(string nama, string pengarang, int tahun) : base(nama, pengarang, tahun) { }

    public override void InfoBuku()
    {
        Console.WriteLine($"[Fiksi] {Nama} oleh {Pengarang} ({TahunTerbit})");
    }
}

public class BukuNonFiksi : KoleksiBuku
{
    public BukuNonFiksi(string nama, string pengarang, int tahun) : base(nama, pengarang, tahun) { }

    public override void InfoBuku()
    {
        Console.WriteLine($"[Non-Fiksi] {Nama} oleh {Pengarang} ({TahunTerbit})");
    }
}

public class EdisiMajalah : KoleksiBuku
{
    public EdisiMajalah(string nama, string pengarang, int tahun) : base(nama, pengarang, tahun) { }

    public override void InfoBuku()
    {
        Console.WriteLine($"[Majalah] {Nama} oleh {Pengarang} ({TahunTerbit})");
    }
}

public interface IPeminjam
{
    void Ambil(KoleksiBuku item);
    void Kembalikan(int posisi);
    void TampilkanPinjaman();
    List<KoleksiBuku> GetPinjaman();
}

public class AnggotaPerpus : IPeminjam
{
    private List<KoleksiBuku> koleksiDipinjam = new List<KoleksiBuku>();
    private const int LimitPinjam = 3;

    public void Ambil(KoleksiBuku item)
    {
        if (koleksiDipinjam.Count >= LimitPinjam)
        {
            Console.WriteLine("Batas maksimum peminjaman telah tercapai.");
            return;
        }

        koleksiDipinjam.Add(item);
        Console.WriteLine($"\"{item.Nama}\" telah berhasil dipinjam.");
    }

    public void Kembalikan(int posisi)
    {
        if (posisi >= 0 && posisi < koleksiDipinjam.Count)
        {
            var buku = koleksiDipinjam[posisi];
            koleksiDipinjam.RemoveAt(posisi);
            Console.WriteLine($"\"{buku.Nama}\" telah dikembalikan.");
        }
        else
        {
            Console.WriteLine("Posisi tidak valid.");
        }
    }

    public void TampilkanPinjaman()
    {
        Console.WriteLine("\nBuku yang Sedang Dipinjam:");
        if (koleksiDipinjam.Count == 0)
        {
            Console.WriteLine("Belum ada buku dipinjam.");
            return;
        }

        for (int i = 0; i < koleksiDipinjam.Count; i++)
        {
            Console.Write($"{i}. ");
            koleksiDipinjam[i].InfoBuku();
        }
    }

    public List<KoleksiBuku> GetPinjaman() => koleksiDipinjam;
}

class AplikasiPerpustakaan
{
    static List<KoleksiBuku> dataPerpus = new List<KoleksiBuku>();
    static AnggotaPerpus user = new AnggotaPerpus();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== Aplikasi Mini Perpustakaan ===");
            Console.WriteLine("Masuk sebagai:");
            Console.WriteLine("1. Pengelola");
            Console.WriteLine("2. Pengunjung");
            Console.WriteLine("3. Keluar Aplikasi");
            Console.Write("Pilihan Anda: ");
            string pilihan = Console.ReadLine();

            switch (pilihan)
            {
                case "1": MenuAdmin(); break;
                case "2": MenuPengunjung(); break;
                case "3": return;
                default: Console.WriteLine("Input tidak dikenali."); break;
            }
        }
    }

    static void MenuAdmin()
    {
        while (true)
        {
            Console.WriteLine("\n--- Menu Pengelola ---");
            Console.WriteLine("1. Tambah Koleksi");
            Console.WriteLine("2. Edit Koleksi");
            Console.WriteLine("3. Lihat Semua Koleksi");
            Console.WriteLine("4. Kembali ke Utama");
            Console.Write("Pilih: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1": TambahKoleksi(); break;
                case "2": EditKoleksi(); break;
                case "3": TampilkanSemua(); break;
                case "4": return;
                default: Console.WriteLine("Pilihan tidak tersedia."); break;
            }
        }
    }

    static void MenuPengunjung()
    {
        while (true)
        {
            Console.WriteLine("\n--- Menu Pengunjung ---");
            Console.WriteLine("1. Lihat Koleksi");
            Console.WriteLine("2. Pinjam Koleksi");
            Console.WriteLine("3. Kembalikan Koleksi");
            Console.WriteLine("4. Daftar Pinjaman Saya");
            Console.WriteLine("5. Kembali ke Menu Awal");
            Console.Write("Pilih: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1": TampilkanSemua(); break;
                case "2": ProsesPinjam(); break;
                case "3": ProsesKembali(); break;
                case "4": user.TampilkanPinjaman(); break;
                case "5": return;
                default: Console.WriteLine("Pilihan tidak sah."); break;
            }
        }
    }

    static void TambahKoleksi()
    {
        Console.Write("Judul Koleksi: ");
        string nama = Console.ReadLine();
        Console.Write("Pengarang: ");
        string pengarang = Console.ReadLine();
        Console.Write("Tahun Terbit: ");
        int tahun = int.Parse(Console.ReadLine());

        Console.WriteLine("Pilih Jenis: (1. Fiksi, 2. Non-Fiksi, 3. Majalah): ");
        string tipe = Console.ReadLine();

        KoleksiBuku koleksi = tipe switch
        {
            "1" => new BukuFiksi(nama, pengarang, tahun),
            "2" => new BukuNonFiksi(nama, pengarang, tahun),
            "3" => new EdisiMajalah(nama, pengarang, tahun),
            _ => null
        };

        if (koleksi != null)
        {
            dataPerpus.Add(koleksi);
            Console.WriteLine("Koleksi berhasil ditambahkan!");
        }
        else
        {
            Console.WriteLine("Tipe tidak dikenali.");
        }
    }

    static void EditKoleksi()
    {
        TampilkanSemua();
        Console.Write("Masukkan indeks koleksi yang ingin diperbarui: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 0 && idx < dataPerpus.Count)
        {
            var koleksi = dataPerpus[idx];
            Console.Write("Judul baru: ");
            koleksi.Nama = Console.ReadLine();
            Console.Write("Pengarang baru: ");
            koleksi.Pengarang = Console.ReadLine();
            Console.Write("Tahun baru: ");
            koleksi.TahunTerbit = int.Parse(Console.ReadLine());
            Console.WriteLine("Data berhasil diperbarui.");
        }
        else
        {
            Console.WriteLine("Indeks tidak valid.");
        }
    }

    static void TampilkanSemua()
    {
        Console.WriteLine("\nSeluruh Koleksi Perpustakaan:");
        if (dataPerpus.Count == 0)
        {
            Console.WriteLine("Perpustakaan masih kosong.");
            return;
        }

        for (int i = 0; i < dataPerpus.Count; i++)
        {
            Console.Write($"{i}. ");
            dataPerpus[i].InfoBuku();
        }
    }

    static void ProsesPinjam()
    {
        TampilkanSemua();
        Console.Write("Masukkan indeks koleksi yang ingin dipinjam: ");
        if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 0 && idx < dataPerpus.Count)
        {
            user.Ambil(dataPerpus[idx]);
        }
        else
        {
            Console.WriteLine("Input tidak valid.");
        }
    }

    static void ProsesKembali()
    {
        Console.WriteLine("\nKoleksi yang Anda pinjam:");
        user.TampilkanPinjaman();

        Console.Write("Masukkan indeks koleksi yang ingin dikembalikan: ");
        if (int.TryParse(Console.ReadLine(), out int idx))
        {
            user.Kembalikan(idx);
        }
        else
        {
            Console.WriteLine("Input salah.");
        }
    }
}